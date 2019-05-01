using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : SnappingGrid
{
    [SerializeField] Shop shop = null;
    [SerializeField] SpriteButton button = null;
    [SerializeField] Transform disposal = null;

    void Start()
    {
        button.OnPress += Recycle;
    }

    void Recycle()
    {
        Module trashed = GetModuleAt(0, 0);
        if (trashed != null)
        {
            shop.Populate(trashed.Tier + 1);
            Destroy(trashed.gameObject, 2); // insert animation delay here
            trashed.enabled = false;
            trashed.transform.SetParent(disposal);
            //Deactivate module component
            //Deactivate grid
            //Make module a child of disposal
        }
        //Start animation (open doors and make chip fall through it)
        //When animation ended, destroy chip and reactivate grid
    }
}
