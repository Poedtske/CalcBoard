using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Layouts;
using System.Linq;

public class ElektroGameManager : MonoBehaviour
{
    private string gamePath = "games/elektro/maps/";
    private UIDocument doc;
    private VisualElement visualElement;
    private Label label;
    ElektroMapData mapData;
    public int language = 0;
    private List<ElektroTileData> tileList;
    private ElektroTileData selectedTile;
    public string input;
    private int score = 0;
    private int rounds = 0;
    private bool untilEverytingIsCorrect;
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
        tileList = new List<ElektroTileData>(mapData.Tiles.Take(6));
        SelectTile();
    }

    private void SelectTile()
    {

        if (tileList.Count == 0)
        {
            Debug.Log("No tileIds to play.");
            return;
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, tileList.Count);
            selectedTile = tileList[randomIndex];
            label.text = selectedTile.Words[language];
            tileList.RemoveAt(randomIndex); // Remove the selected tile from the list

            // Here, you can process the selected tile (e.g., display it in the UI, wait for user input, etc.)
        }

        //while (tileList.Count > 0)
        //{

        //}

        //Debug.Log("All tileIds have been used!");
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(input))
        {
            if (ValidateInput())
            {
                score++;
                SelectTile();
                Debug.Log("correct");
            }
            else
            {
                Debug.Log("incorrect");
            }
            rounds++;
            input = null;
        }
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(input) || selectedTile == null)
        {
            Debug.LogWarning("Input is empty or selectedTile is null.");
            return false;
        }

        return input.Trim() == selectedTile.Id.ToString();
    }



    private void LoadTiles()
    {
        string filePath = Path.Combine(Application.dataPath, "..", gamePath, PlayerPrefs.GetString("mapName"), PlayerPrefs.GetString("mapName") + ".json");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"JSON file not found: {filePath}");
        }

        string jsonData = File.ReadAllText(filePath);
        Debug.Log("Raw JSON: " + jsonData);

        try
        {
            this.mapData = JsonConvert.DeserializeObject<ElektroMapData>(jsonData);
            //imgPath = Path.Combine(gamePath, mapData.MapName, "images");
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
