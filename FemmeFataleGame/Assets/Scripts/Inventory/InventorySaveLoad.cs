using System;
using UnityEngine;

namespace FemmeFatale.Save
{
    public class InventorySaveLoad : MonoBehaviour
    {
        private void OnEnable()
        {
            
            SaveManager.Instance.LoadInventoryFromSlot();
        }
        
        
    }
}