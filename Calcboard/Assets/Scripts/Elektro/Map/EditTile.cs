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
    private ElektroTileData tile;
    public ElektroMapManager gameManager;
    private FileSelectorUI<ElektroMapData,ElektroTileData> fileSelectorUI;
    public TMP_InputField inputFieldPrefab; // Prefab for input fields

    

    public ElektroTileData Tile
    {
        get { return tile; }
        set { tile = value; }
    }

    private List<TMP_InputField> inputFields = new List<TMP_InputField>(); // Store references to inputs

    private void Awake()
    {
        fileSelectorUI = FindAnyObjectByType<ElektroMapManager>().fileSelectorUI;
    }

    private void Start()
    {
        selectImg.onClick.AddListener(() => fileSelectorUI.OpenImageFilePicker(tile, img));
        
    }

    private void OnEnable()
    {
        fileSelectorUI.TempImg = null;
        selectImg.GetComponentInChildren<TextMeshProUGUI>().text = tile.Id.ToString();



        Texture2D loadedTexture;
        if (tile.Img == "")
        {
            loadedTexture = fileSelectorUI.LoadFromResourceImage("temp");
        }
        else
        {
            loadedTexture = fileSelectorUI.LoadImage(tile.Img);
        }

        if (loadedTexture == null)
        {
            Debug.LogError("Failed to load texture from: " + tile.Img);
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
        for (int i = 0; i < gameManager.Map.Categories.Count; i++)
        {
            string languageName = gameManager.Map.Categories[i]; // e.g., "English"
            string tileValue = (i < tile.Words.Count) ? tile.Words[i] : ""; // e.g., "Chair"

            // Create Parent GameObject
            GameObject entry = new GameObject($"LanguageEntry_{i}");
            entry.transform.SetParent(languages.transform, false);
            entry.AddComponent<VerticalLayoutGroup>(); // Ensures correct layout

            // Set fixed width of 400
            RectTransform entryRect = entry.GetComponent<RectTransform>();
            entryRect.sizeDelta = new Vector2(400, entryRect.sizeDelta.y); // Width = 400, height unchanged
            entryRect.anchorMin = new Vector2(0.5f, 0.5f); // Centered
            entryRect.anchorMax = new Vector2(0.5f, 0.5f);
            entryRect.pivot = new Vector2(0.5f, 0.5f);

            // Create Label
            TextMeshProUGUI label = new GameObject("Label").AddComponent<TextMeshProUGUI>();
            label.transform.SetParent(entry.transform, false);
            label.text = languageName; // Set label text
            label.fontSize = 40;
            label.alignment = TextAlignmentOptions.Center;

            // Create Input Field
            TMP_InputField input = Instantiate(inputFieldPrefab, entry.transform);
            input.name = $"InputField_{i}"; // Name input fields by index
            input.text = tileValue; // Set input field value
            input.image.enabled = true;
            // Set Placeholder Text Correctly
            if (input.placeholder is TextMeshProUGUI placeholderText)
            {
                placeholderText.text = languageName;
            }
            input.GetComponent<TMP_InputField>().enabled = true;

            inputFields.Add(input); // Store reference
        }
    }



    public void SaveChanges()
    {
        if (tile.Words == null)
        {
            Debug.LogError("tile.Meanings is null!");
            return;
        }

        // Update Meanings with input field values
        for (int i = 0; i < inputFields.Count; i++)
        {
            Debug.Log(tile.Words.Count);
            if (i < tile.Words.Count)
            {
                tile.Words[i] = inputFields[i].text;
                Debug.Log(inputFields[i].text);
            }
        }

        fileSelectorUI.SaveImg(tile);
        Debug.Log(tile.Img);

        Debug.Log("Tile language data updated successfully!");
    }
}
