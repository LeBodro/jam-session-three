using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Module : MonoBehaviour, IDraggable
{
    Vector3 lastAssignedPosition;

    protected bool _isPowered = false;
    public bool IsPowered {
        get {
            return _isPowered;
        }
    }

    event System.Action<Module, PointerEventData> _onDragStart = delegate { };
    public event System.Action<Module, PointerEventData> OnDragStart
    {
        add { _onDragStart += value; }
        remove { _onDragStart -= value; }
    }

    void Start()
    {
        lastAssignedPosition = transform.position;
        PowerOff();
    }

    public void CancelMovement()
    {
        transform.position = lastAssignedPosition;
    }

    public void ConfirmMovement()
    {
        lastAssignedPosition = transform.position;
    }

    public void PowerOn()
    {
        Debug.Log("Power on");
        _isPowered = true;
    }

    public void PowerOff()
    {
        Debug.Log("Power off");
        _isPowered = false;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _onDragStart(this, eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = MouseHelper.toWorldPosition(eventData.position);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        DropManager.HandleDrop(this, eventData);
    }
}
