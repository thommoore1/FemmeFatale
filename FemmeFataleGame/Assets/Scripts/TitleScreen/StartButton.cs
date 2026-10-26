using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public bool PlayIntroCutscene;

    public DialogueLoader Loader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
