using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElektroTile : CalcBoardTile, ITileImg
{
    private string img;
    private List<string> meanings;
    

    // Public properties for accessing private fields
    

    public List<string> Meanings
    {
        get => meanings;
        private set => meanings = value;
    }

    public string Img
    {
        get => img;
        set => img = value;
    }

    public string getImg()
    {
        return img;
    }

    public void Initialize(int id, string img, int lanCount)
    {
        this.id = id;
        
        
        this.meanings =new();
        for (int i = 0; i<lanCount;i++)
        {
            meanings.Add("");
        }
        this.img = img;
    }

    public void setImg(string img)
    {
        this.img = img;
    }

    public override string ToString()
    {
        // Safely handle null img or sprite
        string spriteName = img != null && img != null ? img : "None";
        return $"Image: {spriteName}\nID: {id}";
    }

    internal void Load(ElektroTileData elektroTileData)
    {
        this.id=elektroTileData.id;
        this.img=elektroTileData.img;
        this.meanings = elektroTileData.meanings;
    }
}
