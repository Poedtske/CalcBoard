using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElektroMenuController : MonoBehaviour
{
    private bool playMusic = true;
    public Image image;

    public Sprite playSound;
    public Sprite muteSound;
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

    public void Music()
    {
        if (playMusic)
        {
            backgroundMusic.GetComponent<AudioSource>().Stop();
            playMusic = false;
            image.sprite = muteSound;
        }
        else
        {
            backgroundMusic.GetComponent<AudioSource>().Play();
            playMusic = true;
            image.sprite = playSound;
        }
    }
}
