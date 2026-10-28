using UnityEngine;

namespace FemmeFatale
{
    
    public class Restart : MonoBehaviour
    {
        private string sceneName;
        
        public static void RestartGame()
        {
            // Reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        
        public static void loadDialogueScene()
        {
            print("loaded dialogue scene");
            UnityEngine.SceneManagement.SceneManager.LoadScene("PlayTest_2_Dialogue");
        }

        public static void loadMapScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PlayTest_2Charlotte");
        }
    }
}
