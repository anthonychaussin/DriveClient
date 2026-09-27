"""Split IKDriveClient endpoint methods into domain interfaces."""
from __future__ import annotations

import re
from pathlib import Path

root = Path(r"c:\Users\chaus\source\repos\DriveClient\DriveClient\kDriveClient\Interface")

method_block_re = re.compile(
    r"^\s+(Task<.+?>\??)\s+(\w+)\s*\((.*?)\)\s*;",
    re.M | re.S,
)

# Simpler line-oriented: methods are single-line in these files
line_re = re.compile(r"^\s+(Task<.+?>\??)\s+(\w+)\s*\((.*)\)\s*;\s*$")


def classify(name: str) -> str:
    n = name
    low = n.lower()

    if "Comment" in n:
        return "IKDriveComments"
    if "Categor" in n or n.startswith("Tag") or n.startswith("Untag") or "CategoryAi" in n:
        return "IKDriveCategories"

    share_keys = (
        "ShareLink", "Share", "Access", "Invitation", "Dropbox", "Invite",
        "ForceFileAccess", "ForceItemAccess", "GrantFile", "GrantItem",
        "SyncParent", "CheckFileAccess", "CheckItemAccess",
    )
    if any(k in n for k in share_keys):
        # Dropbox list/search of files can be Files; share/create dropbox -> Shares
        return "IKDriveShares"

    drive_keys = (
        "Drive", "User", "Wake", "Preference", "Setting", "Statistic", "Import",
        "Activity", "Admin", "Member", "Lock", "UnlockDrive", "OAuth", "Webdav",
        "Report", "Accessible", "GlobalPref", "SizeStatistics", "GetDrives",
        "GetUsers", "ListDrive", "ListImport", "ListOAuth", "ListInvitation",
        "SendInvitation", "SendDrive", "CreateDrive", "UpdateDrive", "DeleteDrive",
        "AddDrive", "RemoveDrive", "PatchDrive", "GetActivity", "CreateActivity",
        "DeleteActivity", "GetSize", "GetLinkActivity", "GetSharedFilesActivity",
        "GetUserActivity", "GetStatistics", "CancelImport", "ClearImport",
        "StartKdrive", "StartOAuth", "StartSharelink", "StartWebdav",
        "UploadSession", "UploadSessions", "CancelUpload", "FinishUpload", "StartUpload",
    )
    if any(k in n for k in drive_keys):
        return "IKDriveDrive"

    # UnlockFile/UnlockItem are file ops
    return "IKDriveFiles"


def normalize_ret(ret: str) -> str:
    # Task<T?>? -> Task<T?>
    if ret.endswith("?") and ret.count("?") >= 2:
        # e.g. Task<Foo?>?
        if ret.endswith("?>?"):
            return ret[:-1]  # drop trailing ? on Task
        if ret.endswith(")?"):
            return ret
        # Task<X>? where X has no ?
        if re.match(r"Task<[^>]+>\?$", ret):
            return ret[:-1]
    return ret


methods: dict[str, list[tuple[str, str, str]]] = {
    "IKDriveFiles": [],
    "IKDriveShares": [],
    "IKDriveComments": [],
    "IKDriveCategories": [],
    "IKDriveDrive": [],
}

seen = set()
for p in sorted(root.glob("IkDriveClient*.cs")):
    if "Composition" in p.name:
        continue
    for line in p.read_text(encoding="utf-8").splitlines():
        m = line_re.match(line)
        if not m:
            continue
        ret, name, params = m.group(1), m.group(2), m.group(3)
        if name in seen:
            continue
        seen.add(name)
        ret = normalize_ret(ret)
        methods[classify(name)].append((ret, name, params))

file_map = {
    "IKDriveFiles": "IKDriveFiles.Endpoints.cs",
    "IKDriveShares": "IKDriveShares.cs",
    "IKDriveComments": "IKDriveComments.cs",
    "IKDriveCategories": "IKDriveCategories.cs",
    "IKDriveDrive": "IKDriveDrive.cs",
}

for iface, out_name in file_map.items():
    items = methods[iface]
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
    (root / out_name).write_text("\n".join(lines) + "\n", encoding="utf-8")
    print(f"wrote {out_name}: {len(items)} methods")

# Composition
comp = """namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// Aggregate kDrive client surface (upload, download, endpoints, smart helpers).
    /// </summary>
    public partial interface IKDriveClient
        : IKDriveUpload,
          IKDriveDownload,
          IKDriveFiles,
          IKDriveShares,
          IKDriveComments,
          IKDriveCategories,
          IKDriveDrive,
          IKDriveSmart
    {
    }
}
"""
(root / "IkDriveClient.Composition.cs").write_text(comp, encoding="utf-8")
print("wrote IkDriveClient.Composition.cs")

# Remove old monolith partials (methods now on domain interfaces)
for old in [
    "IkDriveClient.cs",
    "IkDriveClient.Extended.cs",
    "IkDriveClient.ExtendedCoverage.cs",
    "IkDriveClient.RawTyped.cs",
    "IkDriveClient.ExtendedAliases.cs",
]:
    path = root / old
    if path.exists():
        path.unlink()
        print("removed", old)

print("total unique methods", len(seen))
for k, v in methods.items():
    print(f"  {k}: {len(v)}")
