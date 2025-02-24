using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System;

public class DataPersistanceManager : MonoBehaviour
{

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private FileDataHandler dataHandler;
    private ElektroMapData mapData;
    public static DataPersistanceManager instance {  get; private set; }
    private List<IDataPersistance> dataPersistanceObjects;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one DataPersistence Manager in scene");
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersitenceObjects();
        LoadGame();
    }

    private List<IDataPersistance> FindAllDataPersitenceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects= FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public void NewGame()
    {
        this.mapData = new ElektroMapData();
    }

    public void LoadGame()
    {
        //Load any saved data from a file using the data handler
        this.mapData = dataHandler.Load();

        //if no data can be loaded, initialize to a new game
        if(this.mapData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach(IDataPersistance obj in dataPersistanceObjects)
        {
            obj.LoadData(mapData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistance obj in dataPersistanceObjects)
        {
            obj.SaveData(ref mapData);
        }

        dataHandler.Save(mapData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
