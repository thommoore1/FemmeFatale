using FemmeFatale;
using UnityEngine;

public class GiftGiver : MonoBehaviour
{
    public InventoryController inventory;

    public CurrentMonsterBeingTalkedTo Monster;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        
        switch (Monster.MonsterBeingTalkedToName)
        {
            case "Eyeris":
                if (inventory.GetSelectedItem().GetComponent<Gift>().likedByEyeris == 0)
                {
                    print("Eyeris hates the gift! Decrease affection");
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByEyeris == 1)
                {
                    print("Eyeris is neutral about the gift. No change in affection");
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByEyeris == 2)
                {
                    print("Eyeris loves the gift! Increase affection");
                }
                else
                {
                    print("The likedByEyeris value is not set up properly. Value of " + inventory.GetSelectedItem().GetComponent<Gift>().likedByEyeris + " is not supported.");
                }

                break;
            case "Bramble":
                if (inventory.GetSelectedItem().GetComponent<Gift>().likedByBramble == 0)
                {
                    print("Bramble hates the gift! Decrease affection");
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByBramble == 1)
                {
                    print("Bramble is neutral about the gift. No change in affection");
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByBramble == 2)
                {
                    print("Bramble loves the gift! Increase affection");
                }
                else
                {
                    print("The likedByBramble value is not set up properly. Value of " + inventory.GetSelectedItem().GetComponent<Gift>().likedByBramble + " is not supported.");
                }

                break;
            case "Finnigan":
                if (inventory.GetSelectedItem().GetComponent<Gift>().likedByFinnigan == 0)
                {
                    print("Finnigan hates the gift! Decrease affection");
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByFinnigan == 1)
                {
                    print("Finnigan is neutral about the gift. No change in affection");
                }
                else if (inventory.GetSelectedItem().GetComponent<Gift>().likedByFinnigan == 2)
                {
                    print("Finnigan loves the gift! Increase affection");
                }
                else
                {
                    print("The likedByFinnigan value is not set up properly. Value of " + inventory.GetSelectedItem().GetComponent<Gift>().likedByFinnigan + " is not supported.");
                }

                break;
            default:
                Debug.LogError("Monster name " + Monster.MonsterBeingTalkedToName + " is not accounted for.");
                break;
        }
        Destroy(inventory.GetSelectedItem());
    }
}
