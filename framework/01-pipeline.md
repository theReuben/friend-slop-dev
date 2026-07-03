# 01 — Pipeline: concept to shipped in 8 weeks

Every game moves through six phases. Each phase ends in a **gate** — a checklist
the producer verifies and records in `games/<name>/PRODUCTION_LOG.md`. Work
belonging to a later phase is logged to the backlog, not done early. Work from a
failed gate is redone before anything else.

Timings assume part-time human oversight with agents doing the heavy lifting.
The producer owns the calendar.

## Phase 0 — Concept (2–3 days)

**Agents:** game-designer (lead), art-director, producer.

- Generate 5+ pitches using the pitch template in `02-friendslop-design.md`.
- Kill pitches that fail the novelty check, theme-distinctness check (vs. other
  `games/` entries and the live Steam market), or the "explain the clip" test.
- For the winner: one-page design doc (`GAME_DESIGN.md` from template), art
  style bible stub (art-director), feasibility note (gameplay-engineer: can the
  core mechanic be prototyped in a week with free assets?).

**Gate 0:** User approves the pitch. Novel mechanic in one sentence. Theme
distinct. Asset-scout confirms the style is servable from free sources.

## Phase 1 — Mechanic prototype (1 week)

**Agents:** gameplay-engineer (lead), game-designer.

- Greybox only. Primitives, ProBuilder, no art. Local multiplayer via multiple
  editor instances or NGO's built-in tooling.
- Build ONLY the novel mechanic + basic character controller + camera.
- The question this phase answers: **is the mechanic funny?** Iterate on feel
  (`03-unity-conventions.md` § physics feel) until failure states make people
  laugh in playtests.

**Gate 1:** A 4-player greybox session produces at least one genuinely funny
moment per 10 minutes (qa-playtester scores this). If after a week it isn't
funny, kill the concept and return to Phase 0 — this is cheap now and fatal later.

## Phase 2 — Vertical slice (2 weeks)

**Agents:** netcode-engineer (lead), gameplay-engineer, art-director, asset-scout,
tech-artist, audio-designer.

- Real netcode: Steam lobbies, join via friends list, proximity voice, 4 players
  over the internet (`04-netcode.md`).
- One level/area at target art quality: sourced assets unified in Blender,
  lighting pass, post-processing, UI style (`06-art-direction.md`).
- Core loop closed: start → play → fail/win → results → play again.
- First `slop-check` run happens here, on the slice.

**Gate 2:** Two remote machines complete a full loop together with voice.
Slice screenshots pass slop-check. `CREDITS.md` is complete for every asset used.

## Phase 3 — Content (2 weeks)

**Agents:** game-designer, gameplay-engineer, tech-artist, audio-designer,
qa-playtester.

- Expand to launch content: 3–5 levels/biomes/scenarios OR procedural variation,
  whichever the design doc specifies. Friendslop needs 4–10 hours of replayable
  chaos, not a campaign.
- Escalation/variety systems (new hazards, items, mutators) — prefer systems
  that recombine over hand-authored content.
- Full audio pass: SFX for every interaction, ambience per area, 2–4 music
  tracks (`07-audio.md`).

**Gate 3:** A 4-player group plays 60+ minutes without running out of novelty.
No progression blockers. Content complete — after this gate, nothing new gets
added, only fixed and polished.

## Phase 4 — Polish & harden (1 week)

**Agents:** qa-playtester (lead), gameplay-engineer, netcode-engineer, tech-artist.

- Bug triage: crashes and blockers fixed; cosmetic jank that isn't funny fixed;
  physics jank that IS funny kept and protected with a test.
- Performance: steady 60 fps on a GTX 1060-class machine at 1080p.
- Failure-mode QA: host quits, client disconnects mid-action, alt-tab, resolution
  changes, controller + KBM (`08-qa-playtesting.md`).
- Final slop-check on every scene and the UI.

**Gate 4:** ship-check skill passes everything except store-page items.

## Phase 5 — Ship (1 week)

**Agents:** steam-publisher (lead), art-director (capsule), producer.

- Steamworks setup, depots, build upload, store page, capsule art (human-made:
  screenshot paintover or Blender render, per `06-art-direction.md`), trailer
  cut from playtest footage, tags, pricing (`09-steam-shipping.md`).
- Streamer kit + launch checklist from `10-virality.md`.
- Steam review lead times: submit store page ≥ 2 weeks before target launch
  (page review + the mandatory "coming soon" period), build review a few days
  before.

**Gate 5 (launch):** ship-check passes fully. User presses the release button.

## Post-launch — patch window (2 weeks, then stop)

- Watch reviews/forums for crashes, blockers, netcode failures. Fix those.
- One "thanks for playing" patch max for cheap high-impact requests.
- Write the **post-mortem** in `PRODUCTION_LOG.md` and fold lessons back into
  `framework/` docs. Then archive and start the next game.

## Escalate to the user (don't decide alone)

- Killing a concept after Gate 0, extending the time-box, any cash spend,
  pricing, release date, anything legal (license ambiguity, trademark-adjacent
  names), and pressing the launch button.
