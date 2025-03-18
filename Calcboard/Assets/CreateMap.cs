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
    public GameObject CategoryListItemPrefab; // Prefab for category list items

    private List<string> languages = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        // Instantiate prefab and set parent
        GameObject entry = Instantiate(CategoryListItemPrefab, languageContainer);
        entry.name = $"LanguageEntry_{language}";

        // Find child components
        TMP_Text languageText = entry.transform.Find("Elements/Name").GetComponent<TMP_Text>();
        Button deleteButton = entry.transform.Find("Elements/Delete").GetComponent<Button>();

        // Assign values
        languageText.text = language;
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

        ElektroMapHolder holder = FindAnyObjectByType<ElektroMapHolder>();
        holder.Map = new(mapName, null, languages);

        SceneManager.LoadScene(Scenes.ELEKTRO_MAP_CREATION);
    }
}
