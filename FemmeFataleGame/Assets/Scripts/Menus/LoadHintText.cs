using TMPro;
using UnityEngine;
using System.Collections;

public class LoadHintText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI feedbackText; 
    [SerializeField] private CurrentHintText HintText;
    
    void Start()
    {
        feedbackText.text = HintText.savedHintText;
        StartCoroutine(ResetTextTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator ResetTextTimer()
    {
        yield return new WaitForSeconds(2);
        feedbackText.text = "";
    }
}
