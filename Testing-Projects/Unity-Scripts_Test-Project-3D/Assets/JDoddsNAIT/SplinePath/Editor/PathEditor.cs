using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    #region Properties
    SerializedProperty pathType, useControlPoints, closeLoop, points;

    bool gizmoGroup = false;
    SerializedProperty showPath, pathColor, curveSegments;
    SerializedProperty showPoints, pointColor, pointRadius;
    #endregion

    private void OnEnable()
    {
        pathType = serializedObject.FindProperty(nameof(pathType));
        useControlPoints = serializedObject.FindProperty(nameof(useControlPoints));
        closeLoop = serializedObject.FindProperty(nameof(closeLoop));
        points = serializedObject.FindProperty(nameof(points));

        showPath = serializedObject.FindProperty(nameof(showPath));
        pathColor = serializedObject.FindProperty(nameof(pathColor));
        curveSegments = serializedObject.FindProperty(nameof(curveSegments));

        showPoints = serializedObject.FindProperty(nameof(showPoints));
        pointColor = serializedObject.FindProperty(nameof(pointColor));
        pointRadius = serializedObject.FindProperty(nameof(pointRadius));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var path = (Path)target;

        EditorGUILayout.PropertyField(pathType);
        EditorGUILayout.PropertyField(closeLoop);
        if (path.pathType == Path.PathType.CatmullRom && !closeLoop.boolValue)
        {
            EditorGUILayout.PropertyField(useControlPoints);
        }
        EditorGUILayout.PropertyField(points);

        if (!path.PathIsValid)
        {
            string buttonText = (path.points == null || path.points.Count == 0) ? "Generate Path" : "Regenerate Path";
            if (EditorGUILayout.LinkButton(buttonText))
            {
                path.UseChildren();
            }
        }

        EditorGUILayout.Space();
        gizmoGroup = EditorGUILayout.BeginFoldoutHeaderGroup(gizmoGroup, "Gizmo Settings");
        if (gizmoGroup)
        {
            GizmoToggle(
                showPath,
                pathColor,
                curveSegments,
                "Resolution");

            EditorGUILayout.Space();

            GizmoToggle(
                showPoints,
                pointColor,
                pointRadius,
                "Radius");
        }

        serializedObject.ApplyModifiedProperties();
    }

    public static void GizmoToggle(
        SerializedProperty showGizmo,
        SerializedProperty gizmoColor,
        SerializedProperty property,
        string label) => GizmoToggle(
            showGizmo,
            gizmoColor,
            () => EditorGUILayout.PropertyField(property, new GUIContent(label)));

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
}
