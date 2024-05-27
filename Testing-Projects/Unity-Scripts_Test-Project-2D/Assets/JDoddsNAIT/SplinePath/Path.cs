using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
public abstract class Path : MonoBehaviour
{
    #region Inspector Values
    [Header("Gizmo settings")]
    public Color pathColor = Color.white;
    [Tooltip("The amount of segments drawn. Ignored if Path Type is Linear.")]
    [Range(1, 100)] public int curveSegments = 20; 
    [Header("Path settings")]
    public bool closeLoop;
    public List<Transform> points = new();
    #endregion

    private readonly string INVALID_PATH = $"Length of {nameof(points)} cannot be less than 2 or contain any nulls.";
    public virtual bool PathIsValid
    {
        get
        {
            var result = points != null && (points.Count >= 2) && !points.Where(p => p == null).Any();
            if (!result)
            {
                Debug.LogError(INVALID_PATH);
            }
            return result;
        }
    }

    public abstract void GetPoint(float t, out Vector3 position, out Quaternion? rotation);

    [ContextMenu("Generate Path from Children")]
    private void UseChildren()
    {
        List<Transform> transforms = GetComponentsInChildren<Transform>(includeInactive: false).Where(t => t != transform).ToList();
        if (transforms.Count > 0)
        {
            points = transforms;
        }
        else
        {
            Debug.Log("Could not generate path as no children were found");
        }
    }
}
