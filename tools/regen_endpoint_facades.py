"""Regenerate KDriveClient Endpoints façades from KDriveEndpointsService public methods.

IMPORTANT: Client façades (Endpoints*.cs) and domain interfaces (IKDriveFiles.Endpoints.cs etc.)
may wrap FileSystemItem responses as KDriveItemPage / KDriveItemResult. By default this script
only regenerates IKDriveEndpointsService* (raw DTO signatures). Pass --force-client to also
overwrite client Endpoints*.cs / domain interfaces (preserves methods containing
KDriveItemPage.From / KDriveItemResult.From / KDriveAccess.From when possible).
"""
from __future__ import annotations

import re
import sys
from pathlib import Path

root = Path(r"c:\Users\chaus\source\repos\DriveClient\DriveClient\kDriveClient")
svc_dir = root / "Application" / "Endpoints"
iface_dir = root / "Interface"


def _balanced_generic(text: str, open_lt: int) -> int:
    depth = 0
    i = open_lt
    while i < len(text):
        ch = text[i]
        if ch == "<":
            depth += 1
        elif ch == ">":
            depth -= 1
            if depth == 0:
                return i + 1
        i += 1
    raise ValueError("Unbalanced generics in method signature")


def extract_methods(text: str) -> list[tuple[str, str, str]]:
    methods: list[tuple[str, str, str]] = []
    for m in re.finditer(r"public\s+(Task\s*<)", text):
        type_start = m.start(1)
        lt = text.index("<", type_start)
        type_end = _balanced_generic(text, lt)
        while type_end < len(text) and text[type_end] == "?":
            type_end += 1

        ret_decl = text[type_start:type_end].replace(" ", "")
        while ret_decl.endswith("?>?"):
            ret_decl = ret_decl[:-1]
        if ret_decl.endswith(">?") and not ret_decl.endswith("?>"):
            ret_decl = ret_decl[:-1] + "?>"

        rest = text[type_end:]
        name_m = re.match(r"\s+(\w+)\s*\(", rest)
        if not name_m:
            continue
        name = name_m.group(1)
        if name in ("KDriveEndpointsService",):
            continue

        params_start = type_end + name_m.end() - 1
        depth = 0
        j = params_start
        while j < len(text):
            if text[j] == "(":
                depth += 1
            elif text[j] == ")":
                depth -= 1
                if depth == 0:
                    break
            j += 1
        params = text[params_start + 1 : j].strip()
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


DOMAIN_MARKERS = ("KDriveItemPage.From", "KDriveItemResult.From", "KDriveAccess.From")


def load_preserved_methods(path: Path) -> dict[str, str]:
    if not path.exists():
        return {}
    text = path.read_text(encoding="utf-8")
    preserved: dict[str, str] = {}
    for m in re.finditer(r"(public\s+(?:async\s+)?Task<[\s\S]*?^\s{8}\})", text, re.M):
        block = m.group(1)
        if not any(marker in block for marker in DOMAIN_MARKERS):
            continue
        nm = re.search(r"\b(\w+)\s*\(", block)
        if nm:
            preserved[nm.group(1)] = block.rstrip()
    return preserved


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
    for _, name, _ in extract_methods(alias_file.read_text(encoding="utf-8")):
        alias_names.add(name)

CORE = "KDriveEndpointsService.cs"
EXT = "KDriveEndpointsService.Extended.cs"
COV = "KDriveEndpointsService.ExtendedCoverage.cs"
RAW = "KDriveEndpointsService.RawTyped.cs"
ALIASES = "KDriveEndpointsService.ExtendedAliases.cs"


def gen_client_partial(out_name: str, svc_file: str, exclude: set[str]) -> None:
    methods = file_methods.get(svc_file, [])
    preserved = load_preserved_methods(root / out_name)
    lines = [
        "using kDriveClient.Models;",
        "using kDriveClient.Models.Domain;",
        "",
        "namespace kDriveClient.kDriveClient",
        "{",
        "    public partial class KDriveClient",
        "    {",
    ]
    for ret, name, params in methods:
        if name in exclude:
            continue
        if name in preserved:
            lines.append("        " + preserved[name].lstrip())
            lines.append("")
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
    print(
        "wrote",
        out_name,
        "methods",
        sum(1 for _, n, _ in methods if n not in exclude),
        "preserved",
        len([n for _, n, _ in methods if n in preserved and n not in exclude]),
    )


exclude_from_others = set(alias_names)
for ret, name, params in file_methods.get(ALIASES, []):
    exclude_from_others.add(name)

FORCE_CLIENT = "--force-client" in sys.argv
seen: set[str] = set(all_methods)

if FORCE_CLIENT:
    gen_client_partial("Endpoints.cs", CORE, exclude_from_others)
    gen_client_partial("Endpoints.Extended.cs", EXT, exclude_from_others)
    gen_client_partial("Endpoints.ExtendedCoverage.cs", COV, exclude_from_others)
    gen_client_partial("Endpoints.RawTyped.cs", RAW, exclude_from_others)

    domain: dict[str, list[tuple[str, str, str]]] = {
        "IKDriveFiles": [],
        "IKDriveShares": [],
        "IKDriveComments": [],
        "IKDriveCategories": [],
        "IKDriveDrive": [],
    }
    seen = set()
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
            "using kDriveClient.Models.Domain;",
            "",
            "namespace kDriveClient.kDriveClient",
            "{",
            f"    public partial interface {iface}",
            "    {",
        ]
        existing = (iface_dir / out_name).read_text(encoding="utf-8") if (iface_dir / out_name).exists() else ""
        for ret, name, params in items:
            m = re.search(
                rf"(Task<(?:KDriveItemPage|KDriveItemResult|KDriveAccess)\??>\s+{re.escape(name)}\s*\([^;]*\);)",
                existing,
            )
            if m:
                lines.append(f"        {m.group(1)}")
            else:
                lines.append(f"        {ret} {name}({params});")
            lines.append("")
        lines += ["    }", "}"]
        (iface_dir / out_name).write_text("\n".join(lines) + "\n", encoding="utf-8")
        print("wrote Interface/" + out_name, len(items))
else:
    print("skip client Endpoints*.cs / Interface IKDrive* (pass --force-client to overwrite)")

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

print("done, total service methods", len(all_methods), "tracked", len(seen))
