using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    InventorySlot selectedSlot;

    public GameObject[] itemPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<InventorySlot>();
            slot.controller = this;

            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                slot.currentItem = item;
            }
        }
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

}