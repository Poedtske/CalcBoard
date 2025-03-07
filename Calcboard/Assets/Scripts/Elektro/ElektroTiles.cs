using UnityEngine;

public class ElektroTiles : MonoBehaviour
{
    public ElektroMapManager elektroMapManager;
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
            elektroMapManager.ReloadTile();
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
