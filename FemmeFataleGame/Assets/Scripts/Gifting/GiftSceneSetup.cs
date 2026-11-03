using UnityEngine;
using UnityEngine.UI;
using UtageExtensions;

public class GiftSceneSetup : MonoBehaviour
{

    public GameObject EyerisSprite;

    public GameObject BrambleSprite;

    public GameObject FinniganSprite;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (CurrentMonsterBeingTalkedTo.MonsterBeingTalkedToName.Equals("Eyeris"))
        {
            EyerisSprite.SetActive(true);
            BrambleSprite.SetActive(false);
            FinniganSprite.SetActive(false);
        }
        else if(CurrentMonsterBeingTalkedTo.MonsterBeingTalkedToName.Equals("Bramble"))
        {
            EyerisSprite.SetActive(false);
            BrambleSprite.SetActive(true);
            FinniganSprite.SetActive(false);
        }
        else if(CurrentMonsterBeingTalkedTo.MonsterBeingTalkedToName.Equals("Finnigan"))
        {
            EyerisSprite.SetActive(false);
            BrambleSprite.SetActive(false);
            FinniganSprite.SetActive(true);
        }
        else
        {
            Debug.Log("No recognized monster is being talked to. Current name is: " + CurrentMonsterBeingTalkedTo.MonsterBeingTalkedToName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
