using System.Collections.Generic;
using UnityEngine;

public class DialogueScenarioSelector : MonoBehaviour
{
    //TODO: put all of the data into a separate static class so it persists between scenes
    public float lowAffection = -10;
    public float highAffection = 10;

    [SerializeField] private DialogueLoader dialogueLoader;

    [SerializeField] private string firstEyerisConversation;
    [SerializeField] private string firstBrambleConversation;
    [SerializeField] private string firstFinniganConversation;
    
    [SerializeField] private List<string> lowAffectionEyerisConversationLabels = new List<string>();
    [SerializeField] private List<string> neutralAffectionEyerisConversationLabels = new List<string>();
    [SerializeField] private List<string> highAffectionEyerisConversationLabels = new List<string>();
    
    [SerializeField] private List<string> lowAffectionBrambleConversationLabels = new List<string>();
    [SerializeField] private List<string> neutralAffectionBrambleConversationLabels = new List<string>();
    [SerializeField] private List<string> highAffectionBrambleConversationLabels = new List<string>();
    
    [SerializeField] private List<string> lowAffectionFinniganConversationLabels = new List<string>();
    [SerializeField] private List<string> neutralAffectionFinniganConversationLabels = new List<string>();
    [SerializeField] private List<string> highAffectionFinniganConversationLabels = new List<string>();

    private List<string> lowEyerisLabelsCopy = new List<string>();
    private List<string> neutralEyerisLabelsCopy = new List<string>();
    private List<string> highEyerisLabelsCopy = new List<string>();
    
    private List<string> lowBrambleLabelsCopy = new List<string>();
    private List<string> neutralBrambleLabelsCopy = new List<string>();
    private List<string> highBrambleLabelsCopy = new List<string>();
    
    private List<string> lowFinniganLabelsCopy = new List<string>();
    private List<string> neutralFinniganLabelsCopy = new List<string>();
    private List<string> highFinniganLabelsCopy = new List<string>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lowEyerisLabelsCopy = lowAffectionEyerisConversationLabels;
        neutralEyerisLabelsCopy = neutralAffectionEyerisConversationLabels;
        highEyerisLabelsCopy = highAffectionEyerisConversationLabels;

        lowBrambleLabelsCopy = lowAffectionBrambleConversationLabels;
        neutralBrambleLabelsCopy = neutralAffectionBrambleConversationLabels;
        highBrambleLabelsCopy = highAffectionBrambleConversationLabels;

        lowFinniganLabelsCopy = lowAffectionFinniganConversationLabels;
        neutralFinniganLabelsCopy = neutralAffectionFinniganConversationLabels;
        highFinniganLabelsCopy = highAffectionFinniganConversationLabels;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseConversation(string monsterName, float affectionNumber)
    {
        switch (monsterName)
        {
            case "Eyeris":
                switch (DetermineAffection(affectionNumber))
                {
                    case "low":
                        if (lowEyerisLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(lowEyerisLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            lowEyerisLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(lowAffectionEyerisConversationLabels));
                        }
                        break;
                    case "high":
                        if (highEyerisLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(highEyerisLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            highEyerisLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(highAffectionEyerisConversationLabels));
                        }
                        break;
                    case "neutral":
                        if (neutralEyerisLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(neutralEyerisLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            neutralEyerisLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(neutralAffectionEyerisConversationLabels));
                        }
                        break;
                    default:
                        Debug.LogError("Affection level not handled correctly");
                        break;
                }
                break;
            case "Bramble":
                switch (DetermineAffection(affectionNumber))
                {
                    case "low":
                        if (lowBrambleLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(lowBrambleLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            lowBrambleLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(lowAffectionBrambleConversationLabels));
                        }
                        break;
                    case "high":
                        if (highBrambleLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(highBrambleLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            highBrambleLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(highAffectionBrambleConversationLabels));
                        }
                        break;
                    case "neutral":
                        if (neutralBrambleLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(neutralBrambleLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            neutralBrambleLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(neutralAffectionBrambleConversationLabels));
                        }
                        break;
                    default:
                        Debug.LogError("Affection level not handled correctly");
                        break;
                }
                break;
            case "Finnigan":
                switch (DetermineAffection(affectionNumber))
                {
                    case "low":
                        if (lowFinniganLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(lowFinniganLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            lowFinniganLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(lowAffectionFinniganConversationLabels));
                        }
                        break;
                    case "high":
                        if (highFinniganLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(highFinniganLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            highFinniganLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(highAffectionFinniganConversationLabels));
                        }
                        break;
                    case "neutral":
                        if (neutralFinniganLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(neutralFinniganLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            neutralFinniganLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(neutralAffectionFinniganConversationLabels));
                        }
                        break;
                    default:
                        Debug.LogError("Affection level not handled correctly");
                        break;
                }
                break;
            default:
                Debug.LogError("Monster Name " + name + " not identified");
                break;
        }
    }

    private string DetermineAffection(float affectionNumber)
    {
        if (affectionNumber <= lowAffection)
        {
            return "low";
        }
        else if (affectionNumber >= highAffection)
        {
            return "high";
        }
        else
        {
            return "neutral";
        }
    }

    private string getRandomElement(List<string> conversationList)
    {
        return conversationList[getRandomIndex(conversationList)];
    }

    private int getRandomIndex(List<string> conversationList)
    {
        return Random.Range(0, conversationList.Count);
    }
}
