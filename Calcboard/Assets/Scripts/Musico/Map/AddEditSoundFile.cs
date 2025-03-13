using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddEditSoundFile : MonoBehaviour
{
    public Button saveButton;
    public Button selectSoundFile;
    public TMP_InputField nameInputField;
    public MusicoMapManager gameManager;
    private SoundFileManager soundFileManager;
    private MusicoTileData tile;
    public GameObject soundFiles;
    private string tempFilePath;
    private string finalFilePath;
    public GameObject languageEntryPrefab;
    private MusicoSoundFile soundFile;
    private PathManager pathManager;





    public MusicoTileData Tile
    {
        get { return tile; }
        set { tile = value; }
    }

    private void Awake()
    {
        soundFileManager = FindAnyObjectByType<MusicoMapManager>().SoundFileManager;
    }

    private void Start()
    {
        selectSoundFile.onClick.AddListener(HandleSelectSoundFile);
        saveButton.onClick.AddListener(SaveChanges);
    }

    private void OnEnable()
    {
        Tile = gameManager.FindTile(PlayerPrefs.GetInt("tileId"));

        // Clear previous input fields
        foreach (Transform child in soundFiles.transform)
        {
            Destroy(child.gameObject);
        }
        nameInputField.text = "";

        List<string> soundFileNames = soundFileManager.GetSavedSoundFileNames();

        // Create UI elements for each sound file
        foreach (string soundFileName in soundFileNames)
        {
            // Instantiate the prefab for the sound file
            GameObject soundEntry = Instantiate(languageEntryPrefab, soundFiles.transform);
            soundEntry.name = $"SoundFile_{soundFileName}"; // Rename for debugging

            // Get reference to Label (Assuming it has one)
            TextMeshProUGUI soundLabel = soundEntry.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            soundLabel.text = soundFileName; // Display sound file name
            soundEntry.transform.Find("Delete").gameObject.SetActive(false);

            // Add button functionality to assign sound to tile when clicked
            Button button = soundEntry.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => AssignSoundToTile(soundFileName));
            }
        }
    }

    // Function to assign sound file to the tile
    private void AssignSoundToTile(string soundFileName)
    {
        Debug.Log($"Assigning sound file: {soundFileName} to tile {Tile.Id}");

        if (Tile != null)
        {
            Tile.AddExistantSoundFile(soundFileName); // Assuming Tile has a method to set the sound file
        }
    }



    private void HandleSelectSoundFile()
    {
        string nameValue = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(nameValue))
        {
            // Change border to red if empty
            nameInputField.image.color = Color.red;
            Debug.LogWarning("Name field is empty! Please enter a name before selecting a file.");
            return;
        }
        else
        {
            // Reset border color if valid
            nameInputField.image.color = Color.white;
        }

        // Open file picker for sound file selection
        string fileName = soundFileManager.OpenSoundFilePicker(nameValue); // Implement platform-specific file picker

        soundFile = new(tile.Id, nameValue, fileName);
        Debug.Log(soundFile);
    }

    public void SaveChanges()
    {
        if (soundFile == null)
        {
            Debug.LogWarning("No file selected! Please select a sound file before saving.");
            return;
        }

        soundFileManager.SaveSound(soundFile);
        Debug.Log($"File moved to final destination: {finalFilePath}");
    }
}
