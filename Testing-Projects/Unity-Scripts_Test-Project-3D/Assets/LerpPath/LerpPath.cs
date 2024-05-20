using System.Linq;
using UnityEngine;
using JDoddsNAIT.Unity.CommonLib;

public class LerpPath : MonoBehaviour
{
    public enum EndAction
    {
        Stop,       // Stop moving
        Reverse,    // Reverse direction
        Continue,   // return to start
    }
    #region Public members
    public bool reverse = false;
    [Tooltip("What to do when the end of the path is reached.")]
    public EndAction endAction;
    [Space]
    [Tooltip("The path will begin on Start.")]
    public bool moveOnStart = true;
    [Tooltip("The time in seconds to travel between each node.")]
    [Min(0)] public float moveTime = 1.0f;
    [Tooltip("The time offset in seconds.")]
    [Min(0)] public float timeOffset = 0.0f;
    public Path path;
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
            var offset = timeOffset % moveTime;
            moveTimer = new Timer(moveTime, offset);
            pathIndex = (int)offset % path.points.Length;

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
    #endregion

    private void MoveAlongPath()
    {
        try
        {
            var point = path.LerpPosition(pathIndex, moveTimer.Value);
            transform.SetPositionAndRotation(point.Item1, point.Item2);
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
