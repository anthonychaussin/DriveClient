"""Strict typing refactor: DTO payloads, remove JsonNode, rename TypedAsync→Async."""
from __future__ import annotations

import re
from pathlib import Path

root = Path(r"c:\Users\chaus\source\repos\DriveClient")
cs_roots = [
    root / "DriveClient" / "kDriveClient",
    root / "DriveClient" / "Helpers",
    root / "DriveClient" / "Models",
    root / "DriveClientTests",
    root / "ConsoleApp1",
]

# Method-name substring → request DTO type (for `object payload` / `object? payload`)
PAYLOAD_MAP = {
    "CreateFileDropboxTypedAsync": "KDriveDropboxRequest?",
    "UpdateFileDropboxTypedAsync": "KDriveDropboxRequest",
    "InviteFileDropboxTypedAsync": "KDriveDropboxInviteRequest",
    "EnableItemDropboxAsync": "KDriveDropboxRequest?",
    "UpdateItemDropboxAsync": "KDriveDropboxRequest",
    "InviteItemDropboxAsync": "KDriveDropboxInviteRequest",
    "SetFileAccessTypedAsync": "KDriveSetFileAccessRequest",
    "SetItemAccessAsync": "KDriveSetFileAccessRequest",
    "UpdateDriveTypedAsync": "KDriveUpdateDriveRequest",
    "UpdateDriveInfoAsync": "KDriveUpdateDriveRequest",
    "UpdateDriveSettingsAiTypedAsync": "KDriveDriveSettingsAiRequest",
    "UpdateDriveAiSettingsAsync": "KDriveDriveSettingsAiRequest",
    "UpdateDriveSettingsLinkTypedAsync": "KDriveDriveSettingsLinkRequest",
    "UpdateDriveLinkSettingsAsync": "KDriveDriveSettingsLinkRequest",
    "UpdateDriveSettingsOfficeTypedAsync": "KDriveDriveSettingsOfficeRequest",
    "UpdateDriveOfficeSettingsAsync": "KDriveDriveSettingsOfficeRequest",
    "UpdateDriveSettingsTrashTypedAsync": "KDriveDriveSettingsTrashRequest",
    "UpdateDriveTrashSettingsAsync": "KDriveDriveSettingsTrashRequest",
    "UpdateDriveInvitationTypedAsync": "KDriveUpdateInvitationRequest",
    "UpdateInvitationAsync": "KDriveUpdateInvitationRequest",
    "StartOAuthImportTypedAsync": "KDriveStartOAuthImportRequest",
    "StartOAuthImportJobAsync": "KDriveStartOAuthImportRequest",
    "StartKdriveImportTypedAsync": "KDriveStartKdriveImportRequest",
    "StartKdriveImportJobAsync": "KDriveStartKdriveImportRequest",
    "StartWebdavImportTypedAsync": "KDriveStartWebdavImportRequest",
    "StartWebdavImportJobAsync": "KDriveStartWebdavImportRequest",
    "StartSharelinkImportTypedAsync": "KDriveStartSharelinkImportRequest",
    "StartSharelinkImportJobAsync": "KDriveStartSharelinkImportRequest",
    "AddFileAccessUsersTypedAsync": "KDriveFileAccessUsersRequest",
    "ShareItemWithUsersAsync": "KDriveFileAccessUsersRequest",
    "UpdateFileAccessUserTypedAsync": "KDriveFileAccessUserUpdateRequest",
    "UpdateItemAccessUserAsync": "KDriveFileAccessUserUpdateRequest",
    "AddFileAccessTeamsTypedAsync": "KDriveFileAccessTeamsRequest",
    "AddItemAccessTeamsAsync": "KDriveFileAccessTeamsRequest",
    "UpdateFileAccessTeamTypedAsync": "KDriveFileAccessTeamUpdateRequest",
    "UpdateItemAccessTeamAsync": "KDriveFileAccessTeamUpdateRequest",
    "CreateFileAccessInvitationsTypedAsync": "KDriveFileAccessInvitationsRequest",
    "CreateItemAccessInvitationsAsync": "KDriveFileAccessInvitationsRequest",
    "CheckFileAccessInvitationsTypedAsync": "KDriveFileAccessInvitationsCheckRequest",
    "CheckItemAccessInvitationsAsync": "KDriveFileAccessInvitationsCheckRequest",
    "CreateFileAccessRequestTypedAsync": "KDriveCreateFileAccessRequestBody",
    "CreateItemAccessRequestAsync": "KDriveCreateFileAccessRequestBody",
    "GrantFileAccessApplicationsTypedAsync": "KDriveFileAccessApplicationsRequest",
    "GrantItemAccessApplicationsAsync": "KDriveFileAccessApplicationsRequest",
    "CheckFileAccessTypedAsync": "KDriveFileAccessCheckRequest",
    "CheckItemAccessAsync": "KDriveFileAccessCheckRequest",
    "ForceFileAccessTypedAsync": "KDriveFileAccessForceRequest",
    "ForceItemAccessAsync": "KDriveFileAccessForceRequest",
    "SendCategoryAiFeedbackTypedAsync": "KDriveCategoryAiFeedbackRequest",
    "SendItemCategoryAiFeedbackAsync": "KDriveCategoryAiFeedbackRequest",
    "AddCategoryOnFilesTypedAsync": "KDriveCategoryFilesRequest",
    "AddCategoryOnItemsAsync": "KDriveCategoryFilesRequest",
    "RemoveCategoryFromFilesTypedAsync": "KDriveCategoryFilesRequest",
    "RemoveCategoryFromItemsAsync": "KDriveCategoryFilesRequest",
    "UpdateFileVersionV2TypedAsync": "KDriveUpdateFileVersionRequest",
    "SetCurrentFileVersionTypedAsync": "KDriveSetCurrentVersionRequest",
    "SetCurrentItemVersionAsync": "KDriveSetCurrentVersionRequest",
    "PatchDriveUserManagerTypedAsync": "KDrivePatchUserManagerRequest",
    "PatchDriveMemberManagerAsync": "KDrivePatchUserManagerRequest",
    "CancelUploadByPathTypedAsync": "KDriveCancelUploadByPathRequest",
    "CancelUploadAtPathAsync": "KDriveCancelUploadByPathRequest",
    "StartUploadSessionBatchV3TypedAsync": "KDriveUploadSessionBatchRequest",
    "StartUploadSessionsBatchAsync": "KDriveUploadSessionBatchRequest",
    "FinishUploadSessionBatchV3TypedAsync": "KDriveFinishUploadSessionBatchRequest",
    "FinishUploadSessionsBatchAsync": "KDriveFinishUploadSessionBatchRequest",
    "PatchGlobalPreferencesTypedAsync": "KDriveUserPreferencesRequest",
    "PatchUserGlobalPreferencesAsync": "KDriveUserPreferencesRequest",
    "UpdateDrivePreferencesTypedAsync": "KDriveUserPreferencesRequest",
    "UpdateUserDrivePreferencesAsync": "KDriveUserPreferencesRequest",
    "CreateActivityReportTypedAsync": "KDriveCreateActivityReportRequest",
    "CreateDriveActivityReportAsync": "KDriveCreateActivityReportRequest",
    "CreateDriveUserTypedAsync": "KDriveCreateUserRequest",
    "AddDriveUserAsync": "KDriveCreateUserRequest",
    "UpdateDriveUserTypedAsync": "KDriveUpdateUserRequest",
    "UpdateDriveMemberAsync": "KDriveUpdateUserRequest",
    "SetCategoryRightsTypedAsync": "KDriveCategoryRightsRequest",
    "SetDriveCategoryRightsAsync": "KDriveCategoryRightsRequest",
}

