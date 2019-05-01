using System.Collections.Generic;
using UnityEngine;

public class StatDictionnary : MonoBehaviour
{
    [SerializeField] Stat[] stats;

    IDictionary<string, Stat> statDict;

    public Stat this [string statName] { get { return statDict[statName]; } }

    void Awake()
    {
        statDict = new Dictionary<string, Stat>(stats.Length);
        for (int i = 0; i < stats.Length; i++)
            statDict.Add(stats[i].ToKeyValuePair());
    }

    void Update()
    {
        foreach (var stat in stats)
            stat.Update();
    }
}
