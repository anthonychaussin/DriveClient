"""Generate ExtendedAliases partials for TypedAsync methods lacking a business alias."""
from __future__ import annotations

import re
from pathlib import Path

root = Path(r"c:\Users\chaus\source\repos\DriveClient")
endpoints_dir = root / "DriveClient" / "kDriveClient" / "Application" / "Endpoints"
client_dir = root / "DriveClient" / "kDriveClient"
iface_dir = client_dir / "Interface"

cs_files = [
    p
    for p in (root / "DriveClient" / "kDriveClient").rglob("*.cs")
    if "ExtendedAliases" not in p.name
]
all_text = "\n".join(p.read_text(encoding="utf-8") for p in cs_files)


def extract_task_return(stmt: str) -> tuple[str, str, str] | None:
    """Return (full_task_type, method_name, params) from a Task<...> Name(...); statement."""
    stmt = stmt.strip()
    if not stmt.startswith("Task<"):
        return None
    i = 5  # after Task<
    depth = 1
    while i < len(stmt) and depth:
        ch = stmt[i]
        if ch == "<":
            depth += 1
        elif ch == ">":
            depth -= 1
        i += 1
    if depth != 0:
        return None
    inner = stmt[5 : i - 1]
    rest = stmt[i:].lstrip()
    # optional nullable on Task itself (rare)
    if rest.startswith("?"):
        rest = rest[1:].lstrip()
    m = re.match(r"(?P<name>\w+TypedAsync)\s*\((?P<params>.*)\)\s*;?\s*$", rest)
    if not m:
        return None
    return inner, m.group("name"), m.group("params").strip()


def parse_interface_methods(text: str) -> dict[str, tuple[str, str]]:
    methods: dict[str, tuple[str, str]] = {}
    stmts = []
    buf = []
    obsolete_pending = False
    for line in text.splitlines():
        stripped = line.strip()
        if not stripped or stripped.startswith("//") or stripped.startswith("///"):
            continue
        if stripped.startswith("["):
            if "Obsolete" in stripped:
                obsolete_pending = True
            continue
        buf.append(stripped)
        if stripped.endswith(";"):
            stmt = " ".join(buf)
            buf = []
            if obsolete_pending:
                obsolete_pending = False
                continue
            stmts.append(stmt)
    for stmt in stmts:
        parsed = extract_task_return(stmt)
        if not parsed:
            continue
        ret, name, params = parsed
        if "V2Typed" in name:
            continue
        methods[name] = (ret, params)
    return methods


typed_methods: dict[str, tuple[str, str]] = {}
for p in endpoints_dir.glob("IKDriveEndpointsService*.cs"):
    typed_methods.update(parse_interface_methods(p.read_text(encoding="utf-8")))

# Typed methods already targeted by non-Typed aliases
alias_targets: set[str] = set()
for m in re.finditer(
    r"Task<[^;]{0,400}?(\w+Async)\s*\([^;]*?=>\s*(\w+TypedAsync)\s*\(",
    all_text,
    re.S,
):
    caller, target = m.group(1), m.group(2)
    if not caller.endswith("TypedAsync"):
        alias_targets.add(target)

# Also expression-bodied one-liners
for m in re.finditer(r"public\s+Task<[^>]*(?:<[^>]*>)?>\??\s+(\w+Async)\([^)]*\)\s*=>\s*(\w+TypedAsync)\(", all_text):
    if not m.group(1).endswith("TypedAsync"):
        alias_targets.add(m.group(2))

existing_names = set(re.findall(r"(?:public\s+)?Task<(?:[^<>]|<[^<>]*>)+>\??\s+(\w+)\s*\(", all_text))
# broader
existing_names |= set(re.findall(r"\b(\w+Async)\s*\(", all_text))

