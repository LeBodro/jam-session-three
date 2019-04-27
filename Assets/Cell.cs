using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    BoxCollider2D dropCollider = null;
    Module boundModule = null;

    public bool IsFree {
        get {
            return boundModule == null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dropCollider = GetComponent<BoxCollider2D>();
    }

    public bool Contains(Vector3 droppedPosition)
    {
        return dropCollider.OverlapPoint(droppedPosition);
    }

    public void Bind(Module m)
    {
        // Snap!!
        m.transform.position = transform.position;
        boundModule = m;
        boundModule.OnDragStart += Unbind;
    }

    void Unbind(Module m, PointerEventData ped)
    {
        boundModule.OnDragStart -= Unbind;
        boundModule = null;
    }

    void OnDrawGizmos()
    {
        if (IsFree) {
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
