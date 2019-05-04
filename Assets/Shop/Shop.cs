using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : SnappingGrid
{
    [SerializeField] PriceTag[] tags;
    [SerializeField] Module[] modulePrefabs = null;
    [SerializeField] AudioSource moneySound;

    List<Pool<Module>> modulePools;
    int availableTier = 0;
    int tierCount = 6;

    void Start()
    {
        InitializePools();
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
        Clear();
        availableTier = maxTier;
        for (int x = 0; x < gridSize.x; x++)
            for (int y = 0; y < gridSize.y; y++)
                AddArticle(x, y);
    }

    void ProcessTransaction(Module article)
    {
        article.OnBought -= ProcessTransaction;
        moneySound.Play();
    }

    protected override void OnUnsnap(Module m, int index)
    {
        tags[index].Hide();
    }

    void AddArticle(int x, int y)
    {
        int index = Random.Range(0, modulePrefabs.Length);
        Module article = modulePools[index].Get();

        article.transform.position = GetCellCenter(x, y);
        TrySnap(article);

        int tier = Random.Range(0, Mathf.Min(availableTier + 1, tierCount));
        article.Tierify(tier);
        tags[ToIndex(x, y)].DisplayPrice(article.Price);

        article.OnBought += ProcessTransaction;
    }
}
