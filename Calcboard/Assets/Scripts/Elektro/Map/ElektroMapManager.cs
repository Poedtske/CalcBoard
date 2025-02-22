﻿using System.Collections.Generic;
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
        //List<string> lan = new() { "English", "Nederlands" };
        //map = gameObject.AddComponent<ElektroMap>();
        //map.Initialize(1, "elektro", "Temp Test", null, lan);
        //gamePath += map.MapName;
        var mapHolder = FindAnyObjectByType<MapHolder>();
        if (mapHolder != null)
        {
            map = mapHolder.map; // Get the map directly from the MapHolder

            //map.MapName += ".json";
            fileManager = new FileManager<ElektroMapData, ElektroTileData>(map);
        }
        else if(GameObject.Find($"MapHolder")!=null)
        {
            GameObject mapHolderObject = GameObject.Find($"MapHolder");
            mapHolder=mapHolderObject.AddComponent<MapHolder>();
            mapHolder.Initialize(new("test", null, new() { "dutch", "english" }));
            map = mapHolder.map; // Get the map directly from the MapHolder

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
        fileManager.Save(map);
        SceneManager.LoadScene(Scenes.MAIN_MENU);
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
        //string filePath = Path.Combine(Application.dataPath, jsonFileName);

        


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
                            
                        //Debug.Log(imagePath);

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


        editTilePanel.GetComponent<EditTile>().Tile=FindTile(id);
        editTilePanel.SetActive(true);
        tilePanel.SetActive(false);
    }
}





