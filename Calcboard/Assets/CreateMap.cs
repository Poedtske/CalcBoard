using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class CreateMap : MonoBehaviour
{

    // Singleton pattern to ensure only one instance of MapManager exists
    public static CreateMap Instance { get; private set; }

    // This will hold the data for the ElektroMap across scenes
    public ElektroMap elektroMapData;

    public TMP_InputField languageInputField; // TextMeshPro InputField
    public Transform languageContainer; // Parent container with Vertical Layout Group
    public GameObject languagePrefab; // Prefab with a Text UI
    public TMP_InputField mapNameInputField; 

    private List<string> languages = new List<string>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void SaveLanguage()
    {
        string newLanguage = languageInputField.text.Trim();

        if (!string.IsNullOrEmpty(newLanguage) && !languages.Contains(newLanguage))
        {
            languages.Add(newLanguage);
            CreateLanguageUI(newLanguage);
            languageInputField.text = ""; // Clear input field
        }
    }

    private void CreateLanguageUI(string language)
    {
        GameObject newLang = Instantiate(languagePrefab, languageContainer);
        TMP_Text languageText = newLang.transform.Find("LanguageText").GetComponent<TMP_Text>();
        languageText.text = language;
    }


    public void SaveAndGoToNextScene()
    {

        // Map name to string
        string mapName = mapNameInputField.text.Trim();

        GameObject map = GameObject.Find("CreateMap");
        ElektroMap elektroMap = map.AddComponent<ElektroMap>();

        elektroMap.Initialize(0, "elektro", mapName, "img", new List<string>(languages));

        elektroMapData = elektroMap;

        // Load the next scene, mometeel is deze scene een test moet dus vervanger worde deor de juiste scene
        SceneManager.LoadScene("TestMapCreate");


        // een gameobject aanmaken dat naar de volgende scene gaat en de mapdata meeneemt

        

    }

}
