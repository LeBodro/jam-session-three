using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingGrid : MonoBehaviour
{
    Cell[] cells;
    // Start is called before the first frame update
    void Start()
    {
        List<Cell> childCells = new List<Cell>();
        GetComponentsInChildren<Cell>(false, childCells);
        cells = childCells.ToArray();
    }

    // TODO: Make a public function for raycasting a mouse event on its own collider
    // TODO: Make a public function for specifying a candidate Cell for the parent to snap to for the position
}
