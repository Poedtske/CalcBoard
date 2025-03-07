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
using System.Linq;
using System.IO;

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

            ElektroMapData mapData = map.toData();
            Debug.Log("Serialized map data: " + JsonConvert.SerializeObject(mapData, Formatting.Indented));

            // Convert map object to JSON format
            string jsonData = JsonConvert.SerializeObject(new
            {
                userId = userId,
                map = mapData
            }, Formatting.Indented);

            // Define the path for the map and image folder
            string imagesFolderPath = Path.Combine(gamePath, "images");

            // Ensure the images folder exists, create it if not
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
                Debug.Log("Images folder created at: " + imagesFolderPath);
            }

            // Define JSON file path
            string filePath = Path.Combine(gamePath, jsonFileName);

            // Write JSON data to the file
            File.WriteAllText(filePath, jsonData);

            // Now that the map is saved, we proceed to send it along with any image
            StartCoroutine(SendMapToBackend(jsonData, imagesFolderPath));

            string token = PlayerPrefs.GetString("Token", "");
            if (!string.IsNullOrEmpty(token))
            {
                StartCoroutine(ValidateTokenAndLoadScene(token));
            }
            else
            {
                // No token, go to login
                SceneManager.LoadScene("URP2DSceneTemplate");

                // Use LoginManager to show the login screen
                LoginManager loginManager = FindObjectOfType<LoginManager>();
                if (loginManager != null)
                {
                    loginManager.ShowLogin();
                }
                else
                {
                    Debug.LogError("LoginManager not found in the scene.");
                }
            }

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

    IEnumerator ValidateTokenAndLoadScene(string token)
    {
        string validateUrl = "http://yourbackend.com/validate-token"; // Replace with actual API URL
        using (UnityWebRequest request = UnityWebRequest.Get(validateUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            SceneManager.LoadScene("URP2DSceneTemplate"); // Load scene first

            yield return new WaitForSeconds(1); // Give scene time to load

            LoginManager loginManager = FindObjectOfType<LoginManager>();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Token is valid.");
                loginManager?.ShowMenu();  // Show menu if valid
            }
            else
            {
                Debug.LogError("Token is invalid or expired.");
                PlayerPrefs.DeleteKey("Token");
                PlayerPrefs.DeleteKey("UserId");
                loginManager?.ShowLogin(); // Show login if invalid
            }
        }
    }


    IEnumerator SendMapToBackend(string jsonData, string imagesFolderPath)
    {
        string apiUrl = "http://localhost:8081/maps/save";
        string token = PlayerPrefs.GetString("Token", "");

        Debug.Log("🔍 Preparing JSON Data: " + jsonData);
        Debug.Log("🔍 Using API URL: " + apiUrl);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
    {
        new MultipartFormDataSection("mapData", jsonData, "application/json") // ✅ Ensures correct format
    };

        string[] imageFiles = Directory.GetFiles(imagesFolderPath, "*.png")
                              .Concat(Directory.GetFiles(imagesFolderPath, "*.jpg"))
                              .Concat(Directory.GetFiles(imagesFolderPath, "*.jpeg"))
                              .Concat(Directory.GetFiles(imagesFolderPath, "*.bmp"))
                              .Concat(Directory.GetFiles(imagesFolderPath, "*.wav"))
                              .ToArray();

        foreach (string imagePath in imageFiles)
        {
            if (File.Exists(imagePath))
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                formData.Add(new MultipartFormFileSection("file", imageBytes, Path.GetFileName(imagePath), "image/png"));
                Debug.Log("🖼 Adding image: " + Path.GetFileName(imagePath) + " (Size: " + imageBytes.Length + " bytes)");
            }
        }

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, formData))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token); // ✅ Do NOT manually set Content-Type

            Debug.Log("🛠 Headers Set:");
            Debug.Log("  - Authorization: Bearer " + token);
            Debug.Log("  - Content-Type is automatically set by Unity");

            yield return request.SendWebRequest();

            Debug.Log("📡 Response Code: " + request.responseCode);
            Debug.Log("📡 Response Text: " + request.downloadHandler.text);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Map and images successfully saved on backend!");
            }
            else
            {
                Debug.LogError("❌ Failed to save map on backend: " + request.downloadHandler.text);
            }
        }
    }


    public string GetImagePathFromDirectory(string directoryPath)
    {
        string[] files = Directory.GetFiles(directoryPath, "*.png"); // Get all .png files in the directory
        if (files.Length > 0)
        {
            return files[0]; // Return the first image (or use your logic to select one)
        }
        else
        {
            Debug.LogError("No image found in the directory.");
            return null;
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





