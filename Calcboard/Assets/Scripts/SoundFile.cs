using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class SoundFile<U, Y>
    where U : SoundFile<U, Y>
    where Y : CalcBoardTileData<Y>
{
    protected List<int> tileIds;
    protected string name;

    public SoundFile(Y tile, string name) 
    {
        this.tileIds = new();
        this.name = name;
        tileIds.Add(tile.Id);
    }

    public List<int> TileIds
    {
        get { return this.tileIds; }
        set { this.tileIds = value; }
    }
    public int GetTileId(int id)
    {
        return tileIds.Find(tileId => tileId==id);
    }

    public bool RemoveTileId(int id)
    {
        try
        {
            tileIds.Remove(id);
            return true;
        }catch(Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    public bool AddTileId(int id)
    {
        try
        {
            tileIds.Add(id);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }
}
