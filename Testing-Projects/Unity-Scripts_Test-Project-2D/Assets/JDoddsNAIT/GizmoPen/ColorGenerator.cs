using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ColorGenerator : MonoBehaviour
{
    private void Start()
    {
        GenerateColor();
    }

    public void GenerateColor()
    {
        GetComponent<Renderer>().sharedMaterial.color = Random.ColorHSV();
    }

    public void Reset()
    {
        GetComponent<Renderer>().sharedMaterial.color = Color.white;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ColorGenerator))]
class ColorGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var colorGenerator = (ColorGenerator)target;
        if (colorGenerator != null)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Color"))
        {
            colorGenerator.GenerateColor();
        }

        if (GUILayout.Button("Reset"))
        {
            colorGenerator.Reset();
        }

        EditorGUILayout.EndHorizontal();

        DrawDefaultInspector();
    }
}
#endif