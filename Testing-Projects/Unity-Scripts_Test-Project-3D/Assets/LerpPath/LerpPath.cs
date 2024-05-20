using System;
using System.Linq;
using UnityEngine;

public class LerpPath : MonoBehaviour
{
    public enum EndAction
    {
        Stop,       // Stop moving
        Reverse,    // Reverse direction
        Continue,   // return to start
    }
    #region Public members
    [Tooltip("Connects the start and end points of the path.")]
    public bool closeLoop;
    [Tooltip("The path will begin on Start.")]
    public bool moveOnStart = true;
    [Tooltip("What to do when the end of the path is reached.")]
    public EndAction endAction;
    [Tooltip("The time in seconds to travel between each node.")]
    [Min(0)] public float moveTime = 1.0f;
    [Space]
    public Transform[] path = new Transform[2];
    #endregion

    #region Private members
    private Timer moveTimer;
    private int pathIndex = 0;
    private bool reverse = false;

    private bool PathIsValid => path != null && (path.Length >= 2) && !path.Where(p => p == null).Any();
    private readonly string INVALID_PATH = $"Length of {nameof(path)} cannot be less than 2 or contain any nulls.";
    #endregion

    #region Unity Messages
    private void Start()
    {
        if (PathIsValid)
        {
            moveTimer = new Timer(moveTime);
            enabled = moveOnStart;
        }
        else
        {
            Debug.LogError(INVALID_PATH);
            enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (PathIsValid)
        {
            Gizmos.color = Color.yellow;
            for (int i = closeLoop ? 0 : 1; i < path.Length; i++)
            {
                Gizmos.DrawLine(
                    path[i].position,
                    path[closeLoop ? (i + 1) % path.Length : i - 1].position);
            }
        }
    }

    private void Update()
    {
        moveTimer = new Timer(moveTime, moveTimer.Time);
        if (!PathIsValid)
        {
            Debug.LogError(INVALID_PATH);
            enabled = false;
        }
        else
        {
            MoveAlongPath();
            moveTimer.Time += Time.deltaTime;

            if (moveTimer.Alarm)
            {
                moveTimer.Time =  0;
                pathIndex += reverse ? -1 : 1;
            }
        }

    }
    #endregion

    private void MoveAlongPath()
    {
        try
        {
            transform.SetPositionAndRotation(
               Vector3.Lerp(
                   a: path[pathIndex].position,
                   b: path[pathIndex + (reverse ? -1 : 1)].position,
                   t: moveTimer.Value),
               Quaternion.Lerp(
                   a: path[pathIndex].rotation,
                   b: path[pathIndex + (reverse ? -1 : 1)].rotation,
                   t: moveTimer.Value));
        }
        catch (IndexOutOfRangeException)
        {
            switch (endAction)
            {
                case EndAction.Stop:
                    enabled = false;
                    break;
                case EndAction.Reverse:
                    reverse = !reverse;
                    break;
                case EndAction.Continue:
                    pathIndex %= path.Length;
                    break;
                default:
                    throw;
            }
        }
    }

}

public struct Timer
{
    private float time;

    public float Length { get; set; }
    public float Time
    {
        readonly get => time;
        set => time = value >= Length ? Length : value <= 0 ? 0 : value;
    }

    public Timer(float length) : this() => Length = length;

    public Timer(float length, float time) : this(length) => Time = time;

    /// <summary> Returns <see cref="Time"/> divided by <see cref="Length"/>. </summary>
    public readonly float Value => Time / Length;
    /// <summary> Returns true when <see cref="Time"/> is 0 or <see cref="Length"/>. </summary>
    public readonly bool Alarm => Time >= Length | Time <= 0;
}
