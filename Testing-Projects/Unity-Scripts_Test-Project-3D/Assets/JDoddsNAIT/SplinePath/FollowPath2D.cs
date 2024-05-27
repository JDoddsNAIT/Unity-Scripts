using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
[AddComponentMenu("Spline Path/Follow Path (Rigidbody2D)"), RequireComponent(typeof(Rigidbody2D))]
public class FollowPath2D : FollowPath
{
    public Rigidbody2D Body { get; private set; }

    #region Unity Messages
    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
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

    private void OnEnable()
    {
        Body.isKinematic = false;
    }
    private void OnDisable()
    {
        Body.isKinematic = true;
    }
    #endregion

    private void MoveAlongPath()
    {
        _moveTimer += (reverse ? -1 : 1) * Time.deltaTime;
        var t = T;

        path.GetPointAlongPath(t, out var position, out var rotation);
        Body.position = _previousPosition;
        Body.velocity = (position - _previousPosition) / Time.deltaTime;

        switch (rotationMode)
        {
            case RotationMode.Keyframe:
                Body.MoveRotation(rotation);
                break;
            case RotationMode.Path:
                if (Body.velocity != Vector2.zero)
                {
                    Body.MoveRotation(Quaternion.LookRotation(Body.velocity));
                }
                break;
            default:
                break;
        }

        if (endAction == EndAction.Stop && t == 1 || t == 0)
        {
            Reverse();
            enabled = false;
        }

        _previousPosition = position;
    }
}
