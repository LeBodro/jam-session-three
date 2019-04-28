using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell
{
    public bool IsFree { get; private set; }

    public void Bind(Module m)
    {
        IsFree = false;
        m.OnDragStart += Unbind;
    }

    void Unbind(Module m, PointerEventData ped)
    {
        IsFree = true;
        m.OnDragStart -= Unbind;
    }
}
