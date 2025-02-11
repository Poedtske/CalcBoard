using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditTile : MonoBehaviour
{
    public Button selectImg;
    public GameObject languages; // Parent object containing input fields
    public Button saveButton;
    public RawImage img;
    private ElektroTile tile;
    public ElektroMapManager gameManager;
    private FileSelectorUI fileSelectorUI;
    public TMP_InputField inputFieldPrefab; // Prefab for input fields
    private string imagesPath;

    private List<TMP_InputField> inputFields = new List<TMP_InputField>(); // Store references to inputs

    private void Awake()
    {
        fileSelectorUI = FindAnyObjectByType<FileSelectorUI>();
    }

    private void Start()
    {
        selectImg.onClick.AddListener(() => fileSelectorUI.OpenImageFilePicker(tile));
        
    }

    private void OnEnable()
    {
        imagesPath = Path.Combine(Application.dataPath, "..", "games", gameManager.Map.Game, "maps", gameManager.Map.MapName, "images");
        tile = gameManager.FindTile(PlayerPrefs.GetInt("tileId"));
        //fileSelectorUI.TempImg = null;
        //selectImg.GetComponentInChildren<TextMeshProUGUI>().text = tile.Id.ToString();

        string imgPath = Path.Combine(imagesPath, tile.Img);
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

        // Clear previous input fields
        foreach (Transform child in languages.transform)
        {
            Destroy(child.gameObject);
        }
        inputFields.Clear(); // Clear the list

        // Add new GameObject containing both Label and Input Field
        for (int i = 0; i < gameManager.Map.Languages.Count; i++)
        {
            string languageName = gameManager.Map.Languages[i]; // e.g., "English"
            string tileValue = (i < tile.Meanings.Count) ? tile.Meanings[i] : ""; // e.g., "Chair"

            // Create Parent GameObject
            GameObject entry = new GameObject($"LanguageEntry_{i}");
            entry.transform.SetParent(languages.transform, false);
            entry.AddComponent<VerticalLayoutGroup>(); // Ensures correct layout

            // Create Label
            TextMeshProUGUI label = new GameObject("Label").AddComponent<TextMeshProUGUI>();
            label.transform.SetParent(entry.transform, false);
            label.text = languageName; // Set label text
            label.fontSize = 20;
            label.alignment = TextAlignmentOptions.Center;

            // Create Input Field
            TMP_InputField input = Instantiate(inputFieldPrefab, entry.transform);
            input.name = $"InputField_{i}"; // Name input fields by index
            input.text = tileValue; // Set input field value
            input.image.enabled = true;
            input.GetComponent<TMP_InputField>().enabled = true;

            inputFields.Add(input); // Store reference
        }
    }



    public void SaveChanges()
    {
        if (tile.Meanings == null)
        {
            Debug.LogError("tile.Meanings is null!");
            return;
        }
        
        // Update Meanings with input field values
        for (int i = 0; i < inputFields.Count; i++)
        {
            Debug.Log(tile.Meanings.Count);
            if (i < tile.Meanings.Count)
            {
                tile.Meanings[i] = inputFields[i].text;
                Debug.Log(inputFields[i].text);
            }
        }

        fileSelectorUI.SaveImg(tile);
        Debug.Log(tile.Img);

        Debug.Log("Tile language data updated successfully!");
    }
}
