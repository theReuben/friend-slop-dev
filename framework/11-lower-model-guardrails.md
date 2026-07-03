# 11 — Operating guardrails for the models running this factory

This factory will be run by capable-but-not-frontier models. This doc encodes
the failure patterns those models predictably hit in Unity/netcode/asset work,
and the rules that prevent them. Every agent should treat this as binding.

## Rule 1 — Adapt `reference/`, don't invent

The hard systems (character motor, grabbing, ragdoll, voice, impact audio,
run state) have canonical implementations in `reference/unity/` with the
design reasoning in comments. Writing these from scratch instead of adapting
them is the single most expensive mistake available to you. Same for tuning:
start from `reference/unity/TUNING_DEFAULTS.md`, never from a guess.

## Rule 2 — Never write an external API call from memory

Facepunch.Steamworks, NGO, and URP APIs drift between versions, and models
confidently hallucinate plausible-looking methods. The source of truth is in
the project: `Library/PackageCache/<package>/` (full source for Unity
packages) and the Facepunch DLL's XML docs. Protocol:

- Before using an unfamiliar API: find one real usage in the package source
  or the package's `Documentation~/` folder.
- On a compile error: READ the actual signature at the error site. Do not try
  a second guessed overload; do not invent a helper that "should exist".
- Never upgrade/downgrade a package to make an API match your memory. The
  version pin in the production log wins; your training data loses.

## Rule 3 — One change per iteration on anything feel-related

Feel bugs ("jump is floaty", "grabbing feels mushy") are tuned, not fixed.
Change exactly one config value, have a human play it, log value + verdict in
the production log's tuning table, repeat. Batch-changing five values tells
you nothing and destroys previously-good feel. Code changes to feel-critical
systems after gate sign-off require a producer note — `// PROTECTED JANK:`
comments and snapshotted config values are load-bearing; do not "clean up"
either.

## Rule 4 — Reproduce before fixing; verify with artifacts after

- No fix without first reproducing the bug (or stating explicitly that you
  couldn't and the fix is speculative — then it's the human's call to accept).
- "Should work now" is a banned phrase. A claim of done needs an artifact:
  test output, a log line, a screenshot, a profiler number.
- The smoke test and static sweep (`reference/unity/Tests/`, `Editor/`) exist
  so you can verify without a human. Run them before claiming anything works.
- Editing C# with no Unity available (e.g. in this repo's `reference/`)?
  `reference/unity/syntax_check.py` is the compile-adjacent check — run it
  after every .cs edit. Editing the Blender scripts? `reference/blender/selftest.py`.

## Rule 5 — The stuck protocol

After **3 failed attempts** at the same error/task: STOP. Write to the
production log what you tried, the exact error text, and your best hypothesis.
Then either escalate to the user or move to a different task. Grinding attempt
#7 on a hallucinated API burns the time-box and pollutes the codebase with
half-fixes. Being stuck loudly is professional; being stuck silently is how
projects die.

## Rule 6 — Know what needs a human, and queue it up cleanly

You cannot: judge feel or funniness, watch a playtest, press Steamworks
buttons, record voice takes, or verify visuals without a screenshot. When work
reaches one of these, prepare the human's session to be maximally cheap — a
numbered list of exactly what to do/play/click and what to report back. Batch
human asks; don't drip them.

## Rule 7 — Scope traps, by name

Refuse (and cite this doc) any drift toward: client-side prediction, host
migration, mid-run join, >4 players, AI-pathfinding enemies, active-ragdoll
animation blending, meta progression, procedural EVERYTHING (procedural
variation of authored chunks is fine), custom shaders beyond the palette/toon
setup, editor tooling beyond one day's payoff, package upgrades mid-project.

## Known gotchas (hard-won; check here before debugging)

### Unity physics
- Joint chains stretch/explode → raise solver iterations (8/2), check mass
  ratios ≤ 10:1 per link, never scale transforms of jointed bodies.
- Jitter on moving platforms/held props → someone's missing interpolation, or
  is moving a Rigidbody via `transform` (forbidden, framework/03).
- `OnCollisionEnter` not firing → both bodies asleep, or collision layers, or
  the Rigidbody is kinematic without `detectCollisions`.
- Character slides down slopes at rest → hover spring force fighting a
  too-low-friction PhysicMaterial on feet zone; add a grounded drag term, not
  a constraint.

### Netcode for GameObjects
- `NetworkVariable` writes silently ignored → written by a non-server; wrap
  every mutation in `if (!IsServer) return;`.
- RPC method names MUST end in `ServerRpc`/`ClientRpc` — the codegen enforces
  it and errors are cryptic.
- Values read as default/null on clients → read before spawn; subscribe in
  `OnNetworkSpawn`, never `Awake`, and use `OnValueChanged` not polling.
- Parenting a NetworkObject under a non-NetworkObject fails — use joints
  anyway (framework rule) or `NetworkObject.TrySetParent`.
- Scene objects with NetworkObject need matching scene load mode; prefer
  spawning gameplay-relevant objects from the host.

### Facepunch.Steamworks
- Nothing works at all → `SteamClient.Init` not called, or Steam not running,
  or `steam_appid.txt` missing next to the editor executable in dev.
- Callbacks never fire → `SteamClient.RunCallbacks()` must be pumped every
  frame (Boot scene manager's Update).
- AppId 480 lobbies are a PUBLIC playground: strangers can appear in dev
  lobbies. Filter lobbies on a unique metadata key for this game.
- Two clients on one machine won't work with one Steam account — real
  transport tests need two accounts/machines; use the local transport toggle
  for solo iteration.

### URP / rendering
- Post volume does nothing → camera's "Post Processing" checkbox off, or
  volume layer mask mismatch.
- Everything magenta → material/shader not URP-compatible; run the render
  pipeline converter on imported materials (or better: assets shouldn't ship
  materials at all — palette pipeline strips them).
- Lighting looks wrong after moving static geometry → stale bake; re-bake or
  mark objects non-static during iteration and bake at gates.

### Blender → Unity
- No Blender installed / cloud session → `pip install bpy` (standalone wheel,
  Python version must match the wheel's) gives the full headless pipeline.
  ALWAYS run `reference/blender/selftest.py` after any Blender/bpy version
  change — it proves the pipeline or names what drifted.
- Model imports 100× too big/small → FBX unit scale; export with
  `apply_scale_options='FBX_SCALE_ALL'` (the unify script does).
- Model rotated −89.98° → axis mismatch; export `axis_forward='-Z',
  axis_up='Y'`, `bake_space_transform=True` (ditto).
- Flat-shaded model looks faceted-wrong in Unity → normals: match the
  shading choice in the export to the style bible's recipe, don't let Unity
  recalculate.

## Session hygiene (every work session, no exceptions)

1. Read the game's `PRODUCTION_LOG.md` first. 2. Do gate-relevant work only.
3. Verify with artifacts. 4. Update the log (including the tuning table and
Bugs). 5. Commit with `game(<name>): <what>` messages. 6. If you learned a
new gotcha, add it to THIS file in the same session — that's how the factory
gets smarter as models get cheaper.
