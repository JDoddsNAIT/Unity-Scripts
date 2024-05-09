# Follow Transform

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/02/22*|*2024/05/08*|

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
| `float` | `followSpeed` | The speed at which to follow the target(s), in Units/second. |
| `List<Transform>` | `targets` | The transforms to follow. The script will target the average position of every `Transform` in the list. |
| `Vector3` | `offset` | The relative position on the deadzone's centre. |
| `Vector3` | `deadZone` | The size of the deadzone. |
| `enum` | `deadZoneShape` | The shape of the deadzone, either a sphere or cube. |
| `float` | `turnSpeed` | The speed at which to turn towards the target(s) at, in Degrees/second. |
| `Vector3` | `startingAngle` | The object's initial facing angles (in degrees). |
| `Vector3` | `upwardVector` | The upward direction used for the `LookRotation` method. If zero, the script will use the `FromToRotation` method instead. |

## ⚙️ Gizmos
- A yellow line from the target transform's position to the centre of the dead zone to visualize the direction of movement and where the target transform is.  
- A wire sphere/cube to visualize the dead zone. Will be green when the target is outside the dead zone and red when inside.
- A blue ray to visualize the forward direction.
- A green ray to visualize the `upwardVector`.

## 💾 Source Code
``` cs
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
    public Vector3 upwardVector = Vector3.up;

    private Quaternion StartingAngle => Quaternion.Euler(startingAngle);

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
            PointTowards(target, turnSpeed);
        }
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
        Gizmos.DrawRay(transform.position, transform.rotation * (StartingAngle * Vector3.forward));
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, upwardVector.normalized);
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

    private void PointTowards(Vector3 target, float turnSpeed) => transform.rotation = Equals(upwardVector, Vector3.zero)
            ? Quaternion.RotateTowards(
                from: transform.rotation,
                to: Quaternion.FromToRotation(StartingAngle * Vector3.forward, (target - transform.position).normalized),
                maxDegreesDelta: turnSpeed * Time.deltaTime)
            : Quaternion.RotateTowards(
                from: transform.rotation,
                to: Quaternion.FromToRotation(
                    StartingAngle * Vector3.forward,
                    Quaternion.LookRotation((target - transform.position).normalized, upwardVector) * Vector3.forward),
                maxDegreesDelta: turnSpeed * Time.deltaTime);

    public bool ValueInRange(float min, float max, float value) => value >= min && value <= max;
    public bool VectorInRange(Vector3 min, Vector3 max, Vector3 value) => ValueInRange(min.x, max.x, value.x)
                                                                          && ValueInRange(min.y, max.y, value.y)
                                                                          && ValueInRange(min.z, max.z, value.z);
}

```
---
> :paperclip: Done looking? Check out more scripts [here](../).

[transform]: https://docs.unity3d.com/ScriptReference/Transform.html