SPECIAL = {
    "GetAccessibleDrivesTypedAsync": "ListAccessibleDrivesAsync",
    "GetDriveUsersTypedAsync": "ListAccountUsersAsync",
    "GetUserDrivesTypedAsync": "ListUserDrivesAsync",
    "GetFileDetailsTypedAsync": None,
    "ListDirectoryFilesTypedAsync": None,
    "SearchFilesTypedAsync": None,
    "GetRecentFilesTypedAsync": None,
    "GetFavoriteFilesTypedAsync": None,
    "SearchFavoriteFilesTypedAsync": None,
    "GetMySharedFilesTypedAsync": None,
    "SearchMySharedFilesTypedAsync": None,
    "GetSharedWithMeFilesTypedAsync": None,
    "SearchSharedWithMeFilesTypedAsync": None,
    "BuildArchiveTypedAsync": None,
    "GetLargestFilesTypedAsync": None,
    "GetLastModifiedFilesTypedAsync": None,
    "GetMostVersionedFilesTypedAsync": None,
    "GetLinkedFilesTypedAsync": None,
    "SearchLinkedFilesTypedAsync": None,
    "GetDropboxFilesTypedAsync": None,
    "CreateDropboxTypedAsync": None,
    "SearchDropboxFilesTypedAsync": None,
    "GetFileByNameTypedAsync": None,
    "DuplicateFileTypedAsync": None,
    "UnlockFileTypedAsync": None,
    "CountDirectoryElementsTypedAsync": None,
    "GetFileVersionsTypedAsync": None,
    "RestoreFileVersionTypedAsync": None,
    "RestoreFileVersionToDirectoryTypedAsync": None,
    "GetFileActivitiesTypedAsync": None,
    "ConvertFileTypedAsync": None,
    "CreateDefaultFileTypedAsync": None,
    "CreateTeamDirectoryTypedAsync": None,
    "GetRootFilesActivitiesTypedAsync": None,
    "GetDriveActivitiesTypedAsync": None,
    "GetDriveActivitiesTotalTypedAsync": None,
    "WakeDriveTypedAsync": None,
    "GetDriveUsersV3TypedAsync": None,
    "SearchTrashFilesTypedAsync": None,
    "GetTrashedDirectoryFilesTypedAsync": None,
    "CountTrashedDirectoryElementsTypedAsync": None,
    "CreateDirectoryTypedAsync": None,
    "RenameFileTypedAsync": None,
    "MoveFileTypedAsync": None,
    "CopyFileTypedAsync": None,
    "TrashFileTypedAsync": None,
    "GetTrashTypedAsync": None,
    "GetTrashFileTypedAsync": None,
    "RestoreTrashedFileTypedAsync": None,
    "EmptyTrashTypedAsync": None,
    "PermanentlyDeleteTrashedFileTypedAsync": None,
    "GetTrashCountTypedAsync": None,
    "AddFavoriteTypedAsync": None,
    "RemoveFavoriteTypedAsync": None,
    "UndoTypedAsync": None,
    "CheckFilesExistTypedAsync": None,
    "GetFileHashTypedAsync": None,
    "GetFileSizesTypedAsync": None,
    "GetFileTemporaryUrlTypedAsync": None,
    "SetFileLastModifiedTypedAsync": None,
    "GetShareLinkTypedAsync": None,
    "CreateShareLinkTypedAsync": None,
    "UpdateShareLinkTypedAsync": None,
    "DeleteShareLinkTypedAsync": None,
    "InviteShareLinkTypedAsync": None,
    "GetFileCommentsTypedAsync": None,
    "GetFileCommentTypedAsync": None,
    "CreateFileCommentTypedAsync": None,
    "ReplyFileCommentTypedAsync": None,
    "UpdateFileCommentTypedAsync": None,
    "DeleteFileCommentTypedAsync": None,
    "LikeFileCommentTypedAsync": None,
    "UnlikeFileCommentTypedAsync": None,
    "GetCategoriesTypedAsync": None,
    "CreateCategoryTypedAsync": None,
    "AddCategoryToFileTypedAsync": None,
    "RemoveCategoryFromFileTypedAsync": None,
    "GetFileDropboxTypedAsync": None,
    "CreateFileDropboxTypedAsync": None,
    "UpdateFileDropboxTypedAsync": None,
    "DeleteFileDropboxTypedAsync": None,
    "InviteFileDropboxTypedAsync": None,
    "GetFileAccessTypedAsync": None,
    "AddFileAccessUsersTypedAsync": None,
    "RemoveFileAccessUserTypedAsync": None,
    "GetDriveInvitationsTypedAsync": None,
    "SendDriveInvitationTypedAsync": None,
    "GetImportsTypedAsync": None,
    "GetDriveTypedAsync": None,
    "GetArchivedFilesTypedAsync": None,
    "CancelUploadSessionTypedAsync": None,
    "CancelUploadSessionsBatchTypedAsync": None,
    "GetTrashedFileCountTypedAsync": None,
    "GetTrashedFileCountV3TypedAsync": "CountTrashedItemAsync",
    "ListDriveUsersTypedAsync": "ListDriveMemberUsersAsync",
    # Collisions with JsonNode XxxAsync — distinct business names
    "BuildShareLinkArchiveTypedAsync": "CreateShareLinkArchiveAsync",
    "CancelImportTypedAsync": "CancelImportJobAsync",
    "CancelUploadByPathTypedAsync": "CancelUploadAtPathAsync",
    "CreateActivityReportTypedAsync": "CreateDriveActivityReportAsync",
    "CreateDriveUserTypedAsync": "AddDriveUserAsync",
    "DeclineDriveAccessRequestTypedAsync": "DeclineAccessRequestAsync",
    "DeleteActivityReportTypedAsync": "DeleteDriveActivityReportAsync",
    "DeleteCategoryTypedAsync": "RemoveDriveCategoryAsync",
    "DeleteDriveInvitationTypedAsync": "DeleteInvitationAsync",
    "DeleteDriveUserTypedAsync": "RemoveDriveUserAsync",
    "DeleteImportTypedAsync": "DeleteImportJobAsync",
    "DeleteImportsHistoryTypedAsync": "ClearImportsHistoryAsync",
    "FinishUploadSessionBatchV3TypedAsync": "FinishUploadSessionsBatchAsync",
    "GetActivityReportExportTypedAsync": "GetDriveActivityReportExportAsync",
    "GetActivityReportTypedAsync": "GetDriveActivityReportAsync",
    "GetActivityReportsTypedAsync": "GetDriveActivityReportsAsync",
    "GetCategoryRightsTypedAsync": "GetDriveCategoryRightsAsync",
    "GetDriveAccessRequestTypedAsync": "GetAccessRequestAsync",
    "GetDriveInvitationTypedAsync": "GetInvitationAsync",
    "GetDrivePreferencesTypedAsync": "GetUserDrivePreferencesAsync",
    "GetDriveSettingsTypedAsync": "GetDriveSettingsInfoAsync",
    "GetDriveUserTypedAsync": "GetDriveMemberAsync",
    "GetGlobalPreferencesTypedAsync": "GetUserGlobalPreferencesAsync",
    "GetImportTypedAsync": "GetImportJobAsync",
    "GetOAuthImportDrivesTypedAsync": "ListOAuthImportDrivesAsync",
    "GetStatisticsActivitiesExportTypedAsync": "GetActivityStatisticsExportAsync",
    "GetStatisticsActivitiesLinksExportTypedAsync": "GetLinkActivityStatisticsExportAsync",
    "GetStatisticsActivitiesLinksTypedAsync": "GetLinkActivityStatisticsAsync",
    "GetStatisticsActivitiesSharedFilesTypedAsync": "GetSharedFilesActivityStatisticsAsync",
    "GetStatisticsActivitiesTypedAsync": "GetActivityStatisticsAsync",
    "GetStatisticsActivitiesUsersTypedAsync": "GetUserActivityStatisticsAsync",
    "GetStatisticsSizesExportTypedAsync": "GetSizeStatisticsExportAsync",
    "GetStatisticsSizesTypedAsync": "GetSizeStatisticsAsync",
    "LockDriveUserTypedAsync": "LockDriveMemberAsync",
    "PatchDriveUserManagerTypedAsync": "PatchDriveMemberManagerAsync",
    "PatchGlobalPreferencesTypedAsync": "PatchUserGlobalPreferencesAsync",
    "SendCategoryAiFeedbackTypedAsync": "SendItemCategoryAiFeedbackAsync",
    "SetCategoryRightsTypedAsync": "SetDriveCategoryRightsAsync",
    "StartKdriveImportTypedAsync": "StartKdriveImportJobAsync",
    "StartOAuthImportTypedAsync": "StartOAuthImportJobAsync",
    "StartSharelinkImportTypedAsync": "StartSharelinkImportJobAsync",
    "StartUploadSessionBatchV3TypedAsync": "StartUploadSessionsBatchAsync",
    "StartWebdavImportTypedAsync": "StartWebdavImportJobAsync",
    "UnlockDriveUserTypedAsync": "UnlockDriveMemberAsync",
    "UpdateCategoryTypedAsync": "UpdateDriveCategoryAsync",
    "UpdateDriveInvitationTypedAsync": "UpdateInvitationAsync",
    "UpdateDrivePreferencesTypedAsync": "UpdateUserDrivePreferencesAsync",
    "UpdateDriveSettingsAiTypedAsync": "UpdateDriveAiSettingsAsync",
    "UpdateDriveSettingsLinkTypedAsync": "UpdateDriveLinkSettingsAsync",
    "UpdateDriveSettingsOfficeTypedAsync": "UpdateDriveOfficeSettingsAsync",
    "UpdateDriveSettingsTrashTypedAsync": "UpdateDriveTrashSettingsAsync",
    "UpdateDriveTypedAsync": "UpdateDriveInfoAsync",
    "UpdateDriveUserTypedAsync": "UpdateDriveMemberAsync",
}


