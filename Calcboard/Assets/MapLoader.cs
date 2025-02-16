using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;



public class MapLoader : MonoBehaviour
{
    public GameObject buttonPrefab; // Prefab for buttons
    public Transform buttonParent; // Parent where buttons will be instantiated

    private string mapsDirectory;

    /// <summary>
    /// Get the parent directory of the Assets folder and set the maps directory.
    /// </summary>
    void Start()
    {
        string projectPath = Directory.GetParent(Application.dataPath).FullName; // Get parent of Assets/
        mapsDirectory = Path.Combine(projectPath, "games/elektro/maps");
        LoadMaps();
    }


    /// <summary>
    /// Load all maps from the maps directory and create buttons for each map.
    /// </summary>
    void LoadMaps()
    {
        if (!Directory.Exists(mapsDirectory))
        {
            Debug.LogError("Maps directory not found: " + mapsDirectory);
            return;
        }

        // Get all map directories (since maps are in folders)
        string[] mapFolders = Directory.GetDirectories(mapsDirectory);

        Debug.Log("Found " + mapFolders.Length + " maps in: " + mapsDirectory);


        // Clear existing buttons
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (string folder in mapFolders)
        {
            string mapName = Path.GetFileName(folder);
            Debug.Log("Creating button for map: " + mapName);

            GameObject button = Instantiate(buttonPrefab, buttonParent);

            if (buttonParent == null)
            {
                Debug.LogError("buttonParent is NULL! Assign a UI Panel in the Inspector.");
                return;
            }

            Debug.Log("Button instantiated: " + button.name);

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText == null)
            {
                Debug.LogError("Button prefab is missing a TextMeshProUGUI component! Check your prefab.");
                return;
            }

            buttonText.text = mapName;
            string selectedMapPath = folder;
            button.GetComponent<Button>().onClick.AddListener(() => LoadMap(selectedMapPath));

        }
    }

    void LoadMap(string filePath)
    {
        Debug.Log("Loading map: " + filePath);
        // Implement your map-loading logic here
    }


}
