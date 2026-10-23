using UnityEngine;

[CreateAssetMenu(fileName = "DialogueLoader", menuName = "Scriptable Objects/DialogueLoader")]
public class DialogueLoader : ScriptableObject
{
    public string scenarioLabelToPlay = "IntroCutscene";

    public void UpdateScenarioLable(string newScenarioLabel)
    {
        scenarioLabelToPlay = newScenarioLabel;
    }
}
