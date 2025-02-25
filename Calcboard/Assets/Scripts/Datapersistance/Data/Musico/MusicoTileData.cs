using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MusicoTileData : CalcBoardTileData<MusicoTileData>, IHasImg, IHasSound
{
    private string img;
    private List<MusicoSoundFile> soundFiles;

    public MusicoTileData(int id, string img) : base(id)
    {
        Img = img;
        this.soundFiles = new();
    }

    public MusicoTileData() { }

    public string Img { get => img; set => img=value; }

    public List<MusicoSoundFile> SoundFiles { get => soundFiles; set => soundFiles = value; }
    //List<string> IHasSound.SoundFiles { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    //public List<string> Words { get => words; set => words=value; }

    public string GetSoundFile(string soundFileName)
    {
        return this.soundFiles.Find(soundFile => soundFile.Name == soundFileName).Name;
    }


}
