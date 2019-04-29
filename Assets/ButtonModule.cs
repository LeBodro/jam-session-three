using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonModule : Module, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float incomePerClick = 1;
    [SerializeField] SpriteRenderer up = null;
    [SerializeField] SpriteRenderer down = null;

    int amountOfSimultaneousPresses = 0;
    public bool IsDown { get => amountOfSimultaneousPresses > 0; }
    public bool IsUp { get => !IsDown; }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        amountOfSimultaneousPresses = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Press();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Release();
    }

    public void Press()
    {
        amountOfSimultaneousPresses++;
        down.enabled = IsDown;
        up.enabled = IsUp;
    }

    public void Release()
    {
        amountOfSimultaneousPresses--;
        if (amountOfSimultaneousPresses < 0)
        {
            amountOfSimultaneousPresses = 0;
        } 
        else if (IsUp && isPowered)
        {
            GenerateIncome(incomePerClick);
        }
        down.enabled = IsDown;
        up.enabled = IsUp;
    }

    void OnDrawGizmos()
    {
        if (IsUp)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(transform.position + new Vector3(0.5f, 0.5f), 0.25f);
    }
}
