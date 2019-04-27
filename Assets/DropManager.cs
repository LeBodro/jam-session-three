using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropManager : MonoBehaviour
{
    [SerializeField]
    SnappingGrid[] grids = null;

    [SerializeField]
    List<Module> modules = null;

    [SerializeField]
    SnappingGrid fallbackGrid = null;

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

        bool wasBound = false;

        if(droppedOn != null)
        {
            Cell cell = droppedOn.GetDroppedOnCell(droppedPosition);
            if (cell != null && cell.IsFree) {
                wasBound = true;
                cell.Bind(m);
            }
        }

        if (!wasBound)
        {
            Cell availableCell = fallbackGrid.FirstAvailableCell();
            if (availableCell != null) {
                availableCell.Bind(m);
            } else {
                // Holy shit. Nowhere to goooooo
            }
        }
    }
}
