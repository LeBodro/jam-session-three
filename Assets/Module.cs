using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Module : MonoBehaviour, IDraggable
{
    Vector3 lastAssignedPosition;
    protected bool isPowered { get; private set; }

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
        isPowered = true;
    }

    public void PowerOff()
    {
        isPowered = false;
    }

    public virtual void OnBeginDrag(PointerEventData eventData) { }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = MouseHelper.toWorldPosition(eventData.position);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        DropManager.HandleDrop(this, eventData);
    }

    protected void GenerateIncome(float increase)
    {
        Bank.Deposit(increase);
    }
}
