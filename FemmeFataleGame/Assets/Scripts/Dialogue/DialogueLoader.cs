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
    
    // REGULAR CONVERSATION LABELS//
    //EYERIS
    public List<string> lowAffectionEyerisConversationLabels = new List<string>();
    public List<string> neutralAffectionEyerisConversationLabels = new List<string>();
    public List<string> highAffectionEyerisConversationLabels = new List<string>();
    
    //BRAMBLE
    public List<string> lowAffectionBrambleConversationLabels = new List<string>();
    public List<string> neutralAffectionBrambleConversationLabels = new List<string>();
    public List<string> highAffectionBrambleConversationLabels = new List<string>();
    
    //FINNIGAN
    public List<string> lowAffectionFinniganConversationLabels = new List<string>();
    public List<string> neutralAffectionFinniganConversationLabels = new List<string>();
    public List<string> highAffectionFinniganConversationLabels = new List<string>();

    //GIFT REACTION LABELS//
    //EYERIS
    public List<string> positiveGiftReactionEyerisConversationLabels = new List<string>();
    public List<string> neutralGiftReactionEyerisConversationLabels = new List<string>();
    public List<string> negativeGiftReactionEyerisConversationLabels = new List<string>();
    
    //BRAMBLE
    public List<string> positiveGiftReactionBrambleConversationLabels = new List<string>();
    public List<string> neutralGiftReactionBrambleConversationLabels = new List<string>();
    public List<string> negativeGiftReactionBrambleConversationLabels = new List<string>();
    
    //FINNIGAN
    public List<string> positiveGiftReactionFinniganConversationLabels = new List<string>();
    public List<string> neutralGiftReactionFinniganConversationLabels = new List<string>();
    public List<string> negativeGiftReactionFinniganConversationLabels = new List<string>();
    
    //NO GIFT REACTION LABELS//
    //EYERIS
    public List<string> noGiftReactionEyerisLabels = new List<string>();
    
    //BRAMBLE
    public List<string> noGiftReactionBrambleLabels = new List<string>();
    
    //FINNIGAN
    public List<string> noGiftReactionFinniganLabels = new List<string>();
    

    //PRIVATE LISTS TO PREVENT REPEATS UNTIL NECESSARY//
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
            lowEyerisLabelsCopy.Clear();
            foreach (string label in lowAffectionEyerisConversationLabels)
            {
                lowEyerisLabelsCopy.Add(label);
            }
            
            neutralBrambleLabelsCopy.Clear();
            foreach (string label in neutralAffectionEyerisConversationLabels)
            {
                neutralEyerisLabelsCopy.Add(label);
            }
            
            highEyerisLabelsCopy.Clear();
            foreach (string label in highAffectionEyerisConversationLabels)
            {
                highEyerisLabelsCopy.Add(label);
            }
            
            lowBrambleLabelsCopy.Clear();
            foreach (string label in lowAffectionBrambleConversationLabels)
            {
                lowBrambleLabelsCopy.Add(label);
            }
            
            neutralBrambleLabelsCopy.Clear();
            foreach (string label in neutralAffectionBrambleConversationLabels)
            {
                neutralBrambleLabelsCopy.Add(label);
            }
            
            highBrambleLabelsCopy.Clear();
            foreach (string label in highAffectionBrambleConversationLabels)
            {
                highBrambleLabelsCopy.Add(label);
            }
            
            lowFinniganLabelsCopy.Clear();
            foreach (string label in lowAffectionFinniganConversationLabels)
            {
                lowFinniganLabelsCopy.Add(label);
            }
            
            neutralFinniganLabelsCopy.Clear();
            foreach (string label in neutralAffectionFinniganConversationLabels)
            {
                neutralFinniganLabelsCopy.Add(label);
            }
            
            highFinniganLabelsCopy.Clear();
            foreach (string label in highAffectionFinniganConversationLabels)
            {
                highFinniganLabelsCopy.Add(label);
            }

            hasSetUp = true;
        }
    }
}
