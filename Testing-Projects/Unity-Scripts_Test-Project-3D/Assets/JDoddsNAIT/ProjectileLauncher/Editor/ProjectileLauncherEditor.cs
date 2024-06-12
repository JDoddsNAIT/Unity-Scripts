using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileLauncherEditor<TBody> : Editor
{
    SerializedProperty projectile, maxProjectiles, launchForce, lifeTime, launchAngle;

    bool spawnGroup = true;
    SerializedProperty spawnParent, spawnOnStart, spawnDelay;

    bool gizmoGroup = false;
    SerializedProperty showLaunchVelocity, launchVelocityColor;
    SerializedProperty showTrajectory, trajectoryColor, trajectoryResolution;
    SerializedProperty showFinalPosition, finalPositionColor, finalPositionRadius;

    private void OnEnable()
    {
        projectile = serializedObject.FindProperty(nameof(projectile));
        lifeTime = serializedObject.FindProperty(nameof(lifeTime));
        launchForce = serializedObject.FindProperty(nameof(launchForce));
        launchAngle = serializedObject.FindProperty(nameof(launchAngle));

        spawnParent = serializedObject.FindProperty(nameof(spawnParent));
        spawnOnStart = serializedObject.FindProperty(nameof(spawnOnStart));
        spawnDelay = serializedObject.FindProperty(nameof(spawnDelay));
        maxProjectiles = serializedObject.FindProperty(nameof(maxProjectiles));

        //Gizmos
        showLaunchVelocity = serializedObject.FindProperty(nameof(showLaunchVelocity));
        launchVelocityColor = serializedObject.FindProperty(nameof(launchVelocityColor));

        showTrajectory = serializedObject.FindProperty(nameof(showTrajectory));
        trajectoryColor = serializedObject.FindProperty(nameof(trajectoryColor));
        trajectoryResolution = serializedObject.FindProperty(nameof(trajectoryResolution));

        showFinalPosition = serializedObject.FindProperty(nameof(showFinalPosition));
        finalPositionColor = serializedObject.FindProperty(nameof(finalPositionColor));
        finalPositionRadius = serializedObject.FindProperty(nameof(finalPositionRadius));
    }

    public override void OnInspectorGUI()
    {
        float defaultLabelWidth = EditorGUIUtility.labelWidth;
        float defaultFieldWidth = EditorGUIUtility.fieldWidth;
        var launcher = (ProjectileLauncher<TBody>)target;
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(projectile);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(lifeTime);
        EditorGUILayout.PropertyField(launchForce);
        EditorGUILayout.PropertyField(launchAngle);

        EditorGUILayout.Space();

        spawnGroup = EditorGUILayout.BeginFoldoutHeaderGroup(spawnGroup, "Spawn Settings");
        if (spawnGroup)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(spawnParent);
            EditorGUILayout.PropertyField(spawnOnStart);
            EditorGUILayout.PropertyField(spawnDelay);
            EditorGUILayout.PropertyField(maxProjectiles);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        gizmoGroup = EditorGUILayout.BeginFoldoutHeaderGroup(gizmoGroup, "Gizmo Settings");
        if (gizmoGroup)
        {
            GizmoToggle(showLaunchVelocity, launchVelocityColor, null);
            EditorGUILayout.Space();
            GizmoToggle(showTrajectory, trajectoryColor, trajectoryResolution);
            EditorGUILayout.Space();
            GizmoToggle(showFinalPosition, finalPositionColor, finalPositionRadius);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }

    void GizmoToggle(SerializedProperty showGizmo, SerializedProperty gizmoColor, SerializedProperty property)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(showGizmo);
        if (showGizmo.boolValue)
        {
            EditorGUILayout.PropertyField(gizmoColor, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            if (property != null)
            {
                EditorGUILayout.PropertyField(property, new GUIContent(property.displayName.Split(" ")[^1]));
            }
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUILayout.EndHorizontal();
        }
    }
}

[CustomEditor(typeof(ProjectileLauncher3D))]
public class ProjectileLauncher3DEditor : ProjectileLauncherEditor<Rigidbody> { }
[CustomEditor(typeof(ProjectileLauncher2D))]
public class ProjectileLauncher2DEditor : ProjectileLauncherEditor<Rigidbody2D> { }