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
    [Tooltip("Connects the start and end points of the path.")]
    public bool closeLoop;
    [Tooltip("The path will begin on Start.")]
    public bool moveOnStart = true;
    public bool reverse = false;
    [Tooltip("What to do when the end of the path is reached.")]
    public EndAction endAction;
    [Space]
    [Tooltip("The time in seconds to travel between each node.")]
    [Min(0)] public float moveTime = 1.0f;
    [Tooltip("The time offset in seconds.")]
    [Min(0)] public float timeOffset = 0.0f;
    public Transform[] path = new Transform[2];
    #endregion

    #region Private members
    private Timer moveTimer;
    private int pathIndex = 0;

    private int Reverse => reverse ? -1 : 1;
    private bool PathIsValid => path != null && (path.Length >= 2) && !path.Where(p => p == null).Any();
    private readonly string INVALID_PATH = $"Length of {nameof(path)} cannot be less than 2 or contain any nulls.";
    #endregion

    #region Unity Messages
    private void Start()
    {
        if (PathIsValid)
        {

            moveTimer = new Timer(moveTime, timeOffset % moveTime);
            pathIndex = (int)(timeOffset % moveTime) % path.Length;

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

                if (new RangeD(i, i + 1).Contains(timeOffset % moveTime))
                {
                    
                }
            }
        }
        else
        {
            Debug.LogError(INVALID_PATH);
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
        }

    }
    #endregion

    private void MoveAlongPath()
    {
        try
        {
            var nextIndex = closeLoop ? (pathIndex + 1) % path.Length : pathIndex + 1;
            transform.SetPositionAndRotation(
               Vector3.Lerp(
                   a: path[pathIndex].position,
                   b: path[nextIndex].position,
                   t: moveTimer.Value),
               Quaternion.Lerp(
                   a: path[pathIndex].rotation,
                   b: path[nextIndex].rotation,
                   t: moveTimer.Value));

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
                    pathIndex = reverse ? path.Length - 1 : 0;
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
