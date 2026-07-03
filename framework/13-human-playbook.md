# 13 — Human playbook: every action only you can do

The factory automates everything it can; this is the complete list of what it
can't. Agents are bound (framework/11 rule 6) to prepare each of these so your
session is minutes of doing, not hours of figuring out — if an agent hands you
a task without the prep listed here, send it back.

Time estimates assume the prep was done. **Bold** items are hard blockers —
the pipeline stalls until you do them.

---

## Part 1 — One-time setup (before game #1)

| # | Action | Time | Notes |
|---|---|---|---|
| 1 | **Create a Steamworks partner account** (partner.steamgames.com): identity, tax interview, bank details | 30 min + days of Valve processing | Do this NOW, before any game needs it — the wait runs in parallel with development. One-time per company/person |
| 2 | **Dev machine install:** Unity Hub + Unity 6 LTS (Windows Build Support IL2CPP + Mono), Blender, Git | 1–2 h mostly downloads | Free tools only. Unity Personal license |
| 3 | Install the free content tools: Krita or GIMP (capsule paintover), Audacity (voice takes), DaVinci Resolve (trailer) | 30 min | Needed from Phase 2 onward, not day one |
| 4 | Claude Code on the dev machine, opened in a workspace containing this repo + (later) the Unity project | 15 min | Cloud sessions work for design/sourcing/Blender phases; engineering needs the local install |
| 5 | Python tooling for the verification scripts: `python3 -m venv .venv && .venv/bin/pip install -r reference/requirements.txt` | 5 min | Python 3.11 (the bpy wheel's version) |
| 6 | Line up your test resources: a second Steam account (family member / free account), ideally a second machine, and 2–3 friends willing to playtest | — | Real-transport tests (every gate) need two accounts on two machines; the funny metric needs 4 humans |
| 6b | Create the STUDIO short-form accounts (TikTok + YouTube + Instagram, same handle) | 30 min | One account per studio, not per game — followers compound across the portfolio (framework/10). Do once, ever |

## Part 2 — Per game, by phase

### Phase 0 — Concept (your time: ~30 min total)

| # | Action | Time | Agent prep you should expect |
|---|---|---|---|
| 7 | **Read the pitch deck and pick/reject** (Gate 0 is YOUR decision) | 20 min | Top pitch in full template + runners-up + novelty/servability/feasibility cross-checks already done |
| 8 | Sanity-check the name | 5 min | Trademark/Steam/itch collision search already run; you're confirming judgment, not researching |

### Phase 1 — Mechanic prototype (your time: ~3–5 h across the week)

| # | Action | Time | Agent prep |
|---|---|---|---|
| 9 | Create the Unity project | 20 min | Follow `reference/unity-project/README.md` step-by-step; agent tells you which steps it already did |
| 10 | **Play feel builds and report verdicts** — the core loop of this phase. One config change per build (framework/11 rule 3): play 2–5 min, answer the agent's specific question ("does landing feel heavy or floaty?") | 10–15 min per iteration, expect 10–20 iterations | Agent names the ONE value changed and the specific question; it logs your verdict in the tuning table |
| 11 | **Run 3+ four-player greybox sessions** (friends, editor builds or local network). RECORD them (OBS or Steam capture) | 3 × 30–45 min | Agent provides the session plan (scenario, what to note); recordings are the deliverable — the agent timestamps the laughs |
| 12 | **Gate 1 verdict: continue or kill** | 10 min | Funny-metric report with numbers + recommendation; you decide |

### Phase 2 — Vertical slice (your time: ~2–3 h)

| # | Action | Time | Agent prep |
|---|---|---|---|
| 13 | **Two-machine Steam transport test** with a friend: full loop + voice chat | 20–30 min | Agent gives a numbered click-path and exactly what to verify (join via friends list, voice audible+spatial, host-quit screen) |
| 14 | Record character vocalization takes | 10–15 min | Agent hands you a numbered list of ~20 noises ("short effort grunt ×3, panicked yelp ×3…"); any mic, quiet room, Audacity, one file per take |
| 15 | Capture slop-check evidence: screenshots from the 3 angles the agent specifies + 60 s gameplay clip | 15 min | Shot list provided; agent does the actual audit |
| 16 | Play the slice loop once end-to-end and confirm the restart feel (< 5 s, one input) | 15 min | — |
| 16b | **Buy the AppId — the $100** — and fill in the store page (Gate 2, yes this early: every live week is wishlist runway for the short-form engine) | 1.5 h | Every word drafted, every asset named to its upload slot, from slice art; you paste, upload, submit for review |
| 16c | Decide demo / Next Fest with the producer's calendar recommendation (framework/09 decision rule) | 10 min | Producer states whether a Next Fest window aligns and what a ≤ 2-week shift would buy |

### Phase 3 — Content (your time: ~2–4 h)

| # | Action | Time | Agent prep |
|---|---|---|---|
| 17 | **60+ minute 4-player session** on near-final content (Gate 3: does novelty last?) + recordings | 90 min | Session plan; note where energy dipped — that timestamp matters more than any opinion |
| 18 | Feel-verdict iterations continue on new content (as #10, fewer rounds) | ~1 h total | — |

### Phase 4 — Polish & harden (your time: ~3–4 h)

| # | Action | Time | Agent prep |
|---|---|---|---|
| 19 | **Execute the manual matrix hands-on items**: controller + KBM swap, alt-tab, resolution changes, Steam overlay/invites, unplug-the-router disconnect tests | 1–2 h | Full checklist from framework/08 as a tick-sheet, ordered to minimize build relaunches |
| 20 | Enable **Steam Playtest** in Steamworks and invite friends | 20 min | Requires AppId (see #22 — buy it by now); agent drafts the invite blurb and feedback form |
| 21 | Confirm performance on your machine: play 10 min with the FPS overlay on, report avg/dips | 15 min | Build + overlay instructions provided; agent does the profiling math |

### Phase 5 — Ship (your time: ~4–6 h spread over 3–4 weeks — calendar-driven!)

| # | Action | Time | Agent prep |
|---|---|---|---|
| 22 | If a demo was decided (#16c): set the demo app live, enroll in Next Fest | 30 min | Demo build cut and verified by agents; you click through the dashboard enrollment |
| 23 | Capsule art finishing pass in Krita/GIMP if the paintover needs a human hand | 1–2 h | Art-director provides the base render + a precise spec (what to paint, where, which palette colors); skip if the pure render works |
| 24 | Ask playtesters for permission to use their voice audio in the trailer | 10 min | Agent drafts the message; keep the yes in writing |
| 25 | **Refresh the store page to final quality** (page has been live since Gate 2): final capsules, trailer, screenshots, price; at L−7 send Curator Connect keys (prepared list) | 1 h | Final assets named to slots; curator list ranked in the launch kit; you upload and click |
| 26 | **Upload the build**: `steamcmd` login (Steam Guard MFA is you), run the prepared script; set it live on the beta branch, click through build review submission | 30 min | Depot/app VDF scripts written; one login + one command |
| 27 | Set price, launch discount, release date in the dashboard | 15 min | Recommendation with reasoning provided; the decision is yours |
| 28 | **Send streamer outreach from your accounts** (L−5): the DMs/emails, 4 keys each | 1–2 h | Target list (20–40 streamers with why-them notes), personalized drafts, keys generated and grouped |
| 29 | **Press the release button** (L-day) + post the launch threads from your accounts | 30 min | Posts drafted with clips attached; agent tells you the optimal hour |

### Post-launch — patch window (your time: ~2 h across 2 weeks, then STOP)

| # | Action | Time | Agent prep |
|---|---|---|---|
| 30 | Approve hotfixes; re-run #26 to upload patch builds (MFA again) | 20 min each, expect 1–3 | Agent triages reviews/discussions and hands you fix-approval decisions with diffs summarized |
| 31 | Pin the "known issues / tiny team" post; reply to nothing else unless you want to | 10 min | Drafted |
| 32 | Read the post-mortem, confirm lessons folded into framework/ docs, declare the game DONE | 20 min | Post-mortem written with the launch numbers; your sign-off archives it |

---

## The recurring five (any phase, whenever asked)

These repeat forever; agents must batch them, never drip them:

- **Post the week's short-form clips** (Gate 1 → launch only): agent delivers
  cut specs + captions from the playtest laugh logs; you cut (or approve the
  pre-cropped file), optionally record a 20 s dev voiceover for Tier A posts,
  and post to the studio accounts (30–60 min/week, framework/10)
- **Play a build, answer one specific feel question** (10 min)
- **Provide evidence**: a screenshot, a log file, a recording, the profiler number (5 min)
- **Be the second machine** for a transport test, or recruit one (20 min)
- **Decide**: kill/continue, spend/don't, extend/cut — always delivered with a recommendation and one paragraph of reasoning
- **Be the taste**: if something looks off, sounds off, or isn't funny — say so plainly; your "this feels wrong" outranks every checklist

## What you should NEVER be asked to do

Research a decision the agent could have made, debug something without a
prepared repro, write store copy from scratch, figure out Steamworks
navigation unaided, or execute anything without a numbered list. Those are
agent failures — point the agent at this doc and framework/11 rule 6.
