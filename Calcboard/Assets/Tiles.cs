using UnityEngine;

public class Tiles : MonoBehaviour
{
    public GameManagerElektro gameManagerElektro;
    private bool startup = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (startup)
        {
            startup = false;
        }
        else
        {
            gameManagerElektro.ReloadImages();
        }
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
