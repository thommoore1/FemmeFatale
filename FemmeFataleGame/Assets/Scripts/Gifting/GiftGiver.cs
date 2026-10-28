using UnityEngine;

public class GiftGiver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiftGiveTo(string monsterName, Gift gift)
    {
        switch (monsterName)
        {
            case "Eyeris":
                if (gift.likedByEyeris == 0)
                {
                    print("Eyeris hates the gift! Decrease affection");
                }
                else if (gift.likedByEyeris == 1)
                {
                    print("Eyeris is neutral about the gift. No change in affection");
                }
                else if (gift.likedByEyeris == 2)
                {
                    print("Eyeris loves the gift! Increase affection");
                }
                else
                {
                    print("The likedByEyeris value is not set up properly. Value of " + gift.likedByEyeris + " is not supported.");
                }

                break;
            case "Bramble":
                if (gift.likedByBramble == 0)
                {
                    print("Bramble hates the gift! Decrease affection");
                }
                else if (gift.likedByBramble == 1)
                {
                    print("Bramble is neutral about the gift. No change in affection");
                }
                else if (gift.likedByBramble == 2)
                {
                    print("Bramble loves the gift! Increase affection");
                }
                else
                {
                    print("The likedByBramble value is not set up properly. Value of " + gift.likedByBramble + " is not supported.");
                }

                break;
            case "Finnigan":
                if (gift.likedByFinnigan == 0)
                {
                    print("Finnigan hates the gift! Decrease affection");
                }
                else if (gift.likedByFinnigan == 1)
                {
                    print("Finnigan is neutral about the gift. No change in affection");
                }
                else if (gift.likedByFinnigan == 2)
                {
                    print("Finnigan loves the gift! Increase affection");
                }
                else
                {
                    print("The likedByFinnigan value is not set up properly. Value of " + gift.likedByFinnigan + " is not supported.");
                }

                break;
            default:
                Debug.LogError("Monster name " + monsterName + " is not accounted for.");
                break;
        }
    }
}
