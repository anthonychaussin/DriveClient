using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public sealed partial class KDriveEndpointsService
    {
        /// <summary>Business alias for <c>AddCategoryOnFilesAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> AddCategoryOnItemsAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default)
            => AddCategoryOnFilesAsync(categoryId, payload, ct);

        /// <summary>Business alias for <c>AddFileAccessTeamsAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> AddItemAccessTeamsAsync(long fileId, KDriveFileAccessTeamsRequest payload, CancellationToken ct = default)
            => AddFileAccessTeamsAsync(fileId, payload, ct);

        /// <summary>Business alias for <c>BuildShareLinkArchiveAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUuidResource>?> CreateShareLinkArchiveAsync(string sharelinkUuid, IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default)
            => BuildShareLinkArchiveAsync(sharelinkUuid, fileIds, parentId, exceptFileIds, ct);

        /// <summary>Business alias for <c>CancelImportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExternalImport>?> CancelImportJobAsync(long importId, CancellationToken ct = default)
            => CancelImportAsync(importId, ct);

        /// <summary>Business alias for <c>CancelUploadByPathAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> CancelUploadAtPathAsync(KDriveCancelUploadByPathRequest payload, CancellationToken ct = default)
            => CancelUploadByPathAsync(payload, ct);

        /// <summary>Business alias for <c>CheckFileAccessInvitationsAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> CheckItemAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsCheckRequest payload, CancellationToken ct = default)
            => CheckFileAccessInvitationsAsync(fileId, payload, ct);

        /// <summary>Business alias for <c>CheckFileAccessAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> CheckItemAccessAsync(long fileId, KDriveFileAccessCheckRequest payload, CancellationToken ct = default)
            => CheckFileAccessAsync(fileId, payload, ct);

        /// <summary>Business alias for <c>CopyFileToDriveAsync</c>.</summary>
        public Task<KDriveResourceResponse<List<KDriveExternalImport>>?> CopyItemToDriveAsync(long fileId, long sourceDriveId, long sourceFileId, CancellationToken ct = default)
            => CopyFileToDriveAsync(fileId, sourceDriveId, sourceFileId, ct);

        /// <summary>Business alias for <c>CreateActivityReportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveActivityReport>?> CreateDriveActivityReportAsync(KDriveCreateActivityReportRequest payload, CancellationToken ct = default)
            => CreateActivityReportAsync(payload, ct);

        /// <summary>Business alias for <c>CreateDriveUserAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUserSummary>?> AddDriveUserAsync(KDriveCreateUserRequest payload, CancellationToken ct = default)
            => CreateDriveUserAsync(payload, ct);

        /// <summary>Business alias for <c>CreateFileAccessInvitationsAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> CreateItemAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsRequest payload, CancellationToken ct = default)
            => CreateFileAccessInvitationsAsync(fileId, payload, ct);

        /// <summary>Business alias for <c>CreateFileAccessRequestAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> CreateItemAccessRequestAsync(long fileId, KDriveCreateFileAccessRequestBody payload, CancellationToken ct = default)
            => CreateFileAccessRequestAsync(fileId, payload, ct);

        /// <summary>Business alias for <c>DeclineDriveAccessRequestAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> DeclineAccessRequestAsync(long requestId, CancellationToken ct = default)
            => DeclineDriveAccessRequestAsync(requestId, ct);

        /// <summary>Business alias for <c>DeleteActivityReportAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> DeleteDriveActivityReportAsync(long reportId, CancellationToken ct = default)
            => DeleteActivityReportAsync(reportId, ct);

        /// <summary>Business alias for <c>DeleteAllFileVersionsAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> DeleteAllItemVersionsAsync(long fileId, CancellationToken ct = default)
            => DeleteAllFileVersionsAsync(fileId, ct);

        /// <summary>Business alias for <c>DeleteCategoryAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> RemoveDriveCategoryAsync(long categoryId, CancellationToken ct = default)
            => DeleteCategoryAsync(categoryId, ct);

        /// <summary>Business alias for <c>DeleteDriveInvitationAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> DeleteInvitationAsync(long invitationId, CancellationToken ct = default)
            => DeleteDriveInvitationAsync(invitationId, ct);

        /// <summary>Business alias for <c>DeleteDriveUserAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> RemoveDriveUserAsync(long userId, CancellationToken ct = default)
            => DeleteDriveUserAsync(userId, ct);

        /// <summary>Business alias for <c>DeleteImportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExternalImport>?> DeleteImportJobAsync(long importId, CancellationToken ct = default)
            => DeleteImportAsync(importId, ct);

        /// <summary>Business alias for <c>DeleteImportsHistoryAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> ClearImportsHistoryAsync(CancellationToken ct = default)
            => DeleteImportsHistoryAsync(ct);

        /// <summary>Business alias for <c>FinishUploadSessionBatchV3Async</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUploadResponse>?> FinishUploadSessionsBatchAsync(KDriveFinishUploadSessionBatchRequest payload, CancellationToken ct = default)
            => FinishUploadSessionBatchV3Async(payload, ct);

        /// <summary>Business alias for <c>ForceFileAccessAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> ForceItemAccessAsync(long fileId, KDriveFileAccessForceRequest payload, CancellationToken ct = default)
            => ForceFileAccessAsync(fileId, payload, ct);

        /// <summary>Business alias for <c>GetActivityReportExportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExportData>?> GetDriveActivityReportExportAsync(long reportId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetActivityReportExportAsync(reportId, query, ct);

        /// <summary>Business alias for <c>GetActivityReportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveActivityReport>?> GetDriveActivityReportAsync(long reportId, CancellationToken ct = default)
            => GetActivityReportAsync(reportId, ct);

        /// <summary>Business alias for <c>GetActivityReportsAsync</c>.</summary>
        public Task<KDrivePagedResponse<KDriveActivityReport>?> GetDriveActivityReportsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetActivityReportsAsync(query, ct);

        /// <summary>Business alias for <c>GetCategoryRightsAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveCategoryPermission>?> GetDriveCategoryRightsAsync(CancellationToken ct = default)
            => GetCategoryRightsAsync(ct);

        /// <summary>Business alias for <c>GetDriveAccessRequestAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveFileAccessRequest>?> GetAccessRequestAsync(long requestId, CancellationToken ct = default)
            => GetDriveAccessRequestAsync(requestId, ct);

        /// <summary>Business alias for <c>GetDriveInvitationAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveDriveInvitation>?> GetInvitationAsync(long invitationId, CancellationToken ct = default)
            => GetDriveInvitationAsync(invitationId, ct);

        /// <summary>Business alias for <c>GetDrivePreferencesAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUserPreference>?> GetUserDrivePreferencesAsync(CancellationToken ct = default)
            => GetDrivePreferencesAsync(ct);

        /// <summary>Business alias for <c>GetDriveSettingsAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveDriveSettings>?> GetDriveSettingsInfoAsync(CancellationToken ct = default)
            => GetDriveSettingsAsync(ct);

        /// <summary>Business alias for <c>GetDriveUserAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUserSummary>?> GetDriveMemberAsync(long userId, CancellationToken ct = default)
            => GetDriveUserAsync(userId, ct);

        /// <summary>Business alias for <c>GetFileAccessInvitationsAsync</c>.</summary>
        public Task<KDrivePagedResponse<KDriveFileAccessInvitation>?> GetItemAccessInvitationsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetFileAccessInvitationsAsync(fileId, query, ct);

        /// <summary>Business alias for <c>GetFileAccessRequestsAsync</c>.</summary>
        public Task<KDrivePagedResponse<KDriveFileAccessRequest>?> GetItemAccessRequestsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetFileAccessRequestsAsync(fileId, query, ct);

        /// <summary>Business alias for <c>GetFileAccessTeamsAsync</c>.</summary>
        public Task<KDrivePagedResponse<KDriveFileAccessTeam>?> GetItemAccessTeamsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetFileAccessTeamsAsync(fileId, query, ct);

        /// <summary>Business alias for <c>GetFileAccessUsersAsync</c>.</summary>
        public Task<KDrivePagedResponse<KDriveFileAccessUser>?> GetItemAccessUsersAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetFileAccessUsersAsync(fileId, query, ct);

        /// <summary>Business alias for <c>GetGlobalPreferencesAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUserPreference>?> GetUserGlobalPreferencesAsync(CancellationToken ct = default)
            => GetGlobalPreferencesAsync(ct);

        /// <summary>Business alias for <c>GetImportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExternalImport>?> GetImportJobAsync(long importId, CancellationToken ct = default)
            => GetImportAsync(importId, ct);

        /// <summary>Business alias for <c>GetOAuthImportDrivesAsync</c>.</summary>
        public Task<KDriveResourceResponse<List<KDriveOAuthImportDrive>>?> ListOAuthImportDrivesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetOAuthImportDrivesAsync(query, ct);

        /// <summary>Business alias for <c>GetStatisticsActivitiesExportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExportData>?> GetActivityStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetStatisticsActivitiesExportAsync(query, ct);

        /// <summary>Business alias for <c>GetStatisticsActivitiesLinksExportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExportData>?> GetLinkActivityStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetStatisticsActivitiesLinksExportAsync(query, ct);

        /// <summary>Business alias for <c>GetStatisticsActivitiesLinksAsync</c>.</summary>
        public Task<KDrivePagedResponse<KDriveStatisticShareLink>?> GetLinkActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetStatisticsActivitiesLinksAsync(query, ct);

        /// <summary>Business alias for <c>GetStatisticsActivitiesSharedFilesAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetSharedFilesActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetStatisticsActivitiesSharedFilesAsync(query, ct);

        /// <summary>Business alias for <c>GetStatisticsActivitiesAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetStatisticsActivitiesAsync(query, ct);

        /// <summary>Business alias for <c>GetStatisticsActivitiesUsersAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetUserActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetStatisticsActivitiesUsersAsync(query, ct);

        /// <summary>Business alias for <c>GetStatisticsSizesExportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExportData>?> GetSizeStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetStatisticsSizesExportAsync(query, ct);

        /// <summary>Business alias for <c>GetStatisticsSizesAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetSizeStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetStatisticsSizesAsync(query, ct);

        /// <summary>Business alias for <c>GetTrashedFileCountV3Async</c>.</summary>
        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashedItemAsync(long fileId, CancellationToken ct = default)
            => GetTrashedFileCountV3Async(fileId, ct);

        /// <summary>Business alias for <c>GrantFileAccessApplicationsAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> GrantItemAccessApplicationsAsync(long fileId, KDriveFileAccessApplicationsRequest payload, CancellationToken ct = default)
            => GrantFileAccessApplicationsAsync(fileId, payload, ct);

        /// <summary>Business alias for <c>ListDriveUsersAsync</c>.</summary>
        public Task<KDrivePagedResponse<KDriveUserSummary>?> ListDriveMemberUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => ListDriveUsersAsync(query, ct);

        /// <summary>Business alias for <c>LockDriveUserAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> LockDriveMemberAsync(long userId, CancellationToken ct = default)
            => LockDriveUserAsync(userId, ct);

        /// <summary>Business alias for <c>PatchDriveUserManagerAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> PatchDriveMemberManagerAsync(long userId, KDrivePatchUserManagerRequest payload, CancellationToken ct = default)
            => PatchDriveUserManagerAsync(userId, payload, ct);

        /// <summary>Business alias for <c>PatchGlobalPreferencesAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUserPreference>?> PatchUserGlobalPreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default)
            => PatchGlobalPreferencesAsync(payload, ct);

        /// <summary>Business alias for <c>RemoveAllCategoriesFromFileAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> RemoveAllCategoriesFromItemAsync(long fileId, CancellationToken ct = default)
            => RemoveAllCategoriesFromFileAsync(fileId, ct);

        /// <summary>Business alias for <c>RemoveCategoryFromFilesAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> RemoveCategoryFromItemsAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default)
            => RemoveCategoryFromFilesAsync(categoryId, payload, ct);

        /// <summary>Business alias for <c>RemoveFileAccessTeamAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> RemoveItemAccessTeamAsync(long fileId, long teamId, CancellationToken ct = default)
            => RemoveFileAccessTeamAsync(fileId, teamId, ct);

        /// <summary>Business alias for <c>SendCategoryAiFeedbackAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> SendItemCategoryAiFeedbackAsync(long fileId, long categoryId, KDriveCategoryAiFeedbackRequest payload, CancellationToken ct = default)
            => SendCategoryAiFeedbackAsync(fileId, categoryId, payload, ct);

        /// <summary>Business alias for <c>SetCategoryRightsAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> SetDriveCategoryRightsAsync(KDriveCategoryRightsRequest rightsPayload, CancellationToken ct = default)
            => SetCategoryRightsAsync(rightsPayload, ct);

        /// <summary>Business alias for <c>SetCurrentFileVersionAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> SetCurrentItemVersionAsync(long fileId, KDriveSetCurrentVersionRequest payload, CancellationToken ct = default)
            => SetCurrentFileVersionAsync(fileId, payload, ct);

        /// <summary>Business alias for <c>SetFileAccessAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> SetItemAccessAsync(long fileId, KDriveSetFileAccessRequest payload, CancellationToken ct = default)
            => SetFileAccessAsync(fileId, payload, ct);

        /// <summary>Business alias for <c>StartKdriveImportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartKdriveImportJobAsync(KDriveStartKdriveImportRequest payload, CancellationToken ct = default)
            => StartKdriveImportAsync(payload, ct);

        /// <summary>Business alias for <c>StartOAuthImportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartOAuthImportJobAsync(KDriveStartOAuthImportRequest payload, CancellationToken ct = default)
            => StartOAuthImportAsync(payload, ct);

        /// <summary>Business alias for <c>StartSharelinkImportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartSharelinkImportJobAsync(KDriveStartSharelinkImportRequest payload, CancellationToken ct = default)
            => StartSharelinkImportAsync(payload, ct);

        /// <summary>Business alias for <c>StartUploadSessionBatchV3Async</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUploadResponse>?> StartUploadSessionsBatchAsync(KDriveUploadSessionBatchRequest payload, CancellationToken ct = default)
            => StartUploadSessionBatchV3Async(payload, ct);

        /// <summary>Business alias for <c>StartWebdavImportAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartWebdavImportJobAsync(KDriveStartWebdavImportRequest payload, CancellationToken ct = default)
            => StartWebdavImportAsync(payload, ct);

        /// <summary>Business alias for <c>SyncParentFileAccessAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> SyncParentItemAccessAsync(long fileId, CancellationToken ct = default)
            => SyncParentFileAccessAsync(fileId, ct);

        /// <summary>Business alias for <c>UnlockDriveUserAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> UnlockDriveMemberAsync(long userId, CancellationToken ct = default)
            => UnlockDriveUserAsync(userId, ct);

        /// <summary>Business alias for <c>UpdateCategoryAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> UpdateDriveCategoryAsync(long categoryId, string? name = null, string? color = null, CancellationToken ct = default)
            => UpdateCategoryAsync(categoryId, name, color, ct);

        /// <summary>Business alias for <c>UpdateDriveInvitationAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveDriveInvitation>?> UpdateInvitationAsync(long invitationId, KDriveUpdateInvitationRequest payload, CancellationToken ct = default)
            => UpdateDriveInvitationAsync(invitationId, payload, ct);

        /// <summary>Business alias for <c>UpdateDrivePreferencesAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUserPreference>?> UpdateUserDrivePreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default)
            => UpdateDrivePreferencesAsync(payload, ct);

        /// <summary>Business alias for <c>UpdateDriveSettingsAiAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveAiSettingsAsync(KDriveDriveSettingsAiRequest payload, CancellationToken ct = default)
            => UpdateDriveSettingsAiAsync(payload, ct);

        /// <summary>Business alias for <c>UpdateDriveSettingsLinkAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveLinkSettingsAsync(KDriveDriveSettingsLinkRequest payload, CancellationToken ct = default)
            => UpdateDriveSettingsLinkAsync(payload, ct);

        /// <summary>Business alias for <c>UpdateDriveSettingsOfficeAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveOfficeSettingsAsync(KDriveDriveSettingsOfficeRequest payload, CancellationToken ct = default)
            => UpdateDriveSettingsOfficeAsync(payload, ct);

        /// <summary>Business alias for <c>UpdateDriveSettingsTrashAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveTrashSettingsAsync(KDriveDriveSettingsTrashRequest payload, CancellationToken ct = default)
            => UpdateDriveSettingsTrashAsync(payload, ct);

        /// <summary>Business alias for <c>UpdateDriveAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveDriveDetail>?> UpdateDriveInfoAsync(KDriveUpdateDriveRequest payload, CancellationToken ct = default)
            => UpdateDriveAsync(payload, ct);

        /// <summary>Business alias for <c>UpdateDriveUserAsync</c>.</summary>
        public Task<KDriveResourceResponse<KDriveUserSummary>?> UpdateDriveMemberAsync(long userId, KDriveUpdateUserRequest payload, CancellationToken ct = default)
            => UpdateDriveUserAsync(userId, payload, ct);

        /// <summary>Business alias for <c>UpdateFileAccessTeamAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> UpdateItemAccessTeamAsync(long fileId, long teamId, KDriveFileAccessTeamUpdateRequest payload, CancellationToken ct = default)
            => UpdateFileAccessTeamAsync(fileId, teamId, payload, ct);

        /// <summary>Business alias for <c>UpdateFileAccessUserAsync</c>.</summary>
        public Task<KDriveResourceResponse<bool>?> UpdateItemAccessUserAsync(long fileId, long userId, KDriveFileAccessUserUpdateRequest payload, CancellationToken ct = default)
            => UpdateFileAccessUserAsync(fileId, userId, payload, ct);

    }
}
