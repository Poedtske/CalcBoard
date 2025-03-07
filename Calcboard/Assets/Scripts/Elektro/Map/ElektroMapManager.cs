using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ElektroMapManager : CalcBoardMapMananger
{
    private ElektroMapData map;
    private FileManager<ElektroMapData, ElektroTileData> fileManager;

    [Header("Unity Components")]
    [SerializeField] private GameObject tilePanel;
    [SerializeField] private GameObject editTilePanel;

    public FileManager<ElektroMapData, ElektroTileData> FileManager 
    { 
        get { return fileManager; } 
    }
    public ElektroMapData Map
    {
        get { return map; }
        set { map = value; }
    }

    private void Awake()
    {
        var mapHolder = FindAnyObjectByType<ElektroMapHolder>();
        if (mapHolder != null)
        {
            map = mapHolder.Map; // Get the map directly from the MapHolder

            //map.MapName += ".json";
            fileManager = new FileManager<ElektroMapData, ElektroTileData>(map);
        }
        else if(GameObject.Find($"MapHolder")!=null)
        {
            GameObject mapHolderObject = GameObject.Find($"MapHolder");
            mapHolder=mapHolderObject.AddComponent<ElektroMapHolder>();
            mapHolder.Initialize(new("test", null, new() { "dutch", "english" }));
            map = mapHolder.Map; // Get the map directly from the MapHolder

            //map.MapName += ".json";
            fileManager = new FileManager<ElektroMapData, ElektroTileData>(map);

        }
        else
        {
            Debug.LogError("No MapHolder found in the scene.");
        }
    }

    void Start()
    {
        LoadTiles();
        Debug.Log(map);


    }

    public override void Save()
    {

//Musico
        fileManager.Save(map);
        SceneManager.LoadScene(Scenes.MAIN_MENU);
        
        //Main
        try
        {

            int userId = PlayerPrefs.GetInt("UserId", -1);
            if (userId == -1)
            {
                Debug.LogError("User not logged in or UserId not set.");
                return;
            }

            ElektroMapData mapData=map.toData();
            Debug.Log("Serialized map data: " + JsonConvert.SerializeObject(mapData, Formatting.Indented));
            // Convert map object to JSON format
            string jsonData = JsonConvert.SerializeObject(new
            {
                userId = userId,
                map = mapData
            }, Formatting.Indented);

            // Send Map Data to Backend
            StartCoroutine(SendMapToBackend(jsonData));

            // Define the file path
            string filePath = Path.Combine(gamePath, jsonFileName);

            // Write JSON data to the file
            File.WriteAllText(filePath, jsonData);

            SceneManager.LoadScene("URP2DSceneTemplate");

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

    IEnumerator SendMapToBackend(string jsonData)
    {
        string apiUrl = "http://localhost:8081/maps/save";
        string token = PlayerPrefs.GetString("Token", ""); // Retrieve the token



        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            Debug.Log("Sending map data to backend: " + jsonData);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token); // Add the token in Authorization header

            Debug.Log("Tolken: " + token);



            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token); // Add token here
            }
            else
            {
                Debug.LogError("No token found. User may not be authenticated.");
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Map successfully saved on backend!");
            }
            else
            {
                Debug.LogError("Failed to save map on backend: " + request.downloadHandler.text);
            }
        }
    }

    public override void ReloadTile()
    {
        try
        {
            // Find the existing GameObject by mapNameGameObject.Find($"ElectroTile ({i})");
            GameObject tileObject = GameObject.Find($"Tile ({PlayerPrefs.GetInt("tileId")})");
            //ElektroTile tile = tileObject.GetComponent<ElektroTile>();
            Image tempImg = tileObject.GetComponent<Image>();
            ElektroTileData tile = FindTile(PlayerPrefs.GetInt("tileId"));
            if (tileObject != null)
            {
                
                Texture2D texture;
                if (tile.Img == "")
                {

                    texture = fileManager.LoadResourceTextureFromFile("temp");
                }
                else
                {

                    texture = fileManager.LoadImage(tile.Img);
                }


                if (texture != null)
                {

                    //Debug.Log(imagePath);

                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));



                    tempImg.sprite = sprite;


                    if (tile != null)
                    {

                        Debug.Log(map.ToString());
                        //elektroTiles.Add(tile);


                    }
                    else
                    {
                        Debug.LogError($"ElektroTile component not found on: {tileObject.name}");
                    }
                }


            }
            else
            {
                Debug.LogError($"GameObject Tile ({tile.Id}) not found in the scene!");
            }
        }
        catch (JsonException ex)
        {
            Debug.LogError("JSON Deserialization Error: " + ex.Message);
        }
    }

    public ElektroTileData FindTile(int id)
    {
        foreach (var tile in map.Tiles)
        {
            if (tile.Id == id)
            {
                return tile;
            }
        }
        return null;
    }

    

    public override void LoadTiles()
    {
        try
        {
            for (int i=1;i<=24;i++)
            {
                // Find the existing GameObject by mapName
                GameObject existingTile = GameObject.Find($"Tile ({i})");
                Button btn= existingTile.AddComponent<Button>();
                Image tempImg = existingTile.AddComponent<Image>();
                int index = i;
                btn.onClick.AddListener(() => SetIdAction(index));
                if (existingTile != null)
                { 
                    Texture2D texture=fileManager.LoadResourceTextureFromFile("temp");
                    if (texture != null)
                    {
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        tempImg.sprite = sprite;
                    }


                }
                else
                {
                    Debug.LogError($"GameObject ElektroTile ({i}) not found in the scene!");
                }


            }
            }
        catch (JsonException ex)
        {
            Debug.LogError("JSON Deserialization Error: " + ex.Message);
        }    
    }

    public override void SetIdAction(int id)
    {
        
        PlayerPrefs.SetInt("tileId",id);


        //editTilePanel.GetComponent<EditElektroTile>().Tile=FindTile(id);
        editTilePanel.SetActive(true);
        tilePanel.SetActive(false);
    }
}
