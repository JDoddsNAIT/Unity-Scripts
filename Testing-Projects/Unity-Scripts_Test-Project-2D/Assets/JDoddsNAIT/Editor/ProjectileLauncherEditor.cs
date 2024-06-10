using UnityEditor;

[CustomEditor(typeof(ProjectileLauncher2D))]
public class ProjectileLauncher2DEditor : Editor
{
    protected bool projectileGroup = true;
    protected SerializedProperty projectile, maxProjectiles, lifeTime, launchForce;

    protected bool spawnGroup = true;
    protected SerializedProperty spawnParent, spawnOnStart, spawnDelay;

    protected bool gizmoGroup = false;
    protected SerializedProperty color, showLaunchVelocity, showTrajectory, resolution, showFinalPosition, radius;

    private void OnEnable()
    {
        EnableProperties();
    }

    protected void EnableProperties()
    {
        projectile = serializedObject.FindProperty(nameof(projectile));
        maxProjectiles = serializedObject.FindProperty(nameof(maxProjectiles));
        lifeTime = serializedObject.FindProperty(nameof(lifeTime));

        spawnParent = serializedObject.FindProperty(nameof(spawnParent));
        spawnOnStart = serializedObject.FindProperty(nameof(spawnOnStart));
        spawnDelay = serializedObject.FindProperty(nameof(spawnDelay));
        launchForce = serializedObject.FindProperty(nameof(launchForce));

        color = serializedObject.FindProperty(nameof(color));
        showLaunchVelocity = serializedObject.FindProperty(nameof(showLaunchVelocity));
        showTrajectory = serializedObject.FindProperty(nameof(showTrajectory));
        resolution = serializedObject.FindProperty(nameof(resolution));
        showFinalPosition = serializedObject.FindProperty(nameof(showFinalPosition));
        radius = serializedObject.FindProperty(nameof(radius));
    }

    public override void OnInspectorGUI()
    {
        ProjectileLauncher2D projectileLauncher2D = (ProjectileLauncher2D)target;
        DrawProjectileInspector(projectileLauncher2D);
    }

    protected void DrawProjectileInspector<Tbody>(ProjectileLauncher<Tbody> projectileLauncher)
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(projectile);
        EditorGUILayout.PropertyField(maxProjectiles);
        EditorGUILayout.PropertyField(lifeTime);
        EditorGUILayout.PropertyField(launchForce);

        EditorGUILayout.Space();
        spawnGroup = EditorGUILayout.BeginFoldoutHeaderGroup(spawnGroup, "Spawn Settings");
        if (spawnGroup)
        {
            EditorGUILayout.PropertyField(spawnParent);
            EditorGUILayout.PropertyField(spawnOnStart);
            EditorGUILayout.PropertyField(spawnDelay);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();
        gizmoGroup = EditorGUILayout.BeginFoldoutHeaderGroup(gizmoGroup, "Gizmo Settings");
        if (gizmoGroup)
        {
            EditorGUILayout.PropertyField(color);
            EditorGUILayout.PropertyField(showLaunchVelocity);

            EditorGUILayout.PropertyField(showTrajectory);
            if (projectileLauncher.showTrajectory)
            {
                EditorGUILayout.PropertyField(resolution);
            }

            EditorGUILayout.PropertyField(showFinalPosition);
            if (projectileLauncher.showFinalPosition)
            {
                EditorGUILayout.PropertyField(radius);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
