using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this on the player. Every frame it raycasts a 360 degree line-of-sight polygon,
/// draws it white-on-black into a small RenderTexture using a hidden camera, and publishes
/// that texture as the global shader texture "_FogMask".
/// Sprite shaders then read the mask to grey out floors/obstacles or hide items/monsters.
/// </summary>
[DefaultExecutionOrder(100)]
public class FogOfWarVision : MonoBehaviour
{
    [Header("Vision")]
    [Tooltip("Layers that block line of sight: walls, closed doors, etc. Keep the player off these layers.")]
    public LayerMask visionBlockers;

    [Tooltip("Max sight distance in world units. Anything farther is fogged.")]
    public float viewRadius = 20f;

    [Tooltip("Rays per degree. 1 = 360 rays. Lower this if you see CPU cost.")]
    [Range(0.25f, 4f)] public float raysPerDegree = 1f;

    [Tooltip("How far the visible area pokes INTO a blocker's surface. Lets you see the face of " +
             "obstacles/walls you're looking at. Keep it smaller than your wall thickness.")]
    public float blockerPadding = 0.3f;

    [Tooltip("Offset from this transform's pivot to the 'eyes'.")]
    public Vector2 eyeOffset;

    [Header("Mask rendering")]
    [Tooltip("Defaults to Camera.main.")]
    public Camera mainCamera;

    [Tooltip("Material using the shader FogOfWar/VisionMask.")]
    public Material maskMaterial;

    [Tooltip("Mask texture size relative to the screen. 0.5 is cheap and gives slightly soft edges.")]
    [Range(0.25f, 1f)] public float maskResolution = 0.5f;

    [Tooltip("An UNUSED layer. Only the hidden mask camera sees it.")]
    [Range(0, 31)] public int maskLayer = 31;

    const int EdgeIterations = 5;
    const float EdgeDistanceThreshold = 0.5f;
    static readonly int FogMaskId = Shader.PropertyToID("_FogMask");

    struct RayInfo
    {
        public float angle;
        public bool hit;
        public float dist;
        public Vector2 point;
    }

    readonly List<Vector2> points = new List<Vector2>(1024);
    readonly List<Vector3> verts = new List<Vector3>(1024);
    readonly List<int> tris = new List<int>(3072);

    Mesh mesh;
    GameObject meshObject;
    Camera maskCam;
    RenderTexture maskRT;

