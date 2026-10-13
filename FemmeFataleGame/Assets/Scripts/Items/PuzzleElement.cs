using UnityEngine;

namespace FemmeFatale
{
    public class PuzzleElement : MonoBehaviour
    {
        [SerializeField] private GameObject solutionItem;
        
        public void Interact(GameObject selectedItem)
        {
            if (!selectedItem)
            {
                return;
            }   
            
            Debug.Log($"selected item name: {selectedItem.name}");
            Debug.Log($"solution item name: {solutionItem.name}");
            
            if (solutionItem.GetComponent<Item>().itemName == selectedItem.GetComponent<Item>().itemName)
            {
                //TODO: Trigger puzzle solved event
                Destroy(selectedItem);
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log($"Puzzle element {name} interacted with. Incorrect item");
            }
        }
    }
}
