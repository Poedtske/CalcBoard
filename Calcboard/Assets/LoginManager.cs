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

    private string apiUrl = "http://localhost:8081/login";


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

        StartCoroutine(SendLoginRequest(username, password));

    }

    IEnumerator SendLoginRequest(string username, string password)
    {
        // Prepare JSON payload
        string jsonData = $"{{\"email\":\"{username}\",\"password\":\"{password}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login successful: " + request.downloadHandler.text);
                loginContainer.SetActive(false);
                menu.SetActive(true);
            }
            else
            {
                Debug.LogError("Login failed: " + request.downloadHandler.text);
                errorMessage.text = "Invalid username or password!";
            }
        }
    }

}
