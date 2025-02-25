using UnityEngine;
using System.Collections.Generic;

public interface IHasSound
{
    //List<string> SoundFiles { get; set; } // Property for storing the sound file name

    string SoundFile { get; set; } // Method to get the sound file
}
