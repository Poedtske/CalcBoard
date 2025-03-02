using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static Unity.VisualScripting.Icons;
using System.Linq;

[System.Serializable]
public class MusicoMapData : CalcBoardMapData<MusicoMapData,MusicoTileData>
{

    public MusicoMapData(string name, string img) : base(name, img)
    {
        this.tiles = new List<MusicoTileData>();

        for (int i = 1; i <= 24; i++)
        {
            tiles.Add(new MusicoTileData(i, ""));
        }
    }
    public MusicoMapData() { }

    public override string Game()
    {
        return Games.MUSICO;
    }

    public override string ToString()
    {
        string tileDataString = string.Join("\n", tiles.Select(tile => tile.ToString()));
        return $"MusicoMapData: ID={id}, Game={Game()}, Name={mapName}, Img={img}\nTiles:\n{tileDataString}";
    }
}
