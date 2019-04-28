using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropManager : SceneSingleton<DropManager>
{
    [SerializeField] SnappingGrid[] grids = null;

    public void Refresh()
    {
        grids = FindObjectsOfType<SnappingGrid>();
    }

    public static void HandleDrop(Module m, PointerEventData data)
    {
        Instance._HandleDrop(m, data);
    }

    void _HandleDrop(Module dropped, PointerEventData data)
    {
        var droppedPosition = dropped.transform.position;
        SnappingGrid targetGrid = null;
        foreach (SnappingGrid grid in grids)
        {
            if (grid.Contains(droppedPosition))
            {
                targetGrid = grid;
                break;
            }
        }

        bool wasBound = false;

        if (targetGrid != null)
        {
            targetGrid.Snap(dropped.transform);
            dropped.ConfirmMovement();
            wasBound = true;
        }

        if (!wasBound)
        {
            dropped.CancelMovement();
        }
    }
}
