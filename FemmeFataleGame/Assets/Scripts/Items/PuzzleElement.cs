using System.Collections;
using UnityEngine;
using FemmeFatale.Save;
using TMPro;

namespace FemmeFatale
{
    public class PuzzleElement : SaveableTransform
    {
        [SerializeField] private GameObject solutionItem;
        [SerializeField] private TextMeshProUGUI feedbackText;
        
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
                Destroy(selectedItem);
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log($"Puzzle element {name} interacted with. Incorrect item");
                feedbackText.text = "This isn't the right item to use here.";
                StartCoroutine(ResetTextTimer());
            }
        }
        
        private IEnumerator ResetTextTimer()
        {
            yield return new WaitForSeconds(2);
            feedbackText.text = "";
        }
    }
}
