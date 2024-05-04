using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public enum DeadZoneShape
    {
        Cube, Sphere
    }

    [Min(0)] public float followSpeed;
    public Vector3 offset;
    [Space]
    public Vector3 deadZone;
    public DeadZoneShape deadZoneShape = DeadZoneShape.Cube;
    [Space]
    public List<Transform> targets;

    private bool ShouldFollow(Vector3 position, Vector3 target)
    {
        Vector3 deviation = target - position;
        return deadZoneShape switch
        {
            DeadZoneShape.Cube => !VectorInRange(-deadZone, deadZone, deviation),
            _ => deviation.magnitude > deadZone.magnitude,
        };
    }

    void Update()
    {
        CalculateTargetAndAverage(out var position, out var target);
        if (ShouldFollow(position, target))
        {
            transform.position = Vector3.MoveTowards(position, target, followSpeed * Time.deltaTime) - offset;
        }
    }

    private void OnDrawGizmosSelected()
    {
        CalculateTargetAndAverage(out var gizmoPosition, out var target);

        Gizmos.color = Color.yellow;

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

    private void CalculateTargetAndAverage(out Vector3 targetPosition, out Vector3 averagePosition)
    {
        targetPosition = transform.position + offset;

        averagePosition = Vector3.zero;
        foreach (var pos in targets)
        {
            averagePosition += pos.position;
        }
        averagePosition /= targets.Count;
    }

    public bool ValueInRange(float min, float max, float value) => value >= min && value <= max;
    public bool VectorInRange(Vector3 min, Vector3 max, Vector3 value) => ValueInRange(min.x, max.x, value.x)
                                                                          && ValueInRange(min.y, max.y, value.y)
                                                                          && ValueInRange(min.z, max.z, value.z);
}
