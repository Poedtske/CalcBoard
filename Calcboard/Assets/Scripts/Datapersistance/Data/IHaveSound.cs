using UnityEngine;
using System.Collections.Generic;

public interface IHaveSound
{
    //List<string> SoundFiles { get; set; } // Property for storing the sound file name

    string GetSoundFile(string soundFileName); // Method to get the sound file
}
