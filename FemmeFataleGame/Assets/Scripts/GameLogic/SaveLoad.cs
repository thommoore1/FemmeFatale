using System;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static event Action OnGameSaved;
    public static event Action OnGameLoaded; 

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
    
    private void AfterOnEnable()
    {
        LoadGameData();
    }
    public void SaveGameData()
    {
        Debug.Log("Game saved");
    }
    
    public void ResetGameData()
    {
        Debug.Log("Game data reset");
    }
    
    public void LoadGameData()
    {
        Debug.Log("Game loaded");
    }
}
