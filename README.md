# Unity Scripts

> :construction: This repositiory is under construction and will be updated in the future. :construction:

This repository is for my own unity scripts. You can find all kinds of scripts here, all designed to be as modular as possible. Each folder will contain a `📝README.md` file containing the requirements, documentation, and source code, along with a `📄.unitypackage` file for you to import into the Unity editor.

**:warning: Unless stated otherwise, all scripts were built in Unity version `2022.3.16f1` :warning:**

## Scripts

There are two kinds of scripts in this repository: **Independant**, and **Dependant**.

**Independant** scripts are located in [`📁Scripts/`](./Scripts/), and are singular files that will work on their own without additional assets. Any components that must be attached to the same object as the script will be noted in `📝README.md`, under *Requirements*.

**Dependant** scripts are located in [`📁dScripts/`](./dScripts/). These are scripts that contain or require additional assets. Assets from the :link:[Unity Asset Store](https://assetstore.unity.com) ***will not*** be in the `📄.unitypackage` file, and instead they will be linked to in the `📝README.md` under *Requirements* for you to download yourself. Most assets used will be free.

> :paperclip: Check out the latest script [here](/dScripts/Rigidbody-Magnet/), hot off the press!