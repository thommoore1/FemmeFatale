using System;

using TMPro;
using UnityEngine;
using UtageExtensions;

public class AffectionBarrierHintText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private string hintDestroyText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (feedbackText.text.IsNullOrEmpty())
        {
            feedbackText.text = hintDestroyText;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (feedbackText.text.Equals(hintDestroyText))
        {
            feedbackText.text = "";
        }
    }
}
