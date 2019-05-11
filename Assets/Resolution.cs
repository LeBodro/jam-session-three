using UnityEngine;

public class Resolution : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Screen.SetResolution(480, 800, true);
    }
}
