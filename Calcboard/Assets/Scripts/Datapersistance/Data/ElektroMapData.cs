using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Icons;

[System.Serializable]
public class ElektroMapData
{
    public int id;
    public string game;
    public string name;
    public List<string> languages;
    public List<ElektroTileData> tiles;
    public string img;

    public ElektroMapData(string game, string name, List<string> languages)
    {
        this.game = game;
        this.name = name;
        this.languages = languages;
        this.tiles = new List<ElektroTileData>();

        for (int i = 1; i <= 24; i++)
        {
            tiles.Add(new ElektroTileData(i, "temp.jpg", languages.Count));
        }

        this.img = null;
    }

    public ElektroMapData() { }

    public ElektroMapData(ElektroMap map)
    {
        this.game = map.Game;
        this.name = map.MapName;
        this.languages = map.Languages;
        this.tiles = new List<ElektroTileData>();

        foreach (var tile in map.Tiles)
        {
            this.tiles.Add(new(tile));
        }

        //not implemented
        this.img = null;
    }

    public ElektroMap ToComponent()
    {
        ElektroMap component = new ElektroMap();
        component.Initialize(id, game, name, img);
        for (int i = 0; i < this.tiles.Count; i++)
        {
            component.Tiles[i].Initialize(tiles[i].id, tiles[i].img, languages.Count);
        }
        return component;
    }

    public override string ToString()
    {
        string tileDataString = "";
        foreach (var tile in tiles)
        {
            tileDataString += tile.ToString() + "\n";
        }
        return $"ElektroMapData: ID={id}, Game={game}, Name={name}, Languages=[{string.Join(", ", languages)}], Img={img}\nTiles:\n{tileDataString}";
    }
}
