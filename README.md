# Unity Scripts

> :construction: This repositiory is under construction and will be updated in the future. :construction:

This repository is for my own unity scripts. You can find all kinds of scripts here, all designed to be as modular as possible. Each folder will contain a `📝README.md` file containing the requirements, documentation, and source code, along with a `📄.unitypackage` file for you to import into the Unity editor.

## Scripts

There are two kinds of scripts in this repository: **Independant**, and **Dependant**.

**Independant** scripts are located in [`📁Scripts/`](./Scripts/), and are singular files that will work on their own without additional assets. Any components that must be attached to the same object as the script will be noted in `📝README.md`, under *Requirements*.

**Dependant** scripts are located in [`📁dScripts/`](./dScripts/). These are scripts that contain or require additional assets. Assets from the :link:[Unity Asset Store](https://assetstore.unity.com) ***will not*** be in the `📄.unitypackage` file, and instead they will be linked to in the `📝README.md` under *Requirements* for you to download yourself. Most assets used will be free.

> :paperclip: Check out my latest script [here](/Scripts/Prefab-Pool/), hot off the press!

## 📃 Format legend

My repositories uses emojis in many places! If you are confused about the usage of these emojis, this section will outline the formatting rules I use for most of my repositories.

### Emojis
- "📁" is used for folders.
- "📄" is used for all files, except for markdown files which use 📝.
- "⚠️" is used for important information, such as Unity version.
- "📎" is for small notes and asides.
- "🔗" is used alongside [links]() to external sites, and :octocat: for repositories.

### Format
- `Inline code` is used when referring to files, folders, and code.
- When a singular folder or file within the repository is referenced, a [link]() to it will be provided.
- > Blockquotes are used for subtext, notes, asides, etc.
