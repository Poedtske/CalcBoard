using UnityEngine;

public abstract class MapHolder : MonoBehaviour
{
    private static MapHolder _instance; // Singleton instance

    public static T GetInstance<T>() where T : MapHolder
    {
        if (_instance == null || !(_instance is T))
        {
            // Try to find an existing instance in the scene
            _instance = FindAnyObjectByType<T>();

            if (_instance == null)
            {
                // If none exists, create a new GameObject and attach the correct type
                GameObject singletonObject = ReturnMapHolderIfNoneExist();
                _instance = singletonObject.AddComponent<T>();
            }
        }
        return _instance as T;
    }

    public static GameObject ReturnMapHolderIfNoneExist()
    {
        GameObject singletonObject = new GameObject(nameof(MapHolder));
        DontDestroyOnLoad(singletonObject);
        return singletonObject;
    }

    // Ensure the singleton instance is not destroyed on scene load
    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keep instance alive across scenes
        }
    }
}
