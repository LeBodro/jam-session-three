using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class GameObjectExtension
{
    public static IList<T> FindAllObjectsOfType<T>() where T : MonoBehaviour
    {
        IList<T> objs = new List<T>();
        T[] allObjects = Resources.FindObjectsOfTypeAll<T>();

        foreach (T obj in allObjects)
        {
            if (obj.gameObject.scene.rootCount != 0)
            {
                objs.Add(obj);
            }
        }

        return objs;
    }
}
