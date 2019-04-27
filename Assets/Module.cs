using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Module : MonoBehaviour, IDraggable, IPointerUpHandler
{
    event System.Action<Module, PointerEventData> _onDragRelease = delegate { };
    public event System.Action<Module, PointerEventData> OnDragRelease
    {
        add { _onDragRelease += value; }
        remove { _onDragRelease -= value; }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Note the position on the module where the mouse is relative to the transform
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Adjust the object position to take the original offset into account in order to feel like a draggable item
        transform.position = MouseHelper.toWorldPosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _onDragRelease(this, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
