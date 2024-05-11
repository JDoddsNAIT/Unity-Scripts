using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    public Vector3 To => target.position;

    public bool useLook;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, To.normalized);
    }

    private void Update()
    {
        transform.rotation = (useLook
            ? LookTowards(transform.rotation, To, speed)
            : RotateTowardsDirection(transform.rotation, To, speed));
    }

    public Quaternion RotateTowardsDirection(Quaternion from, Vector3 toDirection, float maxDegrees)
    {
        return Quaternion.RotateTowards(
            from: from,
            to: Quaternion.FromToRotation(Vector3.forward, toDirection),
            maxDegrees);
    }

    public Quaternion LookTowards(Quaternion from, Vector3 target, float maxDegrees)
    {
        return Quaternion.RotateTowards(
            from: from,
            to: Quaternion.LookRotation(target, Vector3.up),
            maxDegrees);
    }
}
