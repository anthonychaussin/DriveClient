#!/usr/bin/env python3
"""Bulk-replace IDictionary query params with KDriveListQuery across the SDK."""
from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
TARGETS = [
    ROOT / "DriveClient" / "kDriveClient",
    ROOT / "DriveClientTests",
]

# Signature / declaration replacements
REPLACEMENTS = [
    (
        r"IDictionary<string,\s*string\?>\?\s+query",
        "KDriveListQuery? query",
    ),
    (
        r"IDictionary<string,\s*string\?>\?\s+query\s*=\s*null",
        "KDriveListQuery? query = null",
    ),
]

# BuildPath(…, query) → BuildPath(…, query?.ToDictionary())
BUILD_PATH = re.compile(
    r"(QueryStringBuilder\.BuildPath\([^,]+,\s*)query(\s*\))"
)

# CloneQuery(query) stays valid if we update CloneQuery to accept KDriveListQuery

def process_file(path: Path) -> bool:
    text = path.read_text(encoding="utf-8")
    original = text

    for pattern, repl in REPLACEMENTS:
        text = re.sub(pattern, repl, text)

    # Only in service implementation files that call BuildPath
    if "KDriveEndpointsService" in path.name or path.name.startswith("KDriveEndpointsService"):
        text = BUILD_PATH.sub(r"\1query?.ToDictionary()\2", text)

    # Avoid double ToDictionary
    text = text.replace("query?.ToDictionary()?.ToDictionary()", "query?.ToDictionary()")
    text = text.replace("query.ToDictionary().ToDictionary()", "query.ToDictionary()")

    if text != original:
        path.write_text(text, encoding="utf-8", newline="\n")
        return True
    return False


def main() -> None:
    changed = []
    for base in TARGETS:
        if not base.exists():
            continue
        for path in base.rglob("*.cs"):
            if process_file(path):
                changed.append(path.relative_to(ROOT).as_posix())
    print(f"Updated {len(changed)} files")
    for c in changed:
        print(f"  {c}")


if __name__ == "__main__":
    main()
