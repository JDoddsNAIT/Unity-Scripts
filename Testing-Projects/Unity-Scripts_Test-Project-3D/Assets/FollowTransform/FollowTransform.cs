using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public enum DeadZoneShape
    {
        Cube, Sphere
    }

    [Tooltip("Units/sec")]
    [Min(0)] public float followSpeed;
    [Tooltip("Degs/sec")]
    [Min(0)] public float turnSpeed;
    [Space]
    public Vector3 offset;
    public Vector3 deadZone;
    public DeadZoneShape deadZoneShape = DeadZoneShape.Cube;
    [Space]
    public List<Transform> targets;

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
        CalculateTargetAndAverage(out var position, out var target, out var deviation);
        if (ShouldFollow(deviation))
        {
            transform.position = Vector3.MoveTowards(position, target, followSpeed * Time.deltaTime) - offset;
        }
        if (turnSpeed > 0)
        {
            PointTowards(target, turnSpeed);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        CalculateTargetAndAverage(out var gizmoPosition, out var target, out var deviation);

        Gizmos.color = ShouldFollow(deviation) ? Color.green : Color.red;

        Gizmos.DrawLine(gizmoPosition, target);
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

    private void CalculateTargetAndAverage(out Vector3 targetPosition, out Vector3 averagePosition, out Vector3 deviation)
    {
        targetPosition = transform.position + offset;

        averagePosition = Vector3.zero;
        foreach (var pos in targets)
        {
            averagePosition += pos.position;
        }
        averagePosition /= targets.Count;

        deviation = averagePosition - targetPosition;
    }

    private void PointTowards(Vector3 target, float turnSpeed)
    {
        transform.rotation = Quaternion.RotateTowards(
            from: transform.rotation,
            to: Quaternion.LookRotation((target - transform.position).normalized),
            maxDegreesDelta: turnSpeed * Time.deltaTime);
    }

    public bool ValueInRange(float min, float max, float value) => value >= min && value <= max;
    public bool VectorInRange(Vector3 min, Vector3 max, Vector3 value) => ValueInRange(min.x, max.x, value.x)
                                                                          && ValueInRange(min.y, max.y, value.y)
                                                                          && ValueInRange(min.z, max.z, value.z);
}
