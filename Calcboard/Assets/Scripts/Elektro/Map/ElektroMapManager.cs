using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ElektroMapManager : MonoBehaviour, IDataPersistance
{
    public string jsonFileName = "TempElektroMap.json"; // Adjust the path if needed
    private List<ElektroTile> elektroTiles = new List<ElektroTile>();
    private ElektroMap map;
    public GameObject tilePanel;
    public GameObject editTilePanel;
    public string gamePath = "games/elektro/maps/";

    public ElektroMap Map => map;


    private void Awake()
    {
        List<string> lan=new() {"English", "Nederlands" };
        map = gameObject.AddComponent<ElektroMap>();
        map.Initialize(1, "elektro", "Temp Test", null, lan);
        gamePath += map.MapName;
    }

    void Start()
    {
        LoadTiles();



    }

    public void Save()
    {
        try
        {
            ElektroMapData mapData=map.toData();
            // Convert map object to JSON format
            string jsonData = JsonConvert.SerializeObject(mapData, Formatting.Indented);

            // Define the file path
            string filePath = Path.Combine(gamePath, jsonFileName);

            // Write JSON data to the file
            File.WriteAllText(filePath, jsonData);

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
                    string imagePath = Path.Combine(Application.dataPath, "..", "games", map.Game, "maps", map.MapName, "images", tile.Img);

                    Texture2D texture = LoadTextureFromFile(imagePath);
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
                        string imagePath = Path.Combine(Application.dataPath, "..", "games", map.Game, "maps", map.MapName, "images", "temp.jpg");

                        Texture2D texture = LoadTextureFromFile(imagePath);
                        if (texture != null)
                        {
                            
                            //Debug.Log(imagePath);

                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));


                            ElektroTile newTile = existingTile.AddComponent<ElektroTile>();
                            newTile.Initialize(i, "temp.jpg",map.Languages.Count);
                            this.map.Tiles.Add(newTile);
                            Image tempImg = existingTile.AddComponent<Image>();
                            tempImg.sprite = sprite;
                            

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
}





