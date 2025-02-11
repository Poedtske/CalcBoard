using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElektroTile : CalcBoardTile
{
    private string img;
    private List<string> languageList;
    

    // Public properties for accessing private fields
    

    public List<string> LanguageDic
    {
        get => languageList;
        private set => languageList = value;
    }

    public string Img
    {
        get => img;
        set => img = value;
    }

    public void Initialize(int id, string img, List<string> meanings)
    {
        this.id = id;
        languageList = meanings;
        this.img = img;
    }

    public override string ToString()
    {
        // Safely handle null img or sprite
        string spriteName = img != null && img != null ? img : "None";
        return $"Image: {spriteName}\nID: {id}";
    }

    
}
