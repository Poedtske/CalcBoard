using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditTile : MonoBehaviour
{
    public Button selectImg;
    public GameObject languages;
    public Button saveButton;
    public RawImage img;
    private ElektroTile tile;
    public GameManagerElektro gameManager;
    private FileSelectorUI fileSelectorUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        fileSelectorUI = FindAnyObjectByType<FileSelectorUI>();
    }

    private void OnEnable()
    {
        tile = gameManager.FindTile(PlayerPrefs.GetInt("tileId"));
        selectImg.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = tile.TileId.ToString();

        string imgPath = Path.Combine(Application.dataPath, "..", "games", gameManager.Map.game, "maps", gameManager.Map.name, "images", tile.Img);
        Debug.Log("Image Path: " + imgPath);

        Texture2D loadedTexture = fileSelectorUI.LoadImage(imgPath);

        if (loadedTexture == null)
        {
            Debug.LogError("Failed to load texture from: " + imgPath);
        }
        else
        {
            img.texture = loadedTexture;
        }
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
