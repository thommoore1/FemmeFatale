using System;
using UnityEngine;
using FemmeFatale.Save;

public class Item : SaveableTransform
{
    public string itemName;
    public GameObject uiPrefab; // the UI version that goes into the inventory
    [HideInInspector] public bool collected;
}