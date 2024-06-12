using System.Linq;
using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
[AddComponentMenu("Spline Path/Follow Path (Transform)")]
public class FollowPath : MonoBehaviour
{
    public enum EndAction
    {
        Stop,       // Stop moving
        Reverse,    // Reverse direction
        Continue,   // return to start
    }
    public enum RotationMode
    {
        None,       // No rotation
        Keyframe,   // The script will interpolate between the rotation of the points
        Path,       // The script will look int the direction it is moving
    }
    public enum Shape { Sphere, Cube, Mesh }

    #region Inspector Values
    public Path path;

    [Tooltip("The time in seconds to travel the length of the path.")]
    [Min(0)] public float moveTime = 1.0f;
    [Tooltip("The time offset in seconds.")]
    [Min(0)] public float timeOffset = 0.0f;

    [Tooltip("Reverses direction.")]
    public bool reverse = false;
    [Tooltip("What to do when the end of the path is reached.")]
    public EndAction endAction;
    [Tooltip("Dictates how the script will rotate the body.")]
    public RotationMode rotationMode = RotationMode.None;

    public bool showStartPoint = true;
    [SerializeField] Color startPointColor = Color.white;
    public Shape startPointShape = Shape.Sphere;
    [SerializeField] bool startPointWireframe = false;

    [SerializeField] float startPointRadius = 0.2f;
    [SerializeField] Vector3 startPointSize = Vector3.one;
    [SerializeField] Mesh startPointMesh;
    #endregion

    #region Private members
    protected float _moveTimer;

    protected float T => endAction switch
    {
        EndAction.Stop => Mathf.Clamp01((timeOffset + _moveTimer) / moveTime),
        EndAction.Reverse => Mathf.PingPong((timeOffset + _moveTimer) / moveTime, 1),
        _ => Mathf.Repeat((timeOffset + _moveTimer) / moveTime, 1),
    };

    protected Vector3 _previousPosition;
    #endregion

    #region Unity Messages
    private void Awake()
    {
        _previousPosition = transform.position;
        if (!path.PathIsValid)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        if (!path.PathIsValid)
        {
            enabled = false;
        }
        else
        {
            MoveAlongPath();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showStartPoint)
        {
            Gizmos.color = startPointColor;
            var pos = path.GetPointAlongPath(T, out _);
            if (startPointWireframe)
            {
                switch (startPointShape)
                {
                    case Shape.Sphere:
                        Gizmos.DrawWireSphere(pos, startPointRadius);
                        break;
                    case Shape.Cube:
                        Gizmos.DrawWireCube(pos, startPointSize);
                        break;
                    case Shape.Mesh:
                        Gizmos.DrawWireMesh(startPointMesh, pos);
                        break;
                }
            }
            else
            {
                switch (startPointShape)
                {
                    case Shape.Sphere:
                        Gizmos.DrawSphere(pos, startPointRadius);
                        break;
                    case Shape.Cube:
                        Gizmos.DrawCube(pos, startPointSize);
                        break;
                    case Shape.Mesh:
                        Gizmos.DrawMesh(startPointMesh, pos);
                        break;
                }
            } 
        }
    }
    #endregion

    protected virtual void MoveAlongPath()
    {
        _moveTimer += (reverse ? -1 : 1) * Time.deltaTime;
        var t = T;

        Vector3 position = path.GetPointAlongPath(t, out var rotation);
        Quaternion newRotation = rotationMode switch
        {
            RotationMode.Keyframe => rotation,
            RotationMode.Path => Quaternion.LookRotation(position - _previousPosition),
            _ => transform.rotation,
        };
        transform.SetPositionAndRotation(position, newRotation);

        if (endAction == EndAction.Stop && t == 1 || t == 0)
        {
            Reverse();
            enabled = false;
        }

        _previousPosition = position;
    }

    public void Toggle() => enabled = !enabled;
    public void Reverse() => reverse = !reverse;

    [ContextMenu("Find Nearest Path")]
    public void FindPath() => path = FindObjectsByType<Path>(FindObjectsSortMode.None).OrderBy(o => Vector3.Distance(transform.position, o.transform.position)).FirstOrDefault();
}