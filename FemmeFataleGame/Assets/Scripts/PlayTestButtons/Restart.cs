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
            Debug.Log("Method called by:\n" + System.Environment.StackTrace);
            // Load the dialogue scene
            Debug.Log("adfsfdsadsf");
            UnityEngine.SceneManagement.SceneManager.LoadScene("PlayTest_1_Dialogue");
        }

        public static void loadMapScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("AffectionTest");
        }
    }
}
