# BIG FLOAT — Example agent reports (WORKED EXAMPLE)

The three deliverable formats lower models most often get wrong, shown at
target quality: evidence-first, per-item verdicts, no vague approval.

---

## 1. Funny-metric report (qa-playtester, Gate 1 — 2026-06-08)

**Build:** greybox #7 · **Sessions:** 3 × 4 players (dev accounts, local transport)
**Total playtime:** 62 min · **Verdict: PASS** (9 mechanic-caused laughs ≥ 1/10 min bar)

| # | t | What happened | Cause bucket |
|---|---|---|---|
| 1 | 04:12 | P3 lifted during gust-7, P1 grabbed P3's legs, both lifted | lift |
| 2 | 07:40 | anchor clip snapped, P2 through fence, balloon yo-yo'd | anchor snap |
| 3 | 11:05 | team walked balloon into the ONE lamppost while arguing about lampposts | group-drag |
| 4–9 | … | *(full log in playtests/2026-06-08.md)* | lifts ×3, drag ×2, snap ×1 |

**Cause distribution:** lifts 4 / group-drag 3 / anchor snap 2 — three distinct
comedy sources, not a one-liner mechanic. **Feel checklist:** restart 3.8 s ✅ ·
new-player verb < 60 s ✅ (P4 reeled without prompt at 0:40) · zero rage-quits ✅ ·
2 postable clips ✅ · spectator narrated stakes correctly ✅ · max idle 22 s ✅.
**Concern:** solo triple-harness is joyless (by design, but verify it can't
soft-lock — added matrix case).

---

## 2. Slop-check report (art-director, Gate 2 slice — excerpt)

**Evidence:** 6 screenshots (3 angles × 2 lighting), menu UI, 45 s capture.
**Verdict: FAIL — 2 of 24 items.** Return to tech-artist, re-check ≤ 1 day.

| Check | Verdict | Detail |
|---|---|---|
| A1 default sky/fog | ✅ | GradientSkybox + matched fog |
| A2 default font | ❌ | **debug FPS counter uses LiberationSans** — swap to Inter or strip from build |
| A3 texel density mismatch | ✅ | unify batches 1–2 consistent |
| A5 lit-gray material | ❌ | **market stall awning still greybox-gray** (shot 4, left) — palette cell 11 |
| A9 accent focal point | ✅ | ropes carry accent_hot in all 6 shots |
| B interactables saturated | ✅ | ropes/anchors only — nothing else uses accent family |
| C credits three-way | ✅ | ThirdParty/ ↔ CREDITS.md diff clean |
| D 360p readability | ✅ | balloon + ropes identifiable at 640×360 |
| *(remaining 15 items)* | ✅ | *(named individually in the real report)* |

**Cheapest fixes:** A2 = strip debug UI from non-DEV builds (5 min). A5 =
awning is unify batch 3, already scheduled — verify shot 4 after.

---

## 3. Sourcing report (asset-scout, Gate 0 servability check — excerpt)

**Style bible:** round/festive flat-shaded, 16-color palette.
**Verdict: SERVABLE** — 2 pack families + 3 scratch-builds.

- **Kenney City Kit Suburban + Commercial** (CC0, kenney.nl): covers ~85% of
  environment needs (houses, streets, props). License verified on source
  page; CC0 corroborated by kenney.nl/support. Silhouettes already round —
  low unify cost.
- **Kenney Furniture Kit** (CC0): market stalls, depot interior.
- **Gaps (→ tech-artist):** hero balloon, bunting, popcorn machine. 3 items,
  under the ~10 gap budget.
- **Rejected:** see CREDITS.md rejected table (reuploader pack, AI-suspect
  Sketchfab balloon, NC crowd loop) — reasons recorded so nobody re-sources.
- **Note for future scouts:** kenney.nl 403s automated fetchers — verify via
  the site in a browser (human) or corroborating sources; record which you used.
