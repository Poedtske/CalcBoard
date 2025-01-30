using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tileTest : MonoBehaviour
{

    private Sprite imgComponent;
    //private SpriteRenderer rendere;
    private Image imgImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        // Load the texture from Resources
        Texture2D temp = Resources.Load<Texture2D>("Obama");

        // Create a Sprite from the texture
        if (temp != null)
        {
            Sprite sprite = Sprite.Create(temp, new Rect(0, 0, 150, temp.height), new Vector2(0.5f, 0.5f));

            // Add a SpriteRenderer component if not already present
           imgImage = gameObject.AddComponent<Image>();

            // Assign the created sprite
            imgImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load texture: Square");
        }
    }


    public void Initialize()
    {
        imgImage.sprite = imgComponent;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
