using UnityEngine;

public class PCB : SnappingGrid
{
    void Start()
    {
        //TODO: Register on module addition and removal
    }

    void RemoveModule(Module m)
    {
        // TODO: Find a way to remove module when moved
        m.PowerOff();
    }

    void AddModule(Module m)
    {
        // do stuff related to button, clicker and chip
        m.PowerOn();
    }

    void RefreshIncomeRate()
    {

    }

    void Update()
    {
        // generate income from automatic chips
    }
}
