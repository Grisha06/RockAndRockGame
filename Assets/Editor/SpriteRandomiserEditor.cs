using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteRandomiser))]
public class SpriteRandomiserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SpriteRandomiser sr = (SpriteRandomiser)target;

        if (sr.color)
        {
            sr.color_value = EditorGUILayout.Toggle("Use color value", sr.color_value);
            if (!sr.color_value)
            {
                sr.color_value_float = EditorGUILayout.Slider("Color value", sr.color_value_float, 0f, 1f);
            }
        }
    }
}
