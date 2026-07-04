# reference/ — canonical implementations of the hard parts

Written by a frontier model so weaker models don't have to invent these from
scratch. **Adapt, don't copy blindly**: rename the `Friendslop` namespace to
the game's, wire the serialized fields, and re-verify third-party API calls.

## Rules for using this folder

1. **Start here.** Before writing a character controller, grab system,
   ragdoll, voice chat, or impact audio from scratch, adapt the file in
   `unity/`. These encode design decisions (why joints not parenting, why no
   prediction) that are easy to get wrong — the WHY comments are load-bearing.
2. **Verify external APIs against the installed package.** Facepunch.Steamworks
   and Netcode for GameObjects evolve; the calls here were correct as of
   mid-2026 but the source of truth is `Library/PackageCache/<package>/` and
   the shipped XML docs in the project. If a call doesn't compile, read the
   real signature there — do not guess an alternative from memory.
3. **Verification status differs by folder.** `blender/` is PROVEN — executed
   end-to-end headless (see its README and `selftest.py`). `unity/*.cs` is
   SYNTAX-VERIFIED (every file parses clean — `unity/syntax_check.py`, re-run
   it after ANY .cs edit here) but never compiled against Unity: expect to fix
   small semantic things (a using, a renamed enum) on first import; that is
   normal and fine. What should NOT change is the architecture.
4. Start values for every tunable live in `unity/TUNING_DEFAULTS.md` — put
   them in the `*Config` ScriptableObjects, then tune one value at a time
   (log in the production log's tuning table).

## Contents

| File | What it is |
|---|---|
| `unity/PlayerIntent.cs` | The input→intent struct; the netcode boundary from day one |
| `unity/MotorConfig.cs` + `unity/HoverCapsuleMotor.cs` | The physics character (hover spring + upright torque = recoverable clumsiness) |
| `unity/GrabSystem.cs` | ConfigurableJoint grabbing/carrying with break forces |
| `unity/RagdollBlender.cs` | Impact → ragdoll → comedic recovery |
| `unity/SquashStretch.cs` | Scale-punch charm on impacts/jumps |
| `unity/ImpactAudioSystem.cs` + `ImpactAudioConfig.cs` + `ImpactAudioEmitter.cs` | All collision audio through one tunable system (one class per file — Unity requires MonoBehaviour/ScriptableObject names to match their file) |
| `unity/FallSilence.cs` | The signature fall-silence beat |
| `unity/SteamVoiceChat.cs` | Proximity voice over Facepunch + NGO (the fiddliest file — read its header) |
| `unity/VoiceOcclusion.cs` | Raycast low-pass on voice |
| `unity/RunManager.cs` | Host-authoritative run state machine skeleton |
| `unity/Editor/StaticSweep.cs` | Missing-script / missing-reference scanner (QA static sweep) |
| `unity/SeededDeck.cs` | Deterministic deck for escalation systems — the "pure logic in plain classes" shape |
| `unity/Tests/EditMode/SeededDeckTests.cs` | The level-1 test pattern: pure logic, determinism invariants |
| `unity/Tests/PlayMode/GrabBreakTest.cs` | The level-2 pattern: programmatic physics-invariant test / jank-guard shape |
| `unity/Tests/PlayMode/SmokeTest.cs` | The level-3 smoke test skeleton |
| `unity/GradientSkybox.shader` | Palette gradient sky (kills slop tell #1) |
| `unity/syntax_check.py` | tree-sitter C# parse check — the compile-adjacent verification available without Unity |
| `unity/TUNING_DEFAULTS.md` | Starting values for every feel tunable |
| `unity-project/` | New-project scaffold kit: setup procedure, .gitignore, .editorconfig, asmdefs, folder-tree generator |
| `blender/` | Headless bpy pipeline: palette atlas + asset unification pass — **verified end-to-end on Blender 5.0** (`selftest.py` re-proves it on any version; `pip install bpy` runs it anywhere, no Blender install) |

## Where files go in the Unity project

`unity/*.cs` → `Assets/_Project/Code/Runtime/` (Editor/ and Tests/ subfolders
to their own asmdefs per `framework/03-unity-conventions.md`). `blender/*.py`
→ the Unity project's `Tools/` folder, run headless from the shell.