def propose_alias(typed_name: str) -> str | None:
    if typed_name in SPECIAL:
        return SPECIAL[typed_name]
    base = typed_name[: -len("TypedAsync")] + "Async"
    candidates = []
    if "File" in base:
        candidates.append(base.replace("File", "Item", 1))
        candidates.append(base.replace("File", "Item"))
    if "Directory" in base:
        candidates.append(base.replace("Directory", "Folder"))
    candidates.append(base)
    for c in candidates:
        if c not in existing_names and c != typed_name:
            return c
    return None


needed: list[tuple[str, str, str, str]] = []
skipped_existing = []
skipped_none = []

for typed_name, (ret, params) in sorted(typed_methods.items()):
    if typed_name in alias_targets:
        skipped_existing.append(typed_name)
        continue
    alias = propose_alias(typed_name)
    if alias is None:
        skipped_none.append(typed_name)
        continue
    if alias in existing_names:
        skipped_existing.append(typed_name)
        continue
    needed.append((alias, typed_name, ret, params))
    existing_names.add(alias)

print(f"Typed methods: {len(typed_methods)}")
print(f"Already aliased: {len(skipped_existing)}")
print(f"Skipped no name: {len(skipped_none)}")
print(f"To generate: {len(needed)}")
for a, t, _, _ in needed:
    print(f"  {a} => {t}")
