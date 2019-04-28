using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonModule : Module, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float incomePerClick = 1;

    int amountOfSimultaneousPresses = 0;
    public bool IsDown { get { return amountOfSimultaneousPresses > 0; } }
    public bool IsUp { get { return amountOfSimultaneousPresses == 0; } }

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
    }

    public void Release()
    {
        amountOfSimultaneousPresses--;

        if (IsUp)
        {
            Debug.Log("Button " + name + " clicked");
            if (_isPowered)
            {
                // TODO: Generate income
            }
        }
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
