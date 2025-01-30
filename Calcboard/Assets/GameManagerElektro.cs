using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerElektro : MonoBehaviour
{
    public string jsonFileName = "TempElektroMap.json"; // Adjust the path if needed
    private List<ElektroTile> elektroTiles = new List<ElektroTile>();

    void Start()
    {
        LoadTiles();
    }

    private void Save()
    {

    }

    void LoadTiles()
    {
        string filePath = Path.Combine(Application.dataPath, jsonFileName);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            Debug.Log("Raw JSON: " + jsonData);

            try
            {
                ElektroMap mapData = JsonConvert.DeserializeObject<ElektroMap>(jsonData);
                Debug.Log("JSON Parsed Successfully!");

                foreach (var tileData in mapData.tiles)
                {
                    // Find the existing GameObject by name
                    GameObject existingTile = GameObject.Find($"ElectroTile ({tileData.id})");

                    if (existingTile != null)
                    {
                        // Get the ElektroTile component
                        //ElektroTile newTile = new ElektroTile();

                        Texture2D temp = Resources.Load<Texture2D>(tileData.img);
                        Sprite sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));

                        
                        ElektroTile newTile = existingTile.AddComponent<ElektroTile>();
                        newTile.Initialize(tileData.id, tileData.img, tileData.meanings);
                        Image tempImg=existingTile.AddComponent<Image>();
                        tempImg.sprite = sprite;

                        if (newTile != null)
                        {
                            newTile.Initialize(tileData.id, tileData.img, tileData.meanings);
                            Debug.Log($"Initialized: {newTile}");
                            elektroTiles.Add(newTile);
                        }
                        else
                        {
                            Debug.LogError($"ElektroTile component not found on: {existingTile.name}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"GameObject ElektroTile ({tileData.id}) not found in the scene!");
                    }


                }
            }
            catch (JsonException ex)
            {
                Debug.LogError("JSON Deserialization Error: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError($"JSON file not found: {filePath}");
        }
    }
}

[System.Serializable]
public class ElektroMap
{
    public int id;
    public string game;
    public string name;
    public List<string> languages;
    public List<TileData> tiles;
    public string img;
}

[System.Serializable]
public class TileData
{
    public int id;
    public string img;
    public List<string> meanings;
}