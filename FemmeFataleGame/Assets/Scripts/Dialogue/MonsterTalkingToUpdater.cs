using UnityEngine;

public class MonsterTalkingToUpdater : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateMonster(string newMonsterName)
    {
        CurrentMonsterBeingTalkedTo.UpdateMonster(newMonsterName);
    }
}
