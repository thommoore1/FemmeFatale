using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FemmeFatale.Save
{
    [CreateAssetMenu(menuName = "Saving/Prefab Registry", fileName = "PrefabRegistry")]
    public class PrefabRegistry : ScriptableObject
    {
        [SerializeField] private List<SaveableEntity> prefabs = new List<SaveableEntity>();

        private Dictionary<string, GameObject> lookup;

        private void OnEnable() => lookup = null;

        public GameObject Get(string prefabId)
        {
            if (string.IsNullOrEmpty(prefabId)) return null;
            if (lookup == null) BuildLookup();
            lookup.TryGetValue(prefabId, out var prefab);
            return prefab;
        }

        private void BuildLookup()
        {
            lookup = new Dictionary<string, GameObject>();
            foreach (var p in prefabs)
            {
                if (p == null) continue;
                if (string.IsNullOrEmpty(p.PrefabId))
                {
                    Debug.LogWarning($"[PrefabRegistry] '{p.name}' has no prefabId.", p);
                    continue;
                }
                if (!lookup.TryAdd(p.PrefabId, p.gameObject))
                    Debug.LogWarning($"[PrefabRegistry] Duplicate prefabId on '{p.name}'.", p);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Collect All Saveable Prefabs")]
        private void CollectAll()
        {
            prefabs.Clear();
            foreach (string guid in AssetDatabase.FindAssets("t:Prefab"))
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (go == null) continue;
                var entity = go.GetComponent<SaveableEntity>();
                if (entity != null && !string.IsNullOrEmpty(entity.PrefabId))
                    prefabs.Add(entity);
            }
            lookup = null;
            EditorUtility.SetDirty(this);
            Debug.Log($"[PrefabRegistry] Collected {prefabs.Count} prefabs.");
        }
#endif
    }
}