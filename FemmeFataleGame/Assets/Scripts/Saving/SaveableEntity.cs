using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace FemmeFatale.Save
{
    [DisallowMultipleComponent]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private string prefabId;

        [Tooltip("If this object exists in the scene but is NOT in the save file, destroy it on load " +
                 "(e.g. an enemy that was killed or an item that was picked up).")]
        [SerializeField] private bool destroyIfMissingFromSave = true;

        public string Id => id;
        public string PrefabId => prefabId;
        public bool DestroyIfMissingFromSave => destroyIfMissingFromSave;
        
        public bool Excluded
        {
            get => _excluded || GetComponentInParent<SaveExclusionZone>(true) != null;
            set => _excluded = value;
        }
        private bool _excluded;

        private void Awake()
        {
            if (string.IsNullOrEmpty(id)) id = Guid.NewGuid().ToString("N");
        }

        internal void SetId(string newId) => id = newId;

        public virtual EntityRecord Capture()
        {
            var record = new EntityRecord { id = id, prefabId = prefabId };
            foreach (var pair in BuildSaveableMap())
            {
                record.components.Add(new ComponentRecord
                {
                    key = pair.Key,
                    json = pair.Value.CaptureState()
                });
            }
            return record;
        }

        public virtual void Restore(EntityRecord record)
        {
            var map = BuildSaveableMap();
            foreach (var comp in record.components)
            {
                if (map.TryGetValue(comp.key, out var saveable))
                    saveable.RestoreState(comp.json);
            }
        }
        
        private Dictionary<string, ISaveable> BuildSaveableMap()
        {
            var map = new Dictionary<string, ISaveable>();
            var counts = new Dictionary<string, int>();
            foreach (var s in GetComponents<ISaveable>())
            {
                string name = s.GetType().FullName;
                counts.TryGetValue(name, out int n);
                counts[name] = n + 1;
                map[n == 0 ? name : name + "#" + n] = s;
            }
            return map;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            
            if (PrefabUtility.IsPartOfPrefabAsset(this))
            {
                bool changed = false;
                if (!string.IsNullOrEmpty(id)) { id = ""; changed = true; }
                if (string.IsNullOrEmpty(prefabId))
                {
                    string path = AssetDatabase.GetAssetPath(this);
                    if (!string.IsNullOrEmpty(path))
                    {
                        prefabId = AssetDatabase.AssetPathToGUID(path); // stable even if renamed/moved
                        changed = true;
                    }
                }
                if (changed) EditorUtility.SetDirty(this);
                return;
            }
            
            if (PrefabStageUtility.GetPrefabStage(gameObject) != null) return;
            
            if (string.IsNullOrEmpty(id) || IsDuplicateInScene())
                AssignNewId();
        }

        private bool IsDuplicateInScene()
        {
            foreach (var other in FindObjectsByType<SaveableEntity>(FindObjectsInactive.Include))
            {
                if (other != this && other.gameObject.scene == gameObject.scene && other.id == id)
                    return true;
            }
            return false;
        }

        [ContextMenu("Regenerate ID")]
        private void AssignNewId()
        {
            id = Guid.NewGuid().ToString("N");
            EditorUtility.SetDirty(this);
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }
#endif
    }
}
