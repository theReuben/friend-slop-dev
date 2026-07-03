"""The unification pass (framework/06): batch-convert third-party assets onto
the game's palette atlas so assets from different packs read as one style.

Usage:
    blender --background --python unify_pass.py -- \
        --in ThirdParty/pack/Models --out staging/processed \
        --palette palette.png [--scale 1.0] [--decimate 0.8] [--shading flat]

Per mesh file: import -> apply scale/transforms -> UV every face to the
nearest palette cell (matched by the source material's base color) -> replace
all materials with one palette material -> optional decimate -> shading ->
export FBX (Unity axes). Cell layout must match make_palette_atlas.py.

Silhouette edits (the human 20%) still happen in the UI afterwards.
"""
import argparse
import math
import pathlib
import sys

import bpy

# Must mirror make_palette_atlas.py exactly (order + hex).
from make_palette_atlas import PALETTE, hex_to_rgb  # keep both scripts in one dir

IMPORTERS = {
    ".fbx": lambda p: bpy.ops.import_scene.fbx(filepath=p),
    ".glb": lambda p: bpy.ops.import_scene.gltf(filepath=p),
    ".gltf": lambda p: bpy.ops.import_scene.gltf(filepath=p),
    ".obj": lambda p: bpy.ops.wm.obj_import(filepath=p),
}


def material_base_color(mat):
    """Best-effort base color of a source material (nodes, then viewport).
    Reads node_tree directly — Material.use_nodes is deprecated in Blender 5,
    removed in 6."""
    if mat and getattr(mat, "node_tree", None):
        for node in mat.node_tree.nodes:
            if node.type == "BSDF_PRINCIPLED":
                c = node.inputs["Base Color"].default_value
                return (c[0], c[1], c[2])
    if mat:
        c = mat.diffuse_color
        return (c[0], c[1], c[2])
    return (0.5, 0.5, 0.5)


def nearest_cell(rgb):
    best, best_d = 0, 1e9
    for idx, (_, hexcode) in enumerate(PALETTE):
        pr, pg, pb = hex_to_rgb(hexcode)
        d = (rgb[0] - pr) ** 2 + (rgb[1] - pg) ** 2 + (rgb[2] - pb) ** 2
        if d < best_d:
            best, best_d = idx, d
    return best


def cell_center_uv(idx, cols):
    return ((idx % cols + 0.5) / cols, (idx // cols + 0.5) / cols)


def make_palette_material(palette_path):
    mat = bpy.data.materials.new("M_Palette")
    if hasattr(mat, "use_nodes") and not mat.use_nodes:  # pre-6.0 compat; 5.x warns, 6.x removes
        mat.use_nodes = True
    bsdf = next(n for n in mat.node_tree.nodes if n.type == "BSDF_PRINCIPLED")
    tex = mat.node_tree.nodes.new("ShaderNodeTexImage")
    tex.image = bpy.data.images.load(palette_path)
    tex.interpolation = "Closest"  # flat cells, no bleeding
    mat.node_tree.links.new(tex.outputs["Color"], bsdf.inputs["Base Color"])
    bsdf.inputs["Roughness"].default_value = 0.9
    return mat


def process_object(obj, palette_mat, cols, args):
    mesh = obj.data
    # Cache each source slot's nearest palette cell before wiping materials.
    slot_to_cell = [nearest_cell(material_base_color(s.material)) for s in obj.material_slots] \
                   or [nearest_cell((0.5, 0.5, 0.5))]

    uv = mesh.uv_layers.new(name="Palette") if not mesh.uv_layers else mesh.uv_layers[0]
    for poly in mesh.polygons:
        cell = slot_to_cell[min(poly.material_index, len(slot_to_cell) - 1)]
        u, v = cell_center_uv(cell, cols)
        for loop_idx in poly.loop_indices:
            uv.data[loop_idx].uv = (u, v)

    mesh.materials.clear()
    mesh.materials.append(palette_mat)

    if args.decimate < 1.0:
        mod = obj.modifiers.new("Decimate", "DECIMATE")
        mod.ratio = args.decimate
        bpy.context.view_layer.objects.active = obj
        bpy.ops.object.modifier_apply(modifier=mod.name)

    for poly in mesh.polygons:
        poly.use_smooth = args.shading == "smooth"

    obj.scale = [s * args.scale for s in obj.scale]


def run(args):
    """args needs: src, dst, palette, scale, decimate, shading (see main)."""
    src, dst = pathlib.Path(args.src), pathlib.Path(args.dst)
    dst.mkdir(parents=True, exist_ok=True)
    cols = math.ceil(math.sqrt(len(PALETTE)))

    files = [f for f in sorted(src.rglob("*")) if f.suffix.lower() in IMPORTERS]
    print(f"unify_pass: {len(files)} files from {src}")

    for f in files:
        bpy.ops.wm.read_factory_settings(use_empty=True)  # clean scene per file
        palette_mat = make_palette_material(args.palette)
        IMPORTERS[f.suffix.lower()](str(f))

        meshes = [o for o in bpy.context.scene.objects if o.type == "MESH"]
        for obj in meshes:
            process_object(obj, palette_mat, cols, args)

        bpy.ops.object.select_all(action="SELECT")
        bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)

        out = dst / f"prop_{f.stem.lower()}.fbx"
        bpy.ops.export_scene.fbx(
            filepath=str(out), use_selection=True,
            apply_scale_options="FBX_SCALE_ALL", bake_space_transform=True,
            axis_forward="-Z", axis_up="Y", path_mode="COPY", embed_textures=False,
        )
        print(f"  {f.name} -> {out.name} ({sum(len(o.data.polygons) for o in meshes)} tris-ish)")

    print("unify_pass done. Now do the human 20%: silhouette edits in the UI.")


def main():
    argv = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    p = argparse.ArgumentParser()
    p.add_argument("--in", dest="src", required=True)
    p.add_argument("--out", dest="dst", required=True)
    p.add_argument("--palette", required=True)
    p.add_argument("--scale", type=float, default=1.0)
    p.add_argument("--decimate", type=float, default=1.0)
    p.add_argument("--shading", choices=["flat", "smooth"], default="flat")
    run(p.parse_args(argv))


if __name__ == "__main__":
    main()
