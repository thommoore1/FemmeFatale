using Unity.VisualScripting;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace FemmeFatale
{
    public class InventoryController : MonoBehaviour
    {
        public GameObject inventoryPanel;
        public GameObject slotPrefab;
        public int slotCount;
        InventorySlot selectedSlot;
        
        public GameObject[] itemPrefabs;
        
        public GameObject[] itemDatabase;

        private List<InventorySlot> _slots = new List<InventorySlot>();
        private Dictionary<string, GameObject> _prefabLookup;
        private bool _initialized;

        void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            for (int i = 0; i < slotCount; i++)
            {
                InventorySlot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<InventorySlot>();
                slot.controller = this;
                _slots.Add(slot);

                if (i < itemPrefabs.Length)
                    SpawnItemInSlot(itemPrefabs[i], slot);
            }

            SelectSlot(_slots[0]);
        }
        
        private GameObject SpawnItemInSlot(GameObject itemPrefab, InventorySlot slot)
        {
            GameObject item = Instantiate(itemPrefab, slot.transform);
            Debug.Log($"Created {item.name}");
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            var tag = item.GetComponent<InventoryItemId>();
            if (tag == null) tag = item.AddComponent<InventoryItemId>();
            tag.itemId = itemPrefab.name;

            slot.currentItem = item;
            return item;
        }

        public bool AddItem(GameObject itemPrefab)
        {
            Debug.Log("AddItem called for " + itemPrefab.name);
            foreach (Transform slotTransform in inventoryPanel.transform)
            {
                InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
                if (slot != null && slot.IsEmpty)
                {
                    SpawnItemInSlot(itemPrefab, slot);
                    return true;
                }
            }

            Debug.Log("Inventory is full!");
            return false;
        }

        public void SelectSlot(InventorySlot slot)
        {
            if (selectedSlot != null) selectedSlot.SetSelected(false);
            selectedSlot = slot;
            selectedSlot.SetSelected(true);

            if (selectedSlot.currentItem != null)
                Debug.Log("Selected item: " + selectedSlot.currentItem.name);
            else
                Debug.Log("Selected an empty slot");
        }
        
        public void DeleteSelected()
        {
            if (selectedSlot == null || selectedSlot.IsEmpty) return;

            Debug.Log("Deleted: " + selectedSlot.currentItem.name);
            Destroy(selectedSlot.currentItem);
            selectedSlot.currentItem = null;

            selectedSlot.SetSelected(false);
            selectedSlot = null;
        }

        public void Drop(Vector2 dropPosition)
        {
            if (selectedSlot == null || selectedSlot.IsEmpty) return;

            Debug.Log("Dropped: " + selectedSlot.currentItem.name);
            Instantiate(selectedSlot.currentItem, dropPosition, Quaternion.identity);
            Destroy(selectedSlot.currentItem);
            selectedSlot.currentItem = null;
        }

        public void SelectNextSlot()
        {
            if (_slots.Count == 0) return;

            int currentIndex = _slots.IndexOf(selectedSlot);
            int nextIndex = (currentIndex + 1) % _slots.Count;
            SelectSlot(_slots[nextIndex]);
        }

        public void SelectPreviousSlot()
        {
            if (_slots.Count == 0) return;

            int currentIndex = _slots.IndexOf(selectedSlot);
            int previousIndex = (currentIndex - 1 + _slots.Count) % _slots.Count;
            SelectSlot(_slots[previousIndex]);
        }

        public GameObject GetSelectedItem()
        {
            Debug.Log("GetSelectedItem called");
            return selectedSlot.currentItem;
        }
        
        public List<string> CaptureSlotItemIds()
        {
            Initialize();

            var ids = new List<string>();
            foreach (var slot in _slots)
            {
                string id = "";
                if (slot.currentItem != null)
                {
                    var tag = slot.currentItem.GetComponent<InventoryItemId>();
                    if (tag != null) id = tag.itemId;
                    else Debug.LogWarning("[Inventory] Item '" + slot.currentItem.name + "' has no InventoryItemId and won't be saved.");
                }
                ids.Add(id);
            }
            return ids;
        }

        public int GetSelectedIndex()
        {
            return _slots.IndexOf(selectedSlot);
        }
        
        public void RestoreSlotItemIds(List<string> ids, int selectedIndex)
        {
            Initialize();
            BuildPrefabLookup();

            for (int i = 0; i < _slots.Count; i++)
            {
                InventorySlot slot = _slots[i];

                if (slot.currentItem != null)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }

                string id = (ids != null && i < ids.Count) ? ids[i] : "";
                if (string.IsNullOrEmpty(id)) continue;

                if (_prefabLookup.TryGetValue(id, out GameObject prefab))
                    SpawnItemInSlot(prefab, slot);
                else
                    Debug.LogWarning("[Inventory] No prefab named '" + id + "' in itemDatabase. Add it so it can be loaded.");
            }

            if (selectedIndex >= 0 && selectedIndex < _slots.Count)
                SelectSlot(_slots[selectedIndex]);
        }

        private void BuildPrefabLookup()
        {
            _prefabLookup = new Dictionary<string, GameObject>();
            AddToLookup(itemDatabase);
            AddToLookup(itemPrefabs);
        }

        private void AddToLookup(GameObject[] prefabs)
        {
            if (prefabs == null) return;
            foreach (var p in prefabs)
            {
                if (p == null) continue;
                if (!_prefabLookup.TryAdd(p.name, p) && _prefabLookup[p.name] != p)
                    Debug.LogWarning("[Inventory] Two different item prefabs are both named '" + p.name + "'. Rename one.");
            }
        }
    }
}
