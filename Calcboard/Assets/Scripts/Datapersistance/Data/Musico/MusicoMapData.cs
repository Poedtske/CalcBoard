using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class MusicoMapData : CalcBoardMapData<MusicoMapData, MusicoTileData>
{
    private new const string game = Games.MUSICO;

    public override string Game => game;

    public MusicoMapData(string name, string img) : base(name, img)
    {
        this.tiles = new List<MusicoTileData>();

        for (int i = 1; i <= 24; i++)
        {
            tiles.Add(new MusicoTileData(i, ""));
        }
    }
    public MusicoMapData() { }

    public override string ToString()
    {
        string tileDataString = string.Join("\n", tiles.Select(tile => tile.ToString()));
        return $"MusicoMapData: ID={id}, Game={Game}, Name={mapName}, Img={img}\nTiles:\n{tileDataString}";
    }
}
