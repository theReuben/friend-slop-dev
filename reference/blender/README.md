# reference/blender — the headless unification pipeline

These scripts implement the anti-kitbash pass from `framework/06-art-direction.md`:
one palette atlas per game, every asset remapped onto it, so five packs read as
one authored style. They run headless — no Blender UI needed — which means the
tech-artist agent can execute them on any machine with Blender installed (4.x).

⚠ Written without a Blender install to test against (this repo was authored in
a cloud session). The bpy API here targets Blender 4.x and the logic is sound,
but expect to fix small API drift on first run — run on ONE test asset first,
open the result in Blender/Unity, verify scale/orientation/UVs, THEN batch.

## 1. Make the game's palette atlas (once per game, at Gate 0)

Edit the `PALETTE` list in `make_palette_atlas.py` to the style bible's named
hex colors, then:

```
blender --background --python make_palette_atlas.py -- --out /path/to/game/palette.png
```

Produces a small PNG grid (one flat-color cell per palette entry). Import into
Unity with Point filtering, no compression, no mipmaps.

## 2. Unify assets (every third-party import, per framework/05 workflow)

```
blender --background --python unify_pass.py -- \
    --in  /path/to/ThirdParty/kenney_citypack/Models \
    --out /path/to/staging/processed \
    --palette /path/to/game/palette.png \
    --scale 1.0 --decimate 0.8 --shading flat
```

Per input file (.fbx/.glb/.obj) it: imports → applies scale/transforms → maps
every face to the nearest palette color's UV cell (by the source material's
base color) → strips source materials for a single palette material → optional
decimate → flat/smooth shading → exports FBX with Unity-friendly axes.

After the batch, the tech-artist still does the HUMAN part in Blender's UI:
silhouette edits toward the shape language (inflate, round, exaggerate). The
script does the mechanical 80%; the 20% of hand-editing is what makes it art.

## Palette cell convention

Cell (i) of N occupies the UV square `[i%cols/cols .. ] × [i//cols/cols .. ]`;
faces are UV'd to the CENTER of their cell so texture filtering never bleeds
neighbors. Same convention in `make_palette_atlas.py` and `unify_pass.py` —
change one, change both.
