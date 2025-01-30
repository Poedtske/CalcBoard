using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

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
            ElektroMap mapData = JsonConvert.DeserializeObject<ElektroMap>(jsonData);
            Debug.Log(jsonData);

            foreach (var tileData in mapData.tiles)
            {
                ElektroTile newTile = new GameObject($"Tile_{tileData.id}").AddComponent<ElektroTile>();
                newTile.Initialize(tileData.id, tileData.img, tileData.meanings);
                Debug.Log(newTile.ToString());
                elektroTiles.Add(newTile);
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
    public Dictionary<string, string> meanings;
}