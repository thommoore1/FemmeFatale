using UnityEngine;

namespace FemmeFatale
{
    
    public class Restart : MonoBehaviour
    {
        public static void RestartGame()
        {
            // Reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        
        public static void loadDialogueScene()
        {
            // Load the dialogue scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("PlayTest_1_Dialogue");
        }

        public static void loadMapScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PlayTest_1");
        }
    }
}
