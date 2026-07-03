# Concept bank — pre-vetted seeds (authored July 2026)

Eight pitches generated with full pipeline judgment and checked against the
market as of **July 2026**. Occupied territory at authoring time: co-op
climbing (*PEAK*), vehicle convoy (*RV There Yet*), chained players (*Chained
Together*), filming horror (*Content Warning*), quota extraction (*Lethal
Company*), physics hauling (*R.E.P.O.*, *Moving Out*), sensory-impairment bomb
defusal (*Bombanana*), party golf (*Super Battle Golf*), puppet brawling
(*Party Animals*), wobbly puzzle traversal (*Human Fall Flat*).

**These are seeds, not approved concepts.** The `new-game` skill still runs
the full Gate 0 process on any of them: RE-VERIFY novelty against the market
at use time (this file ages fast — anything here may have shipped by the time
you read it), then expand to the full pitch template. Strike entries through
when used or invalidated, with a dated note.

---

## 1. HOSED (working) — tangled-air-hose salvage divers
- **Logline:** Four salvage divers share one air pump; your hoses tangle, snag,
  and yank — untangle or suffocate, together.
- **THE CLIP:** Four hoses braided into a knot around a shipwreck mast; air
  meters blinking; everyone swimming in panicked circles making it worse.
- **Novel mechanic:** Long physical tether that *accumulates entanglement* —
  the players' own paths through 3D space become the obstacle.
- **Coupling:** All hoses to one pump; one diver's shortcut is everyone's knot.
  **Voice:** "go UNDER me, UNDER" untangling choreography; occluded gurgles.
- **Why not Chained Together:** chains are short/rigid pairwise; hoses are
  long, snagging, shared-resource, and the failure is topological, not fall.
- **Assets:** underwater = fog + simple rocks/wrecks; Kenney pirate/fish packs.
  Verlet-rope hose physics is the scope risk — greybox THAT in week one.

## 2. SHHH. (working) — carry the sleeping giant, your real mic is the alarm
- **Logline:** Carry a sleeping giant across the county without waking it —
  and it can hear your actual microphones.
- **THE CLIP:** Tiptoeing perfectly for five minutes, then one player sneezes
  IRL and the giant sits up.
- **Novel mechanic:** Real voice-chat volume is a gameplay input (noise meter
  reacts to mic loudness near the giant) — talking, laughing, screaming have
  literal stakes. Nobody in the genre has made the mic itself the hazard.
- **Coupling:** 4-point carry of one huge ragdoll; drops = thumps = waking.
  **Voice:** the game is ABOUT modulating it; whispering co-op is unbearable
  on mute and hilarious on stream (streamer's chat spam = donation-scream risk).
- **Risks:** mic-level calibration across setups (needs a settings ritual);
  keeping laugh-cascades fun rather than punishing (waking = chase sequence,
  not fail screen).

## 3. LOAD-BEARING FRIENDS (working) — you are the scaffolding
- **Logline:** No ladders, no bridges — latch onto your friends to BECOME
  them, then walk the whole trembling structure to the goal.
- **THE CLIP:** A four-person tower inching toward a ledge, bottom player's
  legs shaking, top player leaning the wrong way; slow-motion collective topple.
- **Novel mechanic:** Players rigidly latch body-to-body into composite
  structures; the bottom player retains locomotion. You are the level geometry.
- **Coupling:** Literal. **Voice:** the bottom player can't see; the top
  player steers by yelling ("LEFT FOOT. YOUR OTHER LEFT.").
- **Why not Human Fall Flat/PICO PARK:** those use players *incidentally* as
  platforms; here latching/formation IS the verb set, in 3D physics.
- **Risks:** stacked-capsule physics stability (solver iterations, mass
  scaling per TUNING_DEFAULTS); level design must demand varied formations.

## 4. BIG FLOAT (working) — parade balloon escort
*(Used as the form reference in `games/_worked-example/` — still AVAILABLE to
build for real; re-run Gate 0 fresh if picked.)*
- **Logline:** Four handlers walk a colossal helium balloon through town on
  ropes, in wind, under bridges, past everything sharp.
- **THE CLIP:** Gust lifts the lightest player off the ground; the other three
  hang on as the balloon drags all four toward power lines.
- **Novel mechanic:** The shared object pulls UP — inverted hauling where
  slack management and wind reading replace lifting. Players are the ballast.
- **Coupling:** One balloon, four tethers; letting go saves you and dooms them.
  **Voice:** wind gust callouts, rope-length coordination.
- **Assets:** one town kit (Kenney city) + cloth/balloon shader kept simple
  (rigid balloon + joint ropes reads fine). Wind system = one Perlin gust
  field, cheap. Theme is visually ownable and joyful — capsule art gift.

## 5. STRETCHER! (working) — two-man ragdoll ambulance service
- **Logline:** Paramedic duo (×2 teams or 4 on one stretcher) hauls ragdoll
  patients down a mountain disaster zone — patient ON the stretcher, please.
- **THE CLIP:** Patient bounces off the stretcher, cartwheels down scree; both
  medics drop everything and dive after the body.
- **Novel mechanic:** The cargo is an unrestrained ragdoll on a tray you tilt —
  a marble-labyrinth where the marble is a body and the labyrinth is two
  stressed friends' grip.
- **Coupling:** Front/back carry: your speed is their tilt. **Voice:**
  "steps-steps-STEPS" cadence calls.
- **Why not R.E.P.O./Moving Out:** hauled objects there are rigid and the
  challenge is geometry; here the challenge is the CARGO's physics (loose
  ragdoll retention), plus triage timers.
