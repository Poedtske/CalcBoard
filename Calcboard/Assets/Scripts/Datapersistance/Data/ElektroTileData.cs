using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElektroTileData
{
    public int id;
    public string img;
    public List<string> meanings;

    public ElektroTileData(int id, string img, int languageCount)
    {
        this.id = id;
        this.img = img;
        this.meanings = new List<string>();

        for (int i = 0; i < languageCount; i++)
        {
            meanings.Add("");
        }
    }
    public ElektroTileData() { }
    public ElektroTileData(ElektroTile tile)
    {
        this.id = tile.Id;
        this.img = tile.Img;
        this.meanings = tile.Meanings;
    }

    public override string ToString()
    {
        return $"ElektroTileData: ID={id}, Img={img}, Meanings=[{string.Join(", ", meanings)}]";
    }
}
