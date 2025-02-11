using UnityEngine;

public class CalcBoardMap : MonoBehaviour
{
    protected int id;
    protected string game;
    protected string mapName;
    protected string img;
    public int Id
    {
        get => id;
        private set => id = value;
    }
    public string Game
    {
        get => game;
        private set => game = value;
    }
    public string MapName
    {
        get => mapName;
        private set => mapName = value;
    }
    public string Img
    {
        get => img;
        private set => img = value;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
