using UnityEngine;

namespace FemmeFatale
{
    public class PlayerItemCollector : MonoBehaviour
    {
        [SerializeField] private InventoryController inventoryController;
        private bool isOverlapping = false;
        public bool IsOverlapping => isOverlapping;


        // private void OnTriggerEnter2D(Collider2D collision)
        // {
        //     if (!collision.CompareTag("Item")) return;
        //
        //     if (inventoryController == null)
        //     {
        //         Debug.LogError("Inventory Controller is not assigned on " + name, this);
        //         return;
        //     }
        //
        //     Item item = collision.GetComponent<Item>();
        //     if (item == null || item.uiPrefab == null || item.collected) return;
        //
        //     if (inventoryController.AddItem(item.uiPrefab))
        //     {
        //         item.collected = true;
        //         Destroy(collision.gameObject);
        //     }
        // }

        private void OnTriggerEnter(Collider other)
        {
            isOverlapping = false;
        }

        private void OnTriggerExit(Collider other)
        {
            isOverlapping = true;
        }

        public void AddToInventory(Item item)
        {
            if (inventoryController == null)
            {
                Debug.LogError("Inventory Controller is not assigned on " + name, this);
                return;
            }

            if (item == null || item.uiPrefab == null || item.collected) return;

            if (inventoryController.AddItem(item.uiPrefab))
            {
                item.collected = true;
                Destroy(item.gameObject);
            }
        }
    }
}