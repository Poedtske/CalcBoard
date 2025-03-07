using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class demoGameManager : MonoBehaviour
{
    private string gamePath = "games/elektro/maps/";
    private string imgPath;
    private UIDocument doc;
    private VisualElement visualElement;
    private Label label1;
    private Label label2;
    private Label scoreLabel;
    private VisualElement img;
    ElektroMapData mapData;
    public int language = 0;
    private List<ElektroTileData> tileList;
    private ElektroTileData selectedTile;
    public string input;
    private int score = 0;
    private int rounds = 0;
    private bool untilEverytingIsCorrect;
    private FileManager<ElektroMapData,ElektroTileData> fileSelectorUI;
    private string correctTile;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        visualElement = doc.rootVisualElement.Q("Container");
        label1 = doc.rootVisualElement.Q("Option1") as Label;
        label2 = doc.rootVisualElement.Q("Option2") as Label;
        img = doc.rootVisualElement.Q("Img");
        scoreLabel = doc.rootVisualElement.Q("Score") as Label;
        
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
        this.LoadTiles();
        fileSelectorUI = new(mapData);
        tileList = new List<ElektroTileData>(mapData.Tiles); // Create a copy of the tile list

        this.SelectTile();
        scoreLabel.text = "Score: 0";
    }

    private void SelectTile()
    {
        if (tileList.Count == 0)
        {
            Debug.Log("No tileIds to play.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, tileList.Count);
        selectedTile = tileList[randomIndex];
        tileList.RemoveAt(randomIndex); // Remove the selected tile from the list

        // Ensure there are other tileIds to pick an incorrect meaning from
        string incorrectMeaning = "Incorrect"; // Default fallback
        if (mapData.Tiles.Count > 1)
        {
            ElektroTileData randomTile;
            do
            {
                randomTile = mapData.Tiles[UnityEngine.Random.Range(0, mapData.Tiles.Count)];
            } while (randomTile == selectedTile); // Ensure it's not the same as the correct one

            incorrectMeaning = randomTile.Words[language];
        }

        // Randomly decide which label gets the correct answer
        if (UnityEngine.Random.value < 0.5f)
        {
            label1.text = "1. "+selectedTile.Words[language]; // Correct
            label2.text = "2. "+incorrectMeaning;               // Incorrect
            correctTile = "1";
        }
        else
        {
            label1.text = "1. "+incorrectMeaning;               // Incorrect
            label2.text = "2. " + selectedTile.Words[language]; // Correct
            correctTile = "2";
        }

        // Load Image from Resources folder

        Texture2D texture = fileSelectorUI.LoadImage(selectedTile.Img);

        if (texture != null)
        {
            img.style.backgroundImage = new StyleBackground(texture);
        }
        else
        {
            Debug.LogError($"Image not found at path: {Path.Combine(Application.dataPath, "..", imgPath, selectedTile.Img)}");
        }
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(input))
        {
            if (ValidateInput())
            {
                score++;
                scoreLabel.text = "Score: "+score;
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

        if(input == correctTile)
        {
            return true;
        }
        return false;

    }


    private void LoadTiles()
    {
        string filePath = Path.Combine(Application.dataPath, "..", gamePath, PlayerPrefs.GetString("mapName"), PlayerPrefs.GetString("mapName")+".json");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"JSON file not found: {filePath}");
        }

        string jsonData = File.ReadAllText(filePath);
        Debug.Log("Raw JSON: " + jsonData);

        try
        {
            this.mapData = JsonConvert.DeserializeObject<ElektroMapData>(jsonData);
            imgPath = Path.Combine(gamePath, mapData.MapName, "images");
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
