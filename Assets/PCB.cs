
using UnityEngine;

[RequireComponent(typeof(SnappingGrid))]
public class PCB : MonoBehaviour
{
    [SerializeField] SnappingGrid grid = null;

    void Reset()
    {
        grid = GetComponent<SnappingGrid>();
    }
}
