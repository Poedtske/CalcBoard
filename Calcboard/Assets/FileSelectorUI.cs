using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB; // StandaloneFileBrowser for file selection

public class FileSelectorUI : MonoBehaviour
{
    public Button selectFileButton;
    public RawImage imagePreview;

    private string selectedFilePath;
    private string saveFolderPath = "games/elektro/maps/"; // Folder name for saved images

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
        var paths = StandaloneFileBrowser.OpenFilePanel("Select Image", "png,jpg,jpeg", "", false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            selectedFilePath = paths[0];
            SaveAndLoadImage(selectedFilePath);
        }
    }

    void SaveAndLoadImage(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);

        // Save the file inside the Unity project root
        string projectRoot = Application.dataPath; // "Assets" folder path
        string saveDirectory = Path.Combine(projectRoot, "..", saveFolderPath); // Move to project root

        if (!Directory.Exists(saveDirectory))
            Directory.CreateDirectory(saveDirectory);

        // Save file in Elektro/maps
        string fileName = Path.GetFileName(filePath);
        string savePath = Path.Combine(saveDirectory, fileName);
        File.WriteAllBytes(savePath, fileData);
        
        Debug.Log("Image saved at: " + savePath);

        LoadImage(fileData);
    }

    public void LoadImage(byte[] fileData)
    {
        // Load and display the image
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        imagePreview.texture = texture;
        imagePreview.SetNativeSize();
    }

    public Texture2D LoadImage(string imgPath)
    {
        Texture2D originalTexture = LoadTextureFromFile(imgPath);

        if (originalTexture == null)
        {
            Debug.LogError("Failed to load texture from: " + imgPath);
            return null;
        }

        return ResizeTexture(originalTexture, 500, 500);
    }

    private Texture2D ResizeTexture(Texture2D originalTexture, int targetWidth, int targetHeight)
    {
        RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 24);
        RenderTexture.active = rt;

        Graphics.Blit(originalTexture, rt);

        Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight);
        resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        resizedTexture.Apply();

        RenderTexture.active = null;
        rt.Release();

        return resizedTexture;
    }



    Texture2D LoadTextureFromFile(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData)) // Automatically resizes the texture
        {
            return texture;
        }
        return null;
    }
}
