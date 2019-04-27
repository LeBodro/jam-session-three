using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingGrid : MonoBehaviour
{
    Cell[] cells;
    BoxCollider2D dropCollider;
    // Start is called before the first frame update
    void Start()
    {
        List<Cell> childCells = new List<Cell>();
        GetComponentsInChildren<Cell>(false, childCells);
        cells = childCells.ToArray();
        dropCollider = GetComponent<BoxCollider2D>();
    }

    public bool Contains(Vector3 droppedPosition)
    {
        return dropCollider.OverlapPoint(droppedPosition);
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

    public Cell FirstAvailableCell()
    {
        foreach (Cell cell in cells)
        {
            if(cell.IsFree)
            {
                return cell;
            }
        }
        return null;
    }
}
