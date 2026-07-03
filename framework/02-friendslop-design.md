# 02 — Friendslop design: pillars, pitch template, novelty check

## Why these games work (design pillars)

1. **One shared physical problem.** The core mechanic couples players together —
   literally (a chain, a stretcher, a rope team) or economically (shared stamina,
   shared quota, one flashlight). Coupling converts one player's mistake into
   everyone's problem, which is the comedy engine.
2. **Failure is louder than success.** Falling, dropping, breaking, and dying must
   be spectacular, physical, and survivable-in-spirit (quick restart, run-based).
   Design the failure animation/ragdoll/sound FIRST — it's the clip.
3. **Voice is a mechanic.** Proximity/occlusion voice creates panic comedy
   ("WHERE ARE YOU"). Any design that works equally well on mute is too weak.
4. **Legible from the couch.** A first-time viewer of a stream must understand
   goal + stakes within 30 seconds of any moment. Simple verbs, readable world,
   big obvious hazards.
5. **Run-based, 20–40 min runs, 30–90 min sessions.** No saves mid-run, no meta
   grind required. Streamers can start and finish an arc in one sitting.
6. **Skill floor on the floor, ceiling in the ceiling.** Anyone can flail
   usefully in minute one; group coordination mastery emerges over hours.
7. **Betrayal affordances, cooperation incentives.** Players CAN grief (push,
   drop, steal) but the win condition punishes it. The tension is the content.

## Pitch template (use for every concept)

```
NAME (working):
LOGLINE (≤ 25 words):
THE CLIP: describe the 20-second failure clip that sells the game.
NOVEL MECHANIC (1 sentence): the thing no shipped friendslop game does.
COUPLING: how does the mechanic bind players to each other?
VOICE: why is this game worse on mute?
VERBS: the 3–5 things a player does (e.g. climb, grab, throw, brace).
RUN SHAPE: start state → escalation → fail/win → restart hook.
THEME/SETTING: and why it's visually ownable with free assets.
ART STYLE (1 sentence + 3 reference games/artists):
SCOPE RISKS: the 2 hardest things to build.
STEAM NEIGHBORS: 3 comparable games, their price + review count.
```

## Novelty check (all must pass at Gate 0)

- [ ] The novel mechanic sentence does not describe any game in `games/` nor any
  friendslop title with >500 Steam reviews (search Steam tags: Online Co-Op +
  Physics + Funny; check recent viral titles via web search).
- [ ] The theme (setting + fantasy) is distinct from every previous `games/` entry.
- [ ] "X but multiplayer / but Y-theme" pitches are rejected unless the mechanic
  itself is new — reskins fail this check.
- [ ] The mechanic creates emergent stories (players will tell "and then Dave…"
  anecdotes), not just a difficulty modifier.

## Fertile mechanic spaces (prompts, not answers)

Generate by colliding: a **physical coupling** (tethered, stacked, carrying one
big object, shared body parts, swapped controls under conditions, one player is
the vehicle/tool) × a **traversal or extraction pressure** (ascend, descend,
escort, haul, evade on a timer) × a **perception asymmetry** (only one can see X,
information must be spoken, darkness/fog/scale differences). Reject collisions
that don't produce a funny failure image.

## Scope guardrails at design time

- ≤ 5 player verbs. Every verb must interact with physics or other players.
- No inventory/crafting/skill trees unless they ARE the novel mechanic.
- No narrative beyond ambient worldbuilding + one tone-setting intro screen.
- No enemies with pathfinding unless the design doc justifies why hazards and
  physics can't create the pressure instead (AI enemies are a scope trap).
- Systems over content: prefer 1 recombining hazard system to 10 bespoke levels.
- Singleplayer = same game with bots ABSENT, not a separate mode. It must be
  playable (menus, no null refs on solo) but never designed for.

## Feel targets (tune in Phase 1, protect forever)

- Restart-after-fail in < 5 seconds, one input, no menu round-trip.
- Characters: heavy-limbed, slightly clumsy, readable silhouette; ragdoll on any
  significant impact; grabbing/holding always physical (joints, not parenting).
- Emotes/pings/mouth-flap on voice — bodies must broadcast what voices say.
