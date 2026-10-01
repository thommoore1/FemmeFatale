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
        Button.SetActive(false);
    }

    public void ShowButton()
    {
        print("Shown");
        Button.SetActive(true);
    }
}
