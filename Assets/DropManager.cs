using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropManager : SceneSingleton<DropManager>
{
    [SerializeField]
    SnappingGrid[] grids = null;
    List<Module> modules = null;

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

        if (droppedOn != null)
        {
            Cell cell = droppedOn.GetDroppedOnCell(droppedPosition);
            if (cell != null)
            {
                m.transform.position = cell.transform.position;
            }
        }

        // TODO: Fallback to inventory when no candidate spot found
    }

}
