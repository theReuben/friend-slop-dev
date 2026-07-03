# 08 — QA & playtesting: "is it funny" is a test criterion

Two QA tracks run in parallel: **does it break** (bugs) and **does it land**
(comedy/feel). The qa-playtester agent owns both.

## Track 1 — Does it break

### Automated (run before every gate, and in CI if the project has it)

- EditMode tests: run state machine, scoring, save/settings serialization.
- PlayMode smoke: boot → menu → host → load game → spawn 4 simulated players →
  60 s scripted-random input → assert zero exceptions, zero errors in log.
- Static sweep: no `Debug.Log` outside `DEV_BUILD`, no missing-script
  components in scenes, no missing references (editor script scans all scenes).

### Manual matrix (Gate 4, full pass; abbreviated at Gates 2–3)

| Area | Cases |
|---|---|
| Session | host+3 join, leave, rejoin between runs; host quit mid-run → clients exit cleanly; client force-kill mid-grab |
| Network | 100 ms + 5% loss simulated: playable, no desync of run state; version mismatch join → readable error |
| Solo | full loop runs solo without nulls; UI never shows ghost players |
| Input | KBM + Xbox pad + Steam Deck layout; rebinding if present; alt-tab, focus loss |
| Display | 16:9, 16:10 (Deck), ultrawide (UI anchors, no letterbox breakage); windowed/fullscreen toggle; resolution switch mid-session |
| Physics abuse | pile all props on one player; everyone grabs the same object; clip-into-geometry attempts at every level border; quit while ragdolled |
| Save/settings | volume/display/mute-player settings persist across restart |
| Steam | overlay opens, invites work, achievements fire once, offline-Steam launch → solo works |

**Severity policy:** crash/blocker = fix now; exploit that's funny and harmless
= keep, protect with a test, consider it a feature; cosmetic jank = fix only in
Phase 4; balance complaints = tune configs, never new systems.

## Track 2 — Does it land (feel & comedy)

### The funny metric (Gate 1 pass/fail)

Run 3+ greybox sessions with 4 players (humans when available; otherwise agents
review session recordings). Log every laugh-out-loud moment with a timestamp
and cause. **Pass: ≥ 1 genuine funny moment per 10 minutes, arising from the
mechanic** (not from bugs or from the players being funny people). Track the
*cause distribution* — if all comedy comes from one interaction, the mechanic
is a one-liner, not a game.

### Feel checklist (every playtest)

- [ ] Restart after fail ≤ 5 s, single input.
- [ ] A new player does the core verb correctly within 60 s without reading.
- [ ] Failure is always survivable-in-spirit: players laugh, nobody quits angry
      (rage must be *fun* rage — recovery is always visible and reachable).
- [ ] The clip test: this session produced ≥ 1 twenty-second segment you'd post.
- [ ] Spectator legibility: someone watching (not playing) narrates the stakes
      correctly.
- [ ] No downtime > 30 s for any player (death = fast respawn or an active
      spectator role — dead players should still cause or commentate chaos).

### Playtest logistics on $0

- Phase 1–3: dev-team sessions + Steam Playtest builds to friends.
- Phase 4: **Steam Playtest** feature (free, no keys needed, builds wishlists)
  with a feedback form link on the exit screen (Google Form).
- Watch recordings, not opinions: ask playtesters to record; clip timestamps of
  laughs and of confusion. Confusion clusters = onboarding fixes; laugh
  clusters = trailer material (save them for `10-virality.md`).

## Bug reporting convention

All findings → `games/<name>/PRODUCTION_LOG.md` under `## Bugs` as
`- [ ] [S1|S2|S3] repro — expected — actual`. S1 crash/blocker, S2 damages the
experience, S3 cosmetic. Gate 4 requires zero S1, zero netcode S2.
