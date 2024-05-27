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

    public abstract void GetPointAlongPath(float t, out Vector3 position, out Quaternion rotation);

    protected Quaternion GetLinearRotation(float t, out int index, out int indexModCount, out float lerp)
    {
        t = Mathf.Clamp01(t);
        int max = points.Count + (closeLoop ? 1 : 0);

        float fl = 0; // full length
        for (int i = 1; i < max; i++)
        {
            fl += Vector3.Distance(points[i - 1].position, points[i % points.Count].position);
        }
        float l = t * fl; // length

        index = -1;
        float sl = 0; // segment length
        float pl = 0; // partial length
        for (int i = 1; i < max && index == -1; i++)
        {
            sl = Vector3.Distance(points[i - 1].position, points[i % points.Count].position);
            pl += sl;
            index = l - pl > 0 ? -1 : i;
        }
        indexModCount = index % points.Count;

        lerp = (l - (pl - sl)) / sl; // lerp value
        
        return Quaternion.Lerp(
            points[index - 1].rotation,
            points[indexModCount].rotation,
            lerp);
    }

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
