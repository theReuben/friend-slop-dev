---
name: new-game
description: Start a new friendslop game — runs concept generation, novelty checks, and scaffolds the game folder from the template. Use when the user says "start a new game", "new concept", or "what should we build next".
---

# new-game — spin up a new game through Gate 0

## Steps

1. **Load context.** Read `framework/00-manifesto.md`, `framework/01-pipeline.md`,
   `framework/02-friendslop-design.md`, and skim every existing
   `games/*/GAME_DESIGN.md` + post-mortems in their production logs (theme
   distinctness and "what did we learn" both feed the new pitch).
2. **Market snapshot.** Web-search the current friendslop landscape: Steam
   tags Online Co-Op + Physics + Funny (recent top sellers + new releases),
   plus "viral co-op game" for the last 6 months. List the mechanics already
   taken. Do not rely on training data — the genre moves monthly.
3. **Generate pitches.** Start from `games/CONCEPT_BANK.md` — pre-vetted
   seeds with the market analysis already done (re-verify each seed's novelty
   against step 2's snapshot; strike out any that shipped in the meantime).
   Then delegate to the `game-designer` agent: expand the best seeds and/or
   add fresh pitches to reach 5+ using the template in 02, self-attack with
   the novelty check and pillars, killing at least half. Rank survivors
   (clip strength > coupling > free-asset buildability > theme ownability).
4. **Cross-checks on the top pick** (parallel delegations):
   - `art-director`: draft style bible stub (palette family, shape language,
     rendering recipe, 3 references).
   - `asset-scout`: style servability check — name the actual ≤ 3 pack
     families and estimate the gaps count.
   - `gameplay-engineer`: feasibility note — can the novel mechanic greybox in
     one week? Name the 2 hardest technical risks.
   - Name check: trademark/Steam/itch collision search on the working title.
5. **Present to the user**: top pitch (full template) + 1–2 runners-up
   (loglines only) + the cross-check results + a clear recommendation. Wait
   for approval — Gate 0 is a user decision, never autonomous.
6. **On approval, scaffold:**
   - Copy `games/_template/` → `games/<kebab-name>/`.
   - Fill `GAME_DESIGN.md` from the winning pitch + style bible.
   - Initialize `PRODUCTION_LOG.md`: phase = 1, time-box start date, week-by-
     week calendar from `01-pipeline.md`, Gate 1 checklist, empty backlog/bugs.
   - Record where the Unity project will live and the Unity version pin.
   - Commit with message `game(<name>): Gate 0 passed — <logline>`.

## Refuse to proceed when

- The pitch fails any novelty-check item (say which).
- The theme overlaps an existing `games/` entry.
- The servability check found > ~10 hero-prop gaps (style too expensive).
- The user hasn't explicitly approved (present and stop).
