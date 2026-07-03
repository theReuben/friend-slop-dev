---
name: ship-check
description: The pre-release gate — verifies build, netcode, settings, credits, store page, and legal items before a Steam launch. Use at Gate 4 (all except store items) and Gate 5 (everything), or when the user asks "are we ready to ship?".
---

# ship-check — nothing launches without this

Run as `steam-publisher` with `qa-playtester` verifying evidence. Every item
needs an artifact (test output, screenshot, config value), not an assurance.
Full detail behind each item: `framework/09-steam-shipping.md`.

## A — Stability (qa-playtester provides evidence)

- [ ] PlayMode smoke test green on the release candidate build.
- [ ] Manual matrix from `framework/08-qa-playtesting.md` fully passed.
- [ ] Zero S1 bugs, zero netcode S2 bugs open in the production log.
- [ ] 4-player internet session (real machines, real Steam accounts, real
      AppId) completed a full run with voice within the last 3 days.
- [ ] Host-quit and client-disconnect paths verified on the RC build.
- [ ] 60 fps at 1080p on the GTX 1060-class target (profile capture attached).

## B — Build correctness

- [ ] Real AppId everywhere; no 480; no `steam_appid.txt` in the depot.
- [ ] `DEV_BUILD` define off; local-transport toggle stripped; log spam absent.
- [ ] Version string on main menu matches the depot build.
- [ ] Settings complete & persistent: resolution/fullscreen, 3 volume
      sliders, voice mode (proximity/all/PTT), per-player mute, sensitivity,
      invert-Y.
- [ ] Steam overlay opens in-game; friend invite → join works end to end.
- [ ] Achievements each fired exactly once in a test run.
- [ ] Steam Input: pad + Deck configs present; game is controllable on both.
- [ ] Solo launch with Steam offline reaches gameplay without errors.

## C — Credits & legal (blocks launch outright if wrong)

- [ ] Three-way match: `games/<name>/CREDITS.md` ≡ in-game credits screen ≡
      store page attribution footer.
- [ ] Every `ThirdParty/` folder has LICENSE.txt; every CC-BY attribution uses
      the author's requested string.
- [ ] No SA/NC/ND/GPL/unlicensed material anywhere (spot-audit 10 random
      assets back to their source URLs).
- [ ] Steam AI disclosure states no AI-generated content, and slop-check
      Gate 5 confirms it's true.
- [ ] Name cleared (trademark search logged); no trademarked props/music
      leakage; content survey answered honestly.

## D — Store page (Gate 5 only)

- [ ] All capsule sizes uploaded, human-made pipeline, readable at 120 px.
- [ ] 5+ genuine gameplay screenshots with UI; trailer ≤ 60 s, funniest fail
      in first 5 s, playtester voice permission on file.
- [ ] Short description ≤ 300 chars, mechanic front-loaded; 20 tags set.
- [ ] Price per policy ($4.99/$6.99) + launch discount configured.
- [ ] Page has been live ≥ 14 days; build review approved; release date set.

## E — Aftermath readiness

- [ ] Streamer kit live; keys allocated (4 per streamer); outreach drafted.
- [ ] Launch-day posts drafted with best clips attached.
- [ ] "Known issues / tiny team" discussion post drafted.
- [ ] Patch-window calendar (2 weeks) in the production log with an owner.

## Output

Per-item ✅/❌ + artifact links in the production log. Overall verdict
READY / NOT READY (with the blocking list). The user presses the release
button — this skill never does.
