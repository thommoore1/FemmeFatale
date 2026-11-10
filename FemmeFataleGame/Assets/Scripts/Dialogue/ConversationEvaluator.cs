using FemmeFatale;
using FemmeFatale.Save;
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
                //UnityEngine.SceneManagement.SceneManager.LoadScene(mapSceneToLoad);
                SaveManager.Instance.Load(4);
                AffectionManager.Instance.PotJail();
            }
        }
        else if (engine.Param.GetParameterBoolean("noGiftReaction"))
        {
            Debug.Log("You had no gift for the monster");
            AffectionManager.Instance.PotJail();
            SaveManager.Instance.Load(4);
            engine.Param.SetParameterBoolean("noGiftReaction", false);
        }
        else
        {
                SaveManager.Instance.Load(4);
              //  UnityEngine.SceneManagement.SceneManager.LoadScene(mapSceneToLoad);
        }
    }
}
