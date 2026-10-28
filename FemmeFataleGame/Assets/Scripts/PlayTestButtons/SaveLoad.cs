using UnityEngine;
using FemmeFatale.Save;

namespace FemmeFatale
{
    public class SaveLoad : MonoBehaviour
    {
        public void SaveGame()
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.SaveButton(0);
            else
                Debug.LogError("SaveManager.Instance is null!");
        }

        public void LoadGame()
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.Load(0);
            else
                Debug.LogError("SaveManager.Instance is null!");
        }

        public void DeleteSave()
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.ClearSlot(0);
            else
                Debug.LogError("SaveManager.Instance is null!");
        }

        public void SaveTemp()
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.SaveButton(4);
            else
                Debug.LogError("SaveManager.Instance is null!");
        }

        public void LoadTemp()
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.Load(4);
            else
                Debug.LogError("SaveManager.Instance is null!");
        }

        public void DeleteTemp()
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.ClearSlot(4);
            else
                Debug.LogError("SaveManager.Instance is null!");
        }
    }
}