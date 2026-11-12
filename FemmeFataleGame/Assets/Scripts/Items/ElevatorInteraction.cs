using System.Collections;
using FemmeFatale;
using TMPro;
using UnityEngine;

public class ElevatorInteraction : MonoBehaviour
{
    public DialogueLoader loader;
    public string DialogueSceneToLoad;
    public string WinScenarioToPlay;
    
    [SerializeField] private TextMeshProUGUI feedbackText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixElevator(InventoryController inventoryController)
    {
        if (inventoryController.InventoryContainsElevatorItems())
        {
            loader.UpdateScenarioLable(WinScenarioToPlay);
            UnityEngine.SceneManagement.SceneManager.LoadScene(DialogueSceneToLoad);
        }

        else
        {
            feedbackText.text = "I don't have all the parts to fix the elevator.";
            StartCoroutine(ResetTextTimer());
        }
    }
    
    private IEnumerator ResetTextTimer()
    {
        yield return new WaitForSeconds(2);
        feedbackText.text = "";
    }
}
