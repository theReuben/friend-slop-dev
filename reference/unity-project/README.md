# unity-project/ — new-project scaffold kit

Turns "set up the Unity project" (Phase 1, day one) into a 20-minute
mechanical procedure. Versions below were verified current as of July 2026
(Unity 6000.4 editor line, NGO 2.13) — pin the exact versions you install in
the game's PRODUCTION_LOG and never change them mid-project.

## Procedure

1. **Install Unity 6 LTS** (latest 6000.x LTS patch) via Unity Hub. License:
   Personal. Modules: Windows Build Support (IL2CPP + Mono).
2. **Create project** from the **Universal 3D** template (URP comes correctly
   configured — never hand-migrate built-in to URP). Name = game codename.
   Location per the production log.
3. **Add packages** (Window > Package Manager > by name):
   - `com.unity.netcode.gameobjects` (2.x — 2.13 verified line)
   - `com.unity.inputsystem`
   - `com.unity.cinemachine` (3.x)
   - `com.unity.probuilder`
   - `com.unity.test-framework` (usually preinstalled)
   Then **Facepunch.Steamworks**: download the release from
   github.com/Facepunch/Facepunch.Steamworks (MIT), drop the Windows 64
   DLLs into `Assets/ThirdParty/Facepunch/` + its LICENSE.txt, and add the
   NGO Facepunch community transport (asset-scout license-checks whichever
   transport repo is current — it's MIT-family historically).
4. **Copy this folder's files in:**
   - `gitignore-unity.txt` → project root as `.gitignore`
   - `editorconfig.txt` → project root as `.editorconfig`
   - `asmdefs/*.asmdef` → into `Assets/_Project/Code/...` per the layout
     (fix any red assembly-name references in the Inspector — names drift
     between package versions; the editor shows you the real ones)
   - `CreateProjectStructure.cs` → `Assets/_Project/Code/Editor/`, then run
     menu **Friendslop > Create Project Structure** to generate the folder
     tree from framework/03.
   - Everything in `reference/unity/*.cs` → `Assets/_Project/Code/Runtime/`
     (Editor/ and Tests/ files to their folders); `reference/blender/*.py`
     → `Tools/`.
5. **Project settings** (framework/03 + TUNING_DEFAULTS):
   - Fixed Timestep 0.02; solver iterations 8/2; gravity −16 (start).
   - Linear color space (URP template default — verify).
   - Layers: create `Player`, `Grabbable`, `VoiceOccluder`; collision matrix
     per design doc.
   - Quality: single quality level, target 60 fps on GTX 1060.
   - Disable: Unity Analytics, Ads, Cloud diagnostics (manifesto rule 3).
6. **Scenes**: create `Boot`, `MainMenu`, `Game` in `_Project/Levels/`; add
   all to Build Settings in that order.
7. **Commit** the pristine project as the first commit of the game repo, and
   record editor version + all package versions in the PRODUCTION_LOG.

## Sanity check (do this before any gameplay work)

Static sweep runs clean (Friendslop > Static Sweep), an empty PlayMode test
passes, and a Windows build compiles. A project that can't build on day one
is a project that debugs its build pipeline at Gate 4 — at the worst time.
