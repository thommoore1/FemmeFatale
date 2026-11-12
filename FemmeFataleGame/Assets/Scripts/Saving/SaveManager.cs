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

        [Tooltip("Logs every entity captured on save and every one respawned on load. Handy for debugging; turn off when done.")]
        [SerializeField] private bool logEntities = true;
        
        public static event Action OnBeforeSave;
        public static event Action OnAfterLoad;

        public bool IsLoading { get; private set; }
        

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ClearSlot(4);
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public bool Save(int slot = 0)
        {
            Debug.Log("SAVE ATTEMPT");
            if (IsLoading) return false;

            OnBeforeSave?.Invoke();

            var file = new SaveFile
            {
                sceneName = SceneManager.GetActiveScene().name,
                savedAtUtc = DateTime.UtcNow.ToString("o")
            };
            
            var inventoryController = FindAnyObjectByType<InventoryController>(FindObjectsInactive.Include);
            if (inventoryController != null)
            {
                file.inventory = new InventorySaveData
                {
                    itemIds = inventoryController.CaptureSlotItemIds(),
                    selectedIndex = inventoryController.GetSelectedIndex()
                };
                Debug.Log($"[Save] Captured {file.inventory.itemIds.Count} inventory items.");
            }
            
            if (AffectionManager.Instance != null)
                file.affectionJson = AffectionManager.Instance.ToJson();

            foreach (var entity in FindEntities())
            {
                if (logEntities)
                    Debug.Log($"[Save] Capturing '{entity.name}' (prefabId '{entity.PrefabId}')", entity);
                
                Debug.Log(
                    $"[Save] Capturing: {entity.name}, " +
                    $"ID: {entity.Id}, " +
                    $"PrefabId: {entity.PrefabId}, " +
                    $"Excluded: {entity.Excluded}"
                );
                
                file.entities.Add(entity.Capture());
            }

            bool ok = SaveFileIO.Write(slot, file);
            if (ok) Debug.Log($"[Save] Saved {file.entities.Count} entities to slot {slot}.");
            return ok;
        }
        
        public void SaveButton(int slot = 0)
        {
            Debug.Log("SAVE BUTTON BLAH BLAH");
            if (Save(slot))
                Debug.Log($"[Save] Save successful in slot {slot}.");
            else
                Debug.LogWarning($"[Save] Save failed in slot {slot}.");
        }

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
                    Debug.LogWarning($"[Save] '{e.name}' has a missing or DUPLICATE id ('{e.Id}'), so it will be skipped entirely: not restored and not destroyed. " +
                                     "Right-click its SaveableEntity > Regenerate ID.", e);
            }
            
            var matched = new Dictionary<string, (SaveableEntity entity, EntityRecord record)>();
            foreach (var record in file.entities)
            {
                Debug.Log(
                    $"[Load] Looking for saved entity: " +
                    $"ID={record.id}, PrefabId={record.prefabId}"
                );

                if (!inScene.TryGetValue(record.id, out var entity))
                {
                    var prefab = prefabRegistry != null
                        ? prefabRegistry.Get(record.prefabId)
                        : null;

                    if (prefab == null)
                    {
                        Debug.LogWarning(
                            $"[Load] Cannot respawn entity. " +
                            $"ID={record.id}, PrefabId={record.prefabId}"
                        );
                        continue;
                    }

                    var instance = Instantiate(prefab);
                    instance.name = prefab.name;
                    entity = instance.GetComponent<SaveableEntity>();
                    entity.SetId(record.id);

                    inScene[record.id] = entity;

                    Debug.Log(
                        $"[Load] Respawned '{entity.name}' " +
                        $"with ID={entity.Id}, PrefabId={entity.PrefabId}"
                    );
                }
                else
                {
                    Debug.Log($"[Load] Matched existing entity '{entity.name}'");
                }

                matched[record.id] = (entity, record);
            }
            
            foreach (var pair in inScene)
            {
                if (!matched.ContainsKey(pair.Key) && pair.Value.DestroyIfMissingFromSave)
                {
                    if (logEntities)
                        Debug.Log($"[Save] Destroying '{pair.Value.name}' (not in the save file)", pair.Value);
                    Destroy(pair.Value.gameObject);
                }
            }
            
            foreach (var pair in matched.Values)
                pair.entity.Restore(pair.record);
            
            if (!string.IsNullOrEmpty(file.affectionJson) && AffectionManager.Instance != null)
                AffectionManager.Instance.FromJson(file.affectionJson);
            
            if (file.inventory != null)
            {
                var inventoryController = FindAnyObjectByType<InventoryController>(FindObjectsInactive.Include);
                if (inventoryController != null)
                {
                    inventoryController.RestoreSlotItemIds(
                        file.inventory.itemIds ?? new List<string>(),
                        file.inventory.selectedIndex);
                    Debug.Log($"[Load] Restored {file.inventory.itemIds?.Count ?? 0} inventory items.");
                }
                else
                    Debug.LogWarning("[Load] No InventoryController found to restore inventory into.");
            }
            

            Debug.Log($"[Save] Loaded {matched.Count} entities.");
        }

        private static List<SaveableEntity> FindEntities()
        {
            var all = FindObjectsByType<SaveableEntity>(FindObjectsInactive.Include);
            var result = new List<SaveableEntity>(all.Length);
            foreach (var e in all)
                if (!e.Excluded) result.Add(e);
            return result;
        }
        
        public void ClearSlot(int slot = 0)
        {
            if (IsLoading) return;
            SaveFileIO.Delete(slot);
            Debug.Log($"[Save] Cleared slot {slot}.");
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded");
            if (scene.name.Contains("level") && SaveFileIO.Exists(4))
            {
                Load(4);
                ClearSlot(4);
                Debug.Log("Finished loading save file from slot 4");
            }
        }
        
        public bool LoadInventoryFromSlot(int sourceSlot = 4)
        {
            if (!SaveFileIO.TryRead(sourceSlot, out SaveFile file))
            {
                Debug.LogWarning($"[Save] Could not read save slot {sourceSlot}.");
                return false;
            }

            if (file.inventory == null)
            {
                Debug.LogWarning($"[Save] Slot {sourceSlot} does not contain inventory data.");
                return false;
            }

            InventoryController inventoryController =
                FindAnyObjectByType<InventoryController>();

            if (inventoryController == null)
            {
                Debug.LogError("[Save] Could not find InventoryController.");
                return false;
            }
            
            var all = FindObjectsByType<InventoryController>(FindObjectsInactive.Exclude);

            Debug.Log($"[Save] Controllers in scene: {all.Length}. Using: {inventoryController.name} " +
                      $"(scene '{inventoryController.gameObject.scene.name}'). " +
                      $"Saved items: {file.inventory.itemIds?.Count ?? -1}, ids: [{string.Join(", ", file.inventory.itemIds ?? new List<string>())}]");
            
            inventoryController.RestoreSlotItemIds(
                file.inventory.itemIds ?? new List<string>(),
                file.inventory.selectedIndex
            );

            Debug.Log($"[Save] Loaded inventory from slot {sourceSlot}.");

            return true;
        }
        
        public bool SaveInventoryToSlot4()
        {
            const int targetSlot = 4;

            if (!SaveFileIO.TryRead(targetSlot, out SaveFile file))
            {
                Debug.LogWarning(
                    "[Save] Cannot save inventory: save slot 4 does not exist or could not be read."
                );
                return false;
            }

            InventoryController inventoryController =
                FindAnyObjectByType<InventoryController>();

            if (inventoryController == null)
            {
                Debug.LogError("[Save] Could not find InventoryController.");
                return false;
            }

            file.inventory = new InventorySaveData
            {
                itemIds = inventoryController.CaptureSlotItemIds(),
                selectedIndex = inventoryController.GetSelectedIndex()
            };

            // Save monster affection (all ids/values the manager currently holds)
            if (AffectionManager.Instance != null)
            {
                file.affectionJson = AffectionManager.Instance.ToJson();
                Debug.Log($"[Save] Captured affection: {file.affectionJson}");
            }
            else
            {
                Debug.LogWarning("[Save] No AffectionManager found; affection not saved.");
            }

            bool success = SaveFileIO.Write(targetSlot, file);

            if (success)
            {
                Debug.Log("[Save] Inventory and affection saved to slot 4.");
            }

            return success;
        }
        
    }
}