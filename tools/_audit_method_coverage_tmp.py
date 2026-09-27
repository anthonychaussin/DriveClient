import json
import re
from pathlib import Path

root = Path(r"c:\Users\chaus\source\repos\DriveClient")
j = json.loads((root / "DriveClient" / "infomaniak_api_kdrive.json").read_text(encoding="utf-8"))

ops = []
for path, methods in j["paths"].items():
    for method, op in methods.items():
        if method.lower() in ("get", "post", "put", "patch", "delete"):
            ops.append((method.upper(), path, op.get("operationId") or "", op.get("summary") or ""))

# Build map of path shape -> set of HTTP methods found in C#
text_parts = []
for p in (root / "DriveClient").rglob("*.cs"):
    if "obj" in p.parts or "bin" in p.parts:
        continue
    text_parts.append(p.read_text(encoding="utf-8", errors="ignore"))
text = "\n".join(text_parts)

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


def normalize(path: str) -> str:
    path = path.split("?", 1)[0]
    for a, b in reps.items():
        path = path.replace(a, b)
    return path


def shape(p: str) -> str:
    return re.sub(r"\{[^}]+\}", "{}", normalize(p))


# Find Send*Async(HttpMethod.X, "...path...") and new HttpRequestMessage(HttpMethod.X, "...path...")
patterns = [
    re.compile(
        r"Send(?:Json|Typed)Async\(\s*HttpMethod\.(Get|Post|Put|Patch|Delete)\s*,\s*(?:QueryStringBuilder\.BuildPath\()?\$?\"([^\"]+)\"",
        re.M,
    ),
    re.compile(
        r"new HttpRequestMessage\(\s*HttpMethod\.(Get|Post|Put|Patch|Delete)\s*,\s*\$?\"([^\"]+)\"",
        re.M,
    ),
    # CreateDownloadRequest style may not use SendTyped - already covered by factory
]

# Also scan for Create*Request methods that hardcode paths
factory = (root / "DriveClient/kDriveClient/KDriveRequestFactory.cs").read_text(encoding="utf-8")
for m in re.finditer(
    r"HttpMethod\.(Get|Post|Put|Patch|Delete).*?\n.*?\"((?:/2|/3)/[^\"]+)\"",
    factory,
    re.S,
):
    pass

impl: dict[str, set[str]] = {}


def add(method: str, path: str) -> None:
    # strip query and trailing BuildPath second args already excluded
    path = path.split("{", 1)[0] + path[path.find("{") :] if "{" in path else path
    # clean interpolated leftovers like {driveId}/files - already in path
    path = re.sub(r"\{[^}]+\}", lambda m: m.group(0), path)
    # remove string concat fragments if any
    if "/2/" not in path and "/3/" not in path:
        return
    # extract from first /2 or /3
    idx = path.find("/2/")
    if idx < 0:
        idx = path.find("/3/")
    if idx < 0:
        return
    path = path[idx:].split("?")[0]
    # sometimes path is $"{baseUrl}/3/..." 
    if path.startswith("http"):
        return
    s = shape(path)
    impl.setdefault(s, set()).add(method.upper())


for pat in patterns:
    for m in pat.finditer(text):
        add(m.group(1), m.group(2))

# HttpRequestMessage with interpolated URL variable containing path - handle factory methods manually
for m in re.finditer(
    r'return new HttpRequestMessage\(HttpMethod\.(Get|Post|Put|Patch|Delete),\s*\$?"([^"]+)"',
    text,
):
    add(m.group(1), m.group(2))

# Download factory paths
for m in re.finditer(
    r'HttpMethod\.(Get|Post|Put|Patch|Delete),\s*\$?"((?:/2|/3)/[^"]+)"',
    text,
):
    add(m.group(1), m.group(2))

# Also: $"...{var}..." where method is on previous lines within 3 lines - weaker
# Fallback: for each path literal, look back 200 chars for HttpMethod
for m in re.finditer(r'\$?"((?:/2|/3)/(?:drive|app)[^"?]*)', text):
    path = m.group(1)
    start = max(0, m.start() - 250)
    window = text[start : m.start()]
    methods_found = re.findall(r"HttpMethod\.(Get|Post|Put|Patch|Delete)", window)
    if methods_found:
        add(methods_found[-1], path)

missing_method = []
covered = 0
for method, path, oid, summary in ops:
    s = shape(path)
    methods = impl.get(s, set())
    # also try openapi param names already normalized via shape
    if method in methods:
        covered += 1
    else:
        missing_method.append((method, path, sorted(methods), oid, summary))

print("Ops with matching METHOD+path shape:", covered, "/", len(ops))
print("Missing method or path:", len(missing_method))
print()
if missing_method:
    print("=== METHOD+PATH GAPS ===")
    for method, path, have, oid, summary in missing_method:
        print(f"NEED {method} {path}")
        print(f"  have methods on same shape: {have or '-'}")
        print(f"  {oid} | {summary}")
