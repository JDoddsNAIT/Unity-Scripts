using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public enum DeadZoneShape
    {
        Sphere, Cube
    }

    [Min(0)] public float followSpeed;
    public Vector3 offset;
    [Space]
    public Vector3 deadZone;
    public DeadZoneShape deadZoneShape = DeadZoneShape.Cube;
    [Space]
    public List<Transform> targets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        CalculateTargetAndDirection(out var gizmoPosition, out var target);

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

    private void CalculateTargetAndDirection(out Vector3 targetPosition, out Vector3 targetDirection)
    {
        targetPosition = transform.position + offset;
        Vector3 averagePosition = Vector3.zero;
        foreach (var pos in targets)
        {
            averagePosition += pos.position;
        }
        averagePosition /= targets.Count;

        targetDirection = Vector3.Normalize(averagePosition - targetPosition);
    }
}
