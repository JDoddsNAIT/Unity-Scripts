# Spline Path

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/05/20*|*2024/05/25*|

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

> :paperclip: This requires multiple scripts in order to function. To add them to your Unity project, simply import the [Unity Package][package] into the assets folder.

## 🛠️ Requirements

These scripts make use of the following components and assets:
- [`Transform`][transform] (component)

## 📖 Description
Follow Path allows you to create a `Path` consisting of multiple transforms, and have a object travel between their positions. To use the [`FollowPath`](#followpath-script) script, you must first create a path for it to follow using the [`Path`](#path-script) script.

---
# FollowPath Script

## 📖 Description
Attaching the `FollowPath` script to an object will make it translate along the assigned `path`. You may stop movement by modifying the `enabled` member. The `moveTime` property determines how long it will take for the object reach the end of path. Depending on `endAction`, the script will behave differently when the end of the path is reached. Either the script will be disabled, the movement direction will be reversed, or the object will return to the starting point in the `path`.

## 🔧 Properties
| Property | Summary |
|-|-|
| `path` | The path that the object will follow. |
| `moveTime` | The number of seconds to travel the length of the `path`. |
| `timeOffset` | Allows you to offset the object further along the path. |
| `endAction` | Determines the behavior when the object reaches the end of the `path`. `Stop` means the object will stop moving, `Reverse` means the direction will be reversed and movement will continue, and `Continue` means the object will loop around the path without stopping. |
| `reverse` | The object will travel in the opposite direction if checked. |

## ⚙️ Gizmos
When the script is selected, a yellow sphere will show where the object will start from, which can be changed  using `timeOffset`.

## 💾 Source Code
```cs
using UnityEngine;

[AddComponentMenu("Spline Path/Follow Path")]
[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
public class FollowPath : MonoBehaviour
{
    public enum EndAction
    {
        Stop,       // Stop moving
        Reverse,    // Reverse direction
        Continue,   // return to start
    }

    #region Inspector Values
    [Space]
    public Path path;
    [Header("Settings")]
    [Tooltip("The time in seconds to travel between each node.")]
    [Min(0)] public float moveTime = 1.0f;
    [Tooltip("The time offset in seconds.")]
    [Min(0)] public float timeOffset = 0.0f;
    [Tooltip("What to do when the end of the path is reached.")]
    public EndAction endAction;
    [Tooltip("Reverses direction.")]
    public bool reverse = false;
    #endregion

    #region Private members
    private Timer _moveTimer;
    private int Reverse => reverse ? -1 : 1;
    #endregion

    #region Unity Messages
    private void Start()
    {
        if (path.PathIsValid)
        {
            _moveTimer = new Timer(moveTime, timeOffset % moveTime);
        }
        else
        {
            enabled = false;
        }
    }

    private void Update()
    {
        _moveTimer = new Timer(moveTime, _moveTimer.Time);
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
        Gizmos.color = Color.yellow;
        path.GetPoint(timeOffset % moveTime, out var position, out _);
        Gizmos.DrawSphere(position, 0.2f);
    }
    #endregion

    private void MoveAlongPath()
    {
        _moveTimer.Time += Reverse * Time.deltaTime;

        path.GetPoint(_moveTimer.Value, out var position, out var rotation);
        transform.SetPositionAndRotation(position, rotation ?? transform.rotation);

        if (_moveTimer.Alarm)
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
                default:
                    break;
            }

            _moveTimer.Time = reverse ? moveTime : 0;
        }
    }

    public void Toggle() => enabled = !enabled;
}

public struct Timer
{
    private float time;

    public float Length { get; set; }
    public float Time
    {
        readonly get => time;
        set => time = value >= Length ? Length : value <= 0 ? 0 : value;
    }

    public Timer(float length) : this() => Length = length;

    public Timer(float length, float time) : this(length) => Time = time;

    /// <summary> Returns <see cref="Time"/> divided by <see cref="Length"/>. </summary>
    public readonly float Value => Time / Length;
    /// <summary> Returns true when <see cref="Time"/> is 0 or <see cref="Length"/>. </summary>
    public readonly bool Alarm => Time >= Length | Time <= 0;
}
```

# Path Script

## 📖 Description
The `Path` script allows you to create a path to follow. To create a path, fill the `points` array with `Transforms`. `FollowPath` will traverse these points in order, inheriting their position and rotation. To connect the first and last points of the array, check the `closeLoop` box.

> :paperclip: You can also easily create a path by setting all the `Transforms` as children of the script in the heirarchy, the selecting "Generate Path" in the context menu. (Access the context menu by right-clicking the component.)

There are three types of paths: `Linear`, `Bezier`, and `Catmull-Rom`. 

`Linear` paths are simple, connecting each `Transform` to the next in a stright line.

`Bezier` creates a path that is smooth. The path will pass only through the start an end points.

`Catmull-Rom` is simmilar to `Bezier`, being a smooth path, the only difference is that `Catmull-Rom` will pass through all points on the path if  `closeLoop` is checked. If not, the first and last `Transform` in the `points` list are excluded from the path, acting instead as control points.

## ✒️ Signatures
| Property | Summary |
|-|-|
| `pathColor` | The path's drawn colour. |
| `curveSegments` | The amount of segements drawn in the curve. Only affects the Bezier and Catmull-Rom path types. |
| `closeLoop` | If checked, the first and last `points` in the path will be connected. |
| `points` | The list of points that make up the path. `FollowPath` will move to each point in order. |
| `UseChildren` | Generates a path using child transforms. |

## ⚙️ Gizmos

Lines depicting the path.

## 💾 Source Code
> :paperclip: This section only contains the source code for the abstract class `Path`. You can view the source code for `Linear`, `Bezier`, and `Catmull-Rom` path types by importing the [Unity Package][package] into any project.
```cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Follow-Path")]
public abstract class Path : MonoBehaviour
{
    #region Inspector Values
    [Header("Gizmo settings")]
    public Color pathColor = Color.white;
    [Tooltip("The amount of segments drawn. Ignored if Path Type is Linear.")]
    [Range(1, 100)] public int curveSegments = 20; 
    [Header("Path settings")]
    public bool closeLoop;
    public List<Transform> points = new();
    #endregion

    private readonly string INVALID_PATH = $"Length of {nameof(points)} cannot be less than 2 or contain any nulls.";
    public virtual bool PathIsValid
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

    public abstract void GetPoint(float t, out Vector3 position, out Quaternion? rotation);

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

[package]: ./splinePath.unitypackage
[transform]: https://docs.unity3d.com/ScriptReference/Transform.html
