# 06 — Art direction: the anti-slop bible

The physics can be janky. The art direction cannot. This doc defines how a game
made of free assets ends up looking authored. The art-director agent enforces
it; the `slop-check` skill audits against it.

## The two failure modes we're preventing

1. **AI slop** — obviously generated content: incoherent detail, mushy shapes,
   wrong-count fingers, sourceless "concept art" vibes, style drift between
   assets, uncanny store capsules. Our fix: we simply never use AI-generated
   assets (manifesto rule), so the only risk is accidentally importing one —
   asset-scout's AI filter (05) is the gate.
2. **Kitbash slop** — the free-asset look: mixed texel densities, one PBR
   photoscan rock next to a flat-shaded low-poly tree, default Unity skybox,
   default font, untouched lighting. This is the real daily enemy and 90% of
   this doc.

## Per-game style bible (art-director writes at Gate 0, ~1 page)

Must define, concretely:

- **Palette:** exactly 12–20 named colors (hex), typically as one small palette
  atlas texture that most meshes UV into. Include: 1 sky/fog family, 2–3
  environment families, 1 high-saturation player/interactive accent family.
  Interactive = saturated is a readability law, not a suggestion — and the
  accent must survive a deuteranopia/protanopia simulation (free sim tools /
  Krita filters): differ from the environment in LUMINANCE, not hue alone.
  ~8% of the male-skewed audience is colorblind; an accent that vanishes for
  them fails the readability law, slop-check verifies it.
- **Shape language:** one sentence (e.g. "chunky, rounded, slightly inflated —
  nothing has a sharp corner"). Every sourced asset is measured against it.
- **Proportion rule** for characters (e.g. 3-heads-tall, big hands — big hands
  read grabbing on stream).
- **Rendering recipe:** flat-shaded / toon-ramp / soft-gradient — pick ONE.
  Specify the URP setup: shader (flat lit or a single toon shader), fog color +
  distance, one post volume (see recipe below).
- **3 reference images** from human-made games (e.g. *A Short Hike*, *Untitled
  Goose Game*, *Human Fall Flat*, *Astroneer*, *Grow Home*) — references define
  the target, we never copy assets or trade dress.

## The unification pass (tech-artist, in Blender — what makes it "authored")

Every third-party asset, before it enters `_Project/Art/`:

1. Scale to real-world meters; re-origin sensibly.
2. **Palette remap:** delete source textures/materials, UV onto the game's
   palette atlas (or vertex colors mapped to palette). This single step erases
   "from 5 different packs" instantly.
3. Silhouette edit toward the shape language (Blender: 5 minutes of pushing
   verts — inflate, round, exaggerate). Decimate/remesh to the tri budget.
4. Normals: shade-flat or shade-smooth per the rendering recipe, consistently.
5. Export FBX with our naming: `prop_<name>`, `char_<name>`, `env_<name>`.

Hero props the packs don't cover get modeled from scratch in Blender — at
low-poly + palette-atlas quality this is 15–60 minutes per prop, and original
hero props are the strongest "human-made" signal in screenshots.

## The 10 slop tells (slop-check hunts these; all must be absent)

1. Default Unity skybox, default fog, or no fog.
2. Default font (LiberationSans/Arial) anywhere, including debug UI.
3. Two adjacent assets with obviously different texel density or art style.
4. Untouched photoscan/PBR asset inside a stylized scene.
5. Default lit-gray material on anything visible (greybox leftovers).
6. Lighting: no baked/authored lighting — flat ambient everything.
7. UI: unstyled Unity default buttons/sliders.
8. Terrain with visible tiling or default grass billboards.
9. Screenshot where nothing has the accent color (no focal hierarchy).
10. Any asset a reasonable person would guess is AI-generated.

## Cheap moves with outsized "authored" payoff (do all of these)

- **One post-processing volume:** subtle color grading toward the palette,
  vignette 0.2, slight contrast lift. 30 minutes, transforms screenshots.
- **Authored sky:** gradient skybox shader in palette colors + fog matched to
  horizon. Never a photo sky.
- **Face on everything playable:** simple eyes/blink on characters (two quads
  and a blink coroutine) — instant charm, instant streamability.
- **Squash & stretch:** scale-punch on jumps/impacts/grabs (DOTween-free, a
  10-line script). Motion charm hides asset simplicity.
- **Custom cursor, custom font (one display + one text font from Google Fonts),
  UI palette = game palette.**
- **Title screen is a diorama:** the actual game scene with characters posed,
  not a static image. Doubles as capsule-art source.

## Capsule & store art (human-made, mandatory process)

- Base: high-res in-engine screenshot of a posed diorama (characters mid-disaster,
  accent-lit) → paintover/cleanup in Krita/GIMP (art-director) → logotype set in
  the game's display font with hand-adjusted kerning/outline.
- Text readable at 120 px wide (test it — capsules are seen tiny in discovery
  queues). Faces + motion in frame outperform landscapes.
- Absolutely no AI upscaling/generation/inpainting at any step; Steam's AI
  disclosure for the game must truthfully be "none".

## Review cadence

- Gate 2: full slop-check on the vertical slice (screenshots from 3 angles +
  UI). Fix before content phase.
- Gate 4: slop-check on every scene + menus + a 60 s gameplay capture.
- Gate 5: slop-check on capsule, screenshots, trailer stills.
