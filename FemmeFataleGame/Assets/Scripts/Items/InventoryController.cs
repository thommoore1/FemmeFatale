using Unity.VisualScripting;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    InventorySlot selectedSlot;

    public GameObject[] itemPrefabs;

    private List<InventorySlot> _slots = new List<InventorySlot>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<InventorySlot>();
            slot.controller = this;
            _slots.Add(slot);

            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                slot.currentItem = item;
            }
        }
        SelectSlot(_slots[0]);
    }

    public bool AddItem(GameObject itemPrefab)
    {
        Debug.Log("AddItem called for " + itemPrefab.name);
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot != null && slot.IsEmpty)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                slot.currentItem = newItem;
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

// Hook this up to the trash image's button
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
}