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
    private ElektroMap map;
    public GameObject tilePanel;
    public GameObject editTilePanel;
    public string gamePath = "games/elektro/maps/";

    public ElektroMap Map => map;


    private void Awake()
    {
        List<string> lan=new List<string>();
        lan.Add("English");
        lan.Add("Dutch");
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

    //            foreach (var tile in mapData.tiles)
    //            {
    //                // Find the existing GameObject by name
    //                GameObject existingTile = GameObject.Find($"ElectroTile ({tile.id})");

    //                if (existingTile != null)
    //                {
    //                    // Get the ElektroTile component
    //                    //ElektroTile tile = new ElektroTile();

    //                    string imagePath = Path.Combine(Application.dataPath, "..", "games", "elektro", "maps", tile.img);

    //                    Texture2D texture = LoadTextureFromFile(imagePath);
    //                    if (texture != null )
    //                    {
    //                        Texture2D temp = texture;
    //                        Debug.Log(imagePath);

    //                        Sprite sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));


    //                        ElektroTile tile = existingTile.AddComponent<ElektroTile>();
    //                        tile.Initialize(tile.id, tile.img, tile.meanings);
    //                        Image tempImg = existingTile.AddComponent<Image>();
    //                        tempImg.sprite = sprite;

    //                        if (tile != null)
    //                        {
    //                            tile.Initialize(tile.id, tile.img, tile.meanings);
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
    //                    Debug.LogError($"GameObject ElektroTile ({tile.id}) not found in the scene!");
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

    public ElektroMapData(ElektroMap map)
    {
        this.game = map.Game;
        this.name = map.MapName;
        this.languages = map.Languages;
        this.tiles = new List<TileData>();

        foreach (var tile in map.Tiles)
        {
            this.tiles.Add(new(tile));
        }

        //not implemented
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

    public TileData(ElektroTile tile)
    {
        this.id = tile.Id;
        this.img = tile.Img;
        this.meanings = tile.Meanings;
    }

    public override string ToString()
    {
        return $"TileData: ID={id}, Img={img}, Meanings=[{string.Join(", ", meanings)}]";
    }
}

