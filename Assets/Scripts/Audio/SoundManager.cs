using UnityEngine;

public class SoundManager : MonoBehaviour, ISoundManager
{
    public static SoundManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Attempting to create a SoundManager, but one already exists");
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

public interface ISoundManager
{
}