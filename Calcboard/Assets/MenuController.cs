using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Confirmation Prompt")]
    [SerializeField] private GameObject comfirmationPrompt = null;



    private string games;

    /**
     * When the user clicks on games on the stating menu, the user will be taken to the games menu
     * **/
    public void Games()
    {
        SceneManager.LoadScene("Games");
    }


    /**
     * When the user clicks on the exit button, the application will close
     * **/
    public void Exit() { Application.Quit(); }
    

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
        AudioListener.volume = volumeSlider.value;
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        comfirmationPrompt.SetActive(false);
    }
}
