---
name: slop-check
description: Audit a game's visuals against the anti-slop bible — style cohesion, the 10 slop tells, AI-content firewall. Use at Gates 2/4/5, when reviewing screenshots or scenes, or when the user asks "does this look like AI slop / asset-flip?".
---

# slop-check — the anti-slop audit

Run as the `art-director` agent. Verdict is per-item pass/fail with evidence,
never a vibe. A failed slop-check blocks its gate.

## Inputs

- The game's style bible (`games/<name>/GAME_DESIGN.md` § Art Style).
- Evidence to audit: screenshots from ≥ 3 gameplay angles at 1080p, every UI
  screen, and (Gate 4+) a gameplay capture; (Gate 5) capsule images, store
  screenshots, trailer stills. If evidence is missing, request it — do not
  audit from memory or from scene-file reading alone when images are
  obtainable.

## Checklist A — the 10 slop tells (all must be ABSENT)

1. Default Unity skybox, default fog, or no fog.
2. Default font anywhere (LiberationSans/Arial), including debug UI.
3. Adjacent assets with mismatched texel density or art style.
4. Untouched photoscan/PBR asset in a stylized scene.
5. Default lit-gray material visible (greybox leftovers).
6. No authored lighting — flat ambient everything.
7. Unstyled default UI controls.
8. Terrain tiling or default grass billboards.
9. A screenshot with no accent-color focal point.
10. Anything a reasonable person would guess is AI-generated.

## Checklist B — bible conformance

- [ ] Every visible color resolves to the named palette (spot-check pixels).
- [ ] Shape language: sample 10 random props — do they obey the one-sentence
      rule?
- [ ] One rendering recipe throughout (no mixed flat/toon/PBR).
- [ ] Interactables carry the saturated accent family; nothing else does.
- [ ] The six cheap authored-ness moves are present: post volume, gradient
      sky + matched fog, character faces/blinks, squash-stretch, custom
      font/cursor/UI palette, diorama title screen.

## Checklist C — provenance firewall

- [ ] Cross-reference `Assets/ThirdParty/` folders and `_Project/Art/` files
      against `CREDITS.md` — every source represented, no orphan assets.
- [ ] No scene references directly into `ThirdParty/` (processed assets only).
- [ ] Re-screen credited sources for AI suspicion (asset-scout's filter,
      second pass).
- [ ] Gate 5 only: capsule/trailer/screenshots contain zero AI-touched pixels
      (including upscaling/inpainting); Steam AI disclosure "none" is true.

## Checklist D — stream readability

- [ ] Downscale a gameplay screenshot to 640×360: goal object and players
      still identifiable.
- [ ] A random mid-action frame reads as an intentional composition (focal
      hierarchy exists).
- [ ] Colorblind pass: under a deuteranopia simulation (or grayscale as the
      cheap proxy), interactables still separate from environment by
      luminance. Accent that survives only by hue = FAIL (framework/06).

## Output

A report in the production log: per-item ✅/❌, each ❌ with the named rule,
the offending asset/shot, and the cheapest fix. Overall verdict PASS / FAIL.
FAIL at a gate returns the work to tech-artist/art-director before the phase
can close.
