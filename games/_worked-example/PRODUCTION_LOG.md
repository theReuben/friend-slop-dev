# BIG FLOAT — Production Log (WORKED EXAMPLE — events are illustrative)

## State

- **Phase:** 2 (Vertical slice) — day 3 of 14
- **Time-box:** started 2026-06-01 → ships 2026-07-27 (8 weeks; extensions used: 0/1)
- **Unity project path / repo:** `~/dev/big-float-unity` (github.com/EXAMPLE/big-float)
- **Unity version pin:** 6000.4.2f1 · NGO 2.13.0 · Cinemachine 3.1.4 · Facepunch 2.4.1
- **Steam AppId:** 480 (Spacewar) — real AppId purchase queued for L−30

## Current gate checklist (Gate 2)

- [x] Steam lobby: create/invite/join via friends list — evidence: session log 06-16, two real accounts
- [x] Proximity voice with occlusion — evidence: clip `playtests/2026-06-16-voice.mp4`
- [ ] Two remote machines complete a full loop with voice ← **next action**
- [ ] One leg (Suburbs) at target art quality — balloon scratch-build done, bunting pending
- [ ] First slop-check on slice screenshots
- [x] CREDITS.md current — verified against ThirdParty/ 06-17

## Next actions

1. Remote 2-machine full-loop test (needs the user + one friend, ~20 min — batch with feel check #12).
2. tech-artist: bunting + market stall palette remap (unify_pass batch 3).
3. Fix S2 bug #4 before the remote test (it corrupts the test otherwise).

## Backlog (good ideas that don't fit the current phase)

- Balloon panel customization — streams don't need it; player scarves carry identity (deferred at Gate 0)
- Ferry horn knocks hats off — pure charm, Phase 3 if content lands early
- Spectator "news chopper" cam for dead players — Phase 3, cheap and stream-friendly

## Bugs

- [x] [S1] Host quit during lift → clients hang on black screen — expected "Host left" screen — FIXED 06-15, matrix case added
- [ ] [S2] #4: Rope reel-in while clipped to anchor doubles tension force — expected clamp at breakForce — actual 2× spike launches handler
- [ ] [S3] Balloon shadow pops at LOD boundary on Main Street

## Tuning table (feel iteration; snapshot values at gate sign-off)

| Date | Config value changed | From → To | Verdict |
|---|---|---|---|
| 06-04 | buoyancy equivalent | −320 → −240 | 4 players pinned to ground, boring |
| 06-05 | buoyancy equivalent | −240 → −260 | ✅ 3 hold it barely, 4 comfortable — FROZEN at Gate 1 |
| 06-05 | gust strength 7 force | 1400 → 1800 N | lifts one handler reliably, laughter in test — FROZEN |
| 06-06 | uprightDamper | 90 → 70 | wind-wobble reads drunk; 70 kept for this game |
| 06-09 | rope reel speed | 1.5 → 2.2 m/s | reeling feels responsive, no verdict change on comedy |

## Protected jank (funny exploits promoted to features)

- **Jump-while-roped transfers weight upward** (physics accident): three
  players jumping in sync can bounce the fourth over a fence. Kept — it's
  skill expression AND a clip machine. Guard: `RopeWeightTransferTest`.
  `// PROTECTED JANK` comment at `RopeTether.cs:141`.

## History (one line per session / gate event)

- 06-01 — Gate 0 passed: parade balloon escort, logline approved by user
- 06-04..06 — greybox: rope fallback used (3-joint chain, verlet deferred — scope risk #1 resolved)
- 06-08 — **Gate 1 PASSED**: funny metric 9 laughs / 62 min across 3 sessions, cause split: lifts 4, group-drag 3, anchor snap 2. Report in REPORTS.md
- 06-12 — NGO conversion: intents over RPC, host-auth (per reference/unity/PlayerIntent.cs pattern), 1 day as predicted
- 06-15 — S1 host-quit bug fixed; failure matrix row verified
- 06-16 — Steam lobby + voice working on two real accounts
- 06-17 — balloon hero prop scratch-built (Blender, 2,900 tris, palette cells 12–15)

## Post-mortem (fill during the patch window, then archive)

*(empty until launch — see `games/_template/PRODUCTION_LOG.md` for fields)*
