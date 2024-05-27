using UnityEngine;

[AddComponentMenu("Spline Path/Follow Path"), RequireComponent(typeof(Rigidbody))]
[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
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

    #region Inspector Values
    [Space]
    public Path path;
    [Header("Settings")]
    [Tooltip("The time in seconds to travel between each node.")]
    [Min(0)] public float moveTime = 1.0f;
    [Tooltip("The time offset in seconds.")]
    [Min(0)] public float timeOffset = 0.0f;
    [Space]
    [Tooltip("What to do when the end of the path is reached.")]
    public EndAction endAction;
    [Tooltip("Reverses direction.")]
    public bool reverse = false;
    [Tooltip("Dictates how the script will rotate the body.")]
    public RotationMode rotationMode = RotationMode.None;
    #endregion

    #region Private members
    private float _moveTimer;
    private int Reverse => reverse ? -1 : 1;

    private float T => endAction switch
    {
        EndAction.Stop => Mathf.Clamp01(timeOffset + (_moveTimer / moveTime)),
        EndAction.Reverse => Mathf.PingPong(timeOffset + (_moveTimer / moveTime), 1),
        _ => Mathf.Repeat(timeOffset + (_moveTimer / moveTime), 1),
    };

    public Rigidbody Body { get; private set; }
    private Vector3 _previousPosition;
    #endregion

    #region Unity Messages
    private void Start()
    {
        Body = GetComponent<Rigidbody>();
        _previousPosition = Body.position;
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
        Gizmos.color = Color.yellow;
        
        path.GetPointAlongPath(T, out var position, out _);
        Gizmos.DrawSphere(position, 0.2f);
    }
    #endregion

    private void MoveAlongPath()
    {
        _moveTimer += Reverse * Time.deltaTime;

        path.GetPointAlongPath(T, out var position, out var rotation);
        Body.position = _previousPosition;
        Body.velocity = (position - _previousPosition) / Time.deltaTime;

        switch (rotationMode)
        {
            case RotationMode.Keyframe:
                Body.MoveRotation(rotation);
                break;
            case RotationMode.Path:
                Body.MoveRotation(Quaternion.LookRotation(Body.velocity));
                break;
            default:
                break;
        }

        _previousPosition = position;
    }

    public void Toggle() => enabled = !enabled;
}
