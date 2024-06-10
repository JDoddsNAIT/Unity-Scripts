using UnityEngine;

[AddComponentMenu("Spline Path/Follow Path")]
[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
public class FollowPath : MonoBehaviour
{
    enum EndAction
    {
        Stop,       // Stop moving
        Reverse,    // Reverse direction
        Continue,   // return to start
    }

    #region Inspector Values
    public Path path;
    [Header("Settings")]
    [Tooltip("The time in seconds to travel between each node.")]
    [Min(0)] public float moveTime = 1.0f;
    [Tooltip("The time offset in seconds.")]
    [Min(0)] public float timeOffset = 0.0f;
    [Tooltip("What to do when the end of the path is reached.")]
    [SerializeField] EndAction endAction;
    [Tooltip("Reverses direction.")]
    public bool reverse = false;
    #endregion

    #region Private members
    private Timer _moveTimer;
    private int Reverse => reverse ? -1 : 1;
    #endregion

    #region Unity Messages
    private void Start()
    {
        if (path.PathIsValid)
        {
            _moveTimer = new Timer(moveTime, timeOffset % moveTime);
        }
        else
        {
            enabled = false;
        }
    }

    private void Update()
    {
        _moveTimer = new Timer(moveTime, _moveTimer.Time);
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
        path.GetPoint(timeOffset % moveTime, out var position, out _);
        Gizmos.DrawSphere(position, 0.2f);
    }
    #endregion

    private void MoveAlongPath()
    {
        _moveTimer.Time += Reverse * Time.deltaTime;

        path.GetPoint(_moveTimer.Value, out var position, out var rotation);
        transform.SetPositionAndRotation(position, rotation ?? transform.rotation);

        if (_moveTimer.Alarm)
        {
            switch (endAction)
            {
                case EndAction.Stop:
                    reverse = !reverse;
                    enabled = false;
                    break;
                case EndAction.Reverse:
                    reverse = !reverse;
                    break;
                default:
                    break;
            }

            _moveTimer.Time = reverse ? moveTime : 0;
        }
    }

    public void Toggle() => enabled = !enabled;
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