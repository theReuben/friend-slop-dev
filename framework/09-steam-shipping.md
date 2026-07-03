# 09 — Steam shipping: the only money we spend

The steam-publisher agent owns this. Everything here is checklist-shaped
because Steam's process punishes improvisation with review delays.

## Money & accounts

- **Steam Direct fee: $100 per app** (recoupable after $1,000 revenue). This is
  the game's entire cash budget. User pays it and owns the Steamworks account;
  agents prepare everything else.
- Company/tax/bank onboarding is one-time per account, takes days — verify it's
  done before the first game needs it.

## Timeline (work backwards from launch day L)

**The page goes live as EARLY as possible, not at the deadline.** The
short-form engine (10-virality.md) starts converting viewers at Gate 1 —
every week without a live "Coming Soon" page is wishlist runway burned.
2 weeks pre-launch is Steam's minimum, not our target.

| When | What |
|---|---|
| Gate 2 (≈ week 4) | **AppId purchased; store page drafted and submitted** — slice art provides capsule + screenshots (they can be refreshed later) |
| Gate 2 + review time | **Page LIVE ("Coming Soon")** — short-form UTM links now have a destination; wishlists accumulate for the whole back half of production |
| L−30 | Demo decision executed if taken (see below); capsule art final |
| L−14 | Steam Playtest opened; streamer kit ready (`launch-kit` skill) |
| L−7 | Release candidate uploaded; **build review** requested (1–5 days); price + launch discount set; Curator Connect keys out |
| L−2 | Build approved; release date locked; trailer final |
| L | User presses release. Launch discount active (10–20%) |

## Demo & Steam Next Fest (the biggest free lever we can point at)

Steam **Next Fest** (three editions/year: Feb, June, Oct) is the single
largest free wishlist event available to an indie — and a friendslop demo is
cheap to cut: the first leg/biome, 20–30 min, fully multiplayer (a co-op demo
that friends play together converts the whole group).

- **Decision rule (producer + user, at Gate 0 when the calendar is set):**
  if a Next Fest window lands in L−90..L−14, take it — shifting launch by
  ≤ 2 weeks to align is a worthwhile trade and the ONE sanctioned reason to
  adjust the time-box calendar. If nothing aligns, skip; never delay a
  finished game a month+ for a festival.
- Demo mechanics: separate free demo app (no extra fee), content-capped, ends
  on the wishlist screen. Cut it from the Gate 3 build in ≤ 2 days — a demo
  needing more work than that is a scope failure.
- One Next Fest per app, pre-launch only, page must be live well before —
  another reason the page goes up at Gate 2.
- Leave the demo up post-launch (it keeps converting; zero maintenance).

## Free platform levers (all near-zero labor — take them)

- **Curator Connect:** at L−7 send keys through the dashboard to 20–30
  relevant curators (co-op/indie/comedy lists; `launch-kit` prepares the
  list). Free, in-platform, no key-reseller risk.
- **Seasonal sales:** opt into Steam's auto-enrollment for seasonal events at
  a standard 10–20% discount post-launch. Zero labor, recurring visibility.
- **Franchise/creator page** once game #2 exists (see 10-virality.md
  § portfolio flywheel).

## Store page checklist

- [ ] Name: unique, readable at capsule size, no trademark collisions
      (search USPTO/EUIPO + Steam + itch before Gate 0 locks the name).
- [ ] Capsules: small 462×174, header 460×215, main 616×353, vertical 748×896,
      library 600×900 + hero/logo. All from the human-made pipeline (06).
- [ ] 5+ screenshots, ALL actual gameplay with UI as players see it (Steam
      rules), chosen for mid-disaster comedy, 3840×2160 downscaled.
- [ ] Trailer: ≤ 60 s, first 5 s = the funniest fail, real gameplay + real
      playtest voice audio (get playtester permission), no logos intro, ends on
      title + "Wishlist now". Cut in DaVinci Resolve (free).
- [ ] Short description: the logline, ≤ 300 chars, front-loads the mechanic.
- [ ] Tags (all 20): lead with Online Co-Op, Multiplayer, Funny, Physics,
      Casual + theme tags. Tags drive discovery queues — copy the tag sets of
      the 3 Steam-neighbor games from the pitch.
- [ ] **AI disclosure: "No AI generated content"** — must be true, see 06.
- [ ] Attribution footer in the About section (per 05).
- [ ] Specs honest: min = GTX 1060-class (we tested it).
- [ ] Steam Deck: don't block it; test the Deck input layout; Playable-verified
      is a nice-to-have, not a gate.

## Build & tech checklist

- [ ] Windows x64 build (Windows-only is fine; add Linux only if it's free —
      Proton usually covers it. macOS is not worth notarization pain).
- [ ] Steamworks: correct AppId (never 480), depot per platform, launch options
      set; `steam_appid.txt` NOT in shipped depot.
- [ ] Achievements: 8–15, mostly comedy-flavored ("Fall 100 m holding a friend"),
      each tested to fire exactly once.
- [ ] Steam Input: default configs for pad + Deck; overlay works (it's needed
      for invites!).
- [ ] Rich presence strings ("In a lobby — join!") — free virality inside
      friends lists.
- [ ] Settings: resolution/fullscreen, 3 volume sliders, voice mode
      (proximity/all/push-to-talk), mute-player, invert-Y, sensitivity.
- [ ] Version string on the main menu (support sanity when we're gone).
- [ ] Build defines: `DEV_BUILD` off; local-transport toggle stripped; logs quiet.
- [ ] Content review: no licensed music leakage, no trademark props, credits
      screen matches CREDITS.md; age-survey answered honestly (cartoon violence).

## Pricing

$4.99 default; $6.99 if content clears 8+ hours of group replayability; never
higher (the 4-pack impulse math is the business model). Launch discount 10–20%.
No regional price overrides beyond Steam's auto-suggestions. Never free —
free attracts a support burden that violates ship-and-forget.

## If it blows up (the good emergency)

Ship-and-forget is a floor, not a cage. If a game finds real traction
(sustained top-seller placement, six-figure units), the USER may extend the
patch window — but three rules hold even then: never publicly promise a
roadmap (silence preserves options; promises create the support burden we
exist to avoid), price stays put (no greedy mid-spike increases; goodwill IS
the marketing), and a sequel/expansion is a NEW game through Phase 0 like any
other — success does not exempt it from the novelty check against its own
predecessor. Store-page-only localization (short description in top store
languages) is the one cheap post-hoc growth lever worth taking; in-game stays
English/near-zero-text per framework/02.

## Post-launch window (2 weeks, then stop — manifesto rule)

- Days 1–3: watch reviews + discussions twice daily; hotfix S1s same-day.
- Week 2: one patch bundling S2 fixes + trivial popular requests.
- Pin a discussion post: known issues, "small team, game is complete" framing —
  honest expectation-setting protects the review score after we leave.
- Write the post-mortem, update `framework/` docs, archive.
