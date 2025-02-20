using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FileSelectorUI<T,Y>
    where T : CalcBoardMapData<T, Y>
    where Y : CalcBoardTileData<Y>
{
    
    public ElektroMapManager gameManagerElektro;
    private RawImage img;

    private string selectedFilePath;
    private string tempImg;

    // Paths
    private string mapFolderPath;
    private string saveImgPath;
    private string root = Path.Combine(Application.dataPath, "..");
    private string tempDirectory = Path.Combine(Application.dataPath, "..", "TempImages");

    private string game;
    private string mapName;

    public FileSelectorUI(T map)
    {
        this.mapName = map.MapName;
        this.game = map.Game;
        SetPaths();
    }

    private void CheckExistanceDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private void SetPaths()
    {
        // Set paths for the map
        mapFolderPath = Path.Combine(root, "games", game, "maps", mapName);
        saveImgPath = Path.Combine(mapFolderPath, "images");

        List<string> paths = new()
        {
            mapFolderPath,
            saveImgPath,
            tempDirectory
        };

        foreach (var path in paths)
        {
            CheckExistanceDirectory(path);
        }
    }

    public string TempImg
    {
        get => tempImg;
        set => tempImg = value;
    }

    public void OpenImageFilePicker(ElektroTileData tile, RawImage img)
    {
        this.img=img;
        var paths = StandaloneFileBrowser.OpenFilePanel("Select Image", "png,jpg,jpeg", "", false);
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            selectedFilePath = paths[0];
            SaveAndLoadNewImageInTemp(selectedFilePath, tile);
        }
    }

    public Texture2D LoadResourceTextureFromFile(string fileName)
    {
        return Resources.Load<Texture2D>(fileName) ?? null;
    }

    public void Save(T map)
    {
        try
        {

            // Convert map object to JSON format
            string jsonData = JsonConvert.SerializeObject(map, Formatting.Indented);

            // Define the file path
            string filePath = Path.Combine(mapFolderPath, map.MapName+".json");

            // Write JSON data to the file
            File.WriteAllText(filePath, jsonData);

            Debug.Log("Map saved successfully to: " + filePath);
        }
        catch (IOException ex)
        {
            Debug.LogError("File IO Error: " + ex.Message);
        }
        catch (JsonException ex)
        {
            Debug.LogError("JSON Serialization Error: " + ex.Message);
        }
    }

    void SaveAndLoadNewImageInTemp(string filePath, ElektroTileData tile)
    {
        byte[] fileData = File.ReadAllBytes(filePath);

        string fileExtension = Path.GetExtension(filePath);
        string tempFileName = "temp" + tile.Id + fileExtension;
        string tempFilePath = Path.Combine(tempDirectory, tempFileName);

        string[] existingTempFiles = Directory.GetFiles(tempDirectory, "temp" + tile.Id + ".*");
        foreach (string tempFile in existingTempFiles)
        {
            File.Delete(tempFile);
        }

        File.WriteAllBytes(tempFilePath, fileData);
        tempImg = tempFileName;
        LoadImage(fileData);
    }

    public void SaveImg<U>(U tile) where U :Y, IHasImg
    {

        if (tempImg == null)
            return;

        string fileExtension = Path.GetExtension(tempImg);
        string savedFileName = tile.Id + fileExtension;
        string savedFilePath = Path.Combine(saveImgPath, savedFileName);

        string[] existingFiles = Directory.GetFiles(saveImgPath, tile.Id + ".*");
        foreach (string existingFile in existingFiles)
        {
            File.Delete(existingFile);
        }

        string tempFilePath = Path.Combine(tempDirectory, tempImg);
        if (File.Exists(tempFilePath))
        {
            File.Move(tempFilePath, savedFilePath);
            tile.Img = savedFileName;
            tempImg = null;
        }
    }

    public void LoadImage(byte[] fileData)
    {
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            img.texture =ResizeTexture(texture, 500, 500);
        }
    }

    public Texture2D LoadImage(string fileName)
    {
        Texture2D originalTexture = LoadTextureFromFile(fileName);
        return originalTexture == null ? null : ResizeTexture(originalTexture, 500, 500);
    }

    public Texture2D LoadFromResourceImage(string imgPath)
    {
        Texture2D originalTexture = Resources.Load<Texture2D>(imgPath);
        return originalTexture == null ? null : ResizeTexture(originalTexture, 500, 500);
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

    private Texture2D LoadTextureFromFile(string imgName)
    {
        byte[] fileData = File.ReadAllBytes(Path.Combine(saveImgPath, imgName));
        Texture2D texture = new Texture2D(2, 2);
        return texture.LoadImage(fileData) ? texture : null;
    }
}
