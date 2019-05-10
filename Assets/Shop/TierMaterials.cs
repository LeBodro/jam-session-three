using UnityEngine;

[CreateAssetMenu(menuName = "PCB/TierMaterials")]
public class TierMaterials : ScriptableObject
{
    [SerializeField] Material[] materials = null;

    public Material Get(int tier)
    {
        return materials[tier];
    }
}
