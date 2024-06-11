using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
public class Path : MonoBehaviour
{
    public enum PathType { Linear, Bezier, [InspectorName("Catmull-Rom")] CatmullRom }
    #region Inspector Values
    [Header("Path settings")]
    public PathType pathType;
    public bool closeLoop;
    public List<Transform> points = new();

    public bool showPath = true;
    [SerializeField] Color pathColor = Color.white;
    [Tooltip("The amount of segments drawn. Ignored if Path Type is Linear.")]
    [SerializeField, Range(1, 100)] int curveSegments = 20;
    [Space]
    public bool showPoints = true;
    [SerializeField] Color pointColor = Color.white;
    [Range(0, 2)] public float pointRadius = 0.2f;
    #endregion

    private const int MIN_POINTS = 3;

    private Vector3[] spline = new Vector3[4];

    private readonly string INVALID_PATH = $"Length of {nameof(points)} cannot be less than {MIN_POINTS} or contain any nulls.";
    public virtual bool PathIsValid
    {
        get
        {
            var result = points != null && (points.Count >= MIN_POINTS) && !points.Where(p => p == null).Any();
            if (!result)
            {
                Debug.LogError(INVALID_PATH);
            }
            return result;
        }
    }

    public Vector3 GetPointAlongPath(float t, out Quaternion rotation)
    {
        rotation = GetLinearRotation(t);

        return pathType switch
        {
            PathType.Linear => LinearPath(t),
            PathType.Bezier => BezierPath(t),
            PathType.CatmullRom => CatmullRomPath(t),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    #region Spline Functions
    private Vector3 LinearPath(float t)
    {
        t = Mathf.Clamp01(t);
        int max = points.Count + (closeLoop ? 1 : 0);

        float fl = 0; // full length
        for (int i = 1; i < max; i++)
        {
            fl += Vector3.Distance(points[i - 1].position, points[i % points.Count].position);
        }
        float l = t * fl; // length

        int index = -1;
        float sl = 0; // segment length
        float pl = 0; // partial length
        for (int i = 1; i < max && index == -1; i++)
        {
            sl = Vector3.Distance(points[i - 1].position, points[i % points.Count].position);
            pl += sl;
            index = l - pl > 0 ? -1 : i;
        }
        int indexModCount = index % points.Count;

        var lerp = (l - (pl - sl)) / sl; // lerp value

        return Vector3.Lerp(
            points[index - 1].position,
            points[indexModCount].position,
            lerp);
    }

    private Vector3 BezierPath(float t)
    {
        t = Mathf.Clamp01(t);

        Vector3 b = new();
        int n = points.Count - (closeLoop ? 0 : 1);

        for (int i = 0; i <= n; i++)
        {
            b += Combination(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) * points[i % points.Count].position;
        }
        return b;
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

    private Vector3 CatmullRomPath(float t)
    {
        float l = t * (points.Count - (closeLoop ? 0 : 3));
        int index = (closeLoop ? 0 : 1) + (int)l;
        float n = l % 1;

        return GetCatmullRomPosition(n,
                    points[ClampIndex(index - 1)].position,
                    points[ClampIndex(index + 0)].position,
                    points[ClampIndex(index + 1)].position,
                    points[ClampIndex(index + 2)].position);
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
    #endregion

    private Quaternion GetLinearRotation(float t) => GetLinearRotation(t, out _, out _, out _);
    private Quaternion GetLinearRotation(float t, out int index, out int indexModCount, out float lerp)
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

    private void OnDrawGizmos()
    {
        if (showPath)
        {
            Gizmos.color = pathColor;
            switch (pathType)
            {
                case PathType.Linear:
                    DrawLineArray(points, t => t.position);
                    break;
                case PathType.Bezier:
                    DrawSpline(BezierPath);
                    break;
                case PathType.CatmullRom:
                    DrawSpline(CatmullRomPath);
                    break;
            }
        }

        if (showPoints)
        {
            Gizmos.color = pointColor;
            foreach (Transform t in points)
            {
                Gizmos.DrawSphere(t.position, pointRadius);
            }
        }
    }

    private void DrawSpline(Func<float, Vector3> splineFunction)
    {
        if (spline.Length != curveSegments + 1)
        {
            spline = new Vector3[curveSegments + 1];

            float step = 1f / curveSegments;
            for (int i = 0; i < spline.Length; i++)
            {
                spline[i] = splineFunction(step * i);
            }
        }

        DrawLineArray(spline);
    }

    private void DrawLineArray(Vector3[] points) => DrawLineArray(points, v => v);
    private void DrawLineArray<T>(IEnumerable<T> points, Func<T, Vector3> convertToVector3)
    {
        Vector3 previousPosition = convertToVector3(points.ElementAt(0));
        for (int i = 1; i < points.Count(); i++)
        {
            Vector3 position = convertToVector3(points.ElementAt(i));
            Gizmos.DrawLine(previousPosition, position);
            previousPosition = position;
        }
    }
}
