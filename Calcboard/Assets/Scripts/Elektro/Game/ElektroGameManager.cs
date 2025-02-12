using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

public class ElektroGameManager : MonoBehaviour
{
    private string gamePath = "games/elektro/maps/";
    private string jsonFileName = "Temp Test/TempElektroMap.json"; // Adjusted to match your JSON location
    private UIDocument doc;
    private VisualElement visualElement;
    private Label label;
    ElektroMapData mapData;
    public int language=0;
    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        visualElement = doc.rootVisualElement.Q("Container");
        visualElement.RegisterCallback<ClickEvent>(Click);
        label = doc.rootVisualElement.Q("Header") as Label;
    }
    private void Click(ClickEvent e)
    {
        Debug.Log("pressed it");
    }
    private void OnDisable()
    {
        visualElement.UnregisterCallback<ClickEvent>(Click);
    }

    void Start()
    {
        LoadTiles();
        PlayGame();
    }

    private void PlayGame()
    {
        List<ElektroTileData> tileList = mapData.tiles;
        foreach(ElektroTileData tile in tileList) { }
    }

    private void LoadTiles()
    {
        string filePath = Path.Combine(Application.dataPath, "..", gamePath, jsonFileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"JSON file not found: {filePath}");
        }

        string jsonData = File.ReadAllText(filePath);
        Debug.Log("Raw JSON: " + jsonData);

        try
        {
            this.mapData = JsonConvert.DeserializeObject<ElektroMapData>(jsonData);
            if (mapData == null)
            {
                throw new InvalidOperationException("Deserialized JSON resulted in null object.");
            }
            Debug.Log(mapData);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("JSON Deserialization Error: " + ex.Message, ex);
        }
    }

}
