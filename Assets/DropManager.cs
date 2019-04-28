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
        Collider2D[] moduleOverlap = Physics2D.OverlapPointAll(droppedPosition, LayerMask.GetMask("Draggable"));
        bool cellTaken = moduleOverlap.Length > 1;

        Collider2D gridOverlap = Physics2D.OverlapPoint(droppedPosition, LayerMask.GetMask("Grid"));
        bool overGrid = gridOverlap != null;

        if (overGrid && !cellTaken)
        {
            SnappingGrid targetGrid = gridOverlap.GetComponent<SnappingGrid>();
            targetGrid.Snap(dropped.transform);
            dropped.ConfirmMovement();
        }
        else
        {
            dropped.CancelMovement();
        }
    }
}
