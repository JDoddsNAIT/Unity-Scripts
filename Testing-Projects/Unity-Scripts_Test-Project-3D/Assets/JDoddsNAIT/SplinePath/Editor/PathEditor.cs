using UnityEditor;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    #region Properties
    SerializedProperty pathType, closeLoop, points;

    bool gizmoGroup = false;
    SerializedProperty showPath, pathColor, curveSegments;
    SerializedProperty showPoints, pointColor, pointRadius;
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

        EditorGUILayout.Space();
        gizmoGroup = EditorGUILayout.BeginFoldoutHeaderGroup(gizmoGroup, "Gizmo Settings");
        if (gizmoGroup)
        {
            EditorGUILayout.PropertyField(showPath);
            if (path.showPath)
            {
                EditorGUILayout.PropertyField(pathColor);
                if (path.pathType != Path.PathType.Linear)
                {
                    EditorGUILayout.PropertyField(curveSegments);
                }
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField (showPoints);
            if (path.showPoints)
            {
                EditorGUILayout.PropertyField(pointColor);
                EditorGUILayout.PropertyField(pointRadius);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
