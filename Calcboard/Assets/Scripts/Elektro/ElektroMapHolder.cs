using UnityEngine;
using System.Collections.Generic;

public class ElektroMapHolder : MapHolder
{
    public static ElektroMapHolder Instance => GetInstance<ElektroMapHolder>();

    private ElektroMapData map; // Reference to the map

    public ElektroMapData Map
    {
        get => map;
        set => map = value;
    }

    public void Initialize(ElektroMapData newMap)
    {
        if (map != null)
        {
            Debug.LogWarning("ElektroMapHolder is already initialized with a map.");
            return;
        }
        map = newMap;
    }

    void Start()
    {
        if (map != null)
        {
            Debug.Log($"ElektroMapHolder initialized with map: {map.MapName}");
        }
    }

    protected override void Awake()
    {
        base.Awake(); // Call base class Awake to handle singleton logic
    }
}
