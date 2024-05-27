# Spline Path

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/05/20*|*2024/05/27*|

- [Spline Path](#spline-path)
  - [🛠️ Requirements](#️-requirements)
  - [📖 Description](#-description)
- [FollowPath Scripts](#followpath-scripts)
  - [📖 Description](#-description-1)
  - [🔧 Properties](#-properties)
  - [⚙️ Gizmos](#️-gizmos)
- [Path Script](#path-script)
  - [📖 Description](#-description-2)
  - [✒️ Signatures](#️-signatures)
  - [⚙️ Gizmos](#️-gizmos-1)

> :paperclip: This requires multiple scripts in order to function. To add them to your Unity project, simply import the [Unity Package][package] into the assets folder.

## 🛠️ Requirements

These scripts make use of the following components and assets:
- [`Transform`][transform] (component)
- [`Rigidbody`][rigidbody] (component)
- [`Rigidbody2D`][rigidbody2d] (component)

## 📖 Description
Follow Path allows you to create a `Path` consisting of multiple transforms, and have a object travel between their positions. To use the [`FollowPath`](#followpath-script) scripts, you must first create a path for it to follow using the [`Path`](#path-script) script.

---
# FollowPath Scripts

## 📖 Description
There are three variations to this script. `FollowPath` follows the path by setting the position to a point along the path. `FollowPath3D` and `FollowPath2D` work by setting the `velocity` of a `Rigidbody` and `Rigidbody2D` component respectively.

Adding either `FollowPath` script to an object will make it move along the assigned `path` while the script is `enabled`. The `moveTime` property determines how long it will take for the object reach the end of path. Depending on `endAction`, the script will behave differently when the end of the path is reached. Either the script will be disabled, the movement direction will be reversed, or the object will return to the starting point in the `path`.

Avoid adding multiple `FollowPath` scripts to a single object as the results are unpredictable.

## 🔧 Properties
| Property | Summary |
|-|-|
| `path` | The path that the object will follow. |
| `moveTime` | The number of seconds to travel the length of the `path`. |
| `timeOffset` | Allows you to offset the object further along the path. |
| `endAction` | Determines the behavior when the object reaches the end of the `path`. `Stop` means the object will stop moving, `Reverse` means the direction will be reversed and movement will continue, and `Continue` means the object will loop around the path without stopping. |
| `reverse` | The object will travel in the opposite direction if checked. |
| `rotationMode` | Dictates how the script will rotate the body. Use `None` for no ratation, `Keyframe` to interpolate between the rotation of each point, and `Path` to make the body look in the direction it is moving.

## ⚙️ Gizmos
When the script is selected, a yellow sphere will show where the object will start from, which can be changed  using `timeOffset`.

# Path Script

## 📖 Description
The `Path` script allows you to create a path to follow. To create a path, fill the `points` array with `Transforms`. `FollowPath` will traverse these points in order, inheriting their position and rotation. To connect the first and last points of the array, check the `closeLoop` box.

> :paperclip: You can also easily create a path by setting all the `Transforms` as children of the script in the heirarchy, then selecting "Generate Path" in the context menu. (Access the context menu by right-clicking the component.)

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

---
> :paperclip: Done looking? Check out more scripts [here](../).

[package]: ./splinePath.unitypackage
[transform]: https://docs.unity3d.com/ScriptReference/Transform.html
[rigidbody]: https://docs.unity3d.com/ScriptReference/Rigidbody.html
[rigidbody2d]: https://docs.unity3d.com/ScriptReference/Rigidbody2D.html