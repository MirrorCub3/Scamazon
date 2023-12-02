using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditialFieldDrawer : PropertyDrawer
{
    private ConditionalFieldAttribute conditionAttr;
    private SerializedObject currentObject;
    private bool value;
    private bool withInverse;
    private bool passed;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (conditionAttr == null)
            conditionAttr = attribute as ConditionalFieldAttribute;
        
        currentObject = property.serializedObject;

        value = !currentObject.FindProperty(conditionAttr.FieldName).boolValue;
        withInverse = conditionAttr.Inverse ? !value : value;

        passed = false;

        if (withInverse)
            return;

        foreach(string conditionName in conditionAttr.OtherConditions)
        {
            value = currentObject.FindProperty(conditionName).boolValue;

            if (!value) return;
        }

        passed = true;

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!passed) return 0; 
        return EditorGUI.GetPropertyHeight(property, true);
    }

}
#endif

