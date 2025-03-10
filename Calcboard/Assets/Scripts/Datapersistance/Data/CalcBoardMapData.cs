using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CalcBoardMapData<T, Y>
    where T : CalcBoardMapData<T, Y>
    where Y : CalcBoardTileData<Y>
{
    protected int id;
    protected string mapName;
    protected List<Y> tiles; // Now uses the specific tile type
    protected string img;
    protected string game="";

    public abstract string Game {  get; }
    public int Id
    {
        get => id;
        set => id = value;
    }

   
    public string MapName
    {
        get => mapName;
        set => mapName = value;
    }

    public List<Y> Tiles
    {
        get => tiles;
        set => tiles = value;
    }

    public CalcBoardMapData(string name, string img)
    {
        this.mapName = name;
        this.tiles = new List<Y>(); // Now correctly initializes the specific tile type
        this.img = img;
    }

    public CalcBoardMapData()
    {
        tiles = new List<Y>();
    }

    public override string ToString()
    {
        string tileDataString = "";
        foreach (var tile in tiles)
        {
            tileDataString += tile.ToString() + "\n";
        }
        return $"CalcBoardMapData: ID={id}, Game={Game}, Name={mapName}, Img={img}\nTiles:\n{tileDataString}";
    }
}
