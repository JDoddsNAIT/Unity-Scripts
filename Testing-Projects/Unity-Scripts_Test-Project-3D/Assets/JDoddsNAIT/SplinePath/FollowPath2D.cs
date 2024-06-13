using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
[AddComponentMenu("Spline Path/Follow Path (Rigidbody2D)"), RequireComponent(typeof(Rigidbody2D))]
public class FollowPath2D : FollowPath
{
    public Rigidbody2D Body { get; private set; }

    protected override Vector3 Position
    {
        get => Body.position; set => Body.MovePosition(value);
    }
    protected override Quaternion Rotation
    {
        get => Quaternion.Euler(0, 0, Body.rotation); set => Body.MoveRotation(Rotation.eulerAngles.z);
    }

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
}