if skipped_none:
    print("NO NAME:")
    for t in skipped_none:
        print(f"  {t}")


def format_params_call(params: str) -> str:
    if not params.strip():
        return ""
    parts = []
    depth = 0
    current = []
    for ch in params:
        if ch == "<":
            depth += 1
        elif ch == ">":
            depth -= 1
        if ch == "," and depth == 0:
            parts.append("".join(current).strip())
            current = []
        else:
            current.append(ch)
    if current:
        parts.append("".join(current).strip())
    names = []
    for p in parts:
        if not p:
            continue
        p = p.split("=")[0].strip()
        name = p.split()[-1].lstrip("?")
        names.append(name)
    return ", ".join(names)


iface_lines = [
    "using kDriveClient.Models;",
    "",
    "namespace kDriveClient.kDriveClient.Application.Endpoints",
    "{",
    "    public partial interface IKDriveEndpointsService",
    "    {",
]
svc_lines = [
    "using kDriveClient.Models;",
    "",
    "namespace kDriveClient.kDriveClient.Application.Endpoints",
    "{",
    "    public sealed partial class KDriveEndpointsService",
    "    {",
]
client_iface = [
    "using kDriveClient.Models;",
    "",
    "namespace kDriveClient.kDriveClient",
    "{",
    "    public partial interface IKDriveClient",
    "    {",
]
client_impl = [
    "using kDriveClient.Models;",
    "",
    "namespace kDriveClient.kDriveClient",
    "{",
    "    public partial class KDriveClient",
    "    {",
]

for alias, typed, ret, params in needed:
    call_args = format_params_call(params)
    doc = f'        /// <summary>Business alias for <c>{typed}</c>.</summary>'
    # Task return with nullable type inside
    ret_decl = f"Task<{ret}>"

    iface_lines.append(doc)
    iface_lines.append(f"        {ret_decl} {alias}({params});")
    iface_lines.append("")

    svc_lines.append(doc)
    svc_lines.append(f"        public {ret_decl} {alias}({params})")
    svc_lines.append(f"            => {typed}({call_args});")
    svc_lines.append("")

    client_iface.append(doc)
    client_iface.append(f"        {ret_decl} {alias}({params});")
    client_iface.append("")

    client_impl.append(doc)
    client_impl.append(f"        public {ret_decl} {alias}({params})")
    client_impl.append(f"            => EndpointsService.{alias}({call_args});")
    client_impl.append("")

iface_lines += ["    }", "}"]
svc_lines += ["    }", "}"]
client_iface += ["    }", "}"]
client_impl += ["    }", "}"]

(endpoints_dir / "IKDriveEndpointsService.ExtendedAliases.cs").write_text("\n".join(iface_lines) + "\n", encoding="utf-8")
(endpoints_dir / "KDriveEndpointsService.ExtendedAliases.cs").write_text("\n".join(svc_lines) + "\n", encoding="utf-8")
(iface_dir / "IkDriveClient.ExtendedAliases.cs").write_text("\n".join(client_iface) + "\n", encoding="utf-8")
(client_dir / "Endpoints.ExtendedAliases.cs").write_text("\n".join(client_impl) + "\n", encoding="utf-8")
print("Wrote ExtendedAliases partials.")
