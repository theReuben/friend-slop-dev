# games/

One folder per game. `_template/` is the scaffold the `new-game` skill copies —
never work in it directly, and never delete a shipped game's folder (post-
mortems are the factory's memory; the novelty check reads them).

`_worked-example/` is a filled-in reference (BIG FLOAT, simulated Gate 0–2)
showing what good output looks like — imitate its form when filling templates,
EXCLUDE it from theme-distinctness checks, and note BIG FLOAT remains
available in `CONCEPT_BANK.md`.

Each game folder contains:

- `GAME_DESIGN.md` — the one-page living spec (pitch, verbs, style bible).
- `PRODUCTION_LOG.md` — current phase, gate checklist, backlog, bugs, history,
  post-mortem. **Read this first, update it after every session.**
- `CREDITS.md` — every external asset's paper trail; feeds the in-game credits
  and the Steam page footer.

The Unity project itself lives in its own repo/location, recorded in the
production log — this repo holds the paperwork, not the binaries.
