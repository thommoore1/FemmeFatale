using UnityEngine;
using UnityEngine.SceneManagement;

namespace FemmeFatale
{
    public class StartMenu : MonoBehaviour
    {
        private Scene LevelScene;
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        
        public void QuitGame()
        {
            Debug.Log("Game is exiting...");
        
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                        // Closes the actual built application (Windows, Mac, Linux, Android, etc.)
                        Application.Quit();
            #endif
        }
        
        public void LoadLevelScene()
        {
            LevelScene = SceneManager.GetSceneByName("Level");
            if (LevelScene.IsValid())
            {
                SceneManager.LoadScene(LevelScene.name);
            }
            else
            {
                Debug.LogError("Level scene not found!");
            }
        }
    }
}