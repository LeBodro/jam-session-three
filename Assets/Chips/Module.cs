using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(StatDictionnary))]
public class Module : MonoBehaviour, IDraggable
{
    protected const string STAT_HERTZ = "hertz";
    protected const string STAT_INCOME = "income";

    [SerializeField] protected decimal price;
    [SerializeField] protected StatDictionnary stats;
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] bool bought = false;
    [SerializeField] TierMaterials tierMaterials;

    Vector3 lastAssignedPosition;
    bool _isPowered = false;
    public event System.Action<Module> OnBought = delegate { };

    public int Tier { get; private set; }
    protected bool IsBeingDragged { get; private set; }
    protected bool IsPowered { get => _isPowered && !IsBeingDragged; }

    void Reset()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        stats = GetComponent<StatDictionnary>();
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

    protected void GenerateIncome(decimal increase)
    {
        Bank.Deposit(increase);
    }

    public bool TryBuy()
    {
        if (!bought)
        {
            bought = bought || Bank.TryWithdraw(price);
            if (bought) OnBought(this);
        }
        return bought;
    }

    public virtual void Tierify(int tier)
    {
        Tier = tier;
        foreach (var sprite in sprites)
            sprite.material = tierMaterials.Get(tier);
    }

    public void Trash()
    {
        foreach (var s in sprites)
            s.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }
}