RETURN_REPLACEMENTS = [
    (
        r"KDriveResourceResponse<KDriveOpenApiData>",
        "KDriveResourceResponse<KDriveStatisticsData>",
    ),
    (
        r"KDrivePagedResponse<KDriveOpenApiData>",
        "KDrivePagedResponse<KDriveFileAccessInvitation>",
    ),
    (
        r"KDriveJsonContext\.Default\.KDriveResourceResponseKDriveOpenApiData",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveStatisticsData",
    ),
    (
        r"KDriveJsonContext\.Default\.KDrivePagedResponseKDriveOpenApiData",
        "KDriveJsonContext.Default.KDrivePagedResponseKDriveFileAccessInvitation",
    ),
]

# More specific return types by method
METHOD_RETURN_OVERRIDES = {
    "GetFileAccessTypedAsync": (
        "KDriveResourceResponse<KDriveFileAccess>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveFileAccess",
    ),
    "GetItemAccessAsync": (
        "KDriveResourceResponse<KDriveFileAccess>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveFileAccess",
    ),
    "GetStatisticsActivitiesLinksTypedAsync": (
        "KDrivePagedResponse<KDriveStatisticShareLink>",
        "KDriveJsonContext.Default.KDrivePagedResponseKDriveStatisticShareLink",
    ),
    "GetLinkActivityStatisticsAsync": (
        "KDrivePagedResponse<KDriveStatisticShareLink>",
        "KDriveJsonContext.Default.KDrivePagedResponseKDriveStatisticShareLink",
    ),
    "GetOAuthImportDrivesTypedAsync": (
        "KDriveResourceResponse<List<KDriveOAuthImportDrive>>",
        "KDriveJsonContext.Default.KDriveResourceResponseListKDriveOAuthImportDrive",
    ),
    "ListOAuthImportDrivesAsync": (
        "KDriveResourceResponse<List<KDriveOAuthImportDrive>>",
        "KDriveJsonContext.Default.KDriveResourceResponseListKDriveOAuthImportDrive",
    ),
    "GetActivityReportExportTypedAsync": (
        "KDriveResourceResponse<KDriveExportData>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData",
    ),
    "GetDriveActivityReportExportAsync": (
        "KDriveResourceResponse<KDriveExportData>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData",
    ),
    "GetStatisticsActivitiesLinksExportTypedAsync": (
        "KDriveResourceResponse<KDriveExportData>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData",
    ),
    "GetLinkActivityStatisticsExportAsync": (
        "KDriveResourceResponse<KDriveExportData>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData",
    ),
    "GetStatisticsActivitiesExportTypedAsync": (
        "KDriveResourceResponse<KDriveExportData>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData",
    ),
    "GetActivityStatisticsExportAsync": (
        "KDriveResourceResponse<KDriveExportData>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData",
    ),
    "GetStatisticsSizesExportTypedAsync": (
        "KDriveResourceResponse<KDriveExportData>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData",
    ),
    "GetSizeStatisticsExportAsync": (
        "KDriveResourceResponse<KDriveExportData>",
        "KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData",
    ),
}


