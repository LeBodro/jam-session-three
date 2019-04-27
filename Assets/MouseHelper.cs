using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHelper
{
    public static Vector3 toWorldPosition(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        return new Vector3(ray.origin.x, ray.origin.y, 0);
    }
}
