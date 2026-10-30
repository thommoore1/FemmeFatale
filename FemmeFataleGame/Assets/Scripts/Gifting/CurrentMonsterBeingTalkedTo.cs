using UnityEngine;
public static class CurrentMonsterBeingTalkedTo
{
    public static string MonsterBeingTalkedToName = "";

    public static void UpdateMonster(string newMonsterName)
    {
        Debug.Log("Updated monster talking to " + newMonsterName);
        MonsterBeingTalkedToName = newMonsterName;
    }
}