def iter_cs() -> list[Path]:
    files = []
    for base in cs_roots:
        if not base.exists():
            continue
        files.extend(p for p in base.rglob("*.cs") if "obj" not in p.parts and "bin" not in p.parts)
    return files


def replace_payloads(text: str) -> str:
    # Generic: object? payload / object payload near method names
    for method, dto in PAYLOAD_MAP.items():
        # signature forms
        text = re.sub(
            rf"({method}\s*\([^)]*?)\bobject\?\s+payload\b",
            rf"\1{dto} payload",
            text,
        )
        text = re.sub(
            rf"({method}\s*\([^)]*?)\bobject\s+payload\b",
            rf"\1{dto.replace('?', '')} payload",
            text,
        )
    # Catch remaining CreateFileDropboxAsync etc JsonNode with object - handled by removal
    # Broader: any remaining `object payload` on known patterns in Typed/Alias files
    return text


def replace_returns(text: str) -> str:
    for old, new in RETURN_REPLACEMENTS:
        text = re.sub(old, new, text)

    for method, (ret, ctx) in METHOD_RETURN_OVERRIDES.items():
        # Fix method signature return type before method name
        text = re.sub(
            rf"Task<KDrive(?:Resource|Paged)Response<[^>]+>>\??\s+{method}\b",
            rf"Task<{ret}>? {method}",
            text,
        )
        # Fix JsonContext in body of that method (within ~400 chars after name)
        def fix_ctx(m: re.Match[str]) -> str:
            block = m.group(0)
            block = re.sub(
                r"KDriveJsonContext\.Default\.KDrive(?:Resource|Paged)Response\w+",
                ctx,
                block,
                count=1,
            )
            return block

        text = re.sub(
            rf"{method}\s*\([^;]{{0,500}}?KDriveJsonContext\.Default\.KDrive(?:Resource|Paged)Response\w+",
            fix_ctx,
            text,
            flags=re.S,
        )
    return text


