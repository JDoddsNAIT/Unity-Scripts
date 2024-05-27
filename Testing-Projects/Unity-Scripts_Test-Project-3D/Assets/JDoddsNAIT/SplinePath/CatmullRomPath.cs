using System.Linq;
using UnityEngine;

[AddComponentMenu("Spline Path/Catmull-Rom Path")]
[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
public class CatmullRomPath : Path
{
    public override bool PathIsValid
    {
        get
        {
            var result = points != null && (points.Count >= 4) && !points.Where(p => p == null).Any();
            if (!result)
            {
                Debug.LogError($"Length of {nameof(points)} cannot be less than 4 or contain any nulls.");
            }
            return result;
        }
    }

    public override void GetPointAlongPath(float t, out Vector3 position, out Quaternion rotation)
    {
        rotation = GetLinearRotation(t, out _, out _, out _);

        float l = t * (points.Count - (closeLoop ? 0 : 3));
        int index = (closeLoop ? 0 : 1) + (int)l;
        float n = l % 1;

        position = GetCatmullRomPosition(n,
                    points[ClampIndex(index - 1)].position,
                    points[ClampIndex(index + 0)].position,
                    points[ClampIndex(index + 1)].position,
                    points[ClampIndex(index + 2)].position);
    }

    private void OnDrawGizmos()
    {
        if (PathIsValid)
        {
            Gizmos.color = pathColor;
            float t2 = 0f;
            for (int i = 0; i < curveSegments; i++)
            {
                GetPointAlongPath(t2, out var from, out _);
                t2 += 1 / (float)curveSegments;
                GetPointAlongPath(t2, out var to, out _);
                Gizmos.DrawLine(from, to);
            }
        }
    }

    private int ClampIndex(int index)
    {
        if (index < 0)
        { index = points.Count - 1; }

        if (index > points.Count)
        { index = 1; }
        else if (index > points.Count - 1)
        { index = 0; }

        return index;
    }

    private static Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t,
                t3 = t2 * t;
        return 0.5f * ((2 * p1) + (p2 - p0) * t + (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 + (p3 - p0 + 3 * p1 - 3 * p2) * t3);
    }
}
