using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB; // StandaloneFileBrowser for file selection

public class FileSelectorUI : MonoBehaviour
{
    public Button selectFileButton;
    public RawImage imagePreview;

    private string selectedFilePath;
    private string saveFolderPath = "SavedImages"; // Folder name for saved images

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectFileButton.onClick.AddListener(OpenFilePicker);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenFilePicker()
    {
        // Open file dialog for PNG/JPG
        var paths = StandaloneFileBrowser.OpenFilePanel("Select Image", ".png,.jpg,.jpeg", "", false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            selectedFilePath = paths[0];
            SaveAndLoadImage(selectedFilePath);
        }
    }

    void SaveAndLoadImage(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);

        // Ensure save directory exists
        string directoryPath = Path.Combine(Application.persistentDataPath, saveFolderPath);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        // Save file in Unity's persistent data path
        string fileName = Path.GetFileName(filePath);
        string savePath = Path.Combine(directoryPath, fileName);
        File.WriteAllBytes(savePath, fileData);

        Debug.Log("Image saved at: " + savePath);

        // Load and display the image
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        imagePreview.texture = texture;
        imagePreview.SetNativeSize();
    }
}
