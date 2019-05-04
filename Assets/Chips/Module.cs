using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ModuleData
{
    [SerializeField] public int tier;
    [SerializeField] public bool bought;
    [SerializeField] public bool powered;
    [SerializeField] public ArmModule.Direction direction;

    public ModuleData(int _tier, bool _bought, bool _powered, ArmModule.Direction _direction = ArmModule.Direction.LEFT)
    {
        tier = _tier;
        bought = _bought;
        powered = _powered;
        direction = _direction;
    }
}

[RequireComponent(typeof(StatDictionnary))]
public class Module : Poolable<Module>, IDraggable
{
    protected const string STAT_HERTZ = "hertz";
    protected const string STAT_INCOME = "income";

    [SerializeField] protected StatDictionnary stats;
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] bool bought = false;
    [SerializeField] TierMaterials tierMaterials = null;

    [SerializeField] new AudioSource audio;
    [SerializeField] AudioClip connect;
    [SerializeField] AudioClip disconnect;

    Vector3 lastAssignedPosition;
    bool _isPowered = false;
    protected decimal price;
    public event System.Action<Module> OnBought = delegate { };

    public int Tier { get; private set; }
    protected bool IsBeingDragged { get; private set; }
    protected bool IsPowered
    {
        get => _isPowered && !IsBeingDragged;
        private set => _isPowered = value;
    }

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
        enabled = false;
    }

    public virtual string Serialize()
    {
        return JsonUtility.ToJson(new ModuleData(Tier, bought, IsPowered));
    }

    public virtual void Deserialize(string json)
    {
        ModuleData data = JsonUtility.FromJson<ModuleData>(json);
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
        transform.SetParent(null);
        OnBought = delegate { };
        IsPowered = false;
        bought = false;
    }
}
