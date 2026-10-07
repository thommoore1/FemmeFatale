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
    }
}
