# friend-slop-dev

A reusable agent framework for building **friendslop** games — small, physics-driven,
co-op multiplayer games (think *Peak*, *Chained Together*, *Content Warning*,
*R.E.P.O.*) designed to be cheap to make, funny to watch, and viral on streams.

This repo does not contain a game. It contains the **factory**: agents, playbooks,
checklists, and templates that Claude Code (Sonnet/Opus-class models) uses to take a
game from concept to a shipped Steam release, repeatedly.

## How to use

1. Open this repo in Claude Code.
2. Say: `Start a new game` (or invoke the `new-game` skill).
3. The **producer** agent walks the game through the pipeline defined in
   `framework/01-pipeline.md`, delegating to the specialist agents in `.claude/agents/`.
4. Each game gets its own Unity project; this repo tracks its design docs,
   credits, and production log under `games/<game-name>/`.

## Layout

| Path | What it is |
|---|---|
| `CLAUDE.md` | Entry point — routing rules for the model running here |
| `framework/` | The doctrine: pipeline, design pillars, art direction, netcode, shipping |
| `.claude/agents/` | Specialist subagent definitions (designer, engineer, art director, …) |
| `.claude/skills/` | Repeatable procedures: `new-game`, `asset-hunt`, `slop-check`, `ship-check` |
| `games/_template/` | Scaffold copied for every new game |
| `games/<name>/` | One folder per game in production |

## Non-negotiables (see `framework/00-manifesto.md`)

- **Cheap**: ~$100 cash per game (the Steam Direct fee). Everything else is free/open
  assets and our time.
- **Ship and forget**: hard time-box, one post-launch patch window, then done.
- **No AI slop**: free human-made open-source assets only, unified by hand in Blender.
  Every artist credited. The bar is "looks like a human-made indie game" — enforced by
  the `slop-check` skill and the art-director agent.
- **Thematically distinct + novel mechanic** per game — enforced at the concept gate.
