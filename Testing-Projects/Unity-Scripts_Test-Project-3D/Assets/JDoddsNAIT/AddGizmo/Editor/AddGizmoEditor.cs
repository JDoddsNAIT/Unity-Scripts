using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AddGizmo))]
public class AddGizmoEditor : Editor
{
    SerializedProperty shape, color, wire, position, drawMode;

    SerializedProperty radius, size, mesh, rotate, direction, from, to;

    private void OnEnable()
    {
        shape = serializedObject.FindProperty(nameof(shape));
        color = serializedObject.FindProperty(nameof(color));
        wire = serializedObject.FindProperty(nameof(wire));
        position = serializedObject.FindProperty(nameof(position));
        drawMode = serializedObject.FindProperty(nameof(drawMode));

        radius = serializedObject.FindProperty(nameof(radius));
        size = serializedObject.FindProperty(nameof(size));
        mesh = serializedObject.FindProperty(nameof(mesh));
        rotate = serializedObject.FindProperty(nameof(rotate));
        direction = serializedObject.FindProperty(nameof(direction));
        from = serializedObject.FindProperty(nameof(from));
        to = serializedObject.FindProperty(nameof(to));
    }

    public override void OnInspectorGUI()
    {
        var addGizmo = (AddGizmo)target;

        serializedObject.Update();

        EditorGUIUtility.labelWidth = 90;

        EditorGUILayout.PropertyField(color, GUIContent.none);
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

        EditorGUILayout.PropertyField(drawMode, new GUIContent("Draw:"));
        EditorGUILayout.PropertyField(position);

        EditorGUILayout.Space();
        switch (addGizmo.shape)
        {
            case AddGizmo.Shape.Sphere:
                EditorGUILayout.PropertyField(radius);
                break;
            case AddGizmo.Shape.Cube:
                EditorGUILayout.PropertyField(size);
                break;
            case AddGizmo.Shape.Mesh:
                EditorGUILayout.PropertyField(mesh);
                break;
            case AddGizmo.Shape.Ray:
                EditorGUILayout.PropertyField(rotate);
                EditorGUILayout.PropertyField(direction);
                break;
            case AddGizmo.Shape.Line:
                EditorGUILayout.PropertyField(rotate);
                EditorGUILayout.PropertyField(from);
                EditorGUILayout.PropertyField(to);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
