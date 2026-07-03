# Tuning defaults — start here, not from zero

Starting values for every feel-critical number, with the reasoning so future
tuners know which direction to move and why. Change ONE value per playtest;
log every change in the production log's tuning table.

## Project physics settings

| Setting | Value | Why |
|---|---|---|
| Fixed Timestep | 0.02 (50 Hz) | Stable joints at 4-player prop counts; 60+ Hz wastes host CPU |
| Solver Iterations | 8 position / 2 velocity | Default 6/1 lets ConfigurableJoint chains stretch/explode |
| Default Max Angular Speed | 20 | Default 7 makes flung ragdolls look damped/dead |
| Gravity | −14 to −18 m/s² | Real −9.81 feels floaty at game scale; snappier falls are funnier. Pick once at Gate 1, never touch again |
| Sleep Threshold | 0.05 | Props settle & sleep fast → Rigidbody budget holds |

## Mass chart (the comedy depends on ratios, not absolutes)

| Object | kg | Note |
|---|---|---|
| Player capsule | 80 | The reference unit — everything is felt relative to this |
| Small prop (mug, tool) | 1–5 | Throwable one-handed, harmless |
| Medium prop (chair, crate) | 8–25 | Carryable, staggers you on impact |
| Large prop (couch, barrel) | 30–60 | Two players carry well, one struggles funnily |
| Vehicle/anchor objects | 300+ | Effectively immovable by players — and keep any joint chain to ≤ 10:1 mass ratio per link or it jitters |

## Character motor (MotorConfig, 80 kg body)

| Field | Start | Tuning direction |
|---|---|---|
| rideHeight | 0.6 m | = capsule half-height + 0.1; higher = steppier stairs handling |
| springStrength | 12000 | Too low: sinks on landing. Too high: vibrates on slopes |
| springDamper | 1200 | ~10% of strength; less = bouncy landings (often funnier — try 800) |
| maxSpeed | 5.5 m/s | 4.5–6.5 is the genre window; faster breaks proximity-voice comedy range |
| acceleration | 45 | Lower (30) = more momentum comedy, harder to stop |
| airControl | 0.25 | Keep low — committed jumps create the falls |
| uprightSpring | 800 | Higher = stiffer/less funny; lower = drunk |
| uprightDamper | 90 | THE wobble dial. 60 = tipsy, 150 = sober. Tune this before anything else |
| giveUpAngle | 70° | Below 60° players flop too often to traverse; above 80° they never flop |
| jumpImpulse | 480 N·s | = 6 m/s takeoff ≈ 1.2–1.8 m apex depending on gravity |
| coyoteTime | 0.12 s | Below 0.08 feels unfair; above 0.2 looks like flying |

## Grabbing (GrabSystem)

| Field | Start | Note |
|---|---|---|
| grabRange / grabRadius | 1.6 m / 0.45 | Generous on purpose — whiffed grabs frustrate, they aren't funny |
| jointSpring / damper | 3000 / 300 | Softer (1500) = comically saggy carrying; test both early |
| breakForce | 2500 N | Props ~never break; player-vs-player tug-of-war breaks sometimes = the bit |
| maxGrabMass | 120 kg | Player (80) + held small prop stays grabbable; furniture-throwing needs 2 grips |

## Ragdoll (RagdollBlender)

| Field | Start | Note |
|---|---|---|
| impactThreshold | 350 N·s | ≈ 80 kg hitting a wall at 4.5 m/s. Sprint-into-wall SHOULD flop; walking into it shouldn't |
| minDownTime | 1.6 s | The camera-linger beat. Shorter kills the clip; longer bores the victim |

## Camera (Cinemachine follow)

| Setting | Start |
|---|---|
| FOV | 70 (wide = more friends in frame = more comedy per clip) |
| Distance / height | 5.5 m / 1.8 m, slight down-tilt |
| Follow damping | 0.4–0.8 s positional; NO rotational lag (nauseating on stream) |
| Death cam | cut to victim's ragdoll, hold 2 s, then respawn |

## Audio (ImpactAudioConfig + mix)

| Setting | Start |
|---|---|
| pitchVariance | 0.10 |
| Body-surface band | min 150 N·s, max 800, 4+ clip variants |
| Voice rolloff | Linear, min 1 m, max 25 m — audible slightly beyond visual recognition range |
| Occlusion cutoff | 900 Hz occluded / open 22 kHz, 0.15 s smooth |
| Duck under voice | Ambience+Music −6 dB, 80 ms attack / 400 ms release |

## Netcode rates

| Setting | Start |
|---|---|
| Character NetworkTransform | 30 Hz send, position+yaw only, interpolation on |
| Prop NetworkTransform | 15 Hz, full rotation |
| Voice packets | as-produced (~20 ms), unreliable delivery |
| Tick rate (NGO) | 30 |

## The meta-rule

When a value feels 90% right, STOP. The last 10% of polish on feel numbers is
invisible on stream and the time belongs to content. When a value makes
playtesters laugh, snapshot it in the production log and treat it as frozen.
