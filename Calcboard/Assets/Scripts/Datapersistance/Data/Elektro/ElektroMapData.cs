using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Icons;

[System.Serializable]
public class ElektroMapData : CalcBoardMapData<ElektroMapData, ElektroTileData>
{
    private List<string> categories;
    private new const string game = Games.ELEKTRO;

    public override string Game => game;

    public List<string> Categories
    {
        get { return categories; }
        set { categories = value; }
    }

    public ElektroMapData(string name, string img, List<string> languages) : base(name,img)
    {
        this.categories = languages;
        this.tiles = new List<ElektroTileData>();

        for (int i = 1; i <= 24; i++)
        {
            tiles.Add(new ElektroTileData(i, "", languages.Count));
        }
    }

    public ElektroMapData() { }

    public override string ToString()
    {
        string tileDataString = "";
        foreach (var tile in tiles)
        {
            tileDataString += tile.ToString() + "\n";
        }
        return $"ElektroMapData: ID={id}, Game={Game}, Name={mapName}, Categories=[{string.Join(", ", categories)}], Img={img}\nTiles:\n{tileDataString}";
    }
}
