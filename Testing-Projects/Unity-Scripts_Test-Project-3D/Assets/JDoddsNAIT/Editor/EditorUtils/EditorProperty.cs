using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorProperty
{
    public SerializedProperty Property { get; set; }
    public GUIContent Label { get; set; }
    public float FieldWidth { get; set; }
    public float LabelWidth { get; set; }


    public void Draw()
    {
        float oldLabelWidth = EditorGUIUtility.labelWidth;
        float oldFieldWidth = EditorGUIUtility.fieldWidth;

        EditorGUIUtility.labelWidth = LabelWidth;
        EditorGUIUtility.fieldWidth = FieldWidth;

        EditorGUILayout.PropertyField(Property, Label);

        EditorGUIUtility.labelWidth = oldLabelWidth;
        EditorGUIUtility.fieldWidth = oldFieldWidth;
    }
}

public class PropertyList
{
    public Dictionary<string, SerializedProperty> Properties { get; set; } = new();

    public void Add(SerializedProperty property)
    {
        Properties.Add(property.name, property);
    }
}