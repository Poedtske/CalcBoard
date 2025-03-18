using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class ElektroMapLoader : MonoBehaviour
{
    public GameObject buttonPrefab; // Prefab for map selection buttons
    public GameObject rowPrefab;    // Prefab for rows
    public Transform mapsContainer; // Parent container for map selection rows

    public GameObject manageMapPrefab; // Prefab for map management buttons
    public Transform mapManagementContainer; // Parent container for map management rows

    public GameObject categoryList; // Parent for category buttons
    public GameObject categoryItemPrefab; // Prefab for category buttons

    public GameObject mapPanel;  // Panel containing map selection UI
    public GameObject categoryPanel;  // Panel containing category UI

    private PathManager pathManager = new(Games.ELEKTRO);
    private ElektroMapData mapData;
    private string mapsDirectory;

    private const int maxSelectionRows = 4;
    private const int maxSelectionColumns = 7;
    private const int maxManagementRows = 3;
    private const int maxManagementColumns = 4;

    void Start()
    {
        mapsDirectory = pathManager.GameMapsDirectory();
        Debug.Log(mapsDirectory);
        LoadMaps();
    }

    void LoadMaps()
    {
        if (!Directory.Exists(mapsDirectory))
        {
            Debug.LogError("Maps directory not found: " + mapsDirectory);
            return;
        }

        string[] mapFolders = Directory.GetDirectories(mapsDirectory);
        Debug.Log("Found " + mapFolders.Length + " maps in: " + mapsDirectory);

        foreach (Transform child in mapsContainer) { Destroy(child.gameObject); }
        foreach (Transform child in mapManagementContainer) { Destroy(child.gameObject); }

        int rowCountSelection = 0;
        int rowCountManagement = 0;
        GameObject currentSelectionRow = null;
        GameObject currentManagementRow = null;

        for (int i = 0; i < mapFolders.Length; i++)
        {
            string mapName = Path.GetFileName(mapFolders[i]);
            Debug.Log("Creating buttons for map: " + mapName);

            if (i % maxSelectionColumns == 0)
            {
                if (rowCountSelection >= maxSelectionRows) break;
                currentSelectionRow = Instantiate(rowPrefab, mapsContainer);
                rowCountSelection++;
            }

            GameObject selectionButton = Instantiate(buttonPrefab, currentSelectionRow.transform);
            selectionButton.GetComponentInChildren<TextMeshProUGUI>().text = mapName;
            selectionButton.GetComponent<Button>().onClick.AddListener(() => LoadMap(mapName));

            if (i % maxManagementColumns == 0)
            {
                if (rowCountManagement >= maxManagementRows) break;
                currentManagementRow = Instantiate(rowPrefab, mapManagementContainer);
                rowCountManagement++;
            }

            GameObject manageButton = Instantiate(manageMapPrefab, currentManagementRow.transform);
            manageButton.GetComponentInChildren<TextMeshProUGUI>().text = mapName;

            Button editButton = manageButton.transform.Find("Wood/ManageMap/Edit/EditBtn")?.GetComponent<Button>();
            Button deleteButton = manageButton.transform.Find("Wood/ManageMap/Delete/DeleteBtn")?.GetComponent<Button>();

            if (editButton != null) editButton.onClick.AddListener(() => EditMap(mapName));
            if (deleteButton != null) deleteButton.onClick.AddListener(() => DeleteMap(mapName));
        }
    }

    void LoadMap(string mapName)
    {
        Debug.Log("Loading map: " + mapName);
        PlayerPrefs.SetString("mapName", mapName);

        string filePath = Path.Combine(pathManager.GameMapsDirectory(), mapName, mapName + ".json");

        if (!File.Exists(filePath)) throw new FileNotFoundException($"JSON file not found: {filePath}");

        string jsonData = File.ReadAllText(filePath);
        Debug.Log("Raw JSON: " + jsonData);

        try
        {
            mapData = JsonConvert.DeserializeObject<ElektroMapData>(jsonData);
            if (mapData == null) throw new InvalidOperationException("Deserialized JSON resulted in null object.");
            Debug.Log("Map data loaded successfully.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("JSON Deserialization Error: " + ex.Message, ex);
        }

        // Close map selection panel and open category panel
        mapPanel.SetActive(false);
        categoryPanel.SetActive(true);

        LoadCategories();
    }

    void LoadCategories()
    {
        if (mapData == null || mapData.Categories == null)
        {
            Debug.LogError("Map data or categories are missing.");
            return;
        }

        foreach (Transform child in categoryList.transform) { Destroy(child.gameObject); }

        for (int i = 0; i < mapData.Categories.Count; i++)
        {
            string categoryName = mapData.Categories[i];
            Debug.Log("Creating category button: " + categoryName);

            GameObject categoryButton = Instantiate(categoryItemPrefab, categoryList.transform);
            categoryButton.GetComponentInChildren<TextMeshProUGUI>().text = categoryName;
            int categoryIndex = i; // Store index to avoid closure issues
            categoryButton.GetComponent<Button>().onClick.AddListener(() => LoadCategory(categoryIndex));
        }
    }

    void LoadCategory(int categoryIndex)
    {
        Debug.Log("Loading category: " + mapData.Categories[categoryIndex]);
        PlayerPrefs.SetInt("categoryIndex", categoryIndex);
        SceneManager.LoadScene(Scenes.ELEKTRO_GAME); // Replace with correct scene
    }

    void EditMap(string mapName)
    {
        Debug.Log("Editing map: " + mapName);
        // Add logic to handle map editing
    }

    void DeleteMap(string mapName)
    {
        Debug.Log(mapsDirectory);
        string mapPath = Path.Combine(mapsDirectory, mapName);
        Debug.Log(mapPath);
        if (Directory.Exists(mapPath))
        {
            Debug.Log($"Deleting map: {mapName}");
            try
            {
                Directory.Delete(mapPath, true);
                Debug.Log("Map deleted successfully.");
                LoadMaps();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to delete map: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError($"Map directory not found: {mapPath}");
        }
    }
}
