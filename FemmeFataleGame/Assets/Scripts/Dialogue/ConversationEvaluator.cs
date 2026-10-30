using FemmeFatale;
using UnityEngine;
using Utage;

public class ConversationEvaluator : MonoBehaviour
{
    [SerializeField] private AdvEngine engine;
    [SerializeField] private string giftSceneToLoad;
    [SerializeField] private string mapSceneToLoad;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EvaluateConversation()
    {
        if (engine.Param.GetParameterBoolean("conversationNeedsEvaluation"))
        {
            if (engine.Param.GetParameterBoolean("passedConversation"))
            {
                print("You passed!");
                UnityEngine.SceneManagement.SceneManager.LoadScene(giftSceneToLoad);
            }
            else
            {
                print("You failed!");
                UnityEngine.SceneManagement.SceneManager.LoadScene(mapSceneToLoad);
            }
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mapSceneToLoad);
        }
    }
}
