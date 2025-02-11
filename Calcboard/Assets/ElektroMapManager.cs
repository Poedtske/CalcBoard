using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ElektroMapManager : MonoBehaviour
{
    public string jsonFileName = "TempElektroMap.json"; // Adjust the path if needed
    private List<ElektroTile> elektroTiles = new List<ElektroTile>();
    private ElektroMapData map;
    public GameObject tilePanel;
    public GameObject editTilePanel;
    public string gamePath = "games/elektro/maps/";

    public ElektroMapData Map => map;


    private void Awake()
    {
        List<string> lan=new List<string>();
        lan.Add("English");
        lan.Add("Dutch");
        map = new ElektroMapData("elektro", "Temp Test",lan);
        gamePath += map.name;
    }

    void Start()
    {
        LoadTiles();



    }

    public void Save()
    {
        try
        {
            // Convert map object to JSON format
            string jsonData = JsonConvert.SerializeObject(map, Formatting.Indented);

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
            foreach (var tileData in map.tiles)
            {
                // Find the existing GameObject by name
                GameObject existingTile = GameObject.Find($"ElectroTile ({tileData.id})");
                ElektroTile tile = existingTile.GetComponent<ElektroTile>();
                Image tempImg = existingTile.GetComponent<Image>();

                if (existingTile != null)
                {
                    string imagePath = Path.Combine(Application.dataPath, "..", "games", map.game, "maps", map.name, "images", tileData.img);

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
                    Debug.LogError($"GameObject ElektroTile ({tileData.id}) not found in the scene!");
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
        foreach (var tile in elektroTiles)
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
                foreach (var tileData in map.tiles)
                {
                    // Find the existing GameObject by name
                    GameObject existingTile = GameObject.Find($"ElectroTile ({tileData.id})");
                    Button btn= existingTile.GetComponent<Button>();
                    btn.onClick.AddListener(() => setIdAction(tileData.id));

                if (existingTile != null)
                    {
                        string imagePath = Path.Combine(Application.dataPath, "..", "games", map.game, "maps", map.name, "images", tileData.img);

                        Texture2D texture = LoadTextureFromFile(imagePath);
                        if (texture != null)
                        {
                            
                            //Debug.Log(imagePath);

                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));


                            ElektroTile newTile = existingTile.AddComponent<ElektroTile>();
                            newTile.Initialize(tileData.id, tileData.img, tileData.meanings);
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
                        Debug.LogError($"GameObject ElektroTile ({tileData.id}) not found in the scene!");
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
        
        PlayerPrefs.SetInt("id",id);

        editTilePanel.SetActive(true);
        tilePanel.SetActive(false);
    }

    //void LoadTiles()
    //{
    //    string filePath = Path.Combine(Application.dataPath, jsonFileName);

    //    Texture2D LoadTextureFromFile(string path)
    //    {
    //        byte[] fileData = File.ReadAllBytes(path);
    //        Texture2D texture = new Texture2D(2, 2);
    //        if (texture.LoadImage(fileData)) // Automatically resizes the texture
    //        {
    //            return texture;
    //        }
    //        return null;
    //    }

    //    if (File.Exists(filePath))
    //    {
    //        string jsonData = File.ReadAllText(filePath);
    //        Debug.Log("Raw JSON: " + jsonData);

    //        try
    //        {
    //            ElektroMapData mapData = JsonConvert.DeserializeObject<ElektroMapData>(jsonData);
    //            Debug.Log("JSON Parsed Successfully!");

    //            foreach (var tileData in mapData.tiles)
    //            {
    //                // Find the existing GameObject by name
    //                GameObject existingTile = GameObject.Find($"ElectroTile ({tileData.id})");

    //                if (existingTile != null)
    //                {
    //                    // Get the ElektroTile component
    //                    //ElektroTile tile = new ElektroTile();

    //                    string imagePath = Path.Combine(Application.dataPath, "..", "games", "elektro", "maps", tileData.img);

    //                    Texture2D texture = LoadTextureFromFile(imagePath);
    //                    if (texture != null )
    //                    {
    //                        Texture2D temp = texture;
    //                        Debug.Log(imagePath);

    //                        Sprite sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));


    //                        ElektroTile tile = existingTile.AddComponent<ElektroTile>();
    //                        tile.Initialize(tileData.id, tileData.img, tileData.meanings);
    //                        Image tempImg = existingTile.AddComponent<Image>();
    //                        tempImg.sprite = sprite;

    //                        if (tile != null)
    //                        {
    //                            tile.Initialize(tileData.id, tileData.img, tileData.meanings);
    //                            Debug.Log($"Initialized: {tile}");
    //                            elektroTiles.Add(tile);
    //                        }
    //                        else
    //                        {
    //                            Debug.LogError($"ElektroTile component not found on: {existingTile.name}");
    //                        }
    //                    }


    //                }
    //                else
    //                {
    //                    Debug.LogError($"GameObject ElektroTile ({tileData.id}) not found in the scene!");
    //                }


    //            }
    //        }
    //        catch (JsonException ex)
    //        {
    //            Debug.LogError("JSON Deserialization Error: " + ex.Message);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError($"JSON file not found: {filePath}");
    //    }
    //}
}

[System.Serializable]
public class ElektroMapData
{
    public int id;
    public string game;
    public string name;
    public List<string> languages;
    public List<TileData> tiles;
    public string img;

    public ElektroMapData(string game, string name, List<string> languages)
    {
        this.game = game;
        this.name = name;
        this.languages = languages;
        this.tiles = new List<TileData>();

        for (int i = 1; i <= 24; i++)
        {
            tiles.Add(new TileData(i, "temp.jpg", languages.Count));
        }

        this.img = null;
    }

    public override string ToString()
    {
        string tileDataString = "";
        foreach (var tile in tiles)
        {
            tileDataString += tile.ToString() + "\n";
        }
        return $"ElektroMapData: ID={id}, Game={game}, Name={name}, Languages=[{string.Join(", ", languages)}], Img={img}\nTiles:\n{tileDataString}";
    }
}

[System.Serializable]
public class TileData
{
    public int id;
    public string img;
    public List<string> meanings;

    public TileData(int id, string img, int languageCount)
    {
        this.id = id;
        this.img = img;
        this.meanings = new List<string>();

        for (int i = 0; i < languageCount; i++)
        {
            meanings.Add("");
        }
    }

    public override string ToString()
    {
        return $"TileData: ID={id}, Img={img}, Meanings=[{string.Join(", ", meanings)}]";
    }
}

