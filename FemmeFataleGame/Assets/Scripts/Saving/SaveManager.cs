using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FemmeFatale.Save
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        [SerializeField] private PrefabRegistry prefabRegistry;
        
        public static event Action OnBeforeSave;
        public static event Action OnAfterLoad;

        public bool IsLoading { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // ---------------------------------------------------------------- save

        public bool Save(int slot = 0)
        {
            if (IsLoading) return false;

            OnBeforeSave?.Invoke();

            var file = new SaveFile
            {
                sceneName = SceneManager.GetActiveScene().name,
                savedAtUtc = DateTime.UtcNow.ToString("o")
            };

            foreach (var entity in FindEntities())
                file.entities.Add(entity.Capture());

            bool ok = SaveFileIO.Write(slot, file);
            if (ok) Debug.Log($"[Save] Saved {file.entities.Count} entities to slot {slot}.");
            return ok;
        }
        
        public void ButtonSave(int slot = 0)
        {
            if (Save(slot))
                Debug.Log($"[Save] Saved to slot {slot}.");
            else
                Debug.LogWarning($"[Save] Failed to save to slot {slot}.");
        }

        // ---------------------------------------------------------------- load

        public void Load(int slot = 0)
        {
            if (IsLoading) return;
            if (!SaveFileIO.TryRead(slot, out var file))
            {
                Debug.LogWarning($"[Save] No readable save in slot {slot}.");
                return;
            }
            StartCoroutine(LoadRoutine(file));
        }

        private IEnumerator LoadRoutine(SaveFile file)
        {
            IsLoading = true;
            
            var op = SceneManager.LoadSceneAsync(file.sceneName);
            while (!op.isDone) yield return null;
            yield return null;

            Apply(file);

            IsLoading = false;
            OnAfterLoad?.Invoke();
        }

        private void Apply(SaveFile file)
        {
            var inScene = new Dictionary<string, SaveableEntity>();
            foreach (var e in FindEntities())
            {
                if (string.IsNullOrEmpty(e.Id) || !inScene.TryAdd(e.Id, e))
                    Debug.LogWarning($"[Save] '{e.name}' has a missing or duplicate id.", e);
            }
            
            var matched = new Dictionary<string, (SaveableEntity entity, EntityRecord record)>();
            foreach (var record in file.entities)
            {
                if (!inScene.TryGetValue(record.id, out var entity))
                {
                    var prefab = prefabRegistry != null ? prefabRegistry.Get(record.prefabId) : null;
                    if (prefab == null)
                    {
                        Debug.LogWarning($"[Save] Can't find or respawn entity {record.id} (prefabId '{record.prefabId}').");
                        continue;
                    }
                    var instance = Instantiate(prefab);
                    entity = instance.GetComponent<SaveableEntity>();
                    entity.SetId(record.id);
                    inScene[record.id] = entity;
                }
                matched[record.id] = (entity, record);
            }
            
            foreach (var pair in inScene)
            {
                if (!matched.ContainsKey(pair.Key) && pair.Value.DestroyIfMissingFromSave)
                {
                    Debug.Log($"[Save] Destroying entity {pair.Key} because it was not found in the save file.");
                    Destroy(pair.Value.gameObject);
                }
            }
            
            foreach (var pair in matched.Values)
                pair.entity.Restore(pair.record);

            Debug.Log($"[Save] Loaded {matched.Count} entities.");
        }

        private static SaveableEntity[] FindEntities() =>
            FindObjectsByType<SaveableEntity>(FindObjectsInactive.Include);
        
        public void ClearSlot(int slot = 0)
        {
            if (IsLoading) return;
            SaveFileIO.Delete(slot);
            Debug.Log($"[Save] Cleared slot {slot}.");
        }
    }
}