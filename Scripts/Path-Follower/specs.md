> *This is a template file for referance only.*
# Path Follower

| 📆 Date Added | 📆 Updated On |
|-|-|
|*2024/02/23*|*----/--/--*|

> :paperclip: To add this script to your Unity project, simply import the [Unity Package](./) into the assets folder, or create a new C# script and paste in the [souce code](./specs.md#source-code) below.

## Documentation
> *A short description of what the script does and how the user can manipulate it*
---
| Datatype | Name | Summary |
|-|-|-|
| `List<Vector3> ` | `waypoints ` | The ordered list of positions the object will move to. |
| `float ` | `moveTime ` | The time in seconds taken to move between each position in `waypoints`. |
| `bool ` | `closedLoop ` | Whether or not the object will return to it's starting position at the end of the path. |
| `bool ` | `reverse ` | Whether or not the object will reverse direction |
| `void ` | `StartPath() ` | Initiates the object's movement along the path. |

---
## Source Code
``` cs
// View documentation at https://github.com/JDoddsNAIT/Unity-Scripts/blob/main/Scripts/Path-Follower/specs.md
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    
}
```