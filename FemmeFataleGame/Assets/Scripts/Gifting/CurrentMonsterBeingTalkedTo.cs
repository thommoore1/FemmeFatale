using UnityEngine;

[CreateAssetMenu(fileName = "CurrentMonsterBeingTalkedTo", menuName = "Scriptable Objects/CurrentMonsterBeingTalkedTo")]
public class CurrentMonsterBeingTalkedTo : ScriptableObject
{
    public string MonsterBeingTalkedToName;

    public void UpdateMonster(string newMonsterName)
    {
        MonsterBeingTalkedToName = newMonsterName;
    }
}
