# ⚙️Add Gizmo⚙️

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/03/15*|*2024/05/03*|

- [⚙️Add Gizmo⚙️](#️add-gizmo️)
  - [🛠️ Requirements](#️-requirements)
- [Documentation](#documentation)
  - [📖Description](#description)
  - [✒️Signatures](#️signatures)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./addgizmo.unitypackage) into the assets folder, or create a new C# script and paste in the [source code](#-source-code) below.

## 🛠️ Requirements

This script makes use of the following components:
- :link:[`Transform`][transform]

> :warning: This script was written in Unity version `2022.3.20f1`

---
# Documentation

## 📖Description
Easily add a gizmo to your game object. You can choose between a **Sphere**, **Cube**, **WireSphere**, **WireCube**, or **Ray** in the dropdown menu. You can set it's **position**, **size**, and **color**, as well as if the gizmo will only draw when **the object is selected**. Gizmos will only be drawn if the script is **enabled**.

> :warning: Notice:
> 
>  Using `Space.Self` may cause the following exception to be thrown: 
> `Quaternion To Matrix conversion failed because input Quaternion is invalid`
> 
> This exception is likely due to a rounding error outside of my control, and **does not seem** to significantly impact the script's functionality.

## ✒️Signatures
| Datatype | Name | Summary |
|-|-|-|
| `bool` | `onSelected` | If true, the gizmo will only be drawn when the object is selected. |
| `enum` | `gizmo` | The kind of gizmo that will be drawn. |
| `Color` | `color` | The gizmo's color, white by default. |
| `enum` | `space` | What space to use when drawing the gizmo. Using `Space.World` means the gizmo's size is absolute and position will not be affected by rotation. For `Space.Self`, the size and position of the gizmo will be relative to the object's `Transform`.
| `Vector3` | `size` | The size of the gizmo. The `Sphere` and `WireSphere` gizmos will use the magnitude for their radius. |
| `Vector3` | `position` | The relative position of the gizmo. |
## ⚙️ Gizmos

- Either a Sphere, WireSphere, Cube, WireCube, or Ray with a color, size, and position of your choosing.

## 💾 Source Code
``` cs
using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Add-Gizmo")]
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

    public Shape gizmo;
    public Color color = Color.red;
    public bool onSelected;
    [Space]
    public Space space = Space.World;
    public Vector3 size = Vector3.right;
    public Vector3 position;

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
        Vector3 relativePosition = transform.rotation * new Vector3(x: transform.localScale.x * position.x,
                                                                    y: transform.localScale.y * position.y,
                                                                    z: transform.localScale.z * position.z);
        Vector3 relativeSize = new(x: transform.localScale.x * size.x,
                                   y: transform.localScale.y * size.y,
                                   z: transform.localScale.z * size.z);

        Vector3 gizmoPosition = space == Space.Self
            ? transform.position + relativePosition
            : transform.position + position;
        Vector3 gizmoSize = space == Space.Self
            ? relativeSize
            : size;
        Gizmos.color = color;
        switch (gizmo)
        {
            case Shape.Sphere:
                Gizmos.DrawSphere(gizmoPosition, gizmoSize.magnitude);
                break;
            case Shape.WireSphere:
                Gizmos.DrawWireSphere(gizmoPosition, gizmoSize.magnitude);
                break;
            case Shape.Cube:
                Gizmos.DrawCube(gizmoPosition, gizmoSize);
                break;
            case Shape.WireCube:
                Gizmos.DrawWireCube(gizmoPosition, gizmoSize);
                break;
            case Shape.Ray:
                gizmoSize = space == Space.Self ? transform.rotation * gizmoSize : gizmoSize;
                Gizmos.DrawRay(gizmoPosition, gizmoSize);
                break;
        }
    }
}

```
---
> :paperclip: Done looking? Check out more scripts [here.](../)

[transform]: https://docs.unity3d.com/ScriptReference/Transform.html
