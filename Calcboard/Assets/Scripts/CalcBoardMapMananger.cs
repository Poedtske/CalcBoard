using UnityEngine;

public abstract class CalcBoardMapMananger : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Save();
    public abstract void ReloadTile();
    public abstract void LoadTiles();
    public abstract void SetIdAction(int id);
}
