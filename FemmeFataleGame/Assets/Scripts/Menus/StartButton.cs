using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public bool PlayIntroCutscene;

    public DialogueLoader Loader;

    public void PressStart()
    {
        if (PlayIntroCutscene)
        {
            Loader.UpdateScenarioLable("IntroCutscene");
            SceneManager.LoadScene("PlayTest_1_Dialogue");
        }
        else
        {
            SceneManager.LoadScene("AffectionTest");
        }
    }
}
