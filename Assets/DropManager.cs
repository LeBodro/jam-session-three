using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropManager : MonoBehaviour
{
    [SerializeField]
    SnappingGrid[] grids;

    [SerializeField]
    List<Module> modules;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Module m in modules)
        {
            RegisterModule(m);
        }
    }

    public void AddNewModule(Module m)
    {
        modules.Add(m);
        RegisterModule(m);
    }

    public void RemoveModule(Module m)
    {
        modules.Remove(m);
        UnregisterModule(m);
    }

    void RegisterModule(Module m)
    {
        m.OnDragRelease += (Module _m, PointerEventData ped) => {
            HandleDrop(_m, ped);
        };
    }

    void UnregisterModule(Module m)
    {
        // Does that even make sense???
        m.OnDragRelease -= (Module _m, PointerEventData ped) => {
            HandleDrop(_m, ped);
        };
    }

    private Action<Module, PointerEventData> HandleDrop(Module m, PointerEventData ped)
    {
        // TODO: Search into grids array for candiate spot to snap to and then snap to it.
        // TODO: Fallback to inventory when no candidate spot found
        throw new NotImplementedException();
    }
}
