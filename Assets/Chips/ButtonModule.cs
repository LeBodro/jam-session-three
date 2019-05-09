using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonModule : Module, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] SpriteRenderer up = null;
    [SerializeField] SpriteRenderer down = null;

    event System.Action<ButtonModule> _onMouseClick = delegate { };
    public event System.Action<ButtonModule> OnMouseClick
    {
        add { _onMouseClick += value; }
        remove { _onMouseClick -= value; }
    }

    Stat incomePerClick = null;
    HashSet<string> pressSrouces = new HashSet<string>();
    bool wasRecentlyCancelled = false;
    public bool IsDown { get => pressSrouces.Count > 0; }
    public bool IsUp { get => !IsDown; }
    protected override int Prefab { get => 0; }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        CancelAllPresses();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Press("mouse");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Release("mouse");
    }

    public void Press(string source)
    {
        if (IsBeingDragged) return;
        pressSrouces.Add(source);
        wasRecentlyCancelled = false;
        RefreshVisual();
    }

    void RefreshVisual()
    {
        down.enabled = IsDown;
        up.enabled = IsUp;
    }

    void CancelAllPresses()
    {
        pressSrouces.Clear();
        wasRecentlyCancelled = true;
        RefreshVisual();
    }

    public void Release(string source)
    {
        if (IsBeingDragged) return;
        if (wasRecentlyCancelled)
        {
            pressSrouces.Clear();
            wasRecentlyCancelled = false;
            RefreshVisual();
            return;
        }

        if (pressSrouces.Count <= 0)
        {
            pressSrouces.Clear();
            RefreshVisual();
            return;
        }

        pressSrouces.Remove(source);
        if (IsUp && IsPowered)
        {
            GenerateIncome(incomePerClick.ProcessedDecimal);
            if (source == "mouse")
            {
                _onMouseClick(this);
            }
        }
        RefreshVisual();
    }

    public override void Tierify(int tier)
    {
        incomePerClick = stats[STAT_INCOME];
        base.Tierify(tier);
        Price = CalculatePrice(2, 1.25f, 0.5f);
        incomePerClick.BaseValue = CalculateIncome(0.25f, 5, 1.25f);
    }
}
