using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddEditSoundFile : MonoBehaviour
{
    public Button saveButton;
    public Button selectSoundFile;
    public TMP_InputField nameInputField;
    public MusicoMapManager gameManager;
    private FileManager<MusicoMapData, MusicoTileData> fileManager;
    private MusicoTileData tile;
    private string tempFilePath;
    private string finalFilePath;

    public MusicoTileData Tile
    {
        get { return tile; }
        set { tile = value; }
    }

    private void Awake()
    {
        fileManager = FindAnyObjectByType<MusicoMapManager>().FileManager;
    }

    private void Start()
    {
        selectSoundFile.onClick.AddListener(HandleSelectSoundFile);
        saveButton.onClick.AddListener(SaveChanges);
    }

    private void OnEnable()
    {
        Tile = gameManager.FindTile(PlayerPrefs.GetInt("tileId"));
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
        string filePath = OpenFileExplorer(); // Implement platform-specific file picker

        if (!string.IsNullOrEmpty(filePath))
        {
            // Create a new sound file entry
            MusicoSoundFile soundFile = new MusicoSoundFile(tile, nameValue);

            // Move to temporary folder
            tempFilePath = Path.Combine(Application.persistentDataPath, "Temp", Path.GetFileName(filePath));
            File.Copy(filePath, tempFilePath, true);

            Debug.Log($"File copied to temp: {tempFilePath}");
        }
    }

    public void SaveChanges()
    {
        if (string.IsNullOrEmpty(tempFilePath))
        {
            Debug.LogWarning("No file selected! Please select a sound file before saving.");
            return;
        }

        string soundFilesFolder = Path.Combine(Application.persistentDataPath, "SoundFiles");

        if (!Directory.Exists(soundFilesFolder))
        {
            Directory.CreateDirectory(soundFilesFolder);
        }

        finalFilePath = Path.Combine(soundFilesFolder, Path.GetFileName(tempFilePath));

        File.Move(tempFilePath, finalFilePath);
        Debug.Log($"File moved to final destination: {finalFilePath}");
    }

    private string OpenFileExplorer()
    {
        // This function should be implemented with native file selection
        // Unity does not have a built-in file explorer, so you'd need
        // something like Native File Picker for mobile or OpenFileDialog for Windows.
        Debug.Log("Opening file explorer... (Implement this based on platform)");
        return null; // Replace with actual path from file picker
    }
}
