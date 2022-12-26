using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Attempting to create {typeof(T).Name}, but one already exists");
            Destroy(gameObject);
        }

        Instance = (T)this;
        DontDestroyOnLoad(gameObject);
    }
}
