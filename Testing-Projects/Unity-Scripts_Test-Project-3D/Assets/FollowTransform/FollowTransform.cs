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
        Vector3 targetDeviation = target - position;
        return deadZoneShape switch
        {
            DeadZoneShape.Cube => Mathf.Abs(targetDeviation.x) > Mathf.Abs(deadZone.x) || Mathf.Abs(targetDeviation.y) > Mathf.Abs(deadZone.y),
            _ => targetDeviation.magnitude > deadZone.magnitude,
        };
    }

    // Update is called once per frame
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
                Gizmos.DrawWireCube(gizmoPosition, deadZone);
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
}
