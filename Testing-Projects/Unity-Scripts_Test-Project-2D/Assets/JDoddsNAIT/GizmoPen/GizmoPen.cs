using System;
using UnityEngine;

public class GizmoPen
{
    private Vector3[] _points;
    readonly Func<float, Vector3> _circle = theta => new Vector3(Mathf.Cos(theta * 2 * Mathf.PI), Mathf.Sin(theta * 2 * Mathf.PI));

    public int Resolution
    {
        get => _points.Length - 1;
        set => _points = new Vector3[value < 1 ? 1 : value + 1];
    }

    public GizmoPen() : this(25) { }
    public GizmoPen(int resolution)
    {
        Resolution = resolution;
    }

    public static void DrawLineArray(Vector3[] points) 
    {
        Vector3 lastPosition = points[0];
        for (int i = 1; i < points.Length; i++)
        {
            Gizmos.DrawLine(lastPosition, points[i]);
            lastPosition = points[i];
        }
    }
    public void DrawLineArray(Vector3[] points, Color color) { Gizmos.color = color; DrawLineArray(points); }

    public void DrawSpline(Func<float, Vector3> spline)
    {
        float step = 1f / Resolution;
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i] = spline(step * i);
        }
        DrawLineArray(_points);
    }
    public void DrawSpline(Func<float, Vector3> spline, Color color) { Gizmos.color = color; DrawSpline(spline); }

    public void DrawCircle(Vector3 center, float radius)
    {
        DrawSpline(theta => (_circle(theta) * radius) + center);
    }
    public void DrawCircle(Vector3 center, float radius, Color color) { Gizmos.color = color; DrawCircle(center, radius); }

    public void DrawCircle(Vector3 center, float radius, Vector3 axis)
    {
        DrawSpline(theta => Quaternion.FromToRotation(Vector3.forward, axis) * (_circle(theta) * radius) + center);
    }
    public void DrawCircle(Vector3 center, float radius, Vector3 axis, Color color) { Gizmos.color = color; DrawCircle(center, radius, axis); }
}
