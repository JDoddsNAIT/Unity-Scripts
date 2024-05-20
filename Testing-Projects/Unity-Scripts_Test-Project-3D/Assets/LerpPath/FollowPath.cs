using JDoddsNAIT.Unity.CommonLib;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public enum EndAction
    {
        Stop,       // Stop moving
        Reverse,    // Reverse direction
        Continue,   // return to start
    }
    #region Public members
    [Space]
    public Path path;
    [Tooltip("The time in seconds to travel between each node.")]
    [Min(0)] public float moveTime = 1.0f;
    [Tooltip("The time offset in seconds.")]
    [Min(0)] public float timeOffset = 0.0f;
    [Header("Settings")]
    [Tooltip("What to do when the end of the path is reached.")]
    public EndAction endAction;
    [Tooltip("The path will begin on Start if true.")]
    public bool moveOnStart = true;
    [Tooltip("Reverses direction.")]
    public bool reverse = false;
    #endregion

    #region Private members
    private Timer moveTimer;
    private int pathIndex = 0;

    private int Reverse => reverse ? -1 : 1;
    #endregion

    #region Unity Messages
    private void Start()
    {
        if (path.PathIsValid())
        {
            path.PathIsValid();
            moveTimer = new Timer(moveTime, timeOffset % moveTime);
            pathIndex = (int)timeOffset % path.points.Length;

            enabled = moveOnStart;
        }
        else
        {
            enabled = false;
        }
    }

    private void Update()
    {
        moveTimer = new Timer(moveTime, moveTimer.Time);
        if (!path.PathIsValid())
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
        path.LerpPath((int)timeOffset % path.points.Length, timeOffset % moveTime, out var position, out _);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(position, 0.2f);
    }
    #endregion

    private void MoveAlongPath()
    {
        try
        {
            path.LerpPath(pathIndex, moveTimer.Value, out var position, out var rotation);
            transform.SetPositionAndRotation(position, rotation);
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log($"End of path reached. {pathIndex}");
            switch (endAction)
            {
                case EndAction.Stop:
                    enabled = false;
                    break;
                case EndAction.Reverse:
                    reverse = !reverse;
                    break;
                case EndAction.Continue:
                    pathIndex = reverse ? path.points.Length - 1 : 0;
                    break;
                default:
                    throw;
            }
        }
        finally
        {
            moveTimer.Time += Reverse * Time.deltaTime;

            if (moveTimer.Alarm)
            {
                moveTimer.Time = reverse ? moveTime : 0;
                pathIndex += Reverse;
            }
        }
    }

}
