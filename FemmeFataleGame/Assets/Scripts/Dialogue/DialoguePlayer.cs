using UnityEngine;
using Utage;

public class DialoguePlayer : MonoBehaviour
{
    public AdvEngine AdvEngine;

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
        AdvEngine.StartGame(ScenarioLabel);
    }
}
