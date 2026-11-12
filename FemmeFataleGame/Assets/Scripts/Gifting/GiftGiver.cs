using System.Collections.Generic;
using FemmeFatale;
using UnityEngine;

public class GiftGiver : MonoBehaviour
{
    public InventoryController inventory;
    public DialogueLoader loader;
    public string mapSceneToLoad;
    public string dialogueSceneToLoad;

    [SerializeField] int negAmount = -100;
    [SerializeField] int posAmount = 10;
    [SerializeField] int neutralAmount = 0;
    

    public void GiftGiveTo()
    {
        if (inventory.GetSelectedItem() == null)
        {
            Debug.Log("You have no selected item!");
            return;
        }
        
        else if (inventory.GetSelectedItem().GetComponent<Gift>() == null)
        {
            Debug.Log("This is not a giftable item!");
            return;
        }
        
        switch (CurrentMonsterBeingTalkedTo.MonsterBeingTalkedToName)
        {
            case "Eyeris":
                if (inventory.GetSelectedItem().GetComponent<Gift>().likedByEyeris == 0)
                {
                    print("Eyeris hates the gift! Decrease affection");
                    AffectionManager.Instance.Add("Eyeris", negAmount);
                    loader.UpdateScenarioLable(getRandomElement(loader.negativeGiftReactionEyerisConversationLabels));
                    UnityEngine.SceneManagement.SceneManager.LoadScene(dialogueSceneToLoad);
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByEyeris == 1)
                {
                    print("Eyeris is neutral about the gift. No change in affection");
                    AffectionManager.Instance.Add("Eyeris", neutralAmount);
                    loader.UpdateScenarioLable(getRandomElement(loader.neutralGiftReactionEyerisConversationLabels));
                    UnityEngine.SceneManagement.SceneManager.LoadScene(dialogueSceneToLoad);
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByEyeris == 2)
                {
                    print("Eyeris loves the gift! Increase affection");
                    AffectionManager.Instance.Add("Eyeris", posAmount);
                    loader.UpdateScenarioLable(getRandomElement(loader.positiveGiftReactionEyerisConversationLabels));
                    UnityEngine.SceneManagement.SceneManager.LoadScene(dialogueSceneToLoad);
                }
                else
                {
                    print("The likedByEyeris value is not set up properly. Value of " + inventory.GetSelectedItem().GetComponent<Gift>().likedByEyeris + " is not supported.");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(mapSceneToLoad);
                }

                break;
            case "Bramble":
                if (inventory.GetSelectedItem().GetComponent<Gift>().likedByBramble == 0)
                {
                    AffectionManager.Instance.Add("Bramble", negAmount);
                    loader.UpdateScenarioLable(getRandomElement(loader.negativeGiftReactionBrambleConversationLabels));
                    print("Bramble hates the gift! Decrease affection");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(dialogueSceneToLoad);
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByBramble == 1)
                {
                    AffectionManager.Instance.Add("Bramble", neutralAmount);
                    loader.UpdateScenarioLable(getRandomElement(loader.neutralGiftReactionBrambleConversationLabels));
                    print("Bramble is neutral about the gift. No change in affection");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(dialogueSceneToLoad);
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByBramble == 2)
                {
                    AffectionManager.Instance.Add("Bramble", posAmount);
                    loader.UpdateScenarioLable(getRandomElement(loader.positiveGiftReactionBrambleConversationLabels));
                    print("Bramble loves the gift! Increase affection");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(dialogueSceneToLoad);
                }
                else
                {
                    print("The likedByBramble value is not set up properly. Value of " + inventory.GetSelectedItem().GetComponent<Gift>().likedByBramble + " is not supported.");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(mapSceneToLoad);
                }

                break;
            case "Finnigan":
                if (inventory.GetSelectedItem().GetComponent<Gift>().likedByFinnigan == 0)
                {
                    AffectionManager.Instance.Add("Finnigan", negAmount);
                    loader.UpdateScenarioLable(getRandomElement(loader.negativeGiftReactionFinniganConversationLabels));
                    print("Finnigan hates the gift! Decrease affection");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(dialogueSceneToLoad);
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByFinnigan == 1)
                {
                    AffectionManager.Instance.Add("Finnigan", neutralAmount);
                    loader.UpdateScenarioLable(getRandomElement(loader.neutralGiftReactionFinniganConversationLabels));
                    print("Finnigan is neutral about the gift. No change in affection");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(dialogueSceneToLoad);
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByFinnigan == 2)
                {
                    AffectionManager.Instance.Add("Finnigan", posAmount);
                    loader.UpdateScenarioLable(getRandomElement(loader.positiveGiftReactionFinniganConversationLabels));
                    print("Finnigan loves the gift! Increase affection");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(dialogueSceneToLoad);
                }
                else
                {
                    print("The likedByFinnigan value is not set up properly. Value of " + inventory.GetSelectedItem().GetComponent<Gift>().likedByFinnigan + " is not supported.");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(mapSceneToLoad);
                }

                break;
            default:
                Debug.LogError("Monster name " + CurrentMonsterBeingTalkedTo.MonsterBeingTalkedToName + " is not accounted for.");
                UnityEngine.SceneManagement.SceneManager.LoadScene(mapSceneToLoad);
                break;
        }
        inventory.DeleteSelected();
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
