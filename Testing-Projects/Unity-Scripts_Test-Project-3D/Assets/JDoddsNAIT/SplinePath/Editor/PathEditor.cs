using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    #region Properties
    SerializedProperty pathType, closeLoop, points;

    bool gizmoGroup = false;
    SerializedProperty showPath, pathColor, curveSegments;
    SerializedProperty showPoints, pointColor, pointRadius;

    readonly GUIContent empty = new("");
    #endregion

    private void OnEnable()
    {
        pathType = serializedObject.FindProperty(nameof(pathType));
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
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(showPath);
            if (showPath.boolValue)
            {
                EditorGUILayout.PropertyField(pathColor, empty);
                EditorGUILayout.EndHorizontal();
                if (path.pathType != Path.PathType.Linear)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(curveSegments);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(showPoints);
            if (showPoints.boolValue)
            {
                EditorGUILayout.PropertyField(pointColor, empty);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(pointRadius, new GUIContent("Radius"));
                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
