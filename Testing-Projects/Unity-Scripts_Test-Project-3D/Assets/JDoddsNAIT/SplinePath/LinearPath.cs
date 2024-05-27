using UnityEngine;

[AddComponentMenu("Spline Path/Linear Path")]
[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
public class LinearPath : Path
{
    public override void GetPointAlongPath(float t, out Vector3 position, out Quaternion rotation)
    {
        rotation = GetLinearRotation(t, out var idx, out var idx2, out var lt);
        position = Vector3.Lerp(
              a: points[idx - 1].position,
              b: points[idx2].position,
              t: lt);
    }

    private void OnDrawGizmos()
    {
        if (PathIsValid)
        {
            Gizmos.color = pathColor;
            int n = points.Count + (closeLoop ? 1 : 0);
            for (int i = 1; i < n; i++)
            {
                Gizmos.DrawLine(points[i - 1].position, points[i % points.Count].position);
            }
        }
    }
}
