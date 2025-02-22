using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MusicoSoundFile
{
    private List<MusicoTileData> tiles;
    private string name;

    MusicoSoundFile(MusicoTileData tile, string name)
    {
        this.tiles = new();
        this.name = name;
        tiles.Add(tile);
    }

    public List<MusicoTileData> Tiles
    {
        get { return this.tiles; }
        set { this.tiles = value; }
    }
    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }
}
