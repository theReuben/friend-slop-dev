"""Pipeline self-test: proves the unification pipeline works on THIS Blender
version before you batch real assets. Run it whenever Blender/bpy changes.

    python selftest.py            (standalone bpy wheel: pip install bpy)
    blender --background --python selftest.py   (full Blender install)

What it does, in a temp dir:
 1. generates the palette atlas
 2. fabricates a fake "downloaded pack asset" (multi-material crate + bush)
 3. runs unify_pass.py on it via a subprocess-free import
 4. reimports the output FBX and asserts:
      - single M_Palette material per mesh
      - every UV sits exactly on the nearest-matching palette cell center
      - wood-ish faces routed to 'wood', trim to 'rock', bush to 'foliage'
      - flat shading survived the round-trip (loop normals == face normals;
        do NOT trust polygon.use_smooth — the FBX importer sets it even when
        the custom split normals are flat)
Prints SELFTEST PASS / SELFTEST FAIL and exits nonzero on failure.
"""
import math
import pathlib
import sys
import tempfile
import types

import bpy

HERE = pathlib.Path(__file__).parent
sys.path.insert(0, str(HERE))
import make_palette_atlas  # noqa: E402
import unify_pass  # noqa: E402
from make_palette_atlas import PALETTE  # noqa: E402

COLS = math.ceil(math.sqrt(len(PALETTE)))
CELL = {name: i for i, (name, _) in enumerate(PALETTE)}


def cell_center(idx):
    return ((idx % COLS + 0.5) / COLS, (idx // COLS + 0.5) / COLS)


def make_material(name, rgba):
    m = bpy.data.materials.new(name)
    if hasattr(m, "use_nodes") and not m.use_nodes:
        m.use_nodes = True
    bsdf = next(n for n in m.node_tree.nodes if n.type == "BSDF_PRINCIPLED")
    bsdf.inputs["Base Color"].default_value = rgba
    return m


def fabricate_test_asset(path):
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=(0, 0, 0.5))
    crate = bpy.context.active_object
    crate.name = "crate"
    crate.data.materials.append(make_material("wood_src", (0.65, 0.45, 0.30, 1)))
    crate.data.materials.append(make_material("metal_src", (0.60, 0.58, 0.55, 1)))
    crate.data.polygons[0].material_index = 1
    crate.data.polygons[1].material_index = 1

    bpy.ops.mesh.primitive_uv_sphere_add(segments=16, ring_count=8, radius=0.4,
                                         location=(1.2, 0, 0.4))
    bush = bpy.context.active_object
    bush.name = "bush"
    bush.data.materials.append(make_material("leaf_src", (0.35, 0.60, 0.28, 1)))

    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.export_scene.fbx(filepath=str(path), use_selection=True)


def run_pipeline(tmp):
    make_palette_atlas.build(str(tmp / "palette.png"))
    unify_pass.run(types.SimpleNamespace(
        src=str(tmp / "in"), dst=str(tmp / "out"), palette=str(tmp / "palette.png"),
        scale=1.0, decimate=0.9, shading="flat"))


def verify(fbx_path):
    failures = []
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(fbx_path))
    expected = {"crate": {"wood", "rock"}, "bush": {"foliage"}}

    meshes = [o for o in bpy.context.scene.objects if o.type == "MESH"]
    if not meshes:
        return ["no meshes in output FBX"]

    for obj in meshes:
        mesh = obj.data
        mats = {s.material.name.split(".")[0] for s in obj.material_slots if s.material}
        if mats != {"M_Palette"}:
            failures.append(f"{obj.name}: materials {mats}, want only M_Palette")

        hit_cells = set()
        uv = mesh.uv_layers[0]
        for poly in mesh.polygons:
            for li in poly.loop_indices:
                u, v = uv.data[li].uv
                best, dist = None, 9e9
                for i in range(len(PALETTE)):
                    cu, cv = cell_center(i)
                    d = (u - cu) ** 2 + (v - cv) ** 2
                    if d < dist:
                        best, dist = i, d
                if dist > 1e-9:
                    failures.append(f"{obj.name}: UV ({u:.4f},{v:.4f}) off cell center")
                hit_cells.add(PALETTE[best][0])

        want = expected.get(obj.name.split(".")[0])
        if want is not None and hit_cells != want:
            failures.append(f"{obj.name}: routed to {hit_cells}, want {want}")

        max_dev = max((mesh.loops[li].normal - poly.normal).length
                      for poly in mesh.polygons for li in poly.loop_indices)
        if max_dev > 1e-3:
            failures.append(f"{obj.name}: shading not flat (normal dev {max_dev:.3f})")
    return failures


def main():
    print(f"selftest on Blender {bpy.app.version_string}")
    with tempfile.TemporaryDirectory() as tmpdir:
        tmp = pathlib.Path(tmpdir)
        (tmp / "in").mkdir()
        fabricate_test_asset(tmp / "in" / "crate_and_bush.fbx")
        run_pipeline(tmp)
        outputs = list((tmp / "out").glob("*.fbx"))
        failures = verify(outputs[0]) if outputs else ["unify_pass produced no FBX"]

    if failures:
        print("SELFTEST FAIL")
        for f in failures:
            print("  -", f)
        sys.exit(1)
    print("SELFTEST PASS — pipeline verified on this Blender version.")


if __name__ == "__main__":
    main()
