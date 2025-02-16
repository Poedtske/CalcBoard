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
    private string jsonFileName = "Temp Test/TempElektroMap.json"; // Adjusted to match your JSON location
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
    private FileSelectorUI fileSelectorUI;
    private string correctTile;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        visualElement = doc.rootVisualElement.Q("Container");
        label1 = doc.rootVisualElement.Q("Option1") as Label;
        label2 = doc.rootVisualElement.Q("Option2") as Label;
        img = doc.rootVisualElement.Q("Img");
        scoreLabel = doc.rootVisualElement.Q("Score") as Label;
        fileSelectorUI = new FileSelectorUI();
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
        tileList = new List<ElektroTileData>(mapData.tiles); // Create a copy of the tile list
        this.SelectTile();
        scoreLabel.text = "Score: 0";
    }

    private void SelectTile()
    {
        if (tileList.Count == 0)
        {
            Debug.Log("No tiles to play.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, tileList.Count);
        selectedTile = tileList[randomIndex];
        tileList.RemoveAt(randomIndex); // Remove the selected tile from the list

        // Ensure there are other tiles to pick an incorrect meaning from
        string incorrectMeaning = "Incorrect"; // Default fallback
        if (mapData.tiles.Count > 1)
        {
            ElektroTileData randomTile;
            do
            {
                randomTile = mapData.tiles[UnityEngine.Random.Range(0, mapData.tiles.Count)];
            } while (randomTile == selectedTile); // Ensure it's not the same as the correct one

            incorrectMeaning = randomTile.meanings[language];
        }

        // Randomly decide which label gets the correct answer
        if (UnityEngine.Random.value < 0.5f)
        {
            label1.text = "1. "+selectedTile.meanings[language]; // Correct
            label2.text = "2. "+incorrectMeaning;               // Incorrect
            correctTile = "1";
        }
        else
        {
            label1.text = "1. "+incorrectMeaning;               // Incorrect
            label2.text = "2. " + selectedTile.meanings[language]; // Correct
            correctTile = "2";
        }

        // Load Image from Resources folder

        Texture2D texture = fileSelectorUI.LoadImage(Path.Combine(Application.dataPath, "..", imgPath, selectedTile.img));

        if (texture != null)
        {
            img.style.backgroundImage = new StyleBackground(texture);
        }
        else
        {
            Debug.LogError($"Image not found at path: {Path.Combine(Application.dataPath, "..", imgPath, selectedTile.img)}");
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
            imgPath = Path.Combine(gamePath, mapData.name, "images");
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
