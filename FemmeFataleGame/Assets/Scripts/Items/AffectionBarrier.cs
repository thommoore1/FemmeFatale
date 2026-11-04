using FemmeFatale;
using FemmeFatale.Save;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AffectionBarrier : SaveableTransform
{
    [SerializeField] private string AgreeToHelpScenarioLabel;
    [SerializeField] private string DeclineToHelpScenarioLabel;
    [SerializeField] private DialogueLoader loader;
    [SerializeField] private MonsterAffection monsterAffection;
    
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private string failedToOpenHint;

    [SerializeField] private float affectionNumber;

    [SerializeField] private bool wantsLowAffection;
    [SerializeField] private SaveLoad saveLoad;
    [SerializeField] private CurrentHintText HintText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Wanted affection: " + affectionNumber);
        Debug.Log("Wanted low affection was: " + wantsLowAffection);
        Debug.Log("Current monster affection is: " + monsterAffection.Affection);
        
        if (wantsLowAffection)
        {
            if (monsterAffection.Affection <= affectionNumber)
            {
                loader.UpdateScenarioLable(AgreeToHelpScenarioLabel);
                DestroyImmediate(this.gameObject);
                saveLoad.SaveTemp();
                UnityEngine.SceneManagement.SceneManager.LoadScene("PlayTest_2_DialogueCharlotte");
            }
            else
            {
                loader.UpdateScenarioLable(DeclineToHelpScenarioLabel);
                HintText.savedHintText = failedToOpenHint;
                saveLoad.SaveTemp();
                UnityEngine.SceneManagement.SceneManager.LoadScene("PlayTest_2_DialogueCharlotte");
            }
        }
        else
        {
            if (monsterAffection.Affection >= affectionNumber)
            {
                loader.UpdateScenarioLable(AgreeToHelpScenarioLabel);
                UnityEngine.SceneManagement.SceneManager.LoadScene("PlayTest_2_DialogueCharlotte");
                DestroyImmediate(this.gameObject);
                saveLoad.SaveTemp();
            }
            else
            {
                loader.UpdateScenarioLable(DeclineToHelpScenarioLabel);
                HintText.savedHintText = failedToOpenHint;
                saveLoad.SaveTemp();
                UnityEngine.SceneManagement.SceneManager.LoadScene("PlayTest_2_DialogueCharlotte");
            }
        }
    }
}
