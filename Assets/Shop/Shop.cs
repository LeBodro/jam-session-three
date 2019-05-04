﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnappingGrid))]
public class Shop : MonoBehaviour
{
    [SerializeField] Module[] modulePrefabs = null;
    [SerializeField] SnappingGrid stand;
    [SerializeField] Vector2Int gridSize;
    [SerializeField] AudioSource moneySound;

    List<Pool<Module>> modulePools;
    IList<Module> availableChips;
    int availableTier = 0;
    int tierCount = 6;

    void Reset() => stand = GetComponent<SnappingGrid>();

    void Start()
    {
        InitializePools();
        availableChips = new List<Module>(gridSize.x * gridSize.y);
        Populate();
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

    public void Populate(int maxTier = 1)
    {
        foreach (var chip in availableChips) chip.RePool();
        availableChips.Clear();
        availableTier = maxTier;
        for (int x = 0; x < gridSize.x; x++)
            for (int y = 0; y < gridSize.y; y++)
                AddArticle(x, y);
    }

    void ProcessTransaction(Module article)
    {
        availableChips.Remove(article);
        article.OnBought -= ProcessTransaction;
        moneySound.Play();
    }

    void AddArticle(int x, int y)
    {
        int index = Random.Range(0, modulePrefabs.Length);
        Module article = modulePools[index].Get();

        Vector2 position = stand.GetCellCenter(x, y);
        article.transform.position = position;
        stand.TrySnap(article);

        int tier = Random.Range(0, Mathf.Min(availableTier + 1, tierCount));
        article.Tierify(tier);

        article.OnBought += ProcessTransaction;

        availableChips.Add(article);
    }
}
