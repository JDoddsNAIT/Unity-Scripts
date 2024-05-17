# Follow Transform

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/02/22*|*2024/05/16*|

- [Follow Transform](#follow-transform)
  - [🛠️ Requirements](#️-requirements)
- [Documentation](#documentation)
  - [📖 Description](#-description)
  - [✒️ Signatures](#️-signatures)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./followTransform.unitypackage) into the assets folder, or create a new C# script and paste in the [source code](#-source-code) below.

## 🛠️ Requirements

This script makes use of the following components:
- :link:[`Transform`][transform]

> :warning: This script was written in Unity version `2022.3.20f1`

---
# Documentation

## 📖 Description
**Follow Transform** is a simple script that makes an object move towards a specified **transform's position** every frame. You may set an **offset**, which dictates the target postion relative to the object this script is attached to. Movement stops if the transform being followed has it's position within a specified **deadzone**.

## ✒️ Signatures

| Datatype | Name | Summary |
|-|-|-|
| `Transform` | `followTarget` | The target transform to follow. |
| `float` | `moveSpeed` | The speed at which to follow the target, in Units/second. |
| `float` | `turnSpeed` | The speed at which to turn towards the target(s) at, in Degrees/second. |
| `Vector3` | `upwardVector` | The upward direction used for the `LookRotation` method. |
| `enum` | `deadzoneShape` | The shape of the deadzone, either a sphere or cube. |
| `Vector3` | `deadzoneSize` | The size of the deadzone. |
| `Vector3` | `offset` | The relative position on the deadzone's centre. |

## ⚙️ Gizmos
- A yellow line from the target transform's position to the centre of the dead zone to visualize the direction of movement and where the target transform is.  
- A wire sphere/cube to visualize the dead zone. Will be green when the target is outside the dead zone and red when inside.
- A cyan ray to visualize the forward direction.
- A green ray to visualize the `upwardVector`.

## 💾 Source Code
``` cs
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
    [Tooltip("Units/sec"), Min(0)]
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
        Gizmos.DrawRay(transform.position, transform.forward);
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

public static class TransformExtension
{
    // Transform Extension method
    public static void LookTowards(this Transform transform, Vector3 toDirection, Vector3 up, float maxDegrees)
    {
        transform.rotation = Quaternion.RotateTowards(
            from: transform.rotation,
            to: Quaternion.LookRotation(toDirection, up),
            maxDegrees);
    }
}

```
---
> :paperclip: Done looking? Check out more scripts [here](../).

[transform]: https://docs.unity3d.com/ScriptReference/Transform.html
