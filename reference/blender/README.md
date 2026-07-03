# reference/blender — the headless unification pipeline

These scripts implement the anti-kitbash pass from `framework/06-art-direction.md`:
one palette atlas per game, every asset remapped onto it, so five packs read as
one authored style. They run fully headless.

✅ **Verified end-to-end on Blender 5.0.1** (headless, in a cloud container):
multi-material test asset → nearest-palette UV routing (exact cell centers,
correct color→cell mapping) → single M_Palette material → decimation → flat
shading surviving the FBX round-trip. Run `selftest.py` to re-prove the
pipeline on YOUR Blender version before batching real assets — it exits
nonzero with named failures if the API drifted.

**No Blender install required**: the standalone wheel works anywhere —
`pip install bpy` (needs the Python version the wheel targets; 3.11 for
bpy 4.x/5.x) then `python selftest.py`. This means the tech-artist agent can
run the entire unification pipeline in a cloud session; only the silhouette
hand-edits need Blender's UI on a real machine.

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
