using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class SnappingGrid : MonoBehaviour
{
    public enum PowerState
    {
        OFF,
        ON,
    }

    [SerializeField] BoxCollider2D dropCollider;
    [SerializeField] Grid grid;
    [SerializeField] PowerState connectionState = PowerState.OFF;

    // Called from the editor when adding or resetting the component
    void Reset()
    {
        grid = GetComponent<Grid>();
        FindObjectOfType<DropManager>().Refresh();
        dropCollider = GetComponent<BoxCollider2D>();
    }

    public bool Contains(Vector3 droppedPosition)
    {
        return dropCollider.OverlapPoint(droppedPosition);
    }

    public bool TrySnap(Module target)
    {
        Vector3 position = target.transform.position;
        Vector3 cellCenter = GetCellCenter(grid.WorldToCell(position));
        target.transform.position = new Vector3(cellCenter.x, cellCenter.y);
        Collider2D[] moduleOverlap = Physics2D.OverlapPointAll(cellCenter, LayerMask.GetMask("Draggable"));
        foreach (var m in moduleOverlap)
            if (m.transform != target.transform)
                return false;
        ProcessConnection(target);
        return true;
    }

    public Vector3 GetCellCenter(int x, int y)
    {
        return GetCellCenter(new Vector3Int(x, y, 0));
    }

    public Vector3 GetCellCenter(Vector3Int cellCoordinates)
    {
        return grid.CellToWorld(cellCoordinates) + grid.cellSize * 0.5f;
    }

    void ProcessConnection(Module target)
    {
        if (connectionState == PowerState.OFF)
            target.PowerOff();
        else
            target.PowerOn();
    }
}
