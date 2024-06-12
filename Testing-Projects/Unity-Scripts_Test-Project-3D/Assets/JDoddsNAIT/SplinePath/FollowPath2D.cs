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

    protected override void MoveAlongPath()
    {
        _moveTimer += (reverse ? -1 : 1) * Time.deltaTime;
        var t = T;

        Vector3 position = path.GetPointAlongPath(t, out var rotation);
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
                    Body.rotation = Quaternion.LookRotation(Body.velocity).eulerAngles.z;
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
