using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElektroTileData : CalcBoardTileData<ElektroTileData>, IHasImg
{
    private string img;
    private List<string> words;

    public string Img
    {
        get { return img; }
        set { img = value; }
    }

    public List<string> Words
    {
        get { return words; }
        set { words = value; }
    }

    public ElektroTileData(int id, string img, int categoriesCount) : base(id)
    {
        this.img = img;
        this.words = new List<string>();

        for (int i = 0; i < categoriesCount; i++)
        {
            words.Add("");
        }
    }
    public ElektroTileData() { }

    public override string ToString()
    {
        return $"ElektroTileData: ID={id}, Img={img}, SoundFiles=[{string.Join(", ", words)}]";
    }

    public string GetImg()
    {
        return img ;
    }
}
