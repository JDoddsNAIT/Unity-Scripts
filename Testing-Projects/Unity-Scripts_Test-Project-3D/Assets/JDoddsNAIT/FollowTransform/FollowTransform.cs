using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Follow-Transform")]
public class FollowTransform : MonoBehaviour
{
    public enum DeadZoneShape
    {
        Cube, Sphere
    }

    [Space]
    public Transform followTarget;

    [Header("Movement")]
    [Tooltip("Units/sec")]
    public float moveSpeed;
    [Tooltip("Degs/sec"), Min(0)]
    public float turnSpeed;
    public Vector3 upAxis;
    
    [Header("Deadzone")]
    public DeadZoneShape deadzoneShape = DeadZoneShape.Cube;
    public Vector3 deadzoneSize;
    public Vector3 offset;

    void Update()
    {
        CalculatePositions(out var position, out var follow);
        if (follow)
        {
            transform.position = Vector3.MoveTowards(position, followTarget.position, moveSpeed * Time.deltaTime) - offset;
        }
        if (turnSpeed > 0)
        {
            transform.LookTowards((followTarget.position - transform.position).normalized, upAxis, turnSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        CalculatePositions(out var gizmoPosition, out var follow);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gizmoPosition, followTarget.position);
        Gizmos.color = follow ? Color.green : Color.red;
        switch (deadzoneShape)
        {
            case DeadZoneShape.Sphere:
                Gizmos.DrawWireSphere(gizmoPosition, deadzoneSize.magnitude);
                break;
            case DeadZoneShape.Cube:
                Gizmos.DrawWireCube(gizmoPosition, deadzoneSize * 2);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, upAxis);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward * (moveSpeed == 0 ? 1 : moveSpeed + Mathf.Sign(moveSpeed)));
    }

    private void CalculatePositions(out Vector3 targetPosition, out bool follow)
    {
        targetPosition = transform.position + offset;

        var deviation = followTarget.position - targetPosition;
        follow = deadzoneShape switch
        {
            DeadZoneShape.Cube => !VectorInRange(-deadzoneSize, deadzoneSize, deviation),
            _ => deviation.magnitude >= deadzoneSize.magnitude,
        };
    }

    public static bool ValueInRange(float min, float max, float value) => value >= min && value <= max;
    public static bool VectorInRange(Vector3 min, Vector3 max, Vector3 value) => ValueInRange(min.x, max.x, value.x)
                                                                          && ValueInRange(min.y, max.y, value.y)
                                                                          && ValueInRange(min.z, max.z, value.z);
}

public static class Utils
{
    // Transform Extension method
    public static void LookTowards(this Transform transform, Vector3 toDirection, Vector3 up, float maxDegrees)
    {
        transform.rotation = OperableQuaternion.RotateTowards(
            from: transform.rotation,
            to: OperableQuaternion.LookRotation(toDirection, up),
            maxDegrees);
    }
}
