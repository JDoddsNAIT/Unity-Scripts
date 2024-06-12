using UnityEditor;

public class ProjectileLauncherEditor : Editor
{
    SerializedProperty projectile, maxProjectiles, launchForce, lifeTime, launchAngle;

    bool spawnGroup = true;
    SerializedProperty spawnParent, spawnOnStart, spawnDelay;

    bool gizmoGroup = false;
    protected SerializedProperty showLaunchVelocity, launchVelocityColor;
    protected SerializedProperty showTrajectory, trajectoryColor, trajectoryResolution;
    protected SerializedProperty showFinalPosition, finalPositionColor, finalPositionRadius;

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
        serializedObject.Update();

        EditorGUILayout.PropertyField(projectile);

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
            EditorUtils.GizmoToggle(showLaunchVelocity, launchVelocityColor);
            EditorGUILayout.Space();
            EditorUtils.GizmoToggle(showTrajectory, trajectoryColor, trajectoryResolution, "Resolution");
            EditorGUILayout.Space();
            EditorUtils.GizmoToggle(showFinalPosition, finalPositionColor, finalPositionRadius, "Radius");
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void FinalPositionLayout()
    {
        EditorGUILayout.PropertyField(finalPositionRadius);
    }
}

[CustomEditor(typeof(ProjectileLauncher3D))]
public class ProjectileLauncher3DEditor : ProjectileLauncherEditor { }
[CustomEditor(typeof(ProjectileLauncher2D))]
public class ProjectileLauncher2DEditor : ProjectileLauncherEditor { }
