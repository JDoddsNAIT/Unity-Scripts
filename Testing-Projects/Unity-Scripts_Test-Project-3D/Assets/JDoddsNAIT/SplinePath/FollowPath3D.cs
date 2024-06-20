using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
[AddComponentMenu("Spline Path/Follow Path (Rigidbody)"), RequireComponent(typeof(Rigidbody))]
public class FollowPath3D : FollowPath
{
    public Rigidbody Body { get; private set; }

    protected override Vector3 Position {
        get => Body.position; set => Body.MovePosition(value);
    }
    protected override OperableQuaternion Rotation {
        get => Body.rotation; set => Body.MoveRotation(Rotation);
    }

    #region Unity Messages
    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
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
}
