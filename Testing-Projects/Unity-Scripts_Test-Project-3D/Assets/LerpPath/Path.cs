using System;
using System.Linq;
using UnityEngine;

public class Path : MonoBehaviour
{
    public bool closeLoop;
    public Transform[] points = new Transform[2];

    private readonly string INVALID_PATH = $"Length of {nameof(points)} cannot be less than 2 or contain any nulls.";
    public bool PathIsValid()
    {
        var result = points != null && (points.Length >= 2) && !points.Where(p => p == null).Any();
        if (!result)
        {
            Debug.LogError(INVALID_PATH);
        }
        return result;
    }

    public void LerpPath(int index, float t, out Vector3 position, out Quaternion rotation)
    {
        var nextIndex = closeLoop ? (index + 1) % points.Length : index + 1;
        position = Vector3.Lerp(
               a: points[index].position,
               b: points[nextIndex].position,
               t: t);
        rotation = Quaternion.Lerp(
               a: points[index].rotation,
               b: points[nextIndex].rotation,
               t: t);
    }

    private void OnDrawGizmos()
    {
        if (PathIsValid())
        {
            Gizmos.color = Color.yellow;
            for (int i = closeLoop ? 0 : 1; i < points.Length; i++)
            {
                Gizmos.DrawLine(points[i].position, points[closeLoop ? (i + 1) % points.Length : i - 1].position);
            }
        }
    }

    [ContextMenu("Generate Path from Children")]
    private void UseChildren()
    {
        var transforms = GetComponentsInChildren<Transform>().Where(t => t != this.transform).ToArray();
        if (transforms.Length > 0)
        {
            points = transforms;
        }
        else
        {
            Debug.Log("Could not generate path.");
        }
    }
}
