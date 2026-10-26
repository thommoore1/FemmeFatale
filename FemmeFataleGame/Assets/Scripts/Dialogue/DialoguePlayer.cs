using UnityEngine;
using Utage;

public class DialoguePlayer : MonoBehaviour
{
    public AdvEngine AdvEngine;

    public DialogueLoader DialogueLoader;

    public string ScenarioLabel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayScenario();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayScenario()
    {
        if (!string.IsNullOrEmpty(ScenarioLabel))
        {
            AdvEngine.StartGame(ScenarioLabel);
        }
        else
        {
            AdvEngine.StartGame(DialogueLoader.scenarioLabelToPlay);
        }
    }
}
