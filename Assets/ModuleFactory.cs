using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleFactory : MonoBehaviour
{
    [SerializeField] Module[] modulePrefabs = null;
    List<Pool<Module>> modulePools;

    public int Count { get => modulePools.Count; }

    void Awake()
    {
        InitializePools();
    }

    void InitializePools()
    {
        modulePools = new List<Pool<Module>>(modulePrefabs.Length);
        for (int i = 0; i < modulePrefabs.Length; i++)
        {
            int prefabIndex = i;
            modulePools.Add(new Pool<Module>(() => Instantiate(modulePrefabs[prefabIndex])));
        }
    }

    public Module Get(int prefabId) => modulePools[prefabId].Get();
}
