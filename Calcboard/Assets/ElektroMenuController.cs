using System.Collections.Generic;
using UnityEngine;

public class ElektroMenuController : MonoBehaviour
{

    public GameObject backgroundMusic;
    private void Awake()
    {
        List<AudioSource> toBeDeleted = Scenes.DeleteCrossScenesAudio(FindObjectsByType<AudioSource>(new()));
        toBeDeleted.ForEach(Destroy);
        if (backgroundMusic != null)
        {
            DontDestroyOnLoad(backgroundMusic);
        }
    }
}
