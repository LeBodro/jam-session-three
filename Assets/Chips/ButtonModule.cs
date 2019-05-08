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
    int amountOfSimultaneousPresses = 0;
    bool wasRecentlyCancelled = false;
    public bool IsDown { get => amountOfSimultaneousPresses > 0; }
    public bool IsUp { get => !IsDown; }
    protected override int Prefab { get => 0; }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        CancelAllPresses();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Press();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Release(true);
    }

    public void Press()
    {
        if (IsBeingDragged) return;
        amountOfSimultaneousPresses++;
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
        amountOfSimultaneousPresses = 0;
        wasRecentlyCancelled = true;
        RefreshVisual();
    }

    public void Release(bool isMouseClick = false)
    {
        if (IsBeingDragged) return;
        if (wasRecentlyCancelled)
        {
            amountOfSimultaneousPresses = 0;
            wasRecentlyCancelled = false;
            RefreshVisual();
            return;
        }

        if (amountOfSimultaneousPresses <= 0)
        {
            amountOfSimultaneousPresses = 0;
            RefreshVisual();
            return;
        }

        amountOfSimultaneousPresses--;
        if (IsUp && IsPowered)
        {
            GenerateIncome(incomePerClick.ProcessedDecimal);
            if (isMouseClick)
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
