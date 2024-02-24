> *This script is not yet available.*
# Path Follower

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/02/23*|*----/--/--*|

- [Path Follower](#path-follower)
  - [🛠️ Requirements](#️-requirements)
  - [📖 Description](#-description)
- [Documentation](#documentation)
  - [✒️ Signatures](#️-signatures)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./) into the assets folder, or create a new C# script and paste in the [source code](#source-code) below.

## 🛠️ Requirements

This script makes use of the following components:
- [`Transform`][transform]

## 📖 Description
> *A short description of what the script does and how the user can manipulate it*

---
# Documentation

## ✒️ Signatures

| Datatype | Name | Summary |
|-|-|-|
| [`List<Vector3>`][vector3] | `waypoints` | The ordered list of positions the object will move to. |
| `float` | `moveTime` | The time in seconds taken to move between each position in `waypoints`. |
| `bool` | `closedLoop` | Whether or not the object will return to it's starting position at the end of the path. |
| `bool` | `reverse` | Whether or not the object will reverse direction |
| `void` | `StartPath()` | Initiates the object's movement along the path. |

## ⚙️ Gizmos

> *List what gizmos show up and what they mean.*

## 💾 Source Code
``` cs
// Coming Soon!
```
---
> :paperclip: Done looking? Check out more scripts [here](../).

[transform]: https://docs.unity3d.com/ScriptReference/Transform.html
[vector3]: https://docs.unity3d.com/ScriptReference/Vector3.html