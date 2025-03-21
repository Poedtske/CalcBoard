using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FileManager<T,Y>
    where T : CalcBoardMapData<T, Y>
    where Y : CalcBoardTileData<Y>
{
    
    private RawImage img;

    private string selectedFilePath;
    private string tempImg;
    private PathManager pathManager;

    public FileManager(T map)
    {
        pathManager = new(map.Game, map.MapName);
    }

    public string TempImg
    {
        get => tempImg;
        set => tempImg = value;
    }

    public void OpenImageFilePicker<U>(U tile, RawImage img) where U : Y, IHasImg
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
            string filePath = Path.Combine(pathManager.Map(), map.MapName+".json");

            //imagepath
            string imagepath = Path.Combine(pathManager.Map(), "images");

            Debug.Log(imagepath);

            // Ensure the ApiManager is not null before calling it
            if (ApiManager.Instance == null)
            {
                Debug.LogError("ApiManager.Instance is null. Cannot send data.");
                return;
            }


            // Write JSON data to the file
            File.WriteAllText(filePath, jsonData);

            ApiManager.Instance.StartCoroutine(ApiManager.Instance.SendMapToBackend(jsonData, imagepath));

            SceneManager.LoadScene(Scenes.ELEKTRO_ACTIONS);

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

    void SaveAndLoadNewImageInTemp<U>(string filePath, U tile) where U :Y, IHasImg
    {
        byte[] fileData = File.ReadAllBytes(filePath);

        string fileExtension = Path.GetExtension(filePath);
        string tempFileName = "temp" + tile.Id + fileExtension;
        string tempFilePath = Path.Combine(pathManager.TempImg(), tempFileName);

        string[] existingTempFiles = Directory.GetFiles(pathManager.TempImg(), "temp" + tile.Id + ".*");
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
        string savedFilePath = Path.Combine(pathManager.Imgs(), savedFileName);

        string[] existingFiles = Directory.GetFiles(pathManager.Imgs(), tile.Id + ".*");
        foreach (string existingFile in existingFiles)
        {
            File.Delete(existingFile);
        }

        string tempFilePath = Path.Combine(pathManager.TempImg(), tempImg);
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
        byte[] fileData = File.ReadAllBytes(Path.Combine(pathManager.Imgs(), imgName));
        Texture2D texture = new Texture2D(2, 2);
        return texture.LoadImage(fileData) ? texture : null;
    }
}
