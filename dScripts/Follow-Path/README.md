# Spline Path

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/05/20*|*2024/05/20*|

- [Spline Path](#spline-path)
  - [🛠️ Requirements](#️-requirements)
  - [📖 Description](#-description)
- [FollowPath Script](#followpath-script)
  - [📖 Description](#-description-1)
  - [🔧 Properties](#-properties)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)
- [Path Script](#path-script)
  - [📖 Description](#-description-2)
  - [✒️ Signatures](#️-signatures)
  - [⚙️ Gizmos](#️-gizmos-1)
  - [💾 Source Code](#-source-code-1)

> :paperclip: This requires multiple scripts in order to function. To add them to your Unity project, simply import the [Unity Package](./followPath.unitypackage) into the assets folder.

## 🛠️ Requirements

These scripts make use of the following components and assets:
- [`Transform`][transform] (component)

## 📖 Description
Follow Path allows you to create a `Path` consisting of multiple transforms, and have a object travel between their positions. To use the [`FollowPath`](#followpath-script) script, you must first create a path for it to follow using the [`Path`](#path-script) script.

---
# FollowPath Script

## 📖 Description
Attaching the `FollowPath` script to an object will make it translate along the assigned `path`. You may stop movement by modifying the `enabled` member. The `moveTime` property determines how long it will take for the object reach each point in the path. Depending on `endAction`, the script will behave differently when the end of the path is reached. Either the script will be disabled, the movement direction will be reversed, or the object will return to the starting point in the `path`.

## 🔧 Properties
| Property | Summary |
|-|-|
| `path` | The path that the object will follow. |
| `moveTime` | The number of seconds it will take to travel between each point. |
| `timeOffset` | Allows you to offset the object further along the path. |
| `endAction` | Determines the behavior when the object reaches the end of the `path`. `Stop` means the object will stop moving, `Reverse` means the direction will be reversed and movement will continue, and `Continue` means the object will loop around the path without stopping. |
| `reverse` | The object will travel in the opposite direction if checked. |

## ⚙️ Gizmos
When the script is selected, a yellow sphere will show where the object will start from, which can be changed  using `timeOffset`.

## 💾 Source Code
```cs
using JDoddsNAIT.Unity.CommonLib;
using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
public class FollowPath : MonoBehaviour
{
    public enum EndAction
    {
        Stop,       // Stop moving
        Reverse,    // Reverse direction
        Continue,   // return to start
    }
    #region Public members
    [Space]
    public Path path;
    [Tooltip("The time in seconds to travel between each node.")]
    [Min(0)] public float moveTime = 1.0f;
    [Tooltip("The time offset in seconds.")]
    [Min(0)] public float timeOffset = 0.0f;
    [Header("Settings")]
    [Tooltip("What to do when the end of the path is reached.")]
    public EndAction endAction;
    [Tooltip("Reverses direction.")]
    public bool reverse = false;
    #endregion

    #region Private members
    private Timer moveTimer;
    private int pathIndex = 0;

    private int Reverse => reverse ? -1 : 1;
    #endregion

    #region Unity Messages
    private void Start()
    {
        if (path.PathIsValid)
        {
            moveTimer = new Timer(moveTime, timeOffset % moveTime);
            pathIndex = (int)timeOffset % path.points.Count;
        }
        else
        {
            enabled = false;
        }
    }

    private void Update()
    {
        moveTimer = new Timer(moveTime, moveTimer.Time);
        if (!path.PathIsValid)
        {
            enabled = false;
        }
        else
        {
            MoveAlongPath();
        }
    }

    private void OnDrawGizmosSelected()
    {
        path.LerpPath((int)timeOffset % path.points.Count, timeOffset % moveTime, out var position, out _);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(position, 0.2f);
    }
    #endregion

    private void MoveAlongPath()
    {
        try
        {
            path.LerpPath(pathIndex, moveTimer.Value, out var position, out var rotation);
            transform.SetPositionAndRotation(position, rotation);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            switch (endAction)
            {
                case EndAction.Stop:
                    reverse = !reverse;
                    enabled = false;
                    break;
                case EndAction.Reverse:
                    reverse = !reverse;
                    break;
                case EndAction.Continue:
                    pathIndex = reverse ? path.points.Count - 1 : 0;
                    break;
                default:
                    throw;
            }
        }
        finally
        {
            moveTimer.Time += Reverse * Time.deltaTime;

            if (moveTimer.Alarm)
            {
                moveTimer.Time = reverse ? moveTime : 0;
                pathIndex += Reverse;
            }
        }
    }

    public void Toggle() => enabled = !enabled;
}

```

# Path Script

## 📖 Description
The `Path` script allows you to create a path to follow. To create a path, fill the `points` array with `Transforms`. `FollowPath` will traverse these points in order, inheriting their position and rotation. To connect the first and last points of the array, check the `closeLoop` box.

> :paperclip: You can also easily create a path by setting all the `Transforms` as children of the script in the heirarchy, the selecting "Generate Path" in the context menu. (Access the context menu by right-clicking the component.)

## ✒️ Signatures
| Property | Summary |
|-|-|
| `pathColor` | The path's drawn colour. |
| `curveSegments` | The amount of segements drawn in the curve. Only affects the Bezier and Catmull-Rom path types. |
| `closeLoop` | If checked, the first and last `points` in the path will be connected. |
| `points` | The list of points that make up the path. `FollowPath` will move to each point in order. |
| `UseChildren` | Generates a path using child transforms. |

## ⚙️ Gizmos

Yellow lines conneting each point to visualize the path.

## 💾 Source Code
```cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> points = new();
    public bool closeLoop;

    private readonly string INVALID_PATH = $"Length of {nameof(points)} cannot be less than 2 or contain any nulls.";
    public bool PathIsValid
    {
        get
        {
            var result = points != null && (points.Count >= 2) && !points.Where(p => p == null).Any();
            if (!result)
            {
                Debug.LogError(INVALID_PATH);
            }
            return result;
        }
    }

    public void LerpPath(int index, float t, out Vector3 position, out Quaternion rotation)
    {
        var nextIndex = closeLoop ? (index + 1) % points.Count : index + 1;
        position = Vector3.Lerp(
               a: points[index].position,
               b: points[nextIndex].position,
               t: t);
        rotation = Quaternion.Lerp(
               a: points[index].rotation,
               b: points[nextIndex].rotation,
               t: t);
    }

    private void OnDrawGizmos()
    {
        if (PathIsValid)
        {
            Gizmos.color = Color.yellow;
            for (int i = closeLoop ? 0 : 1; i < points.Count; i++)
            {
                Gizmos.DrawLine(points[i].position, points[closeLoop ? (i + 1) % points.Count : i - 1].position);
            }
        }
    }

    [ContextMenu("Generate Path from Children")]
    private void UseChildren()
    {
        List<Transform> transforms = GetComponentsInChildren<Transform>(includeInactive: false).Where(t => t != transform).ToList();
        if (transforms.Count > 0)
        {
            points = transforms;
        }
        else
        {
            Debug.Log("Could not generate path as no children were found");
        }
    }
}

```
---
> :paperclip: Done looking? Check out more scripts [here](../).

[transform]: https://docs.unity3d.com/ScriptReference/Transform.html
