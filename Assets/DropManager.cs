using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropManager : MonoBehaviour
{
    [SerializeField]
    SnappingGrid[] grids;

    [SerializeField]
    List<Module> modules;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Module m in modules)
        {
            RegisterModule(m);
        }
    }

    public void AddNewModule(Module m)
    {
        modules.Add(m);
        RegisterModule(m);
    }

    public void RemoveModule(Module m)
    {
        modules.Remove(m);
        UnregisterModule(m);
    }

    void RegisterModule(Module m)
    {
        m.OnDragRelease += HandleDrop;
    }

    void UnregisterModule(Module m)
    {
        m.OnDragRelease -= HandleDrop;
    }

    void HandleDrop(Module m, PointerEventData ped)
    {
        var droppedPosition = m.transform.position;
        SnappingGrid droppedOn = null;
        foreach (SnappingGrid grid in grids)
        {
            if (grid.Contains(droppedPosition))
            {
                droppedOn = grid;
                break;
            }
        }

        if(droppedOn != null)
        {
            Cell cell = droppedOn.GetDroppedOnCell(droppedPosition);
            if (cell != null) {
                m.transform.position = cell.transform.position;
            }
        }
        
        // TODO: Fallback to inventory when no candidate spot found
    }
}
