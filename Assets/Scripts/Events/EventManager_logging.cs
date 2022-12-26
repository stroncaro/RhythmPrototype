#if UNITY_EDITOR
using UnityEngine;

public partial class EventManager
{
    [SerializeField] private bool logging = true;

    private void Log(string message)
    {
        if (!logging) return;
        Debug.Log($"EventManager => {message}");
    }
}
#endif
