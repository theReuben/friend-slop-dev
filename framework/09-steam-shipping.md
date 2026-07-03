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

| When | What |
|---|---|
| L−30 | AppId purchased; store page drafted; capsule art done |
| L−21 | **Store page submitted for review** (2–5 business days review + Steam requires the page live ("Coming Soon") ≥ 2 weeks before launch) |
| L−14 | Page live; Steam Playtest opened; streamer kit ready (10-virality.md) |
| L−7 | Release candidate build uploaded; **build review** requested (1–5 days); price + launch discount set |
| L−2 | Build approved; release date locked; trailer final |
| L | User presses release. Launch discount active (10–20%) |

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

## Post-launch window (2 weeks, then stop — manifesto rule)

- Days 1–3: watch reviews + discussions twice daily; hotfix S1s same-day.
- Week 2: one patch bundling S2 fixes + trivial popular requests.
- Pin a discussion post: known issues, "small team, game is complete" framing —
  honest expectation-setting protects the review score after we leave.
- Write the post-mortem, update `framework/` docs, archive.
