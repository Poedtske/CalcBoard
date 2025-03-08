using UnityEngine;

public class ApiManager
{



    IEnumerator ValidateTokenAndLoadScene(string token)
    {
        string validateUrl = "http://yourbackend.com/validate-token"; // Replace with actual API URL
        using (UnityWebRequest request = UnityWebRequest.Get(validateUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            SceneManager.LoadScene(Scenes.MAIN_MENU); // Load scene first

            yield return new WaitForSeconds(1); // Give scene time to load

            LoginManager loginManager = FindObjectOfType<LoginManager>();

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


    IEnumerator SendMapToBackend(string jsonData, string imagesFolderPath)
    {
        string apiUrl = "http://localhost:8081/maps/save";
        string token = PlayerPrefs.GetString("Token", "");

        Debug.Log(" Preparing JSON Data: " + jsonData);
        Debug.Log(" Using API URL: " + apiUrl);

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
                formData.Add(new MultipartFormFileSection("file", imageBytes, Path.GetFileName(imagePath), "image/png"));
                Debug.Log(" Adding image: " + Path.GetFileName(imagePath) + " (Size: " + imageBytes.Length + " bytes)");
            }
        }

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, formData))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token); // Do NOT manually set Content-Type

            Debug.Log(" Headers Set:");
            Debug.Log("  - Authorization: Bearer " + token);
            Debug.Log("  - Content-Type is automatically set by Unity");

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
