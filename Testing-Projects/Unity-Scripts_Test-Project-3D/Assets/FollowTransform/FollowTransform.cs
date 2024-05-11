using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/blob/main/Scripts/Follow-Transform/")]
public class FollowTransform : MonoBehaviour
{
    public enum DeadZoneShape
    {
        Cube, Sphere
    }

    [Header("Movement")]
    [Tooltip("Units/sec"), Min(0)]
    public float followSpeed;
    public List<Transform> targets;
    public Vector3 offset;
    public Vector3 deadZone;
    public DeadZoneShape deadZoneShape = DeadZoneShape.Cube;
    [Header("Rotation")]
    [Tooltip("Degs/sec"), Min(0)]
    public float turnSpeed;
    public Vector3 startingAngle;
    public bool useLook;

    private Vector3 StartingDirection => Quaternion.Euler(startingAngle) * Vector3.forward;

    private bool ShouldFollow(Vector3 deviation)
    {
        return deadZoneShape switch
        {
            DeadZoneShape.Cube => !VectorInRange(-deadZone, deadZone, deviation),
            _ => deviation.magnitude >= deadZone.magnitude,
        };
    }

    void Update()
    {
        CalculateDeviation(out var position, out var target, out var deviation);
        if (ShouldFollow(deviation))
        {
            transform.position = Vector3.MoveTowards(position, target, followSpeed * Time.deltaTime) - offset;
        }
        if (turnSpeed > 0)
        {
            transform.rotation = useLook
                ? RotateTowardsDirection(transform.forward, target, StartingDirection, turnSpeed * Time.deltaTime)
                : LookTowardsDirection(transform.forward, target, StartingDirection, turnSpeed * Time.deltaTime);
        }
    }

    public Quaternion RotateTowardsDirection(Vector3 fromDirection, Vector3 toDirection, Vector3 forwards, float maxDegrees)
    {
        Debug.DrawRay(transform.position, Quaternion.Euler(startingAngle) * fromDirection);
        Debug.DrawRay(transform.position, Quaternion.Euler(startingAngle) *  toDirection);

        return Quaternion.RotateTowards(
            from: Quaternion.FromToRotation(forwards, fromDirection),
            to: Quaternion.FromToRotation(forwards, toDirection),
            maxDegrees);
    }

    public Quaternion LookTowardsDirection(Vector3 fromDirection, Vector3 toDirection, Vector3 forwards, float maxDegrees)
    {
        Debug.DrawRay(transform.position, Quaternion.Euler(startingAngle) * fromDirection);
        Debug.DrawRay(transform.position, Quaternion.Euler(startingAngle) * toDirection);

        return Quaternion.RotateTowards(
            from: Quaternion.FromToRotation(forwards, fromDirection),
            to: Quaternion.FromToRotation(forwards, Quaternion.LookRotation(toDirection) * Vector3.right),
            maxDegrees);
    }

    private void OnDrawGizmosSelected()
    {
        //Deadzone
        CalculateDeviation(out var gizmoPosition, out var target, out var deviation);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gizmoPosition, target);
        Gizmos.color = ShouldFollow(deviation) ? Color.green : Color.red;
        switch (deadZoneShape)
        {
            case DeadZoneShape.Sphere:
                Gizmos.DrawWireSphere(gizmoPosition, deadZone.magnitude);
                break;
            case DeadZoneShape.Cube:
                Gizmos.DrawWireCube(gizmoPosition, deadZone * 2);
                break;
        }

        //Forward and upward
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, StartingDirection);
    }

    private void CalculateDeviation(out Vector3 targetPosition, out Vector3 averagePosition, out Vector3 deviation)
    {
        targetPosition = transform.position + offset;

        averagePosition = Vector3.zero;
        if (targets is not null && targets.Count > 0)
        {
            foreach (var pos in targets)
            {
                averagePosition += pos.position;
            }
            averagePosition /= targets.Count;
        }

        deviation = averagePosition - targetPosition;
    }
    
    public static bool ValueInRange(float min, float max, float value) => value >= min && value <= max;
    public static bool VectorInRange(Vector3 min, Vector3 max, Vector3 value) => ValueInRange(min.x, max.x, value.x)
                                                                          && ValueInRange(min.y, max.y, value.y)
                                                                          && ValueInRange(min.z, max.z, value.z);
}
