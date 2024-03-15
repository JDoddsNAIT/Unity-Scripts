# ⚙️Add Gizmo⚙️

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/03/15*|*2024/03/15*|

- [⚙️Add Gizmo⚙️](#️add-gizmo️)
  - [🛠️ Requirements](#️-requirements)
  - [📖Description](#description)
- [Documentation](#documentation)
  - [✒️Signatures](#️signatures)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./addgizmo.unitypackage) into the assets folder, or create a new C# script and paste in the [source code](#source-code) below.

## 🛠️ Requirements
> :warning: This script was written in Unity version `2022.3.20f1`

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
// View documentation at https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Add-Gizmo
using UnityEngine;

public class AddGizmo : MonoBehaviour
{
    public enum Shape
    {
        Sphere,
        WireSphere,
        Cube,
        WireCube,
        Ray,
    }

    public bool onSelected;
    public Shape gizmo;
    public Color color = Color.white;
    public Vector3 position;
    public Vector3 size;

    private void OnDrawGizmos()
    {
        if (enabled && !onSelected)
        {
            DrawGizmo();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (enabled && onSelected)
        {
            DrawGizmo();
        }
    }

    private void DrawGizmo()
    {
        Vector3 gizmoPosition = transform.position + position;
        Gizmos.color = color;
        switch (gizmo)
        {
            case Shape.Sphere:
                Gizmos.DrawSphere(gizmoPosition, size.magnitude);
                break;
            case Shape.WireSphere:
                Gizmos.DrawWireSphere(gizmoPosition, size.magnitude);
                break;
            case Shape.Cube:
                Gizmos.DrawCube(gizmoPosition, size);
                break;
            case Shape.WireCube:
                Gizmos.DrawWireCube(gizmoPosition, size);
                break;
            case Shape.Ray:
                Gizmos.DrawRay(gizmoPosition, size);
                break;
        }
    }
}

```
---
> :paperclip: Done looking? Check out more scripts [here.](../)

[transform]: https://docs.unity3d.com/ScriptReference/Transform.html