- **Risks:** ragdoll-on-tray jitter (needs the joints gotchas from 11).

## 6. EGG DAY (working) — escort the rolling colossus you cannot grab
- **Logline:** A sacred ten-ton egg rolls downhill through the village; your
  bodies are the only brakes.
- **THE CLIP:** Egg steamrolls the player trying to slow it, flattening them
  into the mud mid-sentence, and keeps going toward the church.
- **Novel mechanic:** Pure BLOCKING as the core verb — no grab, no carry; you
  redirect a huge momentum object with body checks, wedges, and sacrificial
  dives. The inverse of every hauling game: the object moves itself.
- **Coupling:** One egg, four bodies, momentum math nobody can solve alone.
  **Voice:** trajectory panic ("it's drifting LEFT, Dave, LEFT").
- **Assets:** one village kit + terrain; the egg is a sphere — the cheapest
  hero object imaginable. Run = uphill sections (push) alternating downhill
  (brake), weather mutators.

## 7. SCENE CHANGE (working) — stagehands during a live play
- **Logline:** Rearrange an entire theater set between (and during!) scenes
  of a live play without the audience noticing you.
- **THE CLIP:** Blackout ends one beat early; four stagehands frozen mid-stage
  holding a couch, a tree, and each other, as the actors improvise around them.
- **Novel mechanic:** Red-light/green-light governed by a *performance
  schedule* you must haul furniture through — visibility windows (blackouts,
  spotlight positions, audience sightlines) are the level's rhythm.
- **Coupling:** Two-person furniture + shared audience-suspicion meter.
  **Voice:** whisper-coordination during scenes, chaos during blackouts.
- **Why fresh:** heist-stealth tension × physics hauling × comedy of freezing
  in plain sight; no shipped friendslop occupies backstage theater.
- **Risks:** needs a readable "you are seen" model (sightline cones); the
  play's script = escalation content (each act adds absurd set pieces).

## 8. BEAST OF BURDEN (working) — one of you is the mule
- **Logline:** One player is the pack beast on all fours — the others load the
  cargo, hold the reins, and deeply regret the route they chose.
- **THE CLIP:** Overloaded player-mule refuses a rickety bridge; handlers pull
  reins from the front while a third pushes from behind; bridge, predictably,
  goes.
- **Novel mechanic:** A player IS the vehicle: asymmetric roles where the
  mule controls locomotion but not the load, handlers control the load and
  route but not the legs. Roles rotate per leg of the journey.
- **Coupling:** Physical reins (joints) + stacked cargo on a living player.
  **Voice:** negotiation comedy — the mule can refuse; reins only *suggest*.
- **Why not RV There Yet:** the vehicle has feelings and a microphone.
- **Risks:** quadruped player controller variant of the hover capsule (same
  motor, four probe points); cargo-stack stability.

---

*Authoring sources for market state: GameSpot's 2026 friendslop roundup,
Gamerant on Bombanana, PEAK sales reporting. Re-search before use.*
