using UnityEngine;

public class MonsterAffection : MonoBehaviour
{
    [Header("Identity")]
    [Tooltip("Same ID on many monsters = shared (species-wide) affection. Unique ID = individual.")]
    [SerializeField] private string affectionId = "slime";
    [SerializeField, Range(-100f, 100f)] private float startingAffection = 0f;

    [Header("Multiplier curves (X: -1 hate .. +1 love, Y: multiplier)")]
    [SerializeField] private AnimationCurve patrolSpeedCurve = Curve(3f, 1f, 0.5f);
    [SerializeField] private AnimationCurve chaseSpeedCurve  = Curve(1.5f, 1f, 0.7f);
    [SerializeField] private AnimationCurve rangeCurve       = Curve(1.3f, 1f, 0.8f);
    [SerializeField] private AnimationCurve visionAngleCurve = Curve(1.25f, 1f, 0.9f);
    [SerializeField] private AnimationCurve searchTimeCurve  = Curve(1.5f, 1f, 0.6f);

    public string AffectionId => affectionId;

    /// Raw value, -100..100
    public float Affection =>
        AffectionManager.Instance.Get(affectionId, startingAffection);

    /// -1..1
    public float AffectionNormalized => Affection / AffectionManager.Max;
    

    public float PatrolModifier(float speed)         => Apply(patrolSpeedCurve, speed);
    public float ChaseModifier(float speed)          => Apply(chaseSpeedCurve, speed);
    public float RangeModifier(float range)          => Apply(rangeCurve, range);
    public float VisionAngleModifier(float angle)    => Mathf.Clamp(Apply(visionAngleCurve, angle), 0f, 360f);
    public float SearchModifier(float searchTime)    => Apply(searchTimeCurve, searchTime);

    private float Apply(AnimationCurve curve, float baseValue)
    {
        return baseValue * curve.Evaluate(AffectionNormalized);
    }

    private void Start()
    {
        AffectionManager.Instance.InitializeIfMissing(affectionId, startingAffection);
    }

    public void Add(float amount) =>
        AffectionManager.Instance.Add(affectionId, amount, startingAffection);

    // Builds a simple 3-point curve: value at hate, neutral, love.
    private static AnimationCurve Curve(float atHate, float atNeutral, float atLove)
    {
        return new AnimationCurve(
            new Keyframe(-1f, atHate),
            new Keyframe(0f, atNeutral),
            new Keyframe(1f, atLove));
    }
}