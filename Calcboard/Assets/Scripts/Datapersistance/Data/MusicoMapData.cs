using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static Unity.VisualScripting.Icons;

[System.Serializable]
public class MusicoMapData : CalcBoardMapData<MusicoMapData,MusicoTileData>
{
    private List<string> categories;

    private new const string game = Games.MUSICO;
    public List<string> Categories
    {
        get { return categories; }
        set { categories = value; }
    }

    public MusicoMapData(string name, string img, List<string> categories) : base(name, img)
    {
        this.categories = categories;
        this.tiles = new List<MusicoTileData>();

        for (int i = 1; i <= 24; i++)
        {
            tiles.Add(new MusicoTileData(i, ""));
        }
    }
    public MusicoMapData() { }

    public override string Game()
    {
        return game;
    }
}
