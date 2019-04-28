
using UnityEngine;

[RequireComponent(typeof(SnappingGrid))]
public class PCB : MonoBehaviour
{
    [SerializeField] SnappingGrid grid = null;

    void Reset()
    {
        grid = GetComponent<SnappingGrid>();
    }

    void Start()
    {
        //TODO: Register on module addition and removal
    }

    void RemoveModule(Module m)
    {

    }

    void AddModule(Module m)
    {
        // do stuff related to button, clicker and chip
    }

    void RefreshIncomeRate()
    {

    }

    void Update()
    {
        // generate income from automatic chips
    }
}
