using UnityEngine;
using Utage;

public class TestButtonFunctions : MonoBehaviour
{
    public GameObject Button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void HideButton()
    {
        print("Hidden");
        var stackTrace = new System.Diagnostics.StackTrace();
        UnityEngine.Debug.Log("Called by: " + stackTrace.GetFrame(1).GetMethod().Name);
        Button.SetActive(false);
    }

    public void ShowButton()
    {
        print("Shown");
        Button.SetActive(true);
    }
}
