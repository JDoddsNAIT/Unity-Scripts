using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path : MonoBehaviour
{
    public enum PathType { Linear, Bezier, Catmull, }

    [Header("Path settings")]
    public List<Transform> points = new();
    public bool closeLoop;
    public PathType type = PathType.Linear;
    [Header("Gizmo settings")]
    public Color gizmoColor = Color.white;
    [Tooltip("The amount of segments drawn. Ignored if Path Type is Linear.")]
    [Min(1)] public int curveSegments;

    private readonly string INVALID_PATH = $"Length of {nameof(points)} cannot be less than 2 or contain any nulls.";
    public bool PathIsValid
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

    public void LerpPath(int idx, float t, out Vector3 position, out Quaternion rotation)
    {
        var idx2 = closeLoop ? (idx + 1) % points.Count : idx + 1;
        position = Vector3.Lerp(
               a: points[idx].position,
               b: points[idx2].position,
               t: t);
        rotation = Quaternion.Lerp(
               a: points[idx].rotation,
               b: points[idx2].rotation,
               t: t);
    }

    public void FindPath(float t, out Vector3 position, out Quaternion? rotation)
    {
        switch (type)
        {
            case PathType.Linear:
                Lerp(t, out position, out rotation);
                break;
            case PathType.Bezier:
                Bezier(t, out position); rotation = null;
                break;
            case PathType.Catmull:
                Catmull(t, out position, out rotation);
                break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    private void Lerp(float t, out Vector3 position, out Quaternion? rotation)
    {
        t = Mathf.Clamp01(t);
        int max = points.Count + (closeLoop ? 1 : 0);

        float fl = 0; // full length
        for (int i = 1; i < max; i++)
        {
            fl += Vector3.Distance(points[i - 1].position, points[i % points.Count].position);
        }
        float l = t % 1 * fl; // length

        int idx = -1;
        float sl = 0; // segment length
        float pl = 0; // partial length
        for (int i = 1; i < max && idx == -1; i++)
        {
            sl = Vector3.Distance(points[i - 1].position, points[i % points.Count].position);
            pl += sl;
            idx = (l - pl < 0) ? i : -1;
        }
        pl -= sl;

        try
        {
            float lt = (l - pl) / sl; // lerp value
            position = Vector3.Lerp(
                  a: points[idx - 1].position,
                  b: points[idx % points.Count].position,
                  t: lt);
            rotation = Quaternion.Lerp(
                   a: points[idx - 1].rotation,
                   b: points[idx % points.Count].rotation,
                   t: lt);
        }
        catch (System.Exception)
        {
            Debug.LogError($"{nameof(points.Count)} = {points.Count}, " +
                $"{nameof(idx)} = {idx}");
            throw;
        }
    }

    private void Bezier(float t, out Vector3 position)
    {
        t = Mathf.Clamp01(t);

        Vector3 b = new();
        int n = points.Count - (closeLoop ? 0 : 1);

        for (int i = 0; i <= n; i++)
        {
            b += Combination(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) * points[i % points.Count].position;
        }
        position = b;
    }

    private void Catmull(float t, out Vector3 position, out Quaternion? rotation)
    {
        throw new System.NotImplementedException();
    }

    private void OnDrawGizmos()
    {
        if (PathIsValid)
        {
            Gizmos.color = gizmoColor;
            if (type != PathType.Linear)
            {
                float t = 0f;
                for (int i = 0; i < curveSegments; i++)
                {
                    FindPath(t, out var from, out _);
                    t += 1 / (float)curveSegments;
                    FindPath(t, out var to, out _);
                    Gizmos.DrawLine(from, to);
                }
            }
            else
            {
                int n = points.Count + (closeLoop ? 1 : 0);
                for (int i = 1; i < n; i++)
                {
                    Gizmos.DrawLine(points[i - 1].position, points[i % points.Count].position);
                }
            }
        }
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
