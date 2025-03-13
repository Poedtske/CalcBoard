using UnityEngine;
using System.Collections.Generic;

public static class Scenes
{
    //Main Menu
    public static readonly string MAIN_MENU = "MainMenu";

    //Elektro
    public static readonly string ELEKTRO_ACTIONS = "ElektroActions";
    public static readonly string ELEKTRO_GAME = "ElektroGame";
    public static readonly string ELEKTRO_MAP_CREATION = "ElektroMapCreation";

    //Musico
    public static readonly string MUSICO_ACTIONS = "MusicoActions";
    public static readonly string MUSICO_GAME = "MusicoGame";
    public static readonly string MUSICO_MAP_CREATION = "MusicoMapCreation";

    public static List<AudioSource> DeleteCrossScenesAudio(AudioSource[] audioSources)
    {
        List<AudioSource> toBeRemoved = new();
        foreach (AudioSource audio in audioSources)
        {
            if (audio.gameObject.scene.buildIndex == -1) // Objects marked as DontDestroyOnLoad have this index
            {
                toBeRemoved.Add(audio);
            }
        }
        return toBeRemoved;

    }
}
