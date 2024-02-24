# 🧲 Rigidbody Magnet 🧲

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/02/24*|*2024/02/24*|

- [🧲 Rigidbody Magnet 🧲](#-rigidbody-magnet-)
  - [🛠️ Requirements](#️-requirements)
  - [📖Description](#description)
- [Documentation](#documentation)
  - [✒️Signatures](#️signatures)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./rigidbodyMagnet.unitypackage) into the assets folder, or create a new C# script and paste in the [source code](#source-code) below.

## 🛠️ Requirements

This script makes use of the following components:
  - [`Rigidbody`][rigidbody]

## 📖Description

**🧲 Rigidbody Magnet** is the **one simple trick** that makes GameObjects 🥵irresistible! Just supply a list of tags, set a distance and force, and all those ❤️‍🔥hot❤️‍🔥 rigidbodies will come 🪽flying at you **instantly**! They'll be so in 💞love, they wont ***ever*** want to let go! And if you call *right now*, you'll get an additional ***TWO*** of these **🧲 Rigidbody Magnets** absolutely **💸FREE!💸** Just pay shipping and processing! But you gotta 📞call now! 

> :paperclip: *In participating regions for a limited time. Conditions may apply. Always remember to read and follow the label. Batteries not included.*

---
# Documentation

## ✒️Signatures
| Datatype | Name | Summary |
|-|-|-|
| `List<string>` | `affectedTags` | Objects that have a rigidbody component and have a tag within this list will be affected by the `forceOfAttraction`. Objects with one of these tags will be given a Rigidbody component automatically. |
| `float` | `forceOfAttraction` | The amount of force applied to each affected object. |
| `float` | `range` | The radius in which affected objects will have the `forceOfAttraction` applied. |

## ⚙️ Gizmos

- A red wire sphere that turns green when an object is within range.
- A green ray on each affected object visualizing the force vetor being applied.

## 💾 Source Code
``` cs
// View documentation at https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Rigidbody-Magnet
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMagnet : MonoBehaviour
{
    public List<string> affectedTags;
    public float forceOfAttraction;
    public float range;

    private Rigidbody myRigidbody;

    void Update()
    {
        List<Rigidbody> affectedBodies = FindBodiesWithTags(affectedTags, out myRigidbody);

        foreach (Rigidbody body in affectedBodies)
        {
            Vector3 bodyDistanceFromThis = myRigidbody.position - body.position;
            Vector3 forceDirection = bodyDistanceFromThis.normalized;
            body.AddForce(forceDirection * forceOfAttraction);
        }
    }

    private void OnDrawGizmos()
    {
        if (enabled)
        {
            List<Rigidbody> affectedBodies = FindBodiesWithTags(affectedTags, out myRigidbody);
            if (affectedBodies.Count > 0)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireSphere(GetComponent<Rigidbody>().position, range);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (enabled)
        {
            Gizmos.color = Color.green;

            List<Rigidbody> affectedBodies = FindBodiesWithTags(affectedTags, out myRigidbody);

            foreach (Rigidbody body in affectedBodies)
            {
                Vector3 bodyDistanceFromThis = myRigidbody.position - body.position;
                Vector3 forceDirection = bodyDistanceFromThis.normalized;
                Gizmos.DrawRay(body.position, forceDirection * forceOfAttraction);
            }
        }
    }

    private List<Rigidbody> FindBodiesWithTags(List<string> tags, out Rigidbody thisRigidbody)
    {
        thisRigidbody = GetComponent<Rigidbody>();

        List<GameObject> targets = new List<GameObject>();
        foreach (string tag in tags)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            targets.AddRange(gameObjects);
            targets.Remove(this.gameObject);
        }

        List<Rigidbody> targetBodies = new List<Rigidbody>();
        foreach (GameObject gameObject in targets)
        {
            if (gameObject.TryGetComponent(out Rigidbody testBody) == false)
            {
                Debug.Log($"Added Rigidbody component to GameObject \"{gameObject.name}\".");
                testBody = gameObject.AddComponent<Rigidbody>();
            }

            if (Vector3.Distance(thisRigidbody.position, testBody.position) <= range)
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
