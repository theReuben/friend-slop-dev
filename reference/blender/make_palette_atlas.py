"""Generate the game's palette atlas PNG from the style bible's hex colors.

Usage:
    blender --background --python make_palette_atlas.py -- --out palette.png

Edit PALETTE below per game (12-20 named colors from the style bible,
framework/06). Order matters: unify_pass.py refers to colors by cell index.
"""
import argparse
import math
import sys

import bpy

# ---- EDIT PER GAME: (name, hex) from the style bible -----------------------
PALETTE = [
    ("sky_top",       "5B8BD9"), ("sky_horizon",   "D9CCB3"),
    ("ground_main",   "8A9A5B"), ("ground_dark",   "5C6B3C"),
    ("rock",          "9C948A"), ("rock_shadow",   "6E6862"),
    ("wood",          "A9744F"), ("wood_dark",     "7A5236"),
    ("foliage",       "6FA352"), ("foliage_dark",  "4C7A38"),
    ("accent_hot",    "FF5C38"),  # interactables ONLY — the readability law
    ("accent_soft",   "FFB03A"),
    ("player_1",      "E84855"), ("player_2",      "3185FC"),
    ("player_3",      "EFBF3E"), ("player_4",      "56E39F"),
]
CELL_PX = 32  # each color is a CELL_PX x CELL_PX block


def hex_to_rgb(h):
    return tuple(int(h[i:i + 2], 16) / 255.0 for i in (0, 2, 4))


def main():
    argv = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    parser = argparse.ArgumentParser()
    parser.add_argument("--out", required=True)
    args = parser.parse_args(argv)

    cols = math.ceil(math.sqrt(len(PALETTE)))
    rows = math.ceil(len(PALETTE) / cols)
    width, height = cols * CELL_PX, rows * CELL_PX

    img = bpy.data.images.new("palette", width=width, height=height, alpha=False)
    pixels = [0.0] * (width * height * 4)

    for idx, (_, hexcode) in enumerate(PALETTE):
        r, g, b = hex_to_rgb(hexcode)
        cx, cy = (idx % cols) * CELL_PX, (idx // cols) * CELL_PX
        for y in range(cy, cy + CELL_PX):
            for x in range(cx, cx + CELL_PX):
                o = (y * width + x) * 4
                pixels[o:o + 4] = (r, g, b, 1.0)

    img.pixels[:] = pixels
    img.filepath_raw = args.out
    img.file_format = "PNG"
    img.save()

    print(f"palette atlas: {args.out} ({cols}x{rows} cells, {len(PALETTE)} colors)")
    for idx, (name, hexcode) in enumerate(PALETTE):
        print(f"  cell {idx:2d}: {name} #{hexcode}")


main()
