using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour
{
    public InputField languageInputField;
    public Transform languageContainer; // Parent container with Vertical Layout Group
    public GameObject languagePrefab; // Prefab with a Text UI

    private List<string> languages = new List<string>();

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
        newLang.GetComponent<Text>().text = language;
    }

}
