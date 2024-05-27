using UnityEngine;

[AddComponentMenu("Spline Path/Bezier Path")]
[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
public class BezierPath : Path
{
    public override void GetPointAlongPath(float t, out Vector3 position, out Quaternion rotation)
    {
        rotation = GetLinearRotation(t, out _, out _, out _);
        t = Mathf.Clamp01(t);

        Vector3 b = new();
        int n = points.Count - (closeLoop ? 0 : 1);

        for (int i = 0; i <= n; i++)
        {
            b += Combination(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) * points[i % points.Count].position;
        }
        position = b;

    }

    private void OnDrawGizmos()
    {
        if (PathIsValid)
        {
            Gizmos.color = pathColor;
            float t = 0f;
            for (int i = 0; i < curveSegments; i++)
            {
                GetPointAlongPath(t, out var from, out _);
                t += 1 / (float)curveSegments;
                GetPointAlongPath(t, out var to, out _);
                Gizmos.DrawLine(from, to);
            }
        }
    }

    public static int Combination(int n, int r) => Factorial(n) / (Factorial(r) * Factorial(n - r));
    public static int Factorial(int n)
    {
        int nf = 1;
        while (n > 1)
        {
            nf *= n;
            n--;
        }
        return nf;
    }
}
