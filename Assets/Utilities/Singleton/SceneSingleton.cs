using UnityEngine;

public abstract class SceneSingleton<T> : CommonSingleton where T : SceneSingleton<T>
{
    protected static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are multiple instance of " + GetType().ToString());
        }
        else
        {
            Instance = this as T;
            gameObject.name = GetGameObjectNameFromType(GetType());
        }
    }

    public virtual void OnDestroy()
    {
        if (Instance == gameObject)
        {
            Instance = null;
        }
    }

    protected static bool IsAvailable
    {
        get
        {
            return Instance != null;
        }
    }
}
