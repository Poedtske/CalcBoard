using UnityEngine;
using System.Collections.Generic;

public interface IHasSound<T> where T : SoundFile
{
    //List<string> SoundFiles { get; set; } // Property for storing the sound file name

    List<T> SoundFiles { get; set; } // Method to get the sound file
    void AddSoundFile(T soundFile);
    void RemoveSoundFile(T soundFile);
    bool GetSoundFile(T soundFile);
    T GetSoundFile(string fileName);
}
