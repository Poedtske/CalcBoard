﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManagerElektro : MonoBehaviour
{
    public string jsonFileName = "TempElektroMap.json"; // Adjust the path if needed
    private List<ElektroTile> elektroTiles = new List<ElektroTile>();
    private ElektroMap map;
    public GameObject tilePanel;
    public GameObject editTilePanel;

    public ElektroMap Map => map;


    private void Awake()
    {
        List<string> lan=new List<string>();
        lan.Add("English");
        lan.Add("Dutch");
        map = new ElektroMap("elektro", "Temp Test",lan);
    }

    void Start()
    {
        LoadTiles();



    }

    private void Save()
    {

    }

    public ElektroTile FindTile(int id)
    {
        foreach (var tile in elektroTiles)
        {
            if (tile.TileId == id)
            {
                return tile;
            }
        }
        return null;
    }

        void LoadTiles()
    {
        //string filePath = Path.Combine(Application.dataPath, jsonFileName);

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
                            
                            Debug.Log(imagePath);

                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));


                            ElektroTile newTile = existingTile.AddComponent<ElektroTile>();
                            newTile.Initialize(tileData.id, tileData.img, tileData.meanings);
                            Image tempImg = existingTile.AddComponent<Image>();
                            tempImg.sprite = sprite;
                            

                            if (newTile != null)
                            {
                                
                                Debug.Log($"Initialized: {newTile}");
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
    //            ElektroMap mapData = JsonConvert.DeserializeObject<ElektroMap>(jsonData);
    //            Debug.Log("JSON Parsed Successfully!");

    //            foreach (var tileData in mapData.tiles)
    //            {
    //                // Find the existing GameObject by name
    //                GameObject existingTile = GameObject.Find($"ElectroTile ({tileData.id})");

    //                if (existingTile != null)
    //                {
    //                    // Get the ElektroTile component
    //                    //ElektroTile newTile = new ElektroTile();

    //                    string imagePath = Path.Combine(Application.dataPath, "..", "games", "elektro", "maps", tileData.img);

    //                    Texture2D texture = LoadTextureFromFile(imagePath);
    //                    if (texture != null )
    //                    {
    //                        Texture2D temp = texture;
    //                        Debug.Log(imagePath);

    //                        Sprite sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));


    //                        ElektroTile newTile = existingTile.AddComponent<ElektroTile>();
    //                        newTile.Initialize(tileData.id, tileData.img, tileData.meanings);
    //                        Image tempImg = existingTile.AddComponent<Image>();
    //                        tempImg.sprite = sprite;

    //                        if (newTile != null)
    //                        {
    //                            newTile.Initialize(tileData.id, tileData.img, tileData.meanings);
    //                            Debug.Log($"Initialized: {newTile}");
    //                            elektroTiles.Add(newTile);
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
public class ElektroMap
{

    public int id;
    public string game;
    public string name;
    public List<string> languages;
    public List<TileData> tiles;
    public string img;

    public ElektroMap(string game, string name, List<string> languages)
    {
        this.game = game;
        this.name = name;
        this.languages = languages;
        this.tiles = new List<TileData>();
        for (int i = 1; i <= 24; i++)
        {
            tiles.Add(new TileData(i,"temp.jpg"));
        }
        this.img = null;
    }
}

[System.Serializable]
public class TileData
{
    public int id;
    public string img;
    public List<string> meanings;

    public TileData(int id, string img)
    {
        this.id = id;
        this.img = img;
        this.meanings = new List<string>();
    }
}

