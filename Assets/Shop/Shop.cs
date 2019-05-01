using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnappingGrid))]
public class Shop : MonoBehaviour
{
    [SerializeField] Module[] modulePrefabs = null;
    [SerializeField] SnappingGrid stand;
    [SerializeField] int slotCount;

    int currentTier = 1;

    void Reset() => stand = GetComponent<SnappingGrid>();

    void Start() => Populate();

    void Populate()
    {
        for (int i = 0; i < slotCount; i++)
        {
            int index = Random.Range(0, modulePrefabs.Length);
            int tier = Random.Range(1, 1 + 1);
            Module article = Instantiate(modulePrefabs[index], stand.GetCellCenter(i, 0), Quaternion.identity);
            article.Tierify(tier);
            stand.TrySnap(article);
        }
    }
}
