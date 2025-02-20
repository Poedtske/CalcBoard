using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Icons;

[System.Serializable]
public class ElektroMapData : CalcBoardMapData<ElektroMapData,ElektroTileData>
{
    private List<string> categories;

    public List<string> Categories
    {
        get { return categories; }
        set { categories = value; }
    }

    public ElektroMapData(string game, string name, string img, List<string> languages) : base(game, name,img)
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
        return $"ElektroMapData: ID={id}, Game={game}, Name={mapName}, Languages=[{string.Join(", ", categories)}], Img={img}\nTiles:\n{tileDataString}";
    }
}
