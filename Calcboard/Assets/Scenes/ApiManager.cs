using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class ApiManager : MonoBehaviour
{
    public static ApiManager Instance { get; private set; }

    private string apiUrl = "http://localhost:8081";


    private void Awake()
    {
        Debug.Log("ApiManager Awake() called!");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("ApiManager instance set.");
        }
        else
        {
            Debug.LogWarning("Duplicate ApiManager detected! Destroying...");
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class LoginResponse
    {
        public long userId;
        public string token; // Ensure backend returns this
    }

    public IEnumerator SendLoginRequest(string username, string password, System.Action<bool, string> callback)
    {
        string jsonData = $"{{\"email\":\"{username}\",\"password\":\"{password}\"}}";

        using (UnityWebRequest request = new UnityWebRequest($"{apiUrl}/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                PlayerPrefs.SetInt("UserId", (int)response.userId);
                PlayerPrefs.SetString("Token", response.token);

                Debug.Log("Login successful: " + request.downloadHandler.text);
                callback(true, "Login successful");
            }
            else
            {
                Debug.LogError("Login failed: " + request.downloadHandler.text);
                callback(false, "Invalid username or password!");
            }
        }
    }


    public IEnumerator ValidateTokenAndLoadScene(string token)
    {
        string validateUrl = $"{apiUrl}/validate-token"; // Replace with actual API URL
        using (UnityWebRequest request = UnityWebRequest.Get(validateUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

          
            yield return new WaitForSeconds(1); // Give scene time to load

            LoginManager loginManager = Component.FindAnyObjectByType<LoginManager>();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Token is valid.");
                loginManager?.ShowMenu();  // Show menu if valid
            }
            else
            {
                Debug.LogError("Token is invalid or expired.");
                PlayerPrefs.DeleteKey("Token");
                PlayerPrefs.DeleteKey("UserId");
                loginManager?.ShowLogin(); // Show login if invalid
            }
        }
    }


    public IEnumerator SendMapToBackend(string jsonData, string imagesFolderPath)
    {
       
        string token = PlayerPrefs.GetString("Token", "");

        Debug.Log(" Preparing JSON Data: " + jsonData);
        Debug.Log(" Using API URL: " + apiUrl);
        Debug.Log(" Image path: " +  imagesFolderPath);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
    {
        new MultipartFormDataSection("mapData", jsonData, "application/json") // Ensures correct format
    };

        string[] imageFiles = Directory.GetFiles(imagesFolderPath, "*.png")
                              .Concat(Directory.GetFiles(imagesFolderPath, "*.jpg"))
                              .Concat(Directory.GetFiles(imagesFolderPath, "*.jpeg"))
                              .Concat(Directory.GetFiles(imagesFolderPath, "*.bmp"))
                              .Concat(Directory.GetFiles(imagesFolderPath, "*.wav"))
                              .ToArray();

        foreach (string imagePath in imageFiles)
        {
            if (File.Exists(imagePath))
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                formData.Add(new MultipartFormFileSection("tileImages", imageBytes, Path.GetFileName(imagePath), "image/png"));
                Debug.Log(" Adding image: " + Path.GetFileName(imagePath) + " (Size: " + imageBytes.Length + " bytes)");
            }
            else
            {
                Debug.LogError("File does not exist: " + imagePath);
            }
        }

        using (UnityWebRequest request = UnityWebRequest.Post($"{apiUrl}/maps/save", formData))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token); // Do NOT manually set Content-Type

            yield return request.SendWebRequest();

            Debug.Log(" Response Code: " + request.responseCode);
            Debug.Log(" Response Text: " + request.downloadHandler.text);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(" Map and images successfully saved on backend!");
            }
            else
            {
                Debug.LogError(" Failed to save map on backend: " + request.downloadHandler.text);
            }
        }
    }
}
