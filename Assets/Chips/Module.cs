﻿using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ModuleData
{
    [SerializeField] public int index;
    [SerializeField] public int prefab;
    [SerializeField] public int tier;
    [SerializeField] public bool bought;
    [SerializeField] public bool powered;
    [SerializeField] public ArmModule.Direction direction;
    [SerializeField] public Segment segment;

    public ModuleData(int _index, int _prefab, int _tier, bool _bought, bool _powered, Segment _segment = null, ArmModule.Direction _direction = ArmModule.Direction.LEFT)
    {
        index = _index;
        prefab = _prefab;
        tier = _tier;
        bought = _bought;
        powered = _powered;
        direction = _direction;
        segment = _segment;
    }
}

[RequireComponent(typeof(StatDictionnary))]
public abstract class Module : Poolable<Module>, IDraggable
{
    protected const string STAT_HERTZ = "hertz";
    protected const string STAT_INCOME = "income";

    [SerializeField] protected StatDictionnary stats = null;
    [SerializeField] protected bool bought = false;
    [SerializeField] SpriteRenderer[] sprites = null;
    [SerializeField] TierMaterials tierMaterials = null;

    [SerializeField] new AudioSource audio = null;
    [SerializeField] AudioClip connect = null;
    [SerializeField] AudioClip disconnect = null;

    Vector3 lastAssignedPosition;
    bool _isPowered = false;
    public event System.Action<Module> OnBought = delegate { };
    public event System.Action<Module> OnRemoved = delegate { };
    protected abstract decimal[] PricePerTier { get; }

    public int Tier { get; private set; }
    public decimal Price { get; protected set; }
    protected bool IsBeingDragged { get; private set; }
    protected bool IsPowered
    {
        get => _isPowered && !IsBeingDragged;
        private set => _isPowered = value;
    }
    protected virtual int Prefab { get => 0; }

    void Reset()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        stats = GetComponent<StatDictionnary>();
    }

    void Start()
    {
        IsBeingDragged = false;
        lastAssignedPosition = transform.position;
    }

    public void CancelMovement()
    {
        transform.position = lastAssignedPosition;
        DropManager.HandleDrop(this);
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

        OnRemoved(this);
        audio.PlayOneShot(disconnect);
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
        audio.PlayOneShot(connect);
        foreach (var s in sprites)
            s.sortingOrder -= 3;
    }

    protected void GenerateIncome(decimal increase)
    {
        Bank.Deposit(increase);
        PopUpMaker.ShowTransaction(transform.position, increase);
    }

    public bool TryBuy()
    {
        if (!bought)
        {
            bought = bought || Bank.TryWithdraw(Price);
            if (bought) OnBought(this);
        }
        return bought;
    }

    public virtual void Tierify(int tier)
    {
        Tier = tier;
        Price = PricePerTier[tier];
        foreach (var sprite in sprites)
            sprite.material = tierMaterials.Get(tier);
    }

    protected float CalculateIncome(float expCoef, float coef, float constant)
    {
        return (Mathf.Pow(2, Tier * expCoef) + coef * Tier + constant);
    }

    public void Trash()
    {
        foreach (var s in sprites)
            s.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        enabled = false;
    }

    public virtual ModuleData Serialize(int index)
    {
        return new ModuleData(index, Prefab, Tier, bought, IsPowered);
    }

    public virtual void Deserialize(ModuleData data)
    {
        Tierify(data.tier);
        bought = data.bought;
        _isPowered = data.powered;
    }

    protected override void OnDePool()
    {
        base.OnDePool();
        enabled = true;
        foreach (var s in sprites)
            s.maskInteraction = SpriteMaskInteraction.None;
    }

    protected override void OnRePool()
    {
        base.OnRePool();
        OnRemoved(this);
        transform.SetParent(null);
        OnBought = delegate { };
        IsPowered = false;
        bought = false;
    }
}
