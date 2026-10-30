using UnityEngine;
using UnityEngine.SceneManagement;

namespace FemmeFatale
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private bool PlayIntroCutscene;
        [SerializeField] private DialogueLoader Loader;
        [SerializeField] private string levelSceneName;
        [SerializeField] private string dialogueSceneName;

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadStart()
        {
            Loader.UpdateScenarioLable("IntroCutscene");
            if(PlayIntroCutscene)
            {
                LoadScene(dialogueSceneName);
            }
            else
            {
                LoadScene(levelSceneName);
            }
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
        
    }
}