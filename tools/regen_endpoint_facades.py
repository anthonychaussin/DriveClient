"""Regenerate KDriveClient Endpoints façades from KDriveEndpointsService public methods."""
from __future__ import annotations

import re
from pathlib import Path

root = Path(r"c:\Users\chaus\source\repos\DriveClient\DriveClient\kDriveClient")
svc_dir = root / "Application" / "Endpoints"
iface_dir = root / "Interface"

method_re = re.compile(
    r"public\s+(?P<ret>Task<.+?>)\??\s+(?P<name>\w+)\s*\((?P<params>[^)]*)\)",
    re.M,
)


def extract_methods(text: str) -> list[tuple[str, str, str]]:
    methods = []
    for m in method_re.finditer(text):
        name = m.group("name")
        if name in ("KDriveEndpointsService",):
            continue
        full = m.group(0)
        before = full[: full.find(name)]
        ret_decl = before.replace("public", "").strip()
        # Normalize Task<T?>? -> Task<T?>
        if ret_decl.endswith("?") and ret_decl.count("?") >= 2 and ret_decl.endswith("?>?"):
            ret_decl = ret_decl[:-1]
        elif re.match(r"Task<[^>?]+>\?$", ret_decl):
            ret_decl = ret_decl[:-1]
        params = m.group("params").strip()
        methods.append((ret_decl, name, params))
    return methods


def call_args(params: str) -> str:
    if not params:
        return ""
    parts = []
    depth = 0
    cur = []
    for ch in params:
        if ch == "<":
            depth += 1
        elif ch == ">":
            depth -= 1
        if ch == "," and depth == 0:
            parts.append("".join(cur).strip())
            cur = []
        else:
            cur.append(ch)
    if cur:
        parts.append("".join(cur).strip())
    names = []
    for p in parts:
        p = p.split("=")[0].strip()
        if not p:
            continue
        names.append(p.split()[-1].lstrip("?"))
    return ", ".join(names)


def classify(name: str) -> str:
    if "Comment" in name:
        return "IKDriveComments"
    if "Categor" in name or name.startswith("Tag") or name.startswith("Untag") or "CategoryAi" in name:
        return "IKDriveCategories"

    share_keys = (
        "ShareLink", "Share", "Access", "Invitation", "Dropbox", "Invite",
        "ForceFileAccess", "ForceItemAccess", "GrantFile", "GrantItem",
        "SyncParent", "CheckFileAccess", "CheckItemAccess",
    )
    if any(k in name for k in share_keys):
        return "IKDriveShares"

    drive_keys = (
        "Drive", "User", "Wake", "Preference", "Setting", "Statistic", "Import",
        "Activity", "Member", "UnlockDrive", "LockDrive", "OAuth", "Webdav",
        "Report", "Accessible", "GlobalPref", "SizeStatistics", "GetDrives",
        "GetUsers", "ListDrive", "ListImport", "ListOAuth", "ListInvitation",
        "SendInvitation", "SendDrive", "CreateDrive", "UpdateDrive", "DeleteDrive",
        "AddDrive", "RemoveDrive", "PatchDrive", "GetActivity", "CreateActivity",
        "DeleteActivity", "GetSize", "GetLinkActivity", "GetSharedFilesActivity",
        "GetUserActivity", "GetStatistics", "CancelImport", "ClearImport",
        "StartKdrive", "StartOAuth", "StartSharelink", "StartWebdav",
        "UploadSession", "UploadSessions", "CancelUpload", "FinishUpload", "StartUpload",
    )
    if any(k in name for k in drive_keys):
        return "IKDriveDrive"

    return "IKDriveFiles"


all_methods: dict[str, tuple[str, str]] = {}
file_methods: dict[str, list[tuple[str, str, str]]] = {}

for p in sorted(svc_dir.glob("KDriveEndpointsService*.cs")):
    text = p.read_text(encoding="utf-8")
    methods = extract_methods(text)
    file_methods[p.name] = methods
    for ret, name, params in methods:
        all_methods[name] = (ret, params)

alias_names = set()
alias_file = root / "Endpoints.ExtendedAliases.cs"
if alias_file.exists():
    for m in method_re.finditer(alias_file.read_text(encoding="utf-8")):
        alias_names.add(m.group("name"))

CORE = "KDriveEndpointsService.cs"
EXT = "KDriveEndpointsService.Extended.cs"
COV = "KDriveEndpointsService.ExtendedCoverage.cs"
RAW = "KDriveEndpointsService.RawTyped.cs"
ALIASES = "KDriveEndpointsService.ExtendedAliases.cs"


def gen_client_partial(out_name: str, svc_file: str, exclude: set[str]) -> None:
    methods = file_methods.get(svc_file, [])
    lines = [
        "using kDriveClient.Models;",
        "",
        "namespace kDriveClient.kDriveClient",
        "{",
        "    public partial class KDriveClient",
        "    {",
    ]
    for ret, name, params in methods:
        if name in exclude:
            continue
        args = call_args(params)
        lines.append(f"        public {ret} {name}({params})")
        if args:
            lines.append(f"            => EndpointsService.{name}({args});")
        else:
            lines.append(f"            => EndpointsService.{name}();")
        lines.append("")
    lines += ["    }", "}"]
    (root / out_name).write_text("\n".join(lines) + "\n", encoding="utf-8")
    print("wrote", out_name, "methods", sum(1 for _, n, _ in methods if n not in exclude))


exclude_from_others = set(alias_names)
for ret, name, params in file_methods.get(ALIASES, []):
    exclude_from_others.add(name)

gen_client_partial("Endpoints.cs", CORE, exclude_from_others)
gen_client_partial("Endpoints.Extended.cs", EXT, exclude_from_others)
gen_client_partial("Endpoints.ExtendedCoverage.cs", COV, exclude_from_others)
gen_client_partial("Endpoints.RawTyped.cs", RAW, exclude_from_others)

# Domain interfaces from all service methods (including aliases)
domain: dict[str, list[tuple[str, str, str]]] = {
    "IKDriveFiles": [],
    "IKDriveShares": [],
    "IKDriveComments": [],
    "IKDriveCategories": [],
    "IKDriveDrive": [],
}
seen: set[str] = set()
for svc_file in (CORE, EXT, COV, RAW, ALIASES):
    for ret, name, params in file_methods.get(svc_file, []):
        if name in seen:
            continue
        seen.add(name)
        domain[classify(name)].append((ret, name, params))

file_map = {
    "IKDriveFiles": "IKDriveFiles.Endpoints.cs",
    "IKDriveShares": "IKDriveShares.cs",
    "IKDriveComments": "IKDriveComments.cs",
    "IKDriveCategories": "IKDriveCategories.cs",
    "IKDriveDrive": "IKDriveDrive.cs",
}

for iface, out_name in file_map.items():
    items = domain[iface]
    lines = [
        "using kDriveClient.Models;",
        "",
        "namespace kDriveClient.kDriveClient",
        "{",
        f"    public partial interface {iface}",
        "    {",
    ]
    for ret, name, params in items:
        lines.append(f"        {ret} {name}({params});")
        lines.append("")
    lines += ["    }", "}"]
    (iface_dir / out_name).write_text("\n".join(lines) + "\n", encoding="utf-8")
    print("wrote Interface/" + out_name, len(items))

# Sync IKDriveEndpointsService interfaces from service impls
iface_svc = root / "Application" / "Endpoints"


def gen_svc_iface(out_name: str, svc_file: str) -> None:
    methods = file_methods.get(svc_file, [])
    lines = [
        "using kDriveClient.Models;",
        "",
        "namespace kDriveClient.kDriveClient.Application.Endpoints",
        "{",
        "    public partial interface IKDriveEndpointsService",
        "    {",
    ]
    for ret, name, params in methods:
        lines.append(f"        {ret} {name}({params});")
        lines.append("")
    lines += ["    }", "}"]
    (iface_svc / out_name).write_text("\n".join(lines) + "\n", encoding="utf-8")
    print("wrote", out_name, len(methods))


gen_svc_iface("IKDriveEndpointsService.cs", CORE)
gen_svc_iface("IKDriveEndpointsService.Extended.cs", EXT)
gen_svc_iface("IKDriveEndpointsService.ExtendedCoverage.cs", COV)
gen_svc_iface("IKDriveEndpointsService.RawTyped.cs", RAW)
gen_svc_iface("IKDriveEndpointsService.ExtendedAliases.cs", ALIASES)

print("done, total service methods", len(all_methods), "domain unique", len(seen))
