using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SnappingGrid))]
public class Shop : MonoBehaviour
{
    [SerializeField] Module[] modulePrefabs = null;
    [SerializeField] SnappingGrid stand;
    [SerializeField] Vector2Int gridSize;

    List<Pool<Module>> modulePools;
    int availableTier = 1;

    void Reset() => stand = GetComponent<SnappingGrid>();

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
        availableTier = maxTier;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (stand.GetModuleAt(x, y) == null)
                    AddArticle(x, y);
            }
        }
    }

    void ProcessTransaction(Module article)
    {
        article.OnBought -= ProcessTransaction;
        // Ka-ching!
    }

    void AddArticle(int x, int y)
    {
        int index = Random.Range(0, modulePrefabs.Length);
        Module article = modulePools[index].Get();

        Vector2 position = stand.GetCellCenter(x, y);
        article.transform.position = position;
        stand.TrySnap(article);

        int tier = Random.Range(1, availableTier + 1);
        article.Tierify(tier);

        article.OnBought += ProcessTransaction;
    }
}
