# Follow Transform

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/02/22*|*2024/02/22*|

- [Follow Transform](#follow-transform)
  - [Requirements](#requirements)
- [Documentation](#documentation)
  - [Description](#description)
  - [Signatures](#signatures)
  - [Gizmos](#gizmos)
- [Source Code](#source-code)

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./followTransform.unitypackage) into the assets folder, or create a new C# script and paste in the [source code](./specs.md#source-code) below.

## Requirements

This script makes use of the following components:
-  [`Transform`](https://docs.unity3d.com/ScriptReference/Transform.html)

---
# Documentation

## Description
**Follow Transform** is a simple script that makes an object move towards a specified **transform's position** every frame. You may set an **offset**, which dictates the target postion relative to the object this script is attached to. Movement stops if the transform being followed has it's position within a certain **radius**.

## Signatures

| Datatype | Name | Summary |
|-|-|-|
| `Transform` | `followTransform` | A transform the attached object will move towards.
| `Vector3 ` | `targetOffset ` | The relative position that this script wants `followTransform` to be at.  |
| `float ` | `deadZoneRadius ` | The radius around `targetOffset` in which `followTransform` can move freely without being followed. |
| `float ` | `followSpeed ` | The speed at which the object follows the target. |

## Gizmos
- A line from the target transform's position to the edge of the dead zone to visualize the direction of movement and where the target transform is.  
- A wire sphere to visualize the dead zone. 
- All gizmos are green when the transform is outside the dead zone and red when inside.

---
# Source Code
``` cs
// View documentation at https://github.com/JDoddsNAIT/Unity-Scripts/blob/main/Scripts/Follow-Transform/
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
