using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnappingGrid))]
public class Shop : MonoBehaviour
{
    [SerializeField] Module[] modulePrefabs = null;
    [SerializeField] SnappingGrid stand;
    [SerializeField] Vector2Int gridSize;

    int availableTier = 1;

    void Reset() => stand = GetComponent<SnappingGrid>();

    void Start() => Populate();

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

    void ProcessTransaction(Module article, int freedSlot)
    {
        // Ka-ching!
    }

    void AddArticle(int x, int y)
    {
        Debug.Log("add");
        int index = Random.Range(0, modulePrefabs.Length);
        int tier = Random.Range(1, availableTier + 1);
        Module article = Instantiate(modulePrefabs[index], stand.GetCellCenter(x, y), Quaternion.identity);
        article.Tierify(tier);
        article.OnBought += (a) => ProcessTransaction(a, x);
        stand.TrySnap(article);
    }
}
