using System.Collections.Generic;
using UnityEngine;

public class MusicoMapHolder : MapHolder
{

    private MusicoMapData map;
    public static MusicoMapHolder Instance => GetInstance<MusicoMapHolder>();

    public  MusicoMapData Map
    {
        get => map;
        set => map = value;
    }

    public void Initialize(MusicoMapData newMap)
    {
        if (map != null)
        {
            Debug.LogWarning("MusicoMapHolder is already initialized with a map.");
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


