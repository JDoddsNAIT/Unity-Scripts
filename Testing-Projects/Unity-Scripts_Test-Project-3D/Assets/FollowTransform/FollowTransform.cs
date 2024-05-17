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
    public Vector3 upwardVector;

    void Update()
    {
        CalculatePositions(out var position, out var target, out var follow);
        if (follow)
        {
            transform.position = Vector3.MoveTowards(position, target, followSpeed * Time.deltaTime) - offset;
        }
        if (turnSpeed > 0)
        {
            transform.rotation = LookTowards(transform.rotation, (target - transform.position).normalized, upwardVector, turnSpeed * Time.deltaTime);
        }
    }

    public static Quaternion LookTowards(Quaternion from, Vector3 to, Vector3 up, float maxDegrees)
    {
        return Quaternion.RotateTowards(
            from: from,
            to: Quaternion.LookRotation(to, up),
            maxDegrees);
    }

    private void OnDrawGizmosSelected()
    {
        //Deadzone
        CalculatePositions(out var gizmoPosition, out var target, out var follow);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gizmoPosition, target);
        Gizmos.color = follow ? Color.green : Color.red;
        switch (deadZoneShape)
        {
            case DeadZoneShape.Sphere:
                Gizmos.DrawWireSphere(gizmoPosition, deadZone.magnitude);
                break;
            case DeadZoneShape.Cube:
                Gizmos.DrawWireCube(gizmoPosition, deadZone * 2);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, upwardVector);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    private void CalculatePositions(out Vector3 targetPosition, out Vector3 averagePosition, out bool follow)
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

        var deviation = averagePosition - targetPosition;
        follow = deadZoneShape switch
        {
            DeadZoneShape.Cube => !VectorInRange(-deadZone, deadZone, deviation),
            _ => deviation.magnitude >= deadZone.magnitude,
        };
    }

    public static bool ValueInRange(float min, float max, float value) => value >= min && value <= max;
    public static bool VectorInRange(Vector3 min, Vector3 max, Vector3 value) => ValueInRange(min.x, max.x, value.x)
                                                                          && ValueInRange(min.y, max.y, value.y)
                                                                          && ValueInRange(min.z, max.z, value.z);
}
