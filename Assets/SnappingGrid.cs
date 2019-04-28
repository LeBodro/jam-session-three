using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class SnappingGrid : MonoBehaviour
{
    [SerializeField] BoxCollider2D dropCollider;
    [SerializeField] Grid grid;

    Table<Cell> cells; // WIP branch: grid

    // Called from the editor when adding or resetting the component
    void Reset()
    {
        grid = GetComponent<Grid>();
        FindObjectOfType<DropManager>().Refresh();
        dropCollider = GetComponent<BoxCollider2D>();
    }

    void Awake() // WIP branch: grid
    {
        // determine number of cells from collider size
        //
    }

    void PopulateCells() // WIP branch: grid
    {
        int width = (int)(dropCollider.size.x / (grid.cellSize.x + grid.cellGap.x));
        int height = (int)(dropCollider.size.y / (grid.cellSize.y + grid.cellGap.y));
        cells = new Table<Cell>(width, height, (x, y) => new Cell());
    }

    public bool Contains(Vector3 droppedPosition)
    {
        return dropCollider.OverlapPoint(droppedPosition);
    }

    public void Snap(Transform target)
    {
        // TODO: Check if place is taken
        target.position = grid.CellToWorld(grid.WorldToCell(target.position)) + grid.cellSize * 0.5f;
    }
}
