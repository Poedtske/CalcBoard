using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class SoundFile
{
    protected List<int> tileIds;
    protected string name;
    protected string fileName;

    public string FileName
    {
        get { return fileName; }
        set { fileName = value; }
    }

    public SoundFile(int id, string name, string FileName) 
    {
        this.tileIds = new();
        this.name = name;
        this.FileName = FileName;
        tileIds.Add(id);
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

    public override string ToString()
    {
        return $"SoundFile: Name='{name}', FileName='{fileName}', TileIds=[{string.Join(", ", tileIds)}]";
    }

}
