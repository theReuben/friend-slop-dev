"""Syntax-check every C# file under reference/ (or a given path) without
Unity, using tree-sitter. Catches the cheap failure class — typos, unbalanced
braces, malformed declarations — before anyone wastes an editor session on it.
It does NOT catch semantic errors (wrong API names, missing usings); those
surface on first Unity compile, which is expected and fine (reference/README).

Setup + run (any Python 3.9+):
    pip install tree-sitter tree-sitter-c-sharp
    python syntax_check.py [path]

Exits nonzero with file/line on any parse error. All reference C# passed this
check when authored. RE-RUN AFTER EVERY EDIT to a .cs file in this repo —
this is the only compile-adjacent verification available outside Unity.
"""
import pathlib
import sys

import tree_sitter_c_sharp
from tree_sitter import Language, Parser

parser = Parser(Language(tree_sitter_c_sharp.language()))


def first_error(node):
    if node.type == "ERROR" or node.is_missing:
        return node
    for child in node.children:
        found = first_error(child)
        if found:
            return found
    return None


def main():
    root = pathlib.Path(sys.argv[1]) if len(sys.argv) > 1 else pathlib.Path(__file__).parent.parent
    files = sorted(root.rglob("*.cs"))
    if not files:
        print(f"no .cs files under {root}")
        sys.exit(1)

    failed = 0
    for f in files:
        err = first_error(parser.parse(f.read_bytes()).root_node)
        if err:
            failed += 1
            snippet = err.text[:60].decode(errors="replace") if err.text else "<missing node>"
            print(f"SYNTAX ERROR {f}: line {err.start_point[0] + 1}, near: {snippet!r}")
        else:
            print(f"OK  {f.relative_to(root)}")

    print("FAILED" if failed else "ALL PARSED CLEAN")
    sys.exit(1 if failed else 0)


if __name__ == "__main__":
    main()
