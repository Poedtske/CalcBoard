using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB; // StandaloneFileBrowser for file selection

public class FileSelectorUI : MonoBehaviour
{
    public Button selectFileButton;
    public RawImage imagePreview;
    public GameManagerElektro gameManagerElektro;

    private string selectedFilePath;
    private string saveFolderPath; // Folder name for saved images
    private string tempImg;
    private string saveDirectory;

    void Start()
    {
        saveFolderPath = gameManagerElektro.gamePath;
        saveDirectory = Path.Combine(Application.dataPath, "..", saveFolderPath, "images"); // Move to project root
    }

    public void OpenImageFilePicker(ElektroTile tile)
    {
        // Open file dialog for PNG/JPG/JPEG
        var paths = StandaloneFileBrowser.OpenFilePanel("Select Image", "png,jpg,jpeg", "", false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            selectedFilePath = paths[0];
            SaveAndLoadNewImageInTemp(selectedFilePath, tile);
        }
    }

    void SaveAndLoadNewImageInTemp(string filePath, ElektroTile tile)
    {
        byte[] fileData = File.ReadAllBytes(filePath);

        // Define temp save directory inside Unity project root
        string tempDirectory = Path.Combine(Application.dataPath, "..", "TempImages");

        if (!Directory.Exists(tempDirectory))
            Directory.CreateDirectory(tempDirectory);

        // Get file extension
        string fileExtension = Path.GetExtension(filePath);
        string tempFileName = "temp" + tile.TileId + fileExtension;
        string tempFilePath = Path.Combine(tempDirectory, tempFileName);

        // **Remove existing temp files with the same TileId (any extension)**
        string[] existingTempFiles = Directory.GetFiles(tempDirectory, "temp" + tile.TileId + ".*");
        foreach (string tempFile in existingTempFiles)
        {
            File.Delete(tempFile);
            Debug.Log("Deleted existing temp file: " + tempFile);
        }

        // Save the new temp image
        File.WriteAllBytes(tempFilePath, fileData);
        Debug.Log("Temp image saved at: " + tempFilePath);

        tempImg = tempFileName;
        LoadImage(fileData);
    }

    public void SaveImg(ElektroTile tile)
    {
        if (!Directory.Exists(saveDirectory))
            Directory.CreateDirectory(saveDirectory);

        if (tempImg == null)
            return;

        string fileExtension = Path.GetExtension(tempImg);
        string savedFileName = tile.TileId + fileExtension;
        string savedFilePath = Path.Combine(saveDirectory, savedFileName);

        // Remove existing saved files with the same name
        string[] existingFiles = Directory.GetFiles(saveDirectory, tile.TileId + ".*");
        foreach (string existingFile in existingFiles)
        {
            File.Delete(existingFile);
            Debug.Log("Deleted existing saved file: " + existingFile);
        }

        // Move temp file to permanent save directory
        string tempFilePath = Path.Combine(Application.dataPath, "..", "TempImages", tempImg);
        if (File.Exists(tempFilePath))
        {
            File.Move(tempFilePath, savedFilePath);
            Debug.Log("Image saved permanently at: " + savedFilePath);

            // Update `ElektroTile`
            tile.Img = savedFileName;

            // **Find and update the corresponding TileData in ElektroMap**
            TileData tileData = gameManagerElektro.Map.tiles.Find(t => t.id == tile.TileId);
            if (tileData != null)
            {
                tileData.img = savedFileName;  // <-- This updates the actual TileData
                Debug.Log($"Updated TileData: ID={tileData.id}, Img={tileData.img}");
            }
            else
            {
                Debug.LogError($"TileData not found for ID: {tile.TileId}");
            }

            // Save the entire map so that changes persist
            gameManagerElektro.Save();
        }
        else
        {
            Debug.LogError("Temp file not found: " + tempFilePath);
        }
    }


    public void LoadImage(byte[] fileData)
    {
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            imagePreview.texture = ResizeTexture(texture, 500, 500);
        }
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
        if (texture.LoadImage(fileData))
        {
            return texture;
        }
        return null;
    }
}
