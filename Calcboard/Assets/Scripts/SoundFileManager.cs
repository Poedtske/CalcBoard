using System;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEngine;

public class SoundFileManager
{
    private string selectedFilePath;
    private string tempSound;
    private PathManager pathManager;

    public SoundFileManager(string game, string mapName)
    {
        this.pathManager = new(game,mapName);
    }

    public string OpenSoundFilePicker(string fileName)
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Select Sound", "mp3,wav,ogg", "", false);
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            selectedFilePath = paths[0];
            return SaveAndLoadNewSoundInTemp(selectedFilePath, fileName);
        }
        else
        {
            return null;
        }
    }

    private string SaveAndLoadNewSoundInTemp(string filePath, string fileName)
    {
        byte[] fileData = File.ReadAllBytes(filePath);

        string fileExtension = Path.GetExtension(filePath);
        string tempFileName = "temp_" + fileName + fileExtension;
        string tempFilePath = Path.Combine(pathManager.TempSound(), tempFileName);

        // Delete existing temp files for this tile
        foreach (string tempFile in Directory.GetFiles(pathManager.TempSound(), $"temp_{fileName}.*"))
        {
            File.Delete(tempFile);
        }

        // Save new temp sound file
        File.WriteAllBytes(tempFilePath, fileData);
        tempSound = tempFileName;
        return tempSound;
    }

    internal List<string> GetSavedSoundFileNames()
    {

        string[] files = Directory.GetFiles(pathManager.Sounds()); // Get all files in the directory
        List<string> soundFileNames = new List<string>();

        foreach (string filePath in files)
        {
            soundFileNames.Add(Path.GetFileName(filePath)); // Extract file name from path
        }

        return soundFileNames;
    }



    public void SaveSound(SoundFile file)
    {
        if (tempSound == null)
            return;

        string fileExtension = Path.GetExtension(tempSound);
        string savedFileName = file.Name + fileExtension;
        string savedFilePath = Path.Combine(pathManager.Sounds(), savedFileName);

        // Move temp file to saved sounds
        string tempFilePath = Path.Combine(pathManager.TempSound(), tempSound);
        if (File.Exists(tempFilePath))
        {
            File.Move(tempFilePath, savedFilePath);
            file.Name = savedFileName;
            tempSound = null;
        }
    }
}
