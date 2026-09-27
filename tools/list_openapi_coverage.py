#!/usr/bin/env python3
"""Compare OpenAPI kDrive operations to paths implemented under DriveClient/."""

from __future__ import annotations

import json
import re
import sys
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parents[1]
OPENAPI = REPO_ROOT / "DriveClient" / "infomaniak_api_kdrive.json"
DRIVE_CLIENT_ROOT = REPO_ROOT / "DriveClient"

# OpenAPI {snake_case} -> common C# interpolation names in path literals
OPENAPI_TO_CS_PARAM: dict[str, str] = {
    "drive_id": "_driveId",
    "sharelink_uuid": "sharelinkUuid",
    "file_id": "fileId",
    "user_id": "userId",
    "team_id": "teamId",
    "version_id": "versionId",
    "category_id": "categoryId",
    "comment_id": "commentId",
    "invitation_id": "invitationId",
    "import_id": "importId",
    "report_id": "reportId",
    "request_id": "requestId",
    "archive_uuid": "archiveUuid",
    "session_token": "sessionToken",
    "destination_directory_id": "destinationDirectoryId",
}


def load_openapi_ops() -> set[tuple[str, str]]:
    data = json.loads(OPENAPI.read_text(encoding="utf-8"))
    ops: set[tuple[str, str]] = set()
    for path, methods in data.get("paths", {}).items():
        if "/drive/" not in path and "/app/" not in path and path not in {"/2/drive", "/2/drive/users"}:
            continue
        for method in methods:
            if method.lower() in {"get", "post", "put", "delete", "patch"}:
                ops.add((method.upper(), path))
    return ops


def load_implemented_path_literals() -> set[str]:
    paths: set[str] = set()
    pattern = re.compile(r'["\']((?:/2|/3)/(?:drive|app)(?:/[^"\']*)?)["\']')
    for file in DRIVE_CLIENT_ROOT.rglob("*.cs"):
        if "obj" in file.parts or "bin" in file.parts:
            continue
        try:
            text = file.read_text(encoding="utf-8")
        except OSError:
            continue
        for match in pattern.finditer(text):
            paths.add(match.group(1))
    return paths


def canonical_path(path: str) -> str:
    """Normalize OpenAPI paths and C# literals to a comparable token form."""
    out = path
    for openapi_name, cs_name in OPENAPI_TO_CS_PARAM.items():
        out = out.replace(f"{{{openapi_name}}}", f"{{{cs_name}}}")
    # Common C# parameter names without underscore prefix
    out = out.replace("{driveId}", "{_driveId}")
    for alias in ("directoryId", "trashedDirectoryId", "parentDirectoryId", "parentId"):
        out = out.replace(f"{{{alias}}}", "{fileId}")
    out = re.sub(r"\$\{([^}]+)\}", r"{\1}", out)
    return out


def path_matches(implemented: str, openapi_path: str) -> bool:
    impl = canonical_path(implemented)
    spec = canonical_path(openapi_path)
    if impl == spec:
        return True
    # Allow query-only differences when base path matches
    impl_base = impl.split("?", 1)[0]
    spec_base = spec.split("?", 1)[0]
    return impl_base == spec_base


# Documented gaps: v2 upload session cancel is superseded by v3 equivalents already implemented.
ALLOWLIST_MISSING: set[tuple[str, str]] = {
    ("DELETE", "/2/drive/{drive_id}/upload/session/{session_token}"),
    ("DELETE", "/2/drive/{drive_id}/upload/session/batch"),
}


def main() -> int:
    if not OPENAPI.is_file():
        print(f"OpenAPI file not found: {OPENAPI}", file=sys.stderr)
        return 1

    openapi_ops = load_openapi_ops()
    implemented = load_implemented_path_literals()

    covered: list[tuple[str, str]] = []
    missing: list[tuple[str, str]] = []

    for method, path in sorted(openapi_ops):
        found = any(path_matches(p, path) for p in implemented)
        if found:
            covered.append((method, path))
        else:
            missing.append((method, path))

    unexpected = [m for m in missing if m not in ALLOWLIST_MISSING]
    allowlisted = [m for m in missing if m in ALLOWLIST_MISSING]

    print(f"OpenAPI drive ops: {len(openapi_ops)}")
    print(f"Distinct path literals in DriveClient/*.cs: {len(implemented)}")
    print(f"Covered (heuristic): {len(covered)}")
    print(f"Missing allowlisted (v2 cancel superseded by v3): {len(allowlisted)}")
    print(f"Missing unexpected: {len(unexpected)}")
    if allowlisted:
        print("\n--- Allowlisted missing ---")
        for method, path in allowlisted:
            print(f"{method} {path}")
    if unexpected:
        print("\n--- Unexpected missing ---")
        for method, path in unexpected:
            print(f"{method} {path}")
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
