using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> _points = new List<Transform>();
    public Transform[] Points { get => _points.ToArray(); set => _points = value.ToList(); }
    public bool closeLoop;

    private readonly string INVALID_PATH = $"Length of {nameof(Points)} cannot be less than 2 or contain any nulls.";
    public bool PathIsValid()
    {
        var result = Points != null && (Points.Length >= 2) && !Points.Where(p => p == null).Any();
        if (!result)
        {
            Debug.LogError(INVALID_PATH);
        }
        return result;
    }

    public void LerpPath(int index, float t, out Vector3 position, out Quaternion rotation)
    {
        var nextIndex = closeLoop ? (index + 1) % Points.Length : index + 1;
        position = Vector3.Lerp(
               a: Points[index].position,
               b: Points[nextIndex].position,
               t: t);
        rotation = Quaternion.Lerp(
               a: Points[index].rotation,
               b: Points[nextIndex].rotation,
               t: t);
    }

    private void OnDrawGizmos()
    {
        if (PathIsValid())
        {
            Gizmos.color = Color.yellow;
            for (int i = closeLoop ? 0 : 1; i < Points.Length; i++)
            {
                Gizmos.DrawLine(Points[i].position, Points[closeLoop ? (i + 1) % Points.Length : i - 1].position);
            }
        }
    }

    [ContextMenu("Generate Path from Children")]
    private void UseChildren()
    {
        var transforms = GetComponentsInChildren<Transform>().Where(t => t != this.transform).ToArray();
        if (transforms.Length > 0)
        {
            Points = transforms;
        }
        else
        {
            Debug.Log("Could not generate path.");
        }
    }
}
