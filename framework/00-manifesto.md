# 00 — Manifesto: what we're building and why

## The product

**Friendslop**: a 2–4+ player co-op game with one novel physics-adjacent mechanic,
a strong comedic failure mode, and sessions of 30–90 minutes. The game is bought
because a streamer's audience watched four friends scream at each other, and it
costs less than lunch.

Reference class (study these, never clone them): *Peak* (co-op climbing, shared
stamina economy), *Chained Together* (players physically tethered), *Content
Warning* (film your own horror, the recording IS the content), *Lethal Company*
(quota + proximity voice comedy), *R.E.P.O.* (physics hauling), *Getting Over It*
lineage (rage + spectation value).

## The business model

- **Cash per game: ~$100** (Steam Direct fee). Assets are free/open-source,
  tools are free (Unity personal, Blender, Audacity, Krita/GIMP, DaVinci Resolve).
- **Price point: $4.99–$7.99**, launch discount 10–20%. Impulse-buy territory —
  a 4-pack must cost less than a pizza.
- **No running costs.** Steam P2P relay for networking, Steam for distribution,
  no backend, no accounts, no telemetry service. If a design needs a server, the
  design is wrong.
- **Portfolio logic.** Most of these games will make little. That's fine and
  expected — the model is many cheap shots at a fat-tailed outcome. A game that
  ships mediocre teaches more than a game polished forever. Never let one game
  eat two games' worth of calendar.

## Ship and forget

- **Time-box: 8 weeks** from concept approval to Steam release. The producer may
  extend once, by 2 weeks, with a written reason in the production log. There is
  no second extension — cut scope instead.
- **Post-launch: one 2-week patch window** for crash/blocker/netcode fixes and
  the cheapest high-impact feedback items. Then the game is done. Archive it,
  start the next one.
- "Ship and forget" is about **support burden**, not quality at launch. The
  ship-check gate exists precisely because we won't be around to fix it later:
  no crashes, no progression blockers, netcode holds a 4-player session.

## Quality bar: "human-made indie", not "polished AAA"

Jank in the *physics* is a feature. Jank in the *presentation* is death.
The game must look like a small team with taste made it:

- One coherent art style per game, enforced by the art-director agent and the
  slop-check gate (`06-art-direction.md`).
- Zero AI-generated assets, anywhere, ever — including the store capsule and
  trailer. This is a hard brand rule, not an aesthetic preference: streamers and
  Steam reviewers actively flag AI slop, and Steam requires AI disclosure on the
  store page. Our disclosure will truthfully say "none".
- Free/open assets are raw material, not finished art. They get unified in
  Blender (palette, proportions, texel density) so nothing looks kitbashed.

## What each game must be

1. **Thematically distinct** from every previous game in `games/` — new setting,
   new fantasy, new verb.
2. **One novel mechanic** you can explain in one sentence and a viewer can
   understand in one clip. Novelty check lives in `02-friendslop-design.md`.
3. **Funnier to watch than to play.** If a 20-second clip of a failure isn't
   shareable, the core loop isn't done.
4. **Co-op mandatory in design, playable at every count 1–4.** Solo must not
   crash or block (streamers preview solo), but the design targets 3–4.

## What we never do

- Paid assets, paid plugins, paid services, ads spend.
- Live-ops, battle passes, DLC roadmaps, community management commitments.
- Clones. "X but multiplayer" is only valid if X isn't already multiplayer
  friendslop.
- Crunch mechanics that need constant balancing (economies, PvP ladders).
- Anything requiring moderation of user-generated content.
