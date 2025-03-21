using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;
    [SerializeField] private GameObject mapsContainer;

    [Header("Confirmation Prompt")]
    [SerializeField] private GameObject comfirmationPrompt = null;


    public void Awake()
    {
        string token = PlayerPrefs.GetString("Token");

        if (!string.IsNullOrEmpty(token))
        {

            ApiManager.Instance.StartCoroutine(ApiManager.Instance.ValidateTokenAndLoadScene(token));

        }
        else
        {
            Debug.LogError("No token.");

        }

        List<AudioSource> toBeDeleted = Scenes.DeleteCrossScenesAudio(FindObjectsByType<AudioSource>(new()));
        toBeDeleted.ForEach(Destroy);

        
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

    public void VolumeReset(string MenuType)
    {
       if(MenuType == "backgroundMusic")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }   
    }

    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        comfirmationPrompt.SetActive(false);
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("Token");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(Scenes.MAIN_MENU);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
