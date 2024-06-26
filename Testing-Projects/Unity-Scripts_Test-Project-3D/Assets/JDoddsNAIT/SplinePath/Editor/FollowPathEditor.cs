using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FollowPath))]
public class FollowPathEditor : Editor
{
    #region Properties
    protected SerializedProperty path, moveTime, timeOffset, reverse, endAction, rotationMode;

    protected bool gizmoGroup = false;
    protected SerializedProperty showStartPoint, startPointColor, startPointShape, startPointWireframe,
        startPointRadius, startPointSize, startPointMesh;
    #endregion

    private void OnEnable()
    {
        path = serializedObject.FindProperty(nameof(path));
        moveTime = serializedObject.FindProperty(nameof(moveTime));
        timeOffset = serializedObject.FindProperty(nameof(timeOffset));
        reverse = serializedObject.FindProperty(nameof(reverse));
        endAction = serializedObject.FindProperty(nameof(endAction));
        rotationMode = serializedObject.FindProperty(nameof(rotationMode));

        showStartPoint = serializedObject.FindProperty(nameof(showStartPoint));
        startPointColor = serializedObject.FindProperty(nameof(startPointColor));
        startPointShape = serializedObject.FindProperty(nameof(startPointShape));
        startPointWireframe = serializedObject.FindProperty(nameof(startPointWireframe));

        startPointRadius = serializedObject.FindProperty(nameof(startPointRadius));
        startPointSize = serializedObject.FindProperty(nameof(startPointSize));
    }

    public override void OnInspectorGUI()
    {
        var followPath = (FollowPath)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(path);

        float defaultWidth = EditorGUIUtility.labelWidth;
        if (followPath.path != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(moveTime);
            EditorGUIUtility.labelWidth = 60;
            EditorGUILayout.PropertyField(timeOffset, new GUIContent("Offset"));
            EditorGUIUtility.labelWidth = defaultWidth;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(reverse);
            EditorGUILayout.PropertyField(endAction);
            EditorGUILayout.PropertyField(rotationMode);

            EditorGUILayout.Space();
            gizmoGroup = EditorGUILayout.BeginFoldoutHeaderGroup(gizmoGroup, "Gizmo Settings");
            if (gizmoGroup)
            {
                PathEditor.GizmoToggle(
                    showStartPoint,
                    startPointColor,
                    () =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(startPointShape, new GUIContent("Shape"));
                        EditorGUILayout.PropertyField(startPointWireframe, new GUIContent("Wireframe"));
                        EditorGUILayout.EndHorizontal();

                        Tuple<SerializedProperty, string> startPointProperty = followPath.startPointShape switch
                        {
                            FollowPath.Shape.Sphere => Tuple.Create(startPointRadius, "Radius"),
                            FollowPath.Shape.Cube => Tuple.Create(startPointSize, "Size"),
                        };
                        EditorGUILayout.PropertyField(startPointProperty.Item1, new GUIContent(startPointProperty.Item2));
                    });
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        else
        {
            if (EditorGUILayout.LinkButton("Find Nearest Path"))
            {
                followPath.FindPath();
            }

        }

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(FollowPath2D))]
public class FollowPath2DEditor : FollowPathEditor { }

[CustomEditor(typeof(FollowPath3D))]
public class FollowPath3DEditor : FollowPathEditor { }