using System.Collections.Generic;
using UnityEngine;

public class ElektroMap : CalcBoardMap
{
    private List<string> languages;
    private List<ElektroTile> tiles;

    public List<string> Languages
    {
        get => languages;
        private set => languages = value;
    }
    public List<ElektroTile> Tiles
    {
        get => tiles;
        private set => tiles = value;
    }

    public void Initialize(int id, string game, string mapName, string img, List<string> languages)
    {
        base.Initialize(id, game, mapName, img);
        this.languages = languages;
        tiles = new List<ElektroTile>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ElektroMapData toData()
    {
        return new ElektroMapData(this);
    }
}
