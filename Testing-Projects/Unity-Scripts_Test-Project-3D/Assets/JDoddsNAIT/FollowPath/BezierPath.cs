using UnityEngine;

[AddComponentMenu("Path/Bezier")]
public class BezierPath : Path
{
    public override void GetPoint(float t, out Vector3 position, out Quaternion? rotation)
    {
        t = Mathf.Clamp01(t);

        Vector3 b = new();
        int n = points.Count - (closeLoop ? 0 : 1);

        for (int i = 0; i <= n; i++)
        {
            b += Combination(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) * points[i % points.Count].position;
        }
        position = b;
        rotation = null;
    }

    private void OnDrawGizmos()
    {
        if (PathIsValid)
        {
            Gizmos.color = pathColor;
            float t = 0f;
            for (int i = 0; i < curveSegments; i++)
            {
                GetPoint(t, out var from, out _);
                t += 1 / (float)curveSegments;
                GetPoint(t, out var to, out _);
                Gizmos.DrawLine(from, to);
            }
        }
    }
}
