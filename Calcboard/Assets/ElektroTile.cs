using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElektroTile : MonoBehaviour
{
    private string img;
    private List<string> languageDic;
    private int tileId;
    private Sprite defaultSprite;

    // Public properties for accessing private fields
    public int TileId
    {
        get => tileId;
        private set => tileId = value;
    }

    public List<string> LanguageDic
    {
        get => languageDic;
        private set => languageDic = value;
    }

    public string Img
    {
        get => img;
        set => img = value;
    }

    public Sprite DefaultSprite
    {
        get => defaultSprite;
        private set => defaultSprite = value;
    }


    public void Initialize(int id, string img, List<string> meanings)
    {
        tileId = id;
        languageDic = meanings;
        this.img = img;
    }

    public override string ToString()
    {
        // Safely handle null img or sprite
        string spriteName = img != null && img != null ? img : "None";
        return $"Image: {spriteName}\nID: {tileId}";
    }

    
}
