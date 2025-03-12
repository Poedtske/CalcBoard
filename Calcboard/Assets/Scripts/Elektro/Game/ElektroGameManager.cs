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
using System.Collections;

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

    private List<AudioClip> audioClipList;
    private Coroutine backgroundMusicCoroutine; // Store coroutine reference

    public AudioClip victoryMusic;
    public AudioClip errorSound;
    public AudioClip victorySound;
    public List<AudioClip> backgroundMusicList;
    public AudioClip correctSound;
    public AudioSource backgroundMusicManager;
    public AudioSource SFXManager;

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
        audioClipList = new List<AudioClip>(backgroundMusicList);
        LoadTiles();
        tileList = new List<ElektroTileData>(mapData.Tiles.Take(6));
        SelectTile();

        // Start background music loop
        backgroundMusicCoroutine = StartCoroutine(PlayBackgroundMusic());
    }

    private IEnumerator PlayBackgroundMusic()
    {
        while (true) // Loop infinitely until stopped in SelectTile()
        {
            if (audioClipList.Count == 0)
            {
                audioClipList = new List<AudioClip>(backgroundMusicList); // Reset playlist
            }

            int randomIndex = UnityEngine.Random.Range(0, audioClipList.Count);
            AudioClip selectedTrack = audioClipList[randomIndex];
            audioClipList.RemoveAt(randomIndex);

            backgroundMusicManager.clip = selectedTrack;
            backgroundMusicManager.Play();

            while (backgroundMusicManager.isPlaying)
            {
                yield return null;
            }
        }
    }

    private void SelectTile()
    {
        if (tileList.Count == 0)
        {
            Debug.Log("No tileIds to play.");

            // Stop the background music process
            if (backgroundMusicCoroutine != null)
            {
                StopCoroutine(backgroundMusicCoroutine);
                backgroundMusicCoroutine = null;
            }
            backgroundMusicManager.Stop();

            // Play victory sound, then start victory music
            SFXManager.clip = victorySound;
            SFXManager.Play();

            StartCoroutine(PlayBackgroundMusicAfterSFX());
            return;
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, tileList.Count);
            selectedTile = tileList[randomIndex];
            label.text = selectedTile.Words[language];
            tileList.RemoveAt(randomIndex);
        }
    }

    private IEnumerator PlayBackgroundMusicAfterSFX()
    {
        while (SFXManager.isPlaying)
        {
            yield return null;
        }

        backgroundMusicManager.loop = true;
        backgroundMusicManager.clip = victoryMusic;
        backgroundMusicManager.Play();
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(input))
        {
            if (ValidateInput())
            {
                SFXManager.clip = correctSound;
                SFXManager.Play();
                score++;
                SelectTile();
                Debug.Log("correct");
            }
            else
            {
                SFXManager.clip = errorSound;
                SFXManager.Play();
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
