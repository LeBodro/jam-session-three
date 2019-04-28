using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class SnappingGrid : MonoBehaviour
{
    [SerializeField] BoxCollider2D dropCollider;
    [SerializeField] Grid grid;

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

    public void Snap(Transform target)
    {
        target.position = grid.CellToWorld(grid.WorldToCell(target.position)) + grid.cellSize * 0.5f;
    }
}
