using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Module : MonoBehaviour, IDraggable
{
    Vector3 lastAssignedPosition;
    private bool _isPowered = false;
    private bool _isBeingDragged = false;
    protected bool IsPowered { get => _isPowered && !_isBeingDragged; }

    void Start()
    {
        _isBeingDragged = false;
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
        _isPowered = true;
    }

    public void PowerOff()
    {
        _isPowered = false;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _isBeingDragged = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = MouseHelper.toWorldPosition(eventData.position);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _isBeingDragged = false;
        DropManager.HandleDrop(this, eventData);
    }

    protected void GenerateIncome(float increase)
    {
        Bank.Deposit(increase);
    }
}
