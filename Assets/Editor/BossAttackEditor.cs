using UnityEditor;
using UnityEngine;

namespace BossTraits
{
    [CustomPropertyDrawer(typeof(BossAttack))]
    public class BossAttackDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.EndProperty();
        }
    }
}