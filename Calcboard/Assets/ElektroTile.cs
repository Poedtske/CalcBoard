using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElektroTile : MonoBehaviour
{
    private Sprite imgComponent;
    private List<string> languageDic;
    private int tileNr;
    private Sprite defaultSprite;

    // Public properties for accessing private fields
    public int TileNr
    {
        get => tileNr;
        private set => tileNr = value;
    }

    public List<string> LanguageDic
    {
        get => languageDic;
        private set => languageDic = value;
    }

    public Sprite ImgComponent
    {
        get => imgComponent;
        private set => imgComponent = value;
    }

    public Sprite DefaultSprite
    {
        get => defaultSprite;
        private set => defaultSprite = value;
    }

    void Awake()
    {
        // Load default sprite safely
        defaultSprite = Resources.Load<Sprite>("Square");
        if (defaultSprite == null)
        {
            Debug.LogError("Default sprite 'Square' not found in Resources!");
        }

        imgComponent = defaultSprite;
    }

    public void Initialize(int id, string imgPath, List<string> meanings)
    {
        tileNr = id;
        languageDic = meanings;

        // Load the sprite
        Sprite loadedSprite = Resources.Load<Sprite>(imgPath);
        if (loadedSprite == null)
        {
            Debug.LogWarning($"Sprite '{imgPath}' not found! Using default.");
            loadedSprite = defaultSprite;
        }

        imgComponent = loadedSprite;
    }

    public override string ToString()
    {
        // Safely handle null imgComponent or sprite
        string spriteName = imgComponent != null && imgComponent != null ? imgComponent.name : "None";
        return $"Image: {spriteName}\nID: {tileNr}";
    }
}
