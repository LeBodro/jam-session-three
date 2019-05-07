using UnityEngine;

public class MouseHelper
{
    public static Vector3 toWorldPosition(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        return new Vector3(ray.origin.x, ray.origin.y, 0);
    }
}
