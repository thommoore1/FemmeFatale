using UnityEngine;
using FemmeFatale.Save;

namespace FemmeFatale
{
    public class MenuActions : MonoBehaviour
    {
        public void Save(int slot = 0)
        {
            if (SaveManager.Instance != null) SaveManager.Instance.Save(slot);
            else Debug.LogWarning("[Save] No SaveManager exists. Make sure one is in your first scene.");
        }

        public void Load(int slot = 0)
        {
            if (SaveManager.Instance != null) SaveManager.Instance.Load(slot);
            else Debug.LogWarning("[Save] No SaveManager exists. Make sure one is in your first scene.");
        }

        public void Delete(int slot = 0)
        {
            if (SaveManager.Instance != null) SaveManager.Instance.ClearSlot(slot);
            else Debug.LogWarning("[Save] No SaveManager exists. Make sure one is in your first scene.");
        }
    }
}