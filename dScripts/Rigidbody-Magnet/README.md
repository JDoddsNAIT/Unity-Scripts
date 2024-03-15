# 🧲 **Rigidbody Magnet** 🧲

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/02/24*|*2024/03/01*|

- [🧲 **Rigidbody Magnet** 🧲](#-rigidbody-magnet-)
  - [🛠️ Requirements](#️-requirements)
  - [📖Description](#description)
- [Rigidbody Magnet (3D)](#rigidbody-magnet-3d)
  - [📖Description](#description-1)
  - [✒️Signatures](#️signatures)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)
- [Rigidbody Magnet (2D)](#rigidbody-magnet-2d)
  - [📖Description](#description-2)
  - [✒️Signatures](#️signatures-1)
  - [⚙️ Gizmos](#️-gizmos-1)
  - [💾 Source Code](#-source-code-1)

> :paperclip: To add these scripts to your Unity project, simply import the [Unity Package](./rigidbodyMagnet.unitypackage) into the assets folder, or create **2** new C# scripts and paste in the [source code](#-source-code) below.

## 🛠️ Requirements
> :warning: This script was written in Unity version `2022.3.16f1`

These scripts make use of the following components:
- [`Transform`](https://docs.unity3d.com/ScriptReference/Transform.html)
- [`Rigidbody`](https://docs.unity3d.com/ScriptReference/Rigidbody.html) - [Rigidbody Magnet (3D)](#rigidbody-magnet-3d)
- [`Rigidbody2D`](https://docs.unity3d.com/ScriptReference/Rigidbody2D.html) - [Rigidbody Magnet (2D)](#rigidbody-magnet-2d)
- [`Collider`](https://docs.unity3d.com/ScriptReference/Collider.html) (Optional) - [Rigidbody Magnet (3D)](#rigidbody-magnet-3d)
- [`Collider2D`](https://docs.unity3d.com/ScriptReference/Collider2D.html) (Optional) - [Rigidbody Magnet (2D)](#rigidbody-magnet-2d)

> :paperclip: All scripts in this package are mutually exclusive and can be used independantly.

## 📖Description

**🧲 Rigidbody Magnet** is the **one simple trick** that makes GameObjects 🥵irresistible! Just supply a list of tags, set a distance and force, and all those ❤️‍🔥hot❤️‍🔥 rigidbodies will come 🪽flying at you **instantly**! They'll be so in 💞love, they wont ***ever*** want to let go! Plus it works in ***both 2D AND 3D!!*** And if you call *right now*, you'll get an additional ***TWO*** of these **🧲 Rigidbody Magnets** absolutely **💸FREE!💸** Just pay shipping and processing! But you gotta 📞call now! 

> :paperclip: *In participating regions for a limited time. Conditions may apply. Always remember to read and follow the label. Batteries not included.*

---
# Rigidbody Magnet (3D)

## 📖Description
Attracts tagged 3D objects within range towards the object this script is attached to, like a magnet.

## ✒️Signatures
| Datatype | Name | Summary |
|-|-|-|
| `List<string>` | `affectedTags` | Objects that have a rigidbody component and have a tag within this list will be affected by the `forceOfAttraction`. Objects with one of these tags will be given a Rigidbody component automatically. |
| `float` | `forceOfAttraction` | The amount of force applied to each affected object. |
| `float` | `range` | The radius in which affected objects will have the `forceOfAttraction` applied. |
| `bool` | `showGizmos` | Whether or not to draw gizmos. Is `true` by default.

## ⚙️ Gizmos

- A red wire sphere that turns green when an object is within range.
- A green ray on each affected object visualizing the force vetor being applied.

## 💾 Source Code
``` cs
// View documentation at https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Rigidbody-Magnet
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMagnet : MonoBehaviour
{
    public List<string> affectedTags;
    public float forceOfAttraction;
    public float range;
    public bool showGizmos = true;

    void FixedUpdate()
    {
        List<Rigidbody> affectedBodies = FindBodiesWithTags(affectedTags);

        foreach (Rigidbody body in affectedBodies)
        {
            Vector3 bodyDistanceFromThis = transform.position - body.position;
            Vector3 forceDirection = bodyDistanceFromThis.normalized;
            body.AddForce(forceDirection * forceOfAttraction);
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            List<Rigidbody> affectedBodies = FindBodiesWithTags(affectedTags);
            
            Gizmos.color = affectedBodies.Count > 0 ?
                Color.green : Color.red;
            
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.green;

            List<Rigidbody> affectedBodies = FindBodiesWithTags(affectedTags);

            foreach (Rigidbody body in affectedBodies)
            {
                Vector3 bodyDistanceFromThis = transform.position - body.position;
                Vector3 forceDirection = bodyDistanceFromThis.normalized;
                Gizmos.DrawRay(body.position, forceDirection * forceOfAttraction);
            }
        }
    }

    private List<Rigidbody> FindBodiesWithTags(List<string> tags)
    {
        List<GameObject> targets = new();
        foreach (string tag in tags)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            targets.AddRange(gameObjects);
            targets.Remove(this.gameObject);
        }

        List<Rigidbody> targetBodies = new();
        foreach (GameObject gameObject in targets)
        {
            if (gameObject.TryGetComponent(out Rigidbody testBody) == false)
            {
                Debug.Log($"Added Rigidbody component to GameObject \"{gameObject.name}\".");
                testBody = gameObject.AddComponent<Rigidbody>();
            }

            if (Vector3.Distance(transform.position, testBody.position) <= range)
            {
                targetBodies.Add(testBody);
            }
        }
        return targetBodies;
    }
}
```

# Rigidbody Magnet (2D)

## 📖Description
Attracts tagged 2D objects within range towards the object this script is attached to, like a magnet.

## ✒️Signatures
| Datatype | Name | Summary |
|-|-|-|
| `List<string>` | `affectedTags` | Objects that have a rigidbody component and have a tag within this list will be affected by the `forceOfAttraction`. Objects with one of these tags will be given a Rigidbody component automatically. |
| `float` | `forceOfAttraction` | The amount of force applied to each affected object. |
| `float` | `range` | The radius in which affected objects will have the `forceOfAttraction` applied. |
| `bool` | `showGizmos` | Whether or not to draw gizmos. Is `true` by default. |

## ⚙️ Gizmos

- A red wire sphere that turns green when an object is within range.
- A green ray on each affected object visualizing the force vetor being applied.

## 💾 Source Code
``` cs
// View documentation at https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/dScripts/Rigidbody-Magnet
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMagnet2D : MonoBehaviour
{
    public List<string> affectedTags;
    public float forceOfAttraction;
    public float range;
    public bool showGizmos = true;

    void FixedUpdate()
    {
        List<Rigidbody2D> affectedBodies = FindBodiesWithTags(affectedTags);

        foreach (Rigidbody2D body in affectedBodies)
        {
            Vector2 bodyDistanceFromThis = (Vector2)transform.position - body.position;
            Vector2 forceDirection = bodyDistanceFromThis.normalized;
            body.AddForce(forceDirection * forceOfAttraction);
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            List<Rigidbody2D> affectedBodies = FindBodiesWithTags(affectedTags);

            Gizmos.color = affectedBodies.Count > 0 ?
                Color.green : Color.red;
            
            Gizmos.DrawWireSphere(GetComponent<Rigidbody2D>().position, range);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.green;

            List<Rigidbody2D> affectedBodies = FindBodiesWithTags(affectedTags);

            foreach (Rigidbody2D body in affectedBodies)
            {
                Vector2 bodyDistanceFromThis = (Vector2)transform.position - body.position;
                Vector2 forceDirection = bodyDistanceFromThis.normalized;
                Gizmos.DrawRay(body.position, forceDirection * forceOfAttraction);
            }
        }
    }

    private List<Rigidbody2D> FindBodiesWithTags(List<string> tags)
    {
        List<GameObject> targets = new();
        foreach (string tag in tags)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            targets.AddRange(gameObjects);
            targets.Remove(this.gameObject);
        }

        List<Rigidbody2D> targetBodies = new();
        foreach (GameObject gameObject in targets)
        {
            if (gameObject.TryGetComponent(out Rigidbody2D testBody) == false)
            {
                Debug.Log($"Added Rigidbody2D component to GameObject \"{gameObject.name}\".");
                testBody = gameObject.AddComponent<Rigidbody2D>();
            }

            if (Vector2.Distance(transform.position, testBody.position) <= range)
            {
                targetBodies.Add(testBody);
            }
        }
        return targetBodies;
    }
}
```
---
> :paperclip: Done looking? Check out more scripts [here.](../)



[rigidbody]: https://docs.unity3d.com/ScriptReference/Rigidbody.html
[rigidbody2d]: https://docs.unity3d.com/ScriptReference/Rigidbody2D.html
