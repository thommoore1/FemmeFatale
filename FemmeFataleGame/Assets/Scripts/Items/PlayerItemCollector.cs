using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item")) return;

        if (inventoryController == null)
        {
            Debug.LogError("Inventory Controller is not assigned on " + name, this);
            return;
        }

        Item item = collision.GetComponent<Item>();
        if (item != null && item.uiPrefab != null)
        {
            if (inventoryController.AddItem(item.uiPrefab))
                Destroy(collision.gameObject);
        }
    }
}
