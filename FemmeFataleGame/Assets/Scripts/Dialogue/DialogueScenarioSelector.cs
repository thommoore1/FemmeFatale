using System.Collections.Generic;
using UnityEngine;

public class DialogueScenarioSelector : MonoBehaviour
{

    [SerializeField] private DialogueLoader dialogueLoader;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueLoader.Setup();
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
                        if (dialogueLoader.lowEyerisLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(dialogueLoader.lowEyerisLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            dialogueLoader.lowEyerisLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(dialogueLoader.lowAffectionEyerisConversationLabels));
                        }
                        break;
                    case "high":
                        if (dialogueLoader.highEyerisLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(dialogueLoader.highEyerisLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            dialogueLoader.highEyerisLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(dialogueLoader.highAffectionEyerisConversationLabels));
                        }
                        break;
                    case "neutral":
                        if (dialogueLoader.neutralEyerisLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(dialogueLoader.neutralEyerisLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            dialogueLoader.neutralEyerisLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(dialogueLoader.neutralAffectionEyerisConversationLabels));
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
                        if (dialogueLoader.lowBrambleLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(dialogueLoader.lowBrambleLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            dialogueLoader.lowBrambleLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(dialogueLoader.lowAffectionBrambleConversationLabels));
                        }
                        break;
                    case "high":
                        if (dialogueLoader.highBrambleLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(dialogueLoader.highBrambleLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            dialogueLoader.highBrambleLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(dialogueLoader.highAffectionBrambleConversationLabels));
                        }
                        break;
                    case "neutral":
                        if (dialogueLoader.neutralBrambleLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(dialogueLoader.neutralBrambleLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            dialogueLoader.neutralBrambleLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(dialogueLoader.neutralAffectionBrambleConversationLabels));
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
                        if (dialogueLoader.lowFinniganLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(dialogueLoader.lowFinniganLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            dialogueLoader.lowFinniganLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(dialogueLoader.lowAffectionFinniganConversationLabels));
                        }
                        break;
                    case "high":
                        if (dialogueLoader.highFinniganLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(dialogueLoader.highFinniganLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            dialogueLoader.highFinniganLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(dialogueLoader.highAffectionFinniganConversationLabels));
                        }
                        break;
                    case "neutral":
                        if (dialogueLoader.neutralFinniganLabelsCopy.Count > 0)
                        {
                            string randomElement = getRandomElement(dialogueLoader.neutralFinniganLabelsCopy);
                            dialogueLoader.UpdateScenarioLable(randomElement);
                            dialogueLoader.neutralFinniganLabelsCopy.Remove(randomElement);
                        }
                        else
                        {
                            dialogueLoader.UpdateScenarioLable(getRandomElement(dialogueLoader.neutralAffectionFinniganConversationLabels));
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
        if (affectionNumber <= dialogueLoader.lowAffection)
        {
            return "low";
        }
        else if (affectionNumber >= dialogueLoader.highAffection)
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
