using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAIISceneSingleton<T> : CommonSingleton where T : RAIISceneSingleton<T>
{
    static T _instance;
    protected static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var i = new GameObject();
                _instance = i.AddComponent<T>();
                i.name = GetGameObjectNameFromType(_instance.GetType());
            }
            return _instance;
        }
    }

    public virtual void OnDestroy()
    {
        if (Instance == gameObject)
        {
            _instance = null;
        }
    }
}
