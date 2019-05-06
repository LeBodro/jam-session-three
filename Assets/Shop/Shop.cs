using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : SnappingGrid
{
    [SerializeField] PriceTag[] tags;
    [SerializeField] AudioSource moneySound;

    int availableTier = 0;
    int tierCount = 6;

    //void Start() => Populate();

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
        int index = Random.Range(0, modules.Count);
        Module article = modules.Get(index);

        article.transform.position = GetCellCenter(x, y);
        TrySnap(article);

        int tier = Random.Range(0, Mathf.Min(availableTier + 1, tierCount));
        article.Tierify(tier);
        tags[ToIndex(x, y)].DisplayPrice(article.Price);

        article.OnBought += ProcessTransaction;
    }

    public override void Deserialize(SnappingGridData data)
    {
        foreach (var tag in tags) tag.Hide();
        foreach (var mData in data.modules)
        {
            Module module = modules.Get(mData.prefab);
            module.Deserialize(mData);
            module.transform.position = GetCellCenter(mData.index % gridSize.x, mData.index / gridSize.x);
            TrySnap(module);
            if (!mData.bought)
            {
                tags[mData.index].DisplayPrice(module.Price);
                module.OnBought += ProcessTransaction;
            }
        }
    }
}
