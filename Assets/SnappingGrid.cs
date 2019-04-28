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

    public void Snap(Module target)
    {
        Vector3 position = target.transform.position;
        target.transform.position = grid.CellToWorld(grid.WorldToCell(position)) + grid.cellSize * 0.5f;
        ProcessConnection(target);
    }

    void ProcessConnection(Module target)
    {
        if (connectionState == PowerState.OFF)
            target.PowerOff();
        else
            target.PowerOn();
    }
}
