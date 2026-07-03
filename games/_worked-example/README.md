# _worked-example/ — what good output looks like

This folder is a **worked example, not a real game**. It shows the concept
bank's BIG FLOAT pitch taken through a simulated Gate 0–2, so every template
in `games/_template/` has a filled-in reference standing next to it.

## Rules for models reading this

1. **Imitate the FORM, not the content.** When filling in a real game's
   design doc / production log / credits, match this level of concreteness
   (numbers everywhere, evidence links, named rules) — not these specific
   values.
2. **Exclude this folder from all checks.** It does not count for theme
   distinctness, and BIG FLOAT **remains available** in `games/CONCEPT_BANK.md`
   — building it for real means re-running the full Gate 0 process fresh.
3. **The events in PRODUCTION_LOG.md are illustrative**, written to
   demonstrate realistic entries (tuning iterations, a killed feature, a
   protected-jank promotion, gate evidence). They did not happen.
4. The CREDITS.md entries reference real asset packs with their real
   licenses (Kenney = CC0, verified via corroborating sources at authoring
   time) — but per `framework/05`, a real game re-verifies on the source
   page at download time.

## What to look at, per role

- **game-designer** → `GAME_DESIGN.md`: note failure-states table specificity
  and that every verb interacts with physics or players.
- **producer** → `PRODUCTION_LOG.md`: gate items closed with evidence, backlog
  items with one-line reasons, the time-box math visible.
- **asset-scout** → `CREDITS.md`: row added per pack with modification notes;
  rejected-assets table preventing re-work.
- **qa-playtester / art-director** → `REPORTS.md`: a funny-metric report, a
  slop-check report with per-item verdicts, and a sourcing report — the three
  deliverable formats, at target quality.
