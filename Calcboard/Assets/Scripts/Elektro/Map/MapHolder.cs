using UnityEngine;
using System.Collections.Generic;

public class MapHolder : MonoBehaviour
{
    private static MapHolder _instance; // Singleton instance
    [Header("Fields ElektroMap")]
    [SerializeField] public string gameName="elektro";
    [SerializeField] public string nameMap;
    [SerializeField] public List<string> categoryList;

    public static MapHolder Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing instance in the scene
                _instance = FindAnyObjectByType<MapHolder>();

                if (_instance == null)
                {
                    // If none exists, create a new GameObject and attach this component
                    GameObject singletonObject = new GameObject(typeof(MapHolder).Name);
                    _instance = singletonObject.AddComponent<MapHolder>();
                    DontDestroyOnLoad(singletonObject); // Ensure it persists
                }
            }
            return _instance;
        }
    }

    public ElektroMapData map; // Reference to the map

    public void Initialize(ElektroMapData newMap)
    {
        if (map != null)
        {
            Debug.LogWarning("MapHolder is already initialized with a map.");
            return; // Prevent re-initialization if already set
        }
        map = newMap; // Assign the new map
    }

    void Start()
    {
        if (map != null)
        {
            Debug.Log($"MapHolder initialized with map: {map.MapName}");
        }
    }

    // Optional: Ensure the singleton instance is not destroyed on scene load
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keep instance alive across scenes
        }
    }
}
