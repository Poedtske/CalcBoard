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

public class ElektroMapManager : MonoBehaviour, IDataPersistance
{
    private string jsonFileName; // Adjust the path if needed
    private List<ElektroTile> elektroTiles = new List<ElektroTile>();
    private ElektroMap map;
    public GameObject tilePanel;
    public GameObject editTilePanel;
    public string gamePath = "games/elektro/maps/";

    public ElektroMap Map => map;


    private void Awake()
    {
        //List<string> lan = new() { "English", "Nederlands" };
        //map = gameObject.AddComponent<ElektroMap>();
        //map.Initialize(1, "elektro", "Temp Test", null, lan);
        //gamePath += map.MapName;

        map = FindAnyObjectByType<ElektroMap>();
        gamePath += map.MapName;
        jsonFileName = map.MapName + ".json";
    }

    void Start()
    {
        LoadTiles();
        Debug.Log(map);


    }

    public void Save()
    {
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

    public void ReloadImages()
    {
        try
        {
            foreach (var tile in map.Tiles)
            {
                // Find the existing GameObject by name
                GameObject existingTile = GameObject.Find($"ElectroTile ({tile.Id})");
                //ElektroTile tile = existingTile.GetComponent<ElektroTile>();
                Image tempImg = existingTile.GetComponent<Image>();

                if (existingTile != null)
                {
                    string imagePath="";
                    Texture2D texture;
                    if (tile.Img=="")
                    {
                        
                        texture=LoadResourceTextureFromFile("temp");
                    }
                    else
                    {
                        imagePath = Path.Combine(Application.dataPath, "..", "games", map.Game, "maps", map.MapName, "images", tile.Img);
                        texture = LoadTextureFromFile(imagePath);
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
                            Debug.LogError($"ElektroTile component not found on: {existingTile.name}");
                        }
                    }


                }
                else
                {
                    Debug.LogError($"GameObject ElektroTile ({tile.Id}) not found in the scene!");
                }


            }
        }
        catch (JsonException ex)
        {
            Debug.LogError("JSON Deserialization Error: " + ex.Message);
        }
    }

    public ElektroTile FindTile(int id)
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

    Texture2D LoadTextureFromFile(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData)) // Automatically resizes the texture
        {
            return texture;
        }
        return null;
    }
    Texture2D LoadResourceTextureFromFile(string fileName)
    {
        
        Texture2D texture = Resources.Load<Texture2D>(fileName);
        if (texture!=null) // Automatically resizes the texture
        {
            return texture;
        }
        return null;
    }

    public void LoadTiles()
    {
        //string filePath = Path.Combine(Application.dataPath, jsonFileName);

        


        try
            {
                for (int i=1;i<=24;i++)
                {
                    // Find the existing GameObject by name
                    GameObject existingTile = GameObject.Find($"ElectroTile ({i})");
                    Button btn= existingTile.GetComponent<Button>();
                    int index = i;
                    btn.onClick.AddListener(() => setIdAction(index));

                if (existingTile != null)
                    {


                        Texture2D texture=LoadResourceTextureFromFile("temp");
                        if (texture != null)
                        {
                            
                            //Debug.Log(imagePath);

                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));


                            ElektroTile newTile = existingTile.AddComponent<ElektroTile>();
                            newTile.Initialize(i, "",map.Languages.Count);
                            this.map.Tiles.Add(newTile);
                            Image tempImg = existingTile.AddComponent<Image>();
                            tempImg.sprite = sprite;
                            Debug.Log("Map tiles before saving: " + JsonConvert.SerializeObject(map.Tiles, Formatting.Indented));


                        if (newTile != null)
                            {
                                
                               // Debug.Log($"Initialized: {newTile}");
                                elektroTiles.Add(newTile);

                            }
                            else
                            {
                                Debug.LogError($"ElektroTile component not found on: {existingTile.name}");
                            }
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

    private void setIdAction(int id)
    {
        
        PlayerPrefs.SetInt("tileId",id);

        editTilePanel.SetActive(true);
        tilePanel.SetActive(false);
    }

    public void LoadData(ElektroMapData data)
    {
        map.Load(data);
    }

    public void SaveData(ref ElektroMapData data)
    {
        data = map.toData();
    }

    public string getImg()
    {
        throw new System.NotImplementedException();
    }

    public void setImg(string img)
    {
        throw new System.NotImplementedException();
    }
}





