# 🎱 Prefab Pool 🎱

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/05/03*|*2024/05/03*|

- [🎱 Prefab Pool 🎱](#-prefab-pool-)
  - [🛠️ Requirements](#️-requirements)
- [Documentation](#documentation)
  - [📖Description](#description)
  - [✒️Signatures](#️signatures)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./) into the assets folder, or create a new C# script and paste in the [source code](#-source-code) below.

## 🛠️ Requirements

This script makes use of the following components:
- This script uses no other components.

> :warning: This script was written in Unity version `2022.3.20f1`

---
# Documentation

## 📖Description
This script allows you to implement object pooling. Object pooling is a technique used to save memory, as it allocates memory for an amount of objects to be accessed at any time, as opposed to allocating new memory for every object that is created.

## ✒️Signatures
| Datatype | Name | Summary |
|-|-|-|
| `GameObject` | `Prefab` | The prefab that the script will create a pool of. |
| `int` | `PoolSize` | The amount of objects to allocate memory for. |
| `Transform` | `PrefabParent` | The parent of all objects in the pool. |
| `GameObject[]` | `Pool` | Returns the entire pool of objects. |
| `GameObject[]` | `ActivePool` | Returns an array of all objects in `Pool` that are active in the hierarchy. |
| `bool` | `IsActive` | Returns true if any objects in the `Pool` are active in the hierarchy. |
| `bool` | `IsEmpty` | Returns true if all objects in the `Pool` are active in the hierarchy. |
| `GameObject` | `Next` | Returns and activates the next inactive object in the `Pool`. Logs a warning if no object is found. |

## ⚙️ Gizmos

This script uses no gizmos.

## 💾 Source Code
``` cs
using System.Linq;
using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Prefab-Pool")]
public class PrefabPool : MonoBehaviour
{
    [Tooltip("The prefab that the script will create a pool of.")]
    public GameObject prefab;
    [Tooltip("The amount of objects to allocate memory for.")]
    [Min(1)] public int poolSize;
    [Tooltip("The parent of all objects in the pool.")]
    public Transform prefabParent;

    // Properties
    /// <summary>
    /// Returns the entire pool of objects.
    /// </summary>
    public GameObject[] Pool { get; private set; }

    /// <summary>
    /// Returns an array of all objects in the <see cref="PrefabPool"/> that are active in the hierarchy.
    /// </summary>
    public GameObject[] ActivePool => Pool.Where(prefab => prefab.activeInHierarchy).ToArray();

    /// <summary>
    /// Returns true if any objects in the <see cref="PrefabPool"/> are active in the hierarchy.
    /// </summary>
    public bool IsActive => ActivePool.Length > 0;

    /// <summary>
    /// Returns true if all objects in the <see cref="PrefabPool"/> are active in the hierarchy.
    /// </summary>
    public bool IsEmpty => ActivePool.Length >= poolSize;

    private void Awake()
    {
        if (prefab == null)
        {
            Debug.LogError($"No value was assigned for member {nameof(prefab)} on {this.gameObject.name}.");
        }
        else
        {
            Pool = new GameObject[poolSize];
            for (int c = 0; c < poolSize; c++)
            {
                Pool[c] = Instantiate(prefab, prefabParent);
                Pool[c].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Returns the next inactive object in <see cref="PrefabPool"/>. Logs a warning if no object is found.
    /// </summary>
    public GameObject Next
    {
        get
        {
            GameObject returnObject = Pool.Where(prefab => !prefab.activeInHierarchy).FirstOrDefault();
            if (returnObject == null)
            {
                Debug.LogWarning($"{prefab.name} pool is empty, null was returned.");
            }
            else
            {
                returnObject.SetActive(true);
            }
            return returnObject;
        }
    }
}

```
---
> :paperclip: Done looking? Check out more scripts [here.](../)
