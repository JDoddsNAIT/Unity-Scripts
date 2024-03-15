# Add Gizmo

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/03/15*|*2024/03/15*|

- [Add Gizmo](#add-gizmo)
  - [🛠️ Requirements](#️-requirements)
  - [📖Description](#description)
- [Documentation](#documentation)
  - [✒️Signatures](#️signatures)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./) into the assets folder, or create a new C# script and paste in the [source code](#source-code) below.

## 🛠️ Requirements

This script makes use of the following components:
- [`Transform`][transform]

## 📖Description
Easily add a gizmo to your game object. You can choose between a **Sphere**, **Cube**, **WireSphere**, **WireCube**, or **Ray** in the dropdown menu. You can set it's **position**, **size**, and **color**, as well as if the gizmo will only draw when **the object is selected**. Gizmos will only be drawn if the script is **enabled**.

---
# Documentation

## ✒️Signatures
| Datatype | Name | Summary |
|-|-|-|
| `bool` | `onSelect` | If true, the gizmo will only be drawn when the object is selected. |
| `enum` | `gizmo` | The kind of gizmo that will be drawn. |
| `Color` | `color` | The gizmo's color, white by default. |
| `Vector3` | `position` | The relative position of the gizmo. |
| `Vector3` | `size` | The size of the gizmo. the `Sphere` and `WireSphere` gizmos will use the magnitude for their radius. |
## ⚙️ Gizmos

- Either a Sphere, WireSphere, Cube, WireCube, or Ray with a color, size, and position of your choosing.

## 💾 Source Code
``` cs
// Coming soon!
```
---
> :paperclip: Done looking? Check out more scripts [here.](../)

[transform]: https://docs.unity3d.com/ScriptReference/Transform.html
