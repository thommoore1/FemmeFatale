using System.Collections;
using UnityEngine;
using FemmeFatale.Save;
using JetBrains.Annotations;
using TMPro;

namespace FemmeFatale
{
    public class DirectionalDoor : SaveableTransform
    {
        [SerializeField] private string unlockDirection;
        [SerializeField] private TextMeshProUGUI feedbackText;
        
        public void Interact(Vector3 playerTransformPosition)
        {
            
            Debug.Log($"Player at position: {playerTransformPosition}");
            Debug.Log($"solution direction was: {unlockDirection}");
            
            switch(unlockDirection.ToLower())
            {
                case "left":
                    if (playerTransformPosition.x < this.gameObject.transform.position.x)
                    {
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        feedbackText.text = "It's locked from the other side.";
                    }

                    break;
                case "right":
                    if (playerTransformPosition.x > this.gameObject.transform.position.x)
                    {
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        feedbackText.text = "It's locked from the other side.";
                    }

                    break;
                case "above":
                    print("above");
                    if (playerTransformPosition.y > this.gameObject.transform.position.y)
                    {
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        feedbackText.text = "It's locked from the other side.";
                    }

                    break;
                case "below":
                    if (playerTransformPosition.y < this.gameObject.transform.position.y)
                    {
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        feedbackText.text = "It's locked from the other side.";
                    }

                    break;
                default:
                    Debug.LogError("Unrecognized solution direction for directional door");
                    break;
            }

            StartCoroutine(ResetTextTimer());
        }

        private IEnumerator ResetTextTimer()
        {
            yield return new WaitForSeconds(2);
            feedbackText.text = "";
        }
    }
}
