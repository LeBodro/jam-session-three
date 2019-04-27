using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropManager : SceneSingleton<DropManager>
{
    [SerializeField]
    SnappingGrid[] grids = null;
    [SerializeField]
    List<Module> modules = null;
    [SerializeField]
    SnappingGrid fallbackGrid = null;

    public void Refresh()
    {
        grids = FindObjectsOfType<SnappingGrid>();
    }

    public static void HandleDrop(Module m, PointerEventData data)
    {
        Instance._HandleDrop(m, data);
    }

    void _HandleDrop(Module m, PointerEventData data)
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
