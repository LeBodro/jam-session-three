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

    event System.Action<Module, PointerEventData> _onDragStart = delegate { };
    public event System.Action<Module, PointerEventData> OnDragStart
    {
        add { _onDragStart += value; }
        remove { _onDragStart -= value; }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _onDragStart(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
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