def remove_jsonnode_methods(text: str) -> str:
    """Remove methods whose return type is Task<JsonNode?>."""
    lines = text.splitlines(keepends=True)
    out: list[str] = []
    i = 0
    while i < len(lines):
        line = lines[i]
        # XML doc block before JsonNode method
        if line.lstrip().startswith("///") or line.lstrip().startswith("["):
            # look ahead for JsonNode method
            j = i
            docs = []
            while j < len(lines) and (
                lines[j].lstrip().startswith("///")
                or lines[j].lstrip().startswith("[")
                or lines[j].strip() == ""
            ):
                docs.append(lines[j])
                j += 1
            if j < len(lines) and "Task<JsonNode?>" in lines[j]:
                # skip docs + method
                i = j
                # consume method until ; for interface or full body for impl
                block = lines[i]
                if block.rstrip().endswith(";"):
                    i += 1
                    continue
                # expression-bodied or block
                if "=>" in block:
                    while i < len(lines) and not lines[i].rstrip().endswith(";"):
                        i += 1
                    if i < len(lines):
                        i += 1
                    continue
                # brace body
                depth = 0
                started = False
                while i < len(lines):
                    for ch in lines[i]:
                        if ch == "{":
                            depth += 1
                            started = True
                        elif ch == "}":
                            depth -= 1
                    i += 1
                    if started and depth <= 0:
                        break
                continue
            # not a JsonNode method — keep docs
            out.extend(docs)
            i = j
            continue

        if "Task<JsonNode?>" in line:
            if line.rstrip().endswith(";"):
                i += 1
                continue
            if "=>" in line:
                while i < len(lines) and not lines[i].rstrip().endswith(";"):
                    i += 1
                if i < len(lines):
                    i += 1
                continue
            depth = 0
            started = False
            while i < len(lines):
                for ch in lines[i]:
                    if ch == "{":
                        depth += 1
                        started = True
                    elif ch == "}":
                        depth -= 1
                i += 1
                if started and depth <= 0:
                    break
            continue

        out.append(line)
        i += 1
    return "".join(out)


def rename_typed(text: str) -> str:
    text = re.sub(r"\b(?!SendTypedAsync\b)(\w+)TypedAsync\b", r"\1Async", text)
    return text


def clean_usings(text: str) -> str:
    if "JsonNode" not in text and "JsonObject" not in text and "JsonArray" not in text:
        text = re.sub(r"using System\.Text\.Json\.Nodes;\r?\n", "", text)
    return text


def process(path: Path) -> bool:
    original = path.read_text(encoding="utf-8")
    text = original
    text = replace_payloads(text)
    text = replace_returns(text)
    text = remove_jsonnode_methods(text)
    text = rename_typed(text)
    text = clean_usings(text)
    # Remove KDriveOpenApiData leftovers (except the class definition file handled separately)
    if path.name != "KDriveRequestModels.cs":
        text = text.replace("KDriveOpenApiData", "KDriveStatisticsData")
    if text != original:
        path.write_text(text, encoding="utf-8")
        return True
    return False


changed = 0
for p in iter_cs():
    if process(p):
        changed += 1
        print("updated", p.relative_to(root))

print("files changed:", changed)
