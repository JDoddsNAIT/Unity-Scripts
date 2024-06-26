﻿using UnityEngine;
using UnityEditor;
using System;

public static class EditorUtils
{
    public static void GizmoToggle(
        SerializedProperty showGizmo,
        SerializedProperty gizmoColor) => GizmoToggle(showGizmo, gizmoColor, () => { });
    public static void GizmoToggle(
        SerializedProperty showGizmo,
        SerializedProperty gizmoColor,
        SerializedProperty property) => GizmoToggle(showGizmo, gizmoColor, () => EditorGUILayout.PropertyField(property));
    public static void GizmoToggle(
        SerializedProperty showGizmo,
        SerializedProperty gizmoColor,
        SerializedProperty property,
        string label) => GizmoToggle(showGizmo, gizmoColor, () => EditorGUILayout.PropertyField(property, new GUIContent(label)));
    public static void GizmoToggle(
        SerializedProperty showGizmo,
        SerializedProperty gizmoColor,
        SerializedProperty[] properties) => GizmoToggle(showGizmo, gizmoColor, () =>
        {
            foreach (var property in properties)
            {
                EditorGUILayout.PropertyField(property);
            }
        });
    public static void GizmoToggle(
        SerializedProperty showGizmo,
        SerializedProperty gizmoColor,
        Action layout)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(showGizmo);
        if (showGizmo.boolValue)
        {
            EditorGUILayout.PropertyField(gizmoColor, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            layout();
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUILayout.EndHorizontal();
        }
    }

    public static void CompoundProperty(SerializedProperty first, SerializedProperty second) => CompoundProperty(second.displayName, first, second);
    public static void CompoundProperty(string label, SerializedProperty first, SerializedProperty second) => CompoundProperty(new GUIContent(label), first, second);
    public static void CompoundProperty(GUIContent label, SerializedProperty first, SerializedProperty second)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(first, label);
        EditorGUILayout.PropertyField(second, GUIContent.none);
        EditorGUILayout.EndHorizontal();
    }

}