using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersistantSceneSingleton<T> : SceneSingleton<T> where T : PersistantSceneSingleton<T>
{
    public override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}
