using UnityEngine;
using UnityEngine.SceneManagement; 

public class gameSelector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Function to load spefiek games
    public void OpenScene()
    {
        SceneManager.LoadScene("Electro");
    }


    
}
