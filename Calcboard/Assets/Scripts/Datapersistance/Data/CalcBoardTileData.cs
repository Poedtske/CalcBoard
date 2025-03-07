using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CalcBoardTileData<T> where T : CalcBoardTileData<T> 
{
    protected int id;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public CalcBoardTileData(int id)
    {
        this.id = id;
    }
    public CalcBoardTileData() { }
    

    public override string ToString()
    {
        return $"ElektroTileData: ID={id}";
    }
}
