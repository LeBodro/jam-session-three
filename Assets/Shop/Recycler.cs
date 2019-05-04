using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : SnappingGrid
{
    [SerializeField] Shop shop = null;
    [SerializeField] SpriteButton button = null;
    [SerializeField] Transform disposal = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AudioSource sound = null;

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
            trashed.RePool(0.3f);
            trashed.transform.SetParent(disposal);
            trashed.Trash();
            //Deactivate grid
        }
        anim.SetTrigger("Recycle");
        sound.Play();
        //When animation ended, reactivate grid
    }
}
