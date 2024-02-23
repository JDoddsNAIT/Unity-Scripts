# Unity Scripts

> :construction: This repositiory is under construction and will be updated in the future. :construction:

This repository is for my own unity scripts. You can find all kinds of scripts here, all designed to be as modular as possible. Each folder will contain a `📝README.md` file containing the requirements, documentation, and source code, along with a `📄.unitypackage` file for you to import into the Unity editor.

**:warning: Unless stated otherwise, all scripts were built on Unity version `2022.3.16f1` :warning:**

## Scripts

There are two kinds of scripts in this repository: **Independant**, and **Dependant**.

**Independant scripts** are located in [`📁Scripts/`](./Scripts/), and are singular files that will work on their own without additional assets. Any components that must be attached to the same object as the script will be noted in `📝README.md`, under *Requirements*.

**Dependant scripts** are located in [`📁dScripts/`](./dScripts/). These are scripts that do require additional assets. The `📝README.md` for these scripts will not contain the source code, so you *must* import the `📄.unitypackage` file in order to use them. If any assets from the :link:[Unity Asset Store](https://assetstore.unity.com) are required, they will be linked to in `📝README.md`, under *Requirements*. Most assets used will be free.
