using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Module : MonoBehaviour, IDraggable
{
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
        DropManager.HandleDrop(this, eventData);
    }
}
