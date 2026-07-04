---
name: launch-kit
description: Generate the complete launch marketing package for a game at L−14 — streamer target list, personalized outreach drafts, press page, clip cut-list, posting calendar, key allocation plan. Use when a game enters Phase 5 or the user says "prepare the launch" / "marketing kit".
---

# launch-kit — the whole marketing package, one run

Run as `steam-publisher` (with `qa-playtester` supplying playtest-recording
timestamps). Everything here is agent work; the ONLY human steps are sending
from personal accounts and pressing post (framework/13 #24, #28, #29 — this
skill produces their prep). Doctrine: `framework/10-virality.md`.

## Inputs (gather first, refuse to run without them)

- The game's `GAME_DESIGN.md` (logline, THE CLIP), `PRODUCTION_LOG.md`
  (laugh-cluster timestamps from playtest reports), release date, price.
- Playtest recordings list (paths + the qa-playtester timestamp logs).
- Steam page URL (must be live — this skill runs at L−14, after page review).

## Outputs (all into `games/<name>/launch/`)

### 1. `streamer-targets.md` — the researched list
Web-search NOW (never from memory — channels rise/die monthly): 20–40
creators, 50–5,000 CCV, who played ≥ 2 of the reference-class games (check
their recent VODs/videos for PEAK / Chained Together / R.E.P.O. / this
month's friendslop hit). Per target: name, platform, typical CCV, which
friendslop they played + when, best contact route (email > Discord > DM),
group size they usually play with, one-line "why them". Rank by fit, not
size. EXCLUDE: pure-solo streamers (it's a 4-player game), anyone who
monetizes key-begging, channels with no co-op content.

### 2. `outreach/` — one personalized draft per target
Short (< 120 words), from the user's voice, references THEIR content
specifically ("your PEAK ridge episode"), states the one-sentence mechanic,
offers **4 keys** ("bring three friends"), suggests (never requires) the
launch-day embargo, zero marketing-speak. A draft that could be sent to any
streamer unchanged is a failed draft — regenerate it.

### 3. `press-page/` — the public kit
Single self-contained HTML page (host: GitHub Pages, $0): factsheet (players,
session length, price, date, logline), logo/capsule PNGs, 3 screenshots, the
5 curated clips embedded, contact line. Plus `factsheet.txt` for
copy-pasting.

### 4. `clips.md` — the cut-list (agents can't edit video; humans cut in
DaVinci in minutes WITH this)
From the playtest laugh logs: the 5 best moments as `recording file +
in/out timecodes + why it works + which audio to keep`, each ≤ 20 s. Plus 3
vertical (9:16) crop plans for TikTok/Shorts with the visual focus point per
clip. Plus the trailer beat-sheet if the trailer isn't final: cold-open fail
(0–5 s), mechanic in 3 beats, title + date + "Wishlist now" (≤ 60 s total).

### 5. `calendar.md` — who posts what, when
L−5: keys out (grouped per streamer, 4 each — generation instructions for
the Steamworks keys page). L−1: reminder nudge draft. L-hour (state the
optimal hour for the genre's audience, justify it): press release button →
launch posts (drafted, clips attached, one per venue: genre subreddits
by name, X/Bluesky, TikTok/Shorts) → pin the known-issues post. L+2 d:
thank-you post draft quoting the best community clip so far.

### 6. `keys.md` — allocation ledger
Target × 4 keys + 20 spare for verified curator requests + the decline
template for key-beggars (framework/10: unverified requests are resellers).

### 7. `curators.md` — Curator Connect list
20–30 Steam curators (co-op / indie / comedy / party-game lists, active in
the last 90 days — check their pages), ranked, with the one-line pitch to
paste into the Curator Connect message box at L−7. In-platform keys only —
never email curators keys directly.

## Rules

- Every factual claim (CCV, played-X) needs a source link in the file —
  unverifiable targets get cut, not guessed.
- No fake grassroots: nothing in this kit may pretend not to be from the
  developer. Disclosure is charming; astroturf is a store-page death.
- Voice-audio clips require the playtester permissions on file
  (framework/13 #24) — flag any clip whose permission is missing.
- This skill DRAFTS; the human SENDS. Never automate posting/DMs through
  their accounts even if credentials are available — deliverability and
  authenticity both die, and platform ToS forbid it.
