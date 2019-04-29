using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Module : MonoBehaviour, IDraggable
{
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] float price;
    [SerializeField] bool bought = false;

    Vector3 lastAssignedPosition;
    bool _isPowered = false;
    protected bool IsBeingDragged { get; private set; }
    protected bool IsPowered { get => _isPowered && !IsBeingDragged; }

    void Reset()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    void Start()
    {
        IsBeingDragged = false;
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
        if (!bought && !TryBuy()) return;

        IsBeingDragged = true;
        foreach (var s in sprites)
            s.sortingOrder += 3;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!bought) return;
        transform.position = MouseHelper.toWorldPosition(eventData.position);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!bought) return;
        IsBeingDragged = false;
        DropManager.HandleDrop(this);
        foreach (var s in sprites)
            s.sortingOrder -= 3;
    }

    protected void GenerateIncome(float increase)
    {
        Bank.Deposit(increase);
    }

    public bool TryBuy()
    {
        bought = bought || Bank.TryWithdraw(price);
        return bought;
    }
}
