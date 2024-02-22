# Follow Transform

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/02/22*|*2024/02/22*|

> To add this script to your Unity project, simply import the [Unity Package](./FollowObject.unitypackage) into the assets folder, or create a new C# script and paste in the [source code](specs.md) below.

## Description


---
## Documentation


---
## Source Code
``` cs
// View documentation at https://github.com/JDoddsNAIT/Unity-Scripts/blob/main/Scripts/Follow-Transform/specs.md
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform followTransform;
    public Vector3 targetOffset;
    public float deadZoneRadius;
    public float followSpeed;

    void Update()
    {
        Vector3 targetPosition = transform.position + targetOffset;
        Vector3 targetDirection = Vector3.Normalize(followTransform.position - targetPosition);
        float targetDistance = Vector3.Distance(followTransform.position, targetPosition) - deadZoneRadius;

        if (targetDistance >= 0)
        {
            transform.Translate(followSpeed * targetDistance * Time.deltaTime * targetDirection);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 targetPosition = transform.position + targetOffset;
        Vector3 targetDirection = Vector3.Normalize(followTransform.position - targetPosition);
        Vector3 gizmoPosition = targetPosition + targetDirection * deadZoneRadius;
        float targetDistance = Vector3.Distance(followTransform.position, targetPosition) - deadZoneRadius;

        if (targetDistance > 0)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(gizmoPosition, followTransform.position);
        Gizmos.DrawWireSphere(targetPosition, deadZoneRadius);
    }
}
```