using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingGrid : MonoBehaviour
{
    Cell[] cells;
    BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        List<Cell> childCells = new List<Cell>();
        GetComponentsInChildren<Cell>(false, childCells);
        cells = childCells.ToArray();
        collider = GetComponent<BoxCollider2D>();
    }

    public bool Contains(Vector3 droppedPosition)
    {
        return collider.OverlapPoint(droppedPosition);
    }

    public Cell GetDroppedOnCell(Vector3 droppedPosition)
    {
        foreach (Cell cell in cells)
        {
            if (cell.Contains(droppedPosition)) {
                return cell;
            }
        }
        return null;
    }

    // TODO: Make a public function for raycasting a mouse event on its own collider
    // TODO: Make a public function for specifying a candidate Cell for the parent to snap to for the position
}
