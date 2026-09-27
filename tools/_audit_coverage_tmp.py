import json
import re
from pathlib import Path

root = Path(r"c:\Users\chaus\source\repos\DriveClient")
j = json.loads((root / "DriveClient" / "infomaniak_api_kdrive.json").read_text(encoding="utf-8"))

ops = []
for path, methods in j["paths"].items():
    for method, op in methods.items():
        if method.lower() in ("get", "post", "put", "patch", "delete"):
            ops.append(
                (
                    method.upper(),
                    path,
                    ",".join(op.get("tags") or []),
                    op.get("operationId") or "",
                    op.get("summary") or "",
                )
            )

print("OpenAPI total ops:", len(ops))
print("OpenAPI paths:", len(j["paths"]))

blob = []
for p in (root / "DriveClient").rglob("*.cs"):
    if "obj" in p.parts or "bin" in p.parts:
        continue
    blob.append(p.read_text(encoding="utf-8", errors="ignore"))
text = "\n".join(blob)

found = set()
for m in re.finditer(r'["\']((?:/2|/3)/(?:drive|app)[^"\']*)["\']', text):
    found.add(m.group(1))
for m in re.finditer(r'\$"((?:/2|/3)/(?:drive|app)[^"]*)"', text):
    found.add(m.group(1))


def normalize(path: str) -> str:
    path = path.split("?", 1)[0]
    reps = {
        "{_driveId}": "{drive_id}",
        "{driveId}": "{drive_id}",
        "{fileId}": "{file_id}",
        "{directoryId}": "{file_id}",
        "{parentDirectoryId}": "{file_id}",
        "{parentId}": "{file_id}",
        "{trashedDirectoryId}": "{file_id}",
        "{versionId}": "{version_id}",
        "{userId}": "{user_id}",
        "{commentId}": "{comment_id}",
        "{categoryId}": "{category_id}",
        "{invitationId}": "{invitation_id}",
        "{importId}": "{import_id}",
        "{reportId}": "{report_id}",
        "{requestId}": "{request_id}",
        "{teamId}": "{team_id}",
        "{archiveUuid}": "{archive_uuid}",
        "{sessionId}": "{session_token}",
        "{sessionToken}": "{session_token}",
        "{destinationDirectoryId}": "{destination_directory_id}",
        "{sharelinkUuid}": "{sharelink_uuid}",
        "{accountId}": "{account_id}",
    }
    for a, b in reps.items():
        path = path.replace(a, b)
    return path


impl_paths = {normalize(p) for p in found}


def shape(p: str) -> str:
    return re.sub(r"\{[^}]+\}", "{}", p)


def path_covered(openapi_path: str) -> bool:
    n = normalize(openapi_path)
    if n in impl_paths:
        return True
    sn = shape(n)
    return any(shape(i) == sn for i in impl_paths)


missing = []
covered = []
for method, path, tags, oid, summary in ops:
    if path_covered(path):
        covered.append((method, path))
    else:
        missing.append((method, path, tags, oid, summary))

print("Covered (path present):", len(covered))
print("Missing (path absent):", len(missing))
print("Distinct normalized impl path templates:", len(impl_paths))
print()
if missing:
    print("=== MISSING ===")
    for m, p, t, oid, s in sorted(missing):
        print(f"{m} {p} | {oid} | {s} | {t}")
else:
    print("No missing paths by heuristic.")

# Method+path coverage is harder; check if we have both POST and GET for archives etc.
# Verify a few critical method mismatches by grepping method near path in service files.
print()
print("=== Sample: check Get vs Post for /files/archives ===")
svc = (root / "DriveClient/kDriveClient/Application/Endpoints").rglob("*.cs")
archive_hits = []
for p in svc:
    t = p.read_text(encoding="utf-8")
    if "files/archives" in t or "share/" in t and "archive" in t:
        for i, line in enumerate(t.splitlines(), 1):
            if "archive" in line.lower() and ("/2/" in line or "/3/" in line):
                archive_hits.append(f"{p.name}:{i}: {line.strip()[:120]}")
for h in archive_hits[:20]:
    print(h)
