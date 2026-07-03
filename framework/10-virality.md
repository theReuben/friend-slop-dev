# 10 — Virality: built in, not bolted on

We spend $0 on marketing. The game itself, the store page, and a streamer kit
are the marketing. Virality can't be guaranteed — it CAN be made cheap to
attempt and impossible to achieve if these basics are missed.

## Design-time virality (enforced at Gates 0–1, cheap; unfixable later)

- **The clip exists by design:** the pitch's "THE CLIP" line is a real, reliably
  reproducible 20-second moment. Gate 1 verifies it happens organically.
- **Readable at 360p:** big silhouettes, accent-colored interactables, huge
  fail feedback. Streams are watched on phones.
- **The name is a searchable phrase** people can say aloud on stream without
  ambiguity (unique-ish spelling helps SEO/Twitch directory; avoid generic
  two-word names that collide with 50 other games).
- **Session shape fits streams:** a full arc in 30–90 min; restart < 5 s keeps
  the VOD dead-air-free.
- **Spectator moments:** corpse-cam, kill-cam replays of big fails, end-of-run
  stat cards ("Dave caused 7 deaths") — stat cards get screenshotted and
  posted, that's free UGC.

## Ship-time features (Phase 4–5, each ≤ a day of work)

- **End-of-run blame card:** superlatives per player (Most Falls, Worst Driver,
  Team Anchor). The single highest ROI virality feature in the genre.
- Photo mode lite: freecam + hide-UI key (streamers make thumbnails with it).
- Steam rich presence + "invite friends" button front-and-center in lobby.
- Default open-mic proximity voice (with easy opt-out) — the comedy transmits
  by default.

## The short-form engine (Reels / TikTok / Shorts — runs Gate 1 → launch)

Short-form is this genre's native discovery channel: a friendslop game IS a
20-second clip, and titles in our reference class were carried to launch by
TikTok before Steam's algorithm ever noticed them. But it pays out for
CONSISTENCY, not for one launch-day dump — so it runs as a program from the
moment the greybox is funny (Gate 1) until launch, then stops (ship and
forget applies to content channels too).

**The engine (agents do all prep; human posts):**

1. **Source material is free:** every playtest is already recorded and every
   laugh already timestamped (framework/08). qa-playtester's laugh log IS the
   content calendar — steam-publisher mines it weekly into clip specs
   (file + timecodes + 9:16 crop focus + hook line + caption + hashtags).
2. **Cadence: 2–3 posts/week from Gate 1.** Below that, the algorithm forgets
   you; above that, it eats the dev schedule. Human cost ≈ 30–60 min/week
   (framework/13).
3. **One account per STUDIO, not per game.** Followers compound across the
   portfolio — the audience from game #1's clips is game #2's launch pad.
   This is the strongest cross-game asset the factory builds.
4. **Two content tiers, use both:**
   - **Tier A — dev-voice ("I added X to my game and my friends found a way
     to ruin it"):** 15–30 s, greybox footage is FINE (jank reads as charm
     mid-development), human records one voiceover take. Highest wishlist
     conversion format in indie games; 2–4× the performance of raw clips.
   - **Tier B — faceless chaos clip:** the fail, the scream, one caption
     line. Near-zero human cost, keeps cadence between Tier A posts.
5. **Anatomy of a post:** motion in frame 1 (no logo cards, no intros), the
   fail arc completes by 0:20, real playtest voice audio (permissions on
   file), caption asks a question or blames a player ("Dave was told about
   the bridge"), wishlist CTA in caption/comment only — on-screen CTAs kill
   reach. Cross-post the identical vertical file to TikTok + Shorts + Reels;
   platform-native re-edits are not worth the time (TikTok/Shorts will
   outperform Reels for games — post to all three anyway, it's the same file).
6. **Measure wishlists, not views.** Steam's UTM links per platform in bio.
   A 500-view clip that converts beats a 100k banger that doesn't; log
   weekly numbers in the production log and let the next game's pitch cite
   them.

**Guardrails:** the clip test (framework/02) already forces reel-able
mechanics at design time — never invert that by adding features FOR the
feed that don't serve the game (that's scope creep with a ring light).
No trend-chasing formats that hide gameplay; gameplay is the asset. No paid
boosting. Same no-astroturf rule as everything else: posts are proudly from
the dev.

## The streamer kit (steam-publisher builds at L−14 — automated end-to-end
by the `launch-kit` skill; the human only sends and posts)

A public press-kit page (free: GitHub Pages or itch devlog) containing:
- 5 curated playtest clips (the funniest fails, with audio), vertical crops too.
- Logo/capsule PNGs on transparent, 3 screenshots, factsheet (players, session
  length, price, release date, one-line pitch).
- **Curator/creator key policy:** we grant keys ONLY via Keymailer/Woovit-style
  verified requests or direct DMs to hand-picked streamers — never to key-beggar
  emails (they're 99% resellers).

## Launch push ($0, one day of producer time)

- Hand-pick 20–40 small-to-mid variety/co-op streamers (50–5,000 CCV) who
  played the reference games; short personal DM/email + 4 keys each ("it's a
  4-player game — bring 3 friends" — this matters, one key gets one bored
  streamer, four keys get a party).
- Post launch-day threads: r/WebGames-adjacent co-op subreddits, the genre's
  subreddit, X/Bluesky with the best clip attached, TikTok/Shorts vertical cuts
  of the 3 best fails.
- Steam visibility: launch discount + "New & Trending" needs day-one velocity —
  time the streamer keys to go out 3–5 days pre-launch with a launch-day
  embargo suggestion (not requirement).

## What we do NOT do

- Paid ads, paid influencer deals, PR agencies, convention booths.
- Discord community server promises (moderation = forever-support; a Steam
  discussions pin is our only official channel).
- Fake hype: no botted wishlists, no astroturfed posts, no review begging
  in-game (a single polite "enjoying it? a review helps a tiny team" on the
  exit screen after 2+ hours played is acceptable and standard).

## Measuring (so the next game learns)

Log in the post-mortem: wishlists at launch, week-1 units, median review
sentiment themes, which streamers played it and what clip spread. The next
game's pitch must cite these numbers when claiming its clip will do better.
