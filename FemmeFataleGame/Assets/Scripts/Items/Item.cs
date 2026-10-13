using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public GameObject uiPrefab; // the UI version that goes into the inventory
    [HideInInspector] public bool collected;

    private void OnEnable()
    {
        SaveLoad.OnGameLoaded += LoadData;
        SaveLoad.OnGameSaved += SaveData;
        LoadData();
    }

    private void OnDestroy()
    {
        SaveLoad.OnGameLoaded -= LoadData;
        SaveLoad.OnGameSaved -= SaveData;
    }

    private void LoadData()
    {
        throw new NotImplementedException();
    }
    
    private void SaveData()
    {
        throw new NotImplementedException();
    }
}