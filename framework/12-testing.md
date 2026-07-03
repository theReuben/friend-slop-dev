# 12 — Testing: every level, what belongs there, how to run it

The complete testing stack. `framework/08` defines the QA *process* (severity
policy, funny metric, playtest logistics); THIS doc defines the *levels* and
the *mechanics* — what kind of test lives where, exact run commands, and the
patterns to copy. qa-playtester owns both docs.

## The pyramid (bottom = most runs, top = most human)

| Level | What | Runs | Owner |
|---|---|---|---|
| 0. Static | compiles; static sweep; (this repo: syntax_check.py) | every session | any engineer |
| 1. EditMode | pure logic: state machines, scoring, seeded decks, config sanity | every commit | gameplay-engineer |
| 2. PlayMode feature | physics invariants + jank guards, single feature each | every commit touching that feature | gameplay-engineer |
| 3. PlayMode smoke | boot → host → 4 fake players → 60 s chaos → zero errors | before every gate + after netcode/flow changes | qa-playtester |
| 4. Netcode integration | host + virtual clients in-process, local transport | before Gates 2–5 | netcode-engineer |
| 5. Real-transport session | 2+ real machines, real Steam accounts, full loop + voice | at every gate | netcode-engineer + human |
| 6. Manual matrix | the table in framework/08 (devices, displays, disconnects, abuse) | Gate 4 full, Gates 2–3 abbreviated | qa-playtester + human |
| 7. Feel & funny | funny metric, feel checklist, clip test | every playtest | humans (agents analyze) |
| 8. Performance | frame budget on target-class hardware | every gate | tech-artist |
| 9. Build verification | ship-check section A/B on the actual RC build | Gate 4/5 | steam-publisher |

Rule of thumb for WHERE a new test belongs: no Unity objects → level 1;
Unity physics/objects but one feature → level 2; whole-game wiring → level 3;
requires another machine or a human sense (fun, feel, fatigue) → 5+.

## Level 0 — static

- Project compiles. This gates everything else (see framework/11: compile
  errors disable tests AND editor menus — check compile state FIRST).
- `Friendslop > Static Sweep` (reference/unity/Editor/StaticSweep.cs):
  missing scripts, broken serialized refs, across all build scenes + prefabs.
- In THIS repo (no Unity): `python reference/unity/syntax_check.py` after any
  .cs edit; `python reference/blender/selftest.py` after any pipeline edit.

## Level 1 — EditMode (pattern: reference/unity/Tests/EditMode/SeededDeckTests.cs)

- Test PURE logic only. If a test needs a GameObject, either extract the
  logic into a plain class (preferred — see SeededDeck.cs for the shape) or
  it's a level-2 test.
- What earns a test here: run/phase state transitions, scoring math, seeded
  randomness determinism (same seed = same sequence — netcode depends on it),
  config asset sanity (all bands ordered, no empty clip arrays).
- Keep the suite < 5 s total. There is no excuse not to run it every commit.

## Level 2 — PlayMode feature tests (pattern: reference/unity/Tests/PlayMode/GrabBreakTest.cs)

- Build the scenario PROGRAMMATICALLY (spawn primitives, add components) —
  no test scenes to maintain, nothing breaks when levels change.
- Physics assertions need tolerance and time: act, then
  `yield return new WaitForFixedUpdate()` several ticks, then assert ranges,
  never exact floats.
- **Jank guards live here.** Every `// PROTECTED JANK` in code gets a level-2
  test proving the exploit still works (the worked example's
  RopeWeightTransferTest is this pattern). A refactor that kills the comedy
  must fail CI, not a playtest three weeks later.
- What earns a test: joint breaks at configured force, ragdoll triggers above
  threshold and recovers, motor stays upright under N shoves, held objects
  keep collision, restart completes < 5 s.

## Level 3 — PlayMode smoke (reference/unity/Tests/PlayMode/SmokeTest.cs)

Already specified — the ISmokeDriver hook is the only per-game work. Keep the
random seed FIXED (failures must reproduce). When smoke catches something,
write the level-1/2 test that would have caught it cheaper, then fix.

## Level 4 — netcode integration, no Steam

- Use the DEV_BUILD local/UTP transport: StartHost + N virtual clients in
  one process (NGO's testing utilities, or simply multiple NetworkManager
  connections via Multiplayer Play Mode for semi-manual runs).
- Assert: intents flow client→host, NetworkVariables replicate, disconnect
  mid-action leaves the run consistent (the framework/04 failure matrix,
  automated where possible).
- Add Unity Transport's simulator (100 ms / 5% loss) to at least one test —
  replication asserts must hold under it.

## Level 5–7 — the human levels

Defined in framework/08 (matrix, funny metric, feel checklist). Agents
PREPARE these (build, plan, forms, fixed scenarios) and ANALYZE the results
(recordings → timestamps → reports per games/_worked-example/REPORTS.md);
humans execute. Batch them per framework/11 rule 6.

## Level 8 — performance

- Editor FPS is a lie; measure in a build. Procedure: development build +
  `Friendslop > Perf Capture` if built, else Unity Profiler attached to the
  build for 60 s of 4-player chaos; record avg/95th frame ms in the
  production log at every gate.
- Budgets from framework/03 (60 fps @ 1080p on GTX 1060-class): fail the gate
  if 95th percentile frame time > 16.6 ms on the reference machine. If no
  such machine exists, the user's machine + 40% headroom is the stand-in —
  note which was used.

## How to run — exact commands

In-editor (human): Window > General > Test Runner → EditMode/PlayMode tabs.

Headless (agent; one Unity process per project — close the editor first):

```
# EditMode
Unity -batchmode -projectPath <proj> -runTests -testPlatform EditMode \
      -testResults <proj>/TestResults/edit.xml -logFile <proj>/Logs/tests.log
# PlayMode (levels 2–4)
Unity -batchmode -projectPath <proj> -runTests -testPlatform PlayMode \
      -testResults <proj>/TestResults/play.xml -logFile <proj>/Logs/tests.log
```

⚠ NEVER add `-quit` to a `-runTests` invocation (framework/11). Parse the
results XML (`<test-run result="Passed(...)">`) — the exit code alone lies on
some Unity versions. Static sweep headless:
`Unity -batchmode -executeMethod Friendslop.Reference.Editor.StaticSweep.Run -quit -logFile -`.

## What NOT to test (as binding as the rest)

- **Feel numbers.** No test asserts jumpImpulse == 480. Configs are meant to
  be tuned; tests assert INVARIANTS (jump height between 1–3 m), not values.
- **Visuals.** Screenshot-diff testing is a maintenance pit; slop-check +
  human eyes own visuals.
- **Third-party internals.** Don't test that NGO replicates or Steam
  matchmakes; test OUR wiring of them.
- **The funny.** Level 7 is human. An agent claiming a mechanic "tests as
  fun" has failed framework/11 rule 4.

## Gate requirements (producer verifies via qa-playtester)

- Gate 1: levels 0–3 exist and green (smoke may use 1 player + 3 dummies).
- Gate 2: + level 4 green, + one level-5 session logged.
- Gate 3: + jank guards for every protected jank; content abuse cases added.
- Gate 4: everything green, full manual matrix, perf numbers recorded.
- Gate 5: ship-check (which re-runs A/B on the actual release candidate).
