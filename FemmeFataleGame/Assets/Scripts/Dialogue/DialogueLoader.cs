using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueLoader", menuName = "Scriptable Objects/DialogueLoader")]
public class DialogueLoader : ScriptableObject
{
    public string scenarioLabelToPlay = "IntroCutscene";
    
    public float lowAffection = -10;
    public float highAffection = 10;
    
    public string firstEyerisConversation;
    public string firstBrambleConversation;
    public string firstFinniganConversation;
    
    public List<string> lowAffectionEyerisConversationLabels = new List<string>();
    public List<string> neutralAffectionEyerisConversationLabels = new List<string>();
    public List<string> highAffectionEyerisConversationLabels = new List<string>();
    
    public List<string> lowAffectionBrambleConversationLabels = new List<string>();
    public List<string> neutralAffectionBrambleConversationLabels = new List<string>();
    public List<string> highAffectionBrambleConversationLabels = new List<string>();
    
    public List<string> lowAffectionFinniganConversationLabels = new List<string>();
    public List<string> neutralAffectionFinniganConversationLabels = new List<string>();
    public List<string> highAffectionFinniganConversationLabels = new List<string>();

    [HideInInspector] public List<string> lowEyerisLabelsCopy = new List<string>();
    [HideInInspector] public List<string> neutralEyerisLabelsCopy = new List<string>();
    [HideInInspector] public List<string> highEyerisLabelsCopy = new List<string>();
    
    [HideInInspector] public List<string> lowBrambleLabelsCopy = new List<string>();
    [HideInInspector] public List<string> neutralBrambleLabelsCopy = new List<string>();
    [HideInInspector] public List<string> highBrambleLabelsCopy = new List<string>();
    
    [HideInInspector] public List<string> lowFinniganLabelsCopy = new List<string>();
    [HideInInspector] public List<string> neutralFinniganLabelsCopy = new List<string>();
    [HideInInspector] public List<string> highFinniganLabelsCopy = new List<string>();

    private bool hasSetUp = false;

    public void UpdateScenarioLable(string newScenarioLabel)
    {
        Debug.Log("updated label to : " + newScenarioLabel);
        scenarioLabelToPlay = newScenarioLabel;
    }

    public void Setup() //populate all of the copies lists with the labels from the scriptable object so that we can run each one once before getting repeats
    {
        if (!hasSetUp)
        {
            foreach (string label in lowAffectionEyerisConversationLabels)
            {
                lowEyerisLabelsCopy.Add(label);
            }
            
            foreach (string label in neutralAffectionEyerisConversationLabels)
            {
                neutralEyerisLabelsCopy.Add(label);
            }
            
            foreach (string label in highAffectionEyerisConversationLabels)
            {
                highEyerisLabelsCopy.Add(label);
            }
            
            foreach (string label in lowAffectionBrambleConversationLabels)
            {
                lowBrambleLabelsCopy.Add(label);
            }
            
            foreach (string label in neutralAffectionBrambleConversationLabels)
            {
                neutralBrambleLabelsCopy.Add(label);
            }
            
            foreach (string label in highAffectionBrambleConversationLabels)
            {
                highBrambleLabelsCopy.Add(label);
            }
            
            foreach (string label in lowAffectionFinniganConversationLabels)
            {
                lowFinniganLabelsCopy.Add(label);
            }
            
            foreach (string label in neutralAffectionFinniganConversationLabels)
            {
                neutralFinniganLabelsCopy.Add(label);
            }
            
            foreach (string label in highAffectionFinniganConversationLabels)
            {
                highFinniganLabelsCopy.Add(label);
            }

            hasSetUp = true;
        }
    }
}
