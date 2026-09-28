using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<InventorySlot>();            
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
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot != null && slot.currentItem == null)
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
}