using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Stat))]
public class StatPropertyDrawer : PropertyDrawer
{
    public static GUIStyle DeleteButton
    {
        get
        {
            var style = GUI.skin.button;
            style.fontStyle = FontStyle.Bold;
            style.onNormal.textColor = Color.red;
            return style;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.width *= 0.5f;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("referenceName"), GUIContent.none);
        position.x += position.width;
        position.width -= 40;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("baseValue"), GUIContent.none);
        position.x += position.width + 10;
        position.width = 30;
        GUI.backgroundColor = Color.red;
        if (GUI.Button(position, "X", DeleteButton))
        {
            property.DeleteCommand();
        }
        GUI.backgroundColor = Color.white;
    }
}