    void OnEnable()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("FogOfWarVision: no main camera found.", this);
            enabled = false;
            return;
        }
        if (!mainCamera.orthographic)
            Debug.LogWarning("FogOfWarVision expects an orthographic camera.", this);

        if (maskMaterial == null)
        {
            Shader s = Shader.Find("FogOfWar/VisionMask");
            if (s != null) maskMaterial = new Material(s);
        }
        if (maskMaterial == null)
        {
            Debug.LogError("FogOfWarVision: assign a material using the FogOfWar/VisionMask shader.", this);
            enabled = false;
            return;
        }

        // Mesh that holds the visible area
        mesh = new Mesh { name = "FogVisionMesh" };
        mesh.MarkDynamic();
        meshObject = new GameObject("FogVisionMesh") { layer = maskLayer };
        meshObject.AddComponent<MeshFilter>().sharedMesh = mesh;
        var mr = meshObject.AddComponent<MeshRenderer>();
        mr.sharedMaterial = maskMaterial;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;

        // Hidden camera that renders only that mesh into a texture
        var camGO = new GameObject("FogMaskCamera");
        camGO.transform.SetParent(mainCamera.transform, false);
        maskCam = camGO.AddComponent<Camera>();
        maskCam.orthographic = true;
        maskCam.clearFlags = CameraClearFlags.SolidColor;
        maskCam.backgroundColor = Color.black;
        maskCam.cullingMask = 1 << maskLayer;
        maskCam.allowHDR = false;
        maskCam.allowMSAA = false;
        maskCam.depth = mainCamera.depth - 10;

        // The main camera must never draw the mask mesh
        mainCamera.cullingMask &= ~(1 << maskLayer);
    }

    void OnDisable()
    {
        if (maskCam != null) Destroy(maskCam.gameObject);
        if (meshObject != null) Destroy(meshObject);
        if (mesh != null) Destroy(mesh);
        if (maskRT != null)
        {
            maskRT.Release();
            Destroy(maskRT);
            maskRT = null;
        }
        // With the script off, everything is simply visible.
        Shader.SetGlobalTexture(FogMaskId, Texture2D.whiteTexture);
    }

    void LateUpdate()
    {
        EnsureTexture();
        SyncCamera();
        BuildMesh();
    }

    void EnsureTexture()
    {
        int w = Mathf.Max(16, Mathf.RoundToInt(mainCamera.pixelWidth * maskResolution));
        int h = Mathf.Max(16, Mathf.RoundToInt(mainCamera.pixelHeight * maskResolution));
        if (maskRT != null && maskRT.width == w && maskRT.height == h) return;

        if (maskRT != null)
        {
            maskCam.targetTexture = null;
            maskRT.Release();
            Destroy(maskRT);
        }

        var format = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8)
            ? RenderTextureFormat.R8
            : RenderTextureFormat.Default;
        // URP's render graph requires a depth/stencil buffer on any camera target (24 = depth+stencil).
        maskRT = new RenderTexture(w, h, 24, format)
        {
            name = "FogMask",
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        maskRT.Create();
        maskCam.targetTexture = maskRT;
        Shader.SetGlobalTexture(FogMaskId, maskRT);
    }

    void SyncCamera()
    {
        // The mask camera is a child of the main camera, so position follows automatically.
        maskCam.transform.localPosition = Vector3.zero;
        maskCam.transform.localRotation = Quaternion.identity;
        maskCam.orthographicSize = mainCamera.orthographicSize;
        maskCam.nearClipPlane = mainCamera.nearClipPlane;
        maskCam.farClipPlane = mainCamera.farClipPlane;
    }

    // ---------------------------------------------------------------- visibility polygon

    void BuildMesh()
    {
        Vector2 origin = (Vector2)transform.position + eyeOffset;
        int rayCount = Mathf.Max(8, Mathf.RoundToInt(360f * raysPerDegree));
        float step = 360f / rayCount;

        points.Clear();
        RayInfo prev = default;
        for (int i = 0; i <= rayCount; i++)
        {
            RayInfo cur = CastRay(origin, step * i);

            // Refine around wall corners so the edges of the visible area are sharp
            if (i > 0)
            {
                bool jump = prev.hit && cur.hit && Mathf.Abs(prev.dist - cur.dist) > EdgeDistanceThreshold;
                if (prev.hit != cur.hit || jump)
                {
                    FindEdge(origin, prev, cur, out Vector2 pa, out Vector2 pb);
                    points.Add(pa);
                    points.Add(pb);
                }
            }

            if (i < rayCount) points.Add(cur.point); // ray at 360 duplicates ray at 0
            prev = cur;
        }

        verts.Clear();
        tris.Clear();
        verts.Add(new Vector3(origin.x, origin.y, 0f));
        for (int i = 0; i < points.Count; i++)
            verts.Add(new Vector3(points[i].x, points[i].y, 0f));

        int n = points.Count;
        for (int i = 0; i < n; i++)
        {
            tris.Add(0);
            tris.Add(i + 1);
            tris.Add(((i + 1) % n) + 1);
        }

        mesh.Clear();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateBounds();
    }

    RayInfo CastRay(Vector2 origin, float angleDeg)
    {
        float r = angleDeg * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(r), Mathf.Sin(r));
        RaycastHit2D h = Physics2D.Raycast(origin, dir, viewRadius, visionBlockers);

        if (h.collider != null)
        {
            float reach = Mathf.Min(h.distance + blockerPadding, viewRadius);
            return new RayInfo { angle = angleDeg, hit = true, dist = h.distance, point = origin + dir * reach };
        }
        return new RayInfo { angle = angleDeg, hit = false, dist = viewRadius, point = origin + dir * viewRadius };
    }

    void FindEdge(Vector2 origin, RayInfo a, RayInfo b, out Vector2 pointA, out Vector2 pointB)
    {
        RayInfo min = a, max = b;
        for (int i = 0; i < EdgeIterations; i++)
        {
            RayInfo mid = CastRay(origin, (min.angle + max.angle) * 0.5f);
            bool farApart = Mathf.Abs(min.dist - mid.dist) > EdgeDistanceThreshold;
            if (mid.hit == a.hit && !farApart) min = mid;
            else max = mid;
        }
        pointA = min.point;
        pointB = max.point;
    }
}