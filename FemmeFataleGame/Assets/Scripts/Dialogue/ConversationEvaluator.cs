using UnityEngine;
using Utage;

public class ConversationEvaluator : MonoBehaviour
{
    [SerializeField] private AdvEngine engine;
    
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
        if (engine.Param.GetParameterBoolean("passedConversation"))
        {
            print("You passed!");
        }
        else
        {
            print("You failed!");
        }
    }
}
