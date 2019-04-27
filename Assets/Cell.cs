using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }
    
    public bool Contains(Vector3 droppedPosition)
    {
        return collider.OverlapPoint(droppedPosition);
    }
    // should have a box collider 2d to raycast with
}
