using UnityEditor;

[CustomEditor(typeof(FollowPath))]
public class FollowPathEditor : Editor
{
    SerializedProperty path;
    SerializedProperty moveTime;
    SerializedProperty timeOffset;
    SerializedProperty endAction;
    SerializedProperty reverse;

    private void OnEnable()
    {
        path = serializedObject.FindProperty(nameof(path));
        moveTime = serializedObject.FindProperty(nameof(moveTime));
        timeOffset = serializedObject.FindProperty(nameof(timeOffset));
        endAction = serializedObject.FindProperty(nameof(endAction));
        reverse = serializedObject.FindProperty(nameof(reverse));
    }

    public override void OnInspectorGUI()
    {
        
    }
}

[CustomEditor(typeof(LinearPath)), CanEditMultipleObjects]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {

    }
}