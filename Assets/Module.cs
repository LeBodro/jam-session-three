using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Module : MonoBehaviour, IDraggable
{
    Vector3 dragPositionOffset;

    event System.Action<Module, PointerEventData> _onDragRelease = delegate { };
    public event System.Action<Module, PointerEventData> OnDragRelease
    {
        add { _onDragRelease += value; }
        remove { _onDragRelease -= value; }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragPositionOffset = toWorldPosition(eventData) - transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = toWorldPosition(eventData) - dragPositionOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _onDragRelease(this, eventData);
    }

    private Vector3 toWorldPosition(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        return new Vector3(ray.origin.x, ray.origin.y, 0);
    }
}
