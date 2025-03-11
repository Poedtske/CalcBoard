//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class LoadPrefs : MonoBehaviour
//{
//    [Header("General Setting")]
//    [SerializeField] private bool canUse = false;
//    [SerializeField] private MenuController menuController;

//    [Header("Volume Setting")]
//    [SerializeField] private TMP_Text volumeTextValue=null;
//    [SerializeField] private Slider volumeSlider = null;

//    [Header("Brightness Setting")]
//    [SerializeField] private Slider brightnessSlider = null;
//    [SerializeField] private TMP_Text brightnessTextValue = null;

//    [Header("Quality Level Setting")]
//    [SerializeField] private TMP_Dropdown qualityDropdown;

//    [Header("Fullscreen Setting")]
//    [SerializeField] private Toggle fullScreenToggle;

//    [Header("Timer Setting")]
//    [SerializeField] private TMP_Text timerTextValue = null;
//    [SerializeField] private Slider TimerSlider = null;

//    private void Awake()
//    {
//        if (canUse)
//        {
//            if (PlayerPrefs.HasKey("masterVolume"))
//            {
//                float localVolume = PlayerPrefs.GetFloat("masterVolume");

//                volumeTextValue.text = localVolume.ToString("0.0");
//                volumeSlider.value = localVolume;
//                AudioListener.volume = localVolume;
//            }
//            else
//            {
//                menuController.ResetButton();
//            }

//            if (PlayerPrefs.HasKey("masterQuality"))
//            {
//                int localQuality = PlayerPrefs.GetInt("masterQuality");
//                qualityDropdown.value = localQuality;
//                QualitySettings.SetQualityLevel(localQuality);
//            }
//            else
//            {
//                menuController.ResetButton();
//            }

//            if (PlayerPrefs.HasKey("masterFullscreen"))
//            {
//                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");

//                if(localFullscreen == 1)
//                {
//                    Screen.fullScreen = true;
//                    fullScreenToggle.isOn = true;
//                }
//                else
//                {
//                    Screen.fullScreen= false;
//                    fullScreenToggle.isOn = false;
//                }
//            }
//            else
//            {
//                menuController.ResetButton();
//            }

//            if (PlayerPrefs.HasKey("masterBrightness"))
//            {
//                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");
//                brightnessTextValue.text = localBrightness.ToString("0.0");
//                brightnessSlider.value = localBrightness;
//                //change brightness
//            }
//            else
//            {
//                menuController.ResetButton();
//            }

//            if (PlayerPrefs.HasKey("masterTimer"))
//            {
//                float localTimer = PlayerPrefs.GetFloat("masterTimer");
//                timerTextValue.text = localTimer.ToString("0.0");
//                TimerSlider.value = localTimer;
//            }
//            else
//            {
//                menuController.ResetButton();
//            }

//        }
//    }

//}
