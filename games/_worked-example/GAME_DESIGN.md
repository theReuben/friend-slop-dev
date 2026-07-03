# BIG FLOAT — Design Doc (WORKED EXAMPLE — see README.md)

## Pitch

- **Logline (≤ 25 words):** Four parade handlers walk a colossal helium
  balloon through town on ropes — in wind, under bridges, past everything
  sharp.
- **THE CLIP:** A gust lifts the lightest handler off the ground; the other
  three hang on, heels dragging, as the balloon tows all four toward power
  lines while everyone screams rope-length instructions.
- **Novel mechanic (1 sentence):** The shared object pulls UP — inverted
  hauling where players are the ballast and slack management replaces lifting.
- **Coupling:** One balloon, four tethers. Letting go saves you and dooms the
  other three; the win condition punishes it (balloon altitude cap fails the
  parade if only two ropes remain taut).
- **Voice:** Gust callouts have a ~2 s lead time (audible wind before force
  hits) — teams that talk survive gusts, mute teams get launched. Proximity
  voice means the lifted player's voice literally rises away from the team.
- **Steam neighbors:** PEAK ($7.99, ~200k reviews) / Chained Together
  ($4.99, ~80k) / RV There Yet (~$6.99, ~90k). *(Illustrative counts — a real
  doc records live numbers at Gate 0.)*

## Verbs (≤ 5, all physics- or player-interacting)

1. **Hold rope** (grab; two-handed halves your walk speed, doubles grip force)
2. **Reel / pay out** (change your rope length — the core skill verb)
3. **Clip anchor** (hook your rope to street furniture for a rest — snaps if
   balloon momentum exceeds break force)
4. **Shove** (standard player push — reposition friends under the balloon)
5. **Jump** (weak; jumping while roped transfers your weight upward — a bug
   promoted to feature, see PRODUCTION_LOG protected jank)

## Run shape

- **Start:** depot — balloon inflates (60 s of free play/grief window while
  ropes get assigned).
- **Escalation:** 5 parade legs: Suburbs (learn) → Main Street (bunting,
  lampposts) → Bridge Underpass (ceiling! reel DOWN) → Market Square (crowd
  stalls, popcorn machine updraft) → Riverfront finale (open wind, ferry
  horn gusts).
- **Fail:** balloon escapes (all ropes slack/released), OR punctures
  (3 hazard strikes), OR a handler airborne > 15 s (carried off = run
  continues with 3, second loss = fail).
- **Win:** reach the grandstand with balloon intact; score = time + altitude
  discipline + handlers remaining.
- **Restart hook:** "AGAIN" on the blame card → same leg restart, < 5 s.
- Run length: 25–35 min. Session: 2–3 runs. Players: 4 (works 1–4; solo gets
  a triple-weight harness, still winnable, never fun — by design).

## Failure states table

| Failure | What the viewer sees | Sound | Recovery |
|---|---|---|---|
| Handler lifted off | Feet kick, rope taut, slow gain in altitude, friends dangling counterweight | Rising wind + doppler scream + creak | Others reel in / clip anchor; lifted player can pay out to descend |
| Balloon strike (lamppost) | Balloon crumples locally, whole team yanked sideways off feet | Rubber squeal + 4× ragdoll thuds | Stand up, re-grip within 8 s or strike #2 |
| Rope snap (overload) | Handler flies backward through market stall, balloon lurches up | Cartoon twang + crash + fall-silence beat | Respawn rope at balloon after 10 s cooldown |
| Full escape | Balloon drifts serenely away; camera holds on 4 tiny figures watching | All sound fades except one party horn | Blame card → restart leg |

## Escalation / variety system (Phase 3 spec)

One recombining system: **GustDeck** — a seeded deck of wind events (direction,
strength 2–9, lead-time 1.5–3 s, duration) drawn per leg; legs add modifiers
(underpass halves lead time, riverfront doubles strength range). Hazards are
static geometry + 3 dynamic types (traffic, popcorn updraft, ferry horn = mega
gust). Every element interacts with ≥ 2 others via the wind vector.
Tuning table: `GustDeckConfig.asset`.

## Mass chart (lock in Phase 1)

| Object | Mass (kg) |
|---|---|
| Player | 80 |
| Balloon buoyancy | −260 kg equivalent (lifts 3 players; 4th makes it controllable) |
| Rope segment (per 1 m, verlet) | 0.5 |
| Market stall / lamppost | 300 (anchor-grade, immovable) |
| Popcorn machine | 45 (grabbable, terrible idea, present anyway) |

## Art style bible (art-director owns; framework/06)

- **Palette:** the 16-color reference palette in
  `reference/blender/make_palette_atlas.py` — parade recolor: accent_hot
  reserved for ropes/grabbables, player colors on scarves + balloon panels.
- **Shape language:** "round and festive — everything inflated a little,
  corners don't exist on Main Street."
- **Proportions:** 3 heads tall, hands oversized 1.4× (grip readability).
- **Rendering recipe:** flat-shaded + palette atlas, gradient sky
  (`GradientSkybox.shader`), fog matched to horizon, post volume standard
  recipe (framework/06).
- **References:** A Short Hike, Untitled Goose Game, Kirby Air Ride (town).
- **Pack families:** Kenney City Kit (Suburban + Commercial), Kenney
  Furniture Kit. Gaps: the balloon (hero, scratch-build), bunting, popcorn
  machine — 3 hero props, within budget.

## Audio identity (framework/07)

- Impacts: rubbery + brass-band-adjacent (a dropped tuba "bwomp" family).
- Vocalizations: recorded in-house, pitched ±3 st per player color.
- Music: menu brass waltz + results sting only (CC-BY, credited); runs are
  wind + voice.
- **Fall-silence moment:** handler lifted > 10 m — ambience cuts, wind and
  their doppler voice remain. Signature confirmed.

## Scope risks (producer tracks)

1. Verlet rope ↔ Rigidbody coupling stability at 4 ropes × 50 Hz (Phase 1
   greybox question #1 — if ropes need > 1 week, fall back to 3-joint chain
   "rope" which is known-stable).
2. Wind + buoyancy tuning space is 2D (lift × gust) — needs the one-change
   discipline hard, or feel work spirals.

## Out of scope (pre-refused; producer enforces)

Client prediction · host migration · mid-run join · > 4 players · pathfinding
enemies · meta progression · economies · level editor · UGC · narrative
beyond one intro screen. Additions: balloon customization (backlogged —
player-color panels suffice for streams), crowd NPC reactions (ambient audio
sells it for $0).
