using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnappingGrid))]
public class Shop : MonoBehaviour
{
    [SerializeField] Module[] modulePrefabs;
    [SerializeField] SnappingGrid stand;

    void Reset() => stand = GetComponent<SnappingGrid>();

    void Start() => Populate();

    void Populate()
    {
        Module article = Instantiate(modulePrefabs[0], stand.GetCellCenter(0, 0), Quaternion.identity);
        stand.TrySnap(article);
    }
}
