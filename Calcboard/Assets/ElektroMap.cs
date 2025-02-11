using System.Collections.Generic;
using UnityEngine;

public class ElektroMap : MonoBehaviour
{
    private List<string> languages;
    private List<TileData> tiles;

    public List<string> Languages
    {
        get => languages;
        private set => languages = value;
    }
    public List<TileData> Tiles
    {
        get => tiles;
        private set => tiles = value;
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
