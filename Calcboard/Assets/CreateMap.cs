using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateMap : MonoBehaviour
{
    public static CreateMap Instance { get; private set; }

    public TMP_InputField languageInputField; // User types language name here
    public Transform languageContainer; // Parent container (should have VerticalLayoutGroup)
    public TMP_InputField mapNameInputField;

    private List<string> languages = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
        // Create Parent GameObject
        GameObject entry = new GameObject($"LanguageEntry_{language}");
        
        GameObject contentBox = GameObject.Find("ElektroCategoriesContent");
        entry.transform.SetParent(contentBox.transform, false);
        HorizontalLayoutGroup layout = entry.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 10;
        layout.childAlignment = TextAnchor.MiddleLeft;

        // Create Language Text
        GameObject textObj = new GameObject("LanguageText");
        textObj.transform.SetParent(entry.transform, false);
        TMP_Text languageText = textObj.AddComponent<TextMeshProUGUI>();
        languageText.text = language;
        languageText.fontSize = 36;
        languageText.color = Color.white;
        languageText.alignment = TextAlignmentOptions.Left;

        // Create Delete Button
        GameObject buttonObj = new GameObject("DeleteButton");
        buttonObj.transform.SetParent(entry.transform, false);
        Button deleteButton = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = Color.red; // Make the button red

        // Add Text to Button
        GameObject buttonTextObj = new GameObject("ButtonText");
        buttonTextObj.transform.SetParent(buttonObj.transform, false);
        TMP_Text buttonText = buttonTextObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = "X";
        buttonText.fontSize = 30;
        buttonText.alignment = TextAlignmentOptions.Center;

        // Resize and Position Elements
        RectTransform entryRect = entry.GetComponent<RectTransform>();
        entryRect.sizeDelta = new Vector2(350, 60); // Width and height
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(400, 60);
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(60, 60);
        RectTransform buttonTextRect = buttonTextObj.GetComponent<RectTransform>();
        buttonTextRect.sizeDelta = new Vector2(60, 60);

        // Set Button Action
        deleteButton.onClick.AddListener(() => RemoveLanguage(language, entry));
    }

    private void RemoveLanguage(string language, GameObject entry)
    {
        languages.Remove(language);
        Destroy(entry);
    }

    public void SaveAndGoToNextScene()
    {
        string mapName = mapNameInputField.text.Trim();

        MapHolder holder = FindAnyObjectByType<MapHolder>();
        holder.map = new(mapName, null, languages);

        SceneManager.LoadScene("ElektroMapCreation");
    }
}
