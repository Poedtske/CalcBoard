using UnityEngine;
using UnityEngine.UI;

public class EditTile : MonoBehaviour
{
    public Button selectImg;
    public GameObject languages;
    public Button saveButton;
    public RawImage img;
    private ElektroTile tile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadElektroTile(ElektroTile tile)
    {
        this.tile = tile;
        img.texture = tile.ImgComponent.texture;
    }
}
