using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathManager
{
    private string game;
    private string mapName;
    private List<string> paths;
    private string root;
    private string tempImg;
    private string tempSound;
    private string img;
    private string sound;
    private string map;
    private string gameMapsPath;

    public PathManager (string game, string mapName)
    {
        this.game = game;
        this.mapName = mapName;
        root = Path.Combine(Application.dataPath, "..");
        AssignGamePaths();
        AssignMapPaths(mapName);
        AssignTempPaths();

    }

    public PathManager(string game)
    {
        this.game = game;
        root = Path.Combine(Application.dataPath, "..");
        AssignGamePaths();
    }


    private void AssignMapPaths(string mapName)
    {
        map = Path.Combine(root, "games", game, "maps", mapName);
        img = Path.Combine(map, "images");
        sound = Path.Combine(map, "sounds");
    }

    private void AssignTempPaths()
    {
        tempImg = Path.Combine(root, "tempimages");
        tempSound = Path.Combine(root, "tempsoundfiles");
    }

    private void AssignGamePaths()
    {
        gameMapsPath = Path.Combine(root, "games", game, "maps");
    }



    private void CheckExistanceDirectory(string path)
    {
        if (path == null) throw new("Method is illegal, wrong construction possible, given string is null");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    public string Root()
    {
        CheckExistanceDirectory(root);
        return root;
    }

    public string TempImg()
    {
        CheckExistanceDirectory(tempImg);
        return tempImg;
    }

    public string GameMapsDirectory()
    {
        CheckExistanceDirectory(gameMapsPath);
        return gameMapsPath;
    }

    public string TempSound()
    {
        CheckExistanceDirectory(tempSound);
        return tempSound;
    }

    public string Map()
    {
        CheckExistanceDirectory(map);
        return map;
    }

    public string Sounds()
    {
        CheckExistanceDirectory(sound);
        return sound;
    }

    public string Imgs()
    {
        CheckExistanceDirectory(img);
        return img;
    }

}
