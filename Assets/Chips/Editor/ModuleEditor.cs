using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Module), true)]
public class ModuleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Snap to grid"))
        {
            FindObjectOfType<DropManager>()._HandleDrop((Module)target);
        }
        base.OnInspectorGUI();
    }
}
