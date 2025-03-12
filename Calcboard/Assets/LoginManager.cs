using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;



public class LoginManager : MonoBehaviour
{


    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Text errorMessage;


    public GameObject loginContainer; // The login UI panel
    public GameObject menu;  // The menu container panel

    public void Start()
    {
        loginButton.onClick.AddListener(AttemptLogin);
        menu.SetActive(false);
    }

    void AttemptLogin()
    {

        if (usernameInput == null || passwordInput == null)
        {
            Debug.LogError("Username or Password input field is not assigned!");
            return;
        }

        if (ApiManager.Instance == null)
        {
            Debug.LogError("ApiManager instance is NULL! Make sure it's in the scene.");
            return;
        }

        string username = usernameInput.text;
        string password = passwordInput.text;

        ApiManager.Instance.StartCoroutine(ApiManager.Instance.SendLoginRequest(username, password, HandleLoginResponse));
    }

    void HandleLoginResponse(bool success, string message)
    {
        if (success)
        {
            ShowMenu();
        }
        else
        {
            ShowLogin();
            errorMessage.text = message;
        }
    }


    public void ShowLogin()
    {
        loginContainer.SetActive(true);
        menu.SetActive(false);
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
        loginContainer.SetActive(false);
    }

   

}
