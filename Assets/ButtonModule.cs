using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonModule : Module, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float incomePerClick = 1;
    [SerializeField] SpriteRenderer up;
    [SerializeField] SpriteRenderer down;

    int amountOfSimultaneousPresses = 0;
    public bool IsDown { get => amountOfSimultaneousPresses > 0; }
    public bool IsUp { get => !IsDown; }

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

        if (IsUp)
        {
            if (isPowered)
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
