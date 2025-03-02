using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Linq;

[System.Serializable]
public class MusicoTileData : CalcBoardTileData<MusicoTileData>, IHasImg, IHasSound<MusicoSoundFile>
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
    public MusicoSoundFile SoundFile { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    //List<string> IHasSound.SoundFiles { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    //public List<string> Words { get => words; set => words=value; }

    public MusicoSoundFile GetSoundFile(string soundFileName)
    {
        return this.soundFiles.Find(soundFile => soundFile.Name == soundFileName);
    }

    public void AddSoundFile(MusicoSoundFile soundFile)
    {
        soundFiles.Add(soundFile);
    }

    public void RemoveSoundFile(MusicoSoundFile soundFile)
    {
        soundFiles.Remove(soundFile);
    }

    public bool GetSoundFile(MusicoSoundFile soundFile)
    {
        return soundFiles.Find(file => file.Equals(soundFile))!=null;
    }

    internal void AddExistantSoundFile(string soundFileName)
    {
        throw new System.NotImplementedException();
    }
    public override string ToString()
    {
        string tileDataString = string.Join("\n", soundFiles.Select(file => file.ToString()));
        return $"MusicoMapData: ID={id}, Img={img}\nSoundFiles:\n{tileDataString}";
    }
}
