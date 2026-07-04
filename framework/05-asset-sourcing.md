# 05 — Asset sourcing: free, legal, credited

Assets are free but never "grabbed". Every asset passes license triage and gets
credited at import time. The asset-scout agent owns this doc.

## License policy

| License | Use? | Obligation |
|---|---|---|
| CC0 / public domain | ✅ preferred | Credit anyway (we credit everyone — it's cheap goodwill and streamer-proof) |
| CC-BY 3.0/4.0 | ✅ | Credit exactly as the author specifies, in CREDITS.md + in-game credits screen |
| MIT / Apache / zlib (code, fonts w/ OFL) | ✅ | Keep license text in `ThirdParty/<pack>/LICENSE.txt` |
| CC-BY-SA | ❌ | Share-alike is untenable for a closed-source game |
| CC-BY-NC / ND | ❌ | We sell the game; NC is disqualifying |
| "Free for personal use", unclear, no license stated | ❌ | Never assume. Ask author or walk away |
| GPL (code/shaders) | ❌ | Viral for our use |
| "Royalty-free" marketplace freebies | ⚠️ | Read the actual EULA; many forbid redistribution in raw form — usually fine embedded in a build, verify per source |

**AI filter:** reject any asset that is AI-generated or from an author whose
gallery smells like AI flooding (hundreds of stylistically incoherent uploads,
telltale artifacts, "made with Midjourney/SD" tags). Sketchfab and itch have
AI-content filters — use them. When in doubt, don't ship it. This is a hard
manifesto rule; a CC0 license does not launder AI output.

## Approved source list (start here, in order)

### 3D models & environments
- **Kenney.nl** — CC0, enormous coherent low-poly packs, the friendslop staple.
- **Quaternius** — CC0 stylized low-poly characters/animals/environments.
- **Kay Lousberg (kaylousberg.itch.io)** — CC0 character & dungeon packs.
- **Poly Haven** — CC0 HDRIs, textures, photoscan models (use sparingly — realism
  clashes with stylized).
- **OpenGameArt.org** — mixed licenses, triage carefully.
- **Sketchfab** — filter: downloadable + CC0/CC-BY + exclude AI-generated.
- **itch.io asset section** — filter free + CC0; check each page's license text.

### Textures & materials
- **ambientCG** — CC0 PBR materials.
- **Poly Haven textures** — CC0.
- Prefer flat/gradient/hand-painted-style palettes generated in-house anyway
  (see 06 — one 256×256 palette atlas beats 50 downloaded PBR sets).

### Audio
- **Freesound.org** — filter CC0 first; CC-BY acceptable with credit.
- **Kenney audio packs** — CC0 UI/impacts/jingles.
- **Sonniss GDC bundles** — royalty-free (check per-pack terms), huge SFX libs.
- **Kevin MacLeod / incompetech** — CC-BY music (credit exactly as specified).
- **FreePD.com** — public domain music.
- Skip anything where provenance is murky (YouTube rips, "no copyright music"
  channels).

### Fonts
- **Google Fonts** (OFL) only. Check the OFL applies to the exact weight used.

### Animation
- **Mixamo** — free with Adobe account; license permits game embedding; NOT
  redistributable as raw files (fine — they ship inside the build). Retarget in
  Unity or Blender onto our characters.
- Kenney/Quaternius rigged packs come with animations — prefer these for
  stylized proportions.

## Import workflow (every asset, no exceptions)

1. **Triage:** confirm license on the SOURCE page (not a reuploader). Screenshot
   or save the page URL + license text.
2. **Record:** add a row to `games/<name>/CREDITS.md` immediately:
   `| asset | author | source URL | license | modifications |`
3. **Quarantine:** import raw into `Assets/ThirdParty/<pack>/` with its
   LICENSE.txt.
4. **Process:** tech-artist runs the unification pass (Blender: proportions,
   scale to real-world meters, palette remap, texel density; see 06) and saves
   the result into `_Project/Art/`. Scenes only ever reference processed assets.
5. **Credit surfaces:** CREDITS.md, the in-game credits screen, and the Steam
   store page "about" section footer all list attributions. Ship-check verifies
   the three match.

## If an infringement claim ever arrives (post-launch)

Don't argue, don't investigate ownership yourself, don't ignore it. Protocol:
(1) escalate to the user immediately with the claim + the CREDITS.md row +
the saved license evidence for that asset; (2) default action is REPLACE —
swap the asset for a scratch-build or alternative and patch within days,
even if we believe we're right (a $0-asset fight is never worth a store
strike); (3) the user sends the polite response; (4) log the source as
rejected in CREDITS.md so no future game re-sources it. This is what the
license-evidence paper trail exists for.

## Sourcing strategy notes

- **Pick packs, not pieces.** 90% of a game's art should come from ≤ 3 pack
  families (e.g. Kenney + Quaternius) so cohesion is nearly free. Single
  one-off downloads are where kitbash-look and license risk creep in.
- **Style-match at the silhouette level.** A perfect-license asset in the wrong
  style is rejected; tech-artist can remodel a simple prop in Blender faster
  than they can restyle a mismatched one.
- **Gaps list:** asset-scout maintains a "couldn't source" list per game;
  tech-artist builds those in Blender (they're usually < 10 hero props).
