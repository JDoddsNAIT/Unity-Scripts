> *This script is not yet available.*
# 🧲 Rigibody Magnet 🧲

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/02/24*|*----/--/--*|

- [🧲 Rigibody Magnet 🧲](#-rigibody-magnet-)
  - [🛠️ Requirements](#️-requirements)
  - [📖Description](#description)
- [Documentation](#documentation)
  - [✒️Signatures](#️signatures)
  - [⚙️ Gizmos](#️-gizmos)
  - [💾 Source Code](#-source-code)

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./) into the assets folder, or create a new C# script and paste in the [source code](#source-code) below.

## 🛠️ Requirements

This script makes use of the following components:
  - [`Rigidbody`][rigidbody]

## 📖Description

**🧲 Rigidbody Magnet** is the **one simple trick** that makes you 🥵irresistible! Just supply a list of tags, set a distance and force, and all those ❤️‍🔥hot❤️‍🔥 rigidbodies will come 🪽flying at you **instantly**! They'll be so in 💞love, they wont ***ever*** want to let go! And if you call *right now*, you'll get an additional ***TWO*** of these **🧲 Rigidbody Magnets** absolutely **💸FREE!💸** Just pay shipping and processing! But you gotta call now! 

> :paperclip: *In participating regions for a limited time. Conditions may apply. Always remember to read and follow the label. Batteries not included.*



---
# Documentation

## ✒️Signatures
| Datatype | Name | Summary |
|-|-|-|
| `List<string>` | `affectedTags` | Objects that have a rigidbody component and have a tag within this list will be affected by the `forceOfAttraction`. |
| `float` | `forceOfAttraction` | The amount of force applied to each affected object. |
| `float` | `range` | The radius in which affected objects will have the `forceOfAttraction` applied. |
## ⚙️ Gizmos

- A red wire sphere that turns green when an object is within range.
- A green ray on each affected object that appears visualizing the force vetor being applied.

## 💾 Source Code
``` cs

```
---
> :paperclip: Done looking? Check out more scripts [here.](../)



[rigidbody]: https://docs.unity3d.com/ScriptReference/Rigidbody.html
