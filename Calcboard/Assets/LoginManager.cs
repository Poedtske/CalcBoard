using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (IsValidLogin(username, password))
        {
            Debug.Log("Login Successful!");
            loginContainer.SetActive(false);
            menu.SetActive(true);

        }
        else
        {
            errorMessage.text = "Invalid username or password!";
        }
    }

    bool IsValidLogin(string username, string password)
    {
        // Temporary hardcoded credentials (replace with a real authentication system)
        return username == "Player1" && password == "1234";
    }


}
