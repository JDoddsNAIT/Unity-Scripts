using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AddGizmo)), CanEditMultipleObjects]
public class AddGizmoEditor : Editor
{
    SerializedProperty color, wire, shape, drawMode, mesh;

    SerializedProperty useTransformPosition, position;
    SerializedProperty useTransformRotation, rotation;
    SerializedProperty useTransformScale, scale;

    SerializedProperty radius, from, to;

    readonly float defaultLabelWidth = EditorGUIUtility.labelWidth;
    readonly float defaultFieldWidth = EditorGUIUtility.fieldWidth;

    private void OnEnable()
    {
        color = serializedObject.FindProperty(nameof(color));
        wire = serializedObject.FindProperty(nameof(wire));
        shape = serializedObject.FindProperty(nameof(shape));
        drawMode = serializedObject.FindProperty(nameof(drawMode));
        mesh = serializedObject.FindProperty(nameof(mesh));

        useTransformPosition = serializedObject.FindProperty(nameof(useTransformPosition));
        position = serializedObject.FindProperty(nameof(position));

        useTransformRotation = serializedObject.FindProperty(nameof(useTransformRotation));
        rotation = serializedObject.FindProperty(nameof(rotation));

        useTransformScale = serializedObject.FindProperty(nameof(useTransformScale));
        scale = serializedObject.FindProperty(nameof(scale));

        radius = serializedObject.FindProperty(nameof(radius));
        from = serializedObject.FindProperty(nameof(from));
        to = serializedObject.FindProperty(nameof(to));
    }

    public override void OnInspectorGUI()
    {
        var addGizmo = (AddGizmo)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(color, GUIContent.none);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (addGizmo.shape != AddGizmo.Shape.Ray & addGizmo.shape != AddGizmo.Shape.Line)
        {
            EditorGUILayout.PropertyField(wire, new GUIContent("Shape"));
            EditorGUILayout.PropertyField(shape, GUIContent.none);
        }
        else
        {
            addGizmo.wire = AddGizmo.WireState.Wire;
            EditorGUILayout.PropertyField(wire, new GUIContent("Shape"));
            EditorGUILayout.PropertyField(shape, GUIContent.none);
        }
        EditorGUILayout.EndHorizontal();

        if (addGizmo.shape == AddGizmo.Shape.Mesh)
        {
            EditorGUILayout.PropertyField(mesh);
        }
        EditorGUILayout.PropertyField(drawMode, new GUIContent("Draw"));


        switch (addGizmo.shape)
        {
            case AddGizmo.Shape.Sphere:
                EditorGUILayout.Space();
                BoolVector3(useTransformPosition, position);
                EditorGUILayout.PropertyField(radius);
                break;
            case AddGizmo.Shape.Cube:
                EditorGUILayout.Space();
                BoolVector3(useTransformPosition, position);
                BoolVector3(useTransformScale, scale);
                break;
            case AddGizmo.Shape.Mesh:
                EditorGUILayout.Space();
                BoolVector3(useTransformPosition, position);
                BoolVector3(useTransformRotation, rotation);
                BoolVector3(useTransformScale, scale);
                break;
            case AddGizmo.Shape.Ray:
                EditorGUILayout.Space();
                BoolVector3(useTransformPosition, position);
                BoolVector3(useTransformRotation, rotation);
                BoolVector3(useTransformScale, scale);
                break;
            case AddGizmo.Shape.Line:
                EditorGUILayout.Space();
                BoolVector3(useTransformPosition, position);
                BoolVector3(useTransformRotation, rotation);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(from);
                EditorGUILayout.PropertyField(to);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void BoolVector3(SerializedProperty boolProperty, SerializedProperty vectorProperty)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(boolProperty, new GUIContent(vectorProperty.displayName, $"The gizmo's {vectorProperty.name}. Use the checkbox to switch between local and world space."));
        EditorGUILayout.PropertyField(vectorProperty, GUIContent.none);
        EditorGUILayout.EndHorizontal();
    }
}
