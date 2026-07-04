# 03 — Unity conventions

Fixed choices so every game starts the same way and knowledge transfers between
games. Deviate only with a producer-approved note in the production log.

## Versions & packages

- **Unity 6 LTS (6000.x)**, latest patch at project start. Pin it in the log;
  never upgrade mid-project.
- **URP** (not HDRP, not built-in). Linear color space.
- **New Input System** with a shared `InputActions` asset; Steam Input handled at
  ship time (`09-steam-shipping.md`).
- Packages: Netcode for GameObjects, Unity Transport, ProBuilder (greybox),
  Cinemachine, TextMeshPro, Input System. Facepunch.Steamworks via NuGet DLL
  drop-in (`04-netcode.md`).
- **No Asset Store purchases. No paid plugins.** Free Asset Store items only if
  license-checked by asset-scout like any other asset.
- Unity **Personal** license (fine under the revenue threshold). Disable Unity
  Analytics/Ads — no telemetry, no accounts.

## Project layout

```
Assets/
  _Project/            # ALL our stuff lives under one folder
    Art/               #   Models/, Materials/, Textures/, Animations/ (per source-pack subfolders forbidden — organize by game object)
    Audio/             #   SFX/, Music/, Ambience/, Mixer
    Code/              #   Runtime/ (asmdef), Editor/ (asmdef)
    Levels/            #   Scenes + scene-specific data
    Prefabs/
    Settings/          #   URP assets, volumes, input actions
    UI/
  ThirdParty/          # imported packs AS IMPORTED, one folder per source, LICENSE.txt in each
Docs/CREDITS.md        # mirrored to games/<name>/CREDITS.md in this repo
```

Rule: nothing references `ThirdParty/` directly from scenes — assets get
processed (Blender pass, material swap to our palette) into `_Project/Art/`.
This is what keeps the game from looking kitbashed.

## Code standards (for Sonnet-class authors: boring and explicit beats clever)

- C#, one class per file, `namespace <GameName>.<Area>`.
- No third-party frameworks (no Zenject/UniRx/Odin). Plain MonoBehaviours,
  ScriptableObjects for tuning data, C# events or a single static `GameEvents`
  hub — nothing fancier.
- Every tunable number is a serialized field on a ScriptableObject
  (`*Config.asset`), never a magic number. Designers tune without code changes.
- `FixedUpdate` for physics, `Update` for input/camera/UI. Never move a
  Rigidbody from `Update`. Never use `transform.position` on a physics object —
  forces/velocity/MovePosition only.
- Null-safe scene wiring: `[SerializeField]` + validate in `OnValidate`/`Awake`
  with a clear error log naming the missing reference.
- Logging: `Debug.Log` gated behind a `DEV_BUILD` define; zero per-frame logs.

## Physics feel (the actual product — tune, then lock)

- `Fixed Timestep` 0.02 (50 Hz). Interpolate on every visible Rigidbody.
- Character: **dynamic Rigidbody capsule** (not CharacterController) so players
  are pushable, stackable, and grabbable. Move via forces with a ground-stick
  spring ("hover capsule"); torque-spring toward upright so characters wobble
  instead of snapping — recoverable clumsiness is the aesthetic.
- Grabbing/carrying: `ConfigurableJoint` with breakable force, never parenting.
  Held objects keep full collision (comedy requires the ladder to hit your
  friend's head).
- Ragdoll: pre-built on the character prefab, blend in on impacts over a force
  threshold; recover to standing after a beat. Death = full ragdoll + camera
  lingers 2s (that's the clip).
- Mass discipline: define a mass chart in the design doc (player 80, prop small
  1–5, prop large 20–60). Physics comedy dies when masses are arbitrary.
- PhysicMaterials: low friction on player capsule sides, moderate on feet zone,
  bouncy only where funny.

## Performance budget (GTX 1060 / 1080p / 60 fps)

- ≤ 150 active non-kinematic Rigidbodies; pool and sleep aggressively.
- Baked or mixed lighting for static world; ≤ 4 realtime shadowed lights visible.
- Textures ≤ 1K for props, 2K for hero/terrain. Meshes: stylized-low-poly range
  (props ≤ 5k tris, characters ≤ 20k).
- Netcode: ≤ 30 synced NetworkObjects active; everything else is deterministic
  or host-spawned-on-event (`04-netcode.md`).
- Profile at every gate on a constrained machine or with GPU frame budget math;
  don't trust editor FPS.

## Scenes & flow

- `Boot` (Steam init, config) → `MainMenu` (lobby UI) → `Game` scene(s).
- All managers are plain scene objects in `Boot`, marked DontDestroyOnLoad, no
  singletons-with-lazy-instantiation (explicit boot order instead).

## Testing

Full doctrine (levels, patterns, exact run commands): `12-testing.md`.
Summary: EditMode for pure logic, PlayMode feature tests for physics
invariants + jank guards, the 4-player smoke test before every gate, manual
matrix in `08-qa-playtesting.md`. Keep logic in plain classes (see
`reference/unity/SeededDeck.cs`) so level-1 tests can reach it.
