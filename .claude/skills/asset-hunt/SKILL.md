---
name: asset-hunt
description: Source free, license-clear, human-made assets for a game need (models, textures, audio, fonts, animation) and update CREDITS.md. Use when the user or an agent needs assets found, license-checked, or credited.
---

# asset-hunt — find it, clear it, credit it

Delegate the search itself to the `asset-scout` agent; this skill defines the
end-to-end procedure and what "done" means.

## Inputs (gather before searching)

- The game's style bible (`games/<name>/GAME_DESIGN.md` § Art Style) — you
  are matching palette + shape language, not keywords.
- The specific needs list (from the design doc, a level plan, or the gaps in
  the current sourcing report).

## Procedure

1. **Search approved sources first**, in order (full list + license table in
   `framework/05-asset-sourcing.md`): Kenney, Quaternius, Kay Lousberg for 3D;
   ambientCG/Poly Haven for textures; Freesound (CC0 filter), Kenney audio,
   Sonniss, incompetech/FreePD for audio; Google Fonts (OFL) for fonts;
   Mixamo/pack rigs for animation. Prefer whole packs over single pieces —
   ≤ 3 pack families should cover 90% of a game.
2. **Verify per candidate, on the source page** (WebFetch the actual page):
   exact license text, author, download link. Site badges lie; reuploads lie
   more — trace to the original author's page.
3. **Apply the two filters:**
   - License: CC0 ✅ / CC-BY ✅ (capture the author's requested attribution
     string) / MIT-OFL ✅ (keep license file) / SA, NC, ND, GPL, unstated ❌.
   - AI: reject AI-generated or AI-suspect work regardless of license
     (flooded incoherent galleries, artifact tells, AI tags; use platform AI
     filters). When unsure, reject — there's always another CC0 pack.
4. **Record immediately** in `games/<name>/CREDITS.md`:
   `| asset/pack | author | source URL | license | modifications |`
   (one row per pack is fine; list notable pieces). CREDITS.md lags reality =
   ship-check failure later.
5. **Report:** chosen assets with download instructions + why they fit the
   bible; rejected near-misses with reasons; **gaps list** (unservable needs)
   handed to `tech-artist` for Blender scratch-builds. Never fill a gap with
   an off-style or license-risky asset.
6. Remind the importer of the quarantine rule: raw files go to
   `Assets/ThirdParty/<pack>/` with LICENSE.txt; only tech-artist's processed
   versions enter `_Project/Art/` and scenes.

## Escalate

License ambiguity → user (legal call). Perfect-but-wrong-license hero asset →
draft a permission-request email for the user to send; do not use it while
waiting.
