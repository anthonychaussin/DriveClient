using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public partial interface IKDriveEndpointsService
    {
        Task<KDriveResourceResponse<bool>?>? AddCategoryOnItemsAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? AddItemAccessTeamsAsync(long fileId, KDriveFileAccessTeamsRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUuidResource>?>? CreateShareLinkArchiveAsync(string sharelinkUuid, IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?>? CancelImportJobAsync(long importId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? CancelUploadAtPathAsync(KDriveCancelUploadByPathRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? CheckItemAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsCheckRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? CheckItemAccessAsync(long fileId, KDriveFileAccessCheckRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveExternalImport>>?>? CopyItemToDriveAsync(long fileId, long sourceDriveId, long sourceFileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveActivityReport>?>? CreateDriveActivityReportAsync(KDriveCreateActivityReportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserSummary>?>? AddDriveUserAsync(KDriveCreateUserRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? CreateItemAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? CreateItemAccessRequestAsync(long fileId, KDriveCreateFileAccessRequestBody payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeclineAccessRequestAsync(long requestId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeleteDriveActivityReportAsync(long reportId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeleteAllItemVersionsAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? RemoveDriveCategoryAsync(long categoryId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeleteInvitationAsync(long invitationId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? RemoveDriveUserAsync(long userId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?>? DeleteImportJobAsync(long importId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? ClearImportsHistoryAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUploadResponse>?>? FinishUploadSessionsBatchAsync(KDriveFinishUploadSessionBatchRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? ForceItemAccessAsync(long fileId, KDriveFileAccessForceRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExportData>?>? GetDriveActivityReportExportAsync(long reportId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveActivityReport>?>? GetDriveActivityReportAsync(long reportId, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveActivityReport>?>? GetDriveActivityReportsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCategoryPermission>?>? GetDriveCategoryRightsAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileAccessRequest>?>? GetAccessRequestAsync(long requestId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveInvitation>?>? GetInvitationAsync(long invitationId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserPreference>?>? GetUserDrivePreferencesAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?>? GetDriveSettingsInfoAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserSummary>?>? GetDriveMemberAsync(long userId, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileAccessInvitation>?>? GetItemAccessInvitationsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileAccessRequest>?>? GetItemAccessRequestsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileAccessTeam>?>? GetItemAccessTeamsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileAccessUser>?>? GetItemAccessUsersAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserPreference>?>? GetUserGlobalPreferencesAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?>? GetImportJobAsync(long importId, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveOAuthImportDrive>>?>? ListOAuthImportDrivesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExportData>?>? GetActivityStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExportData>?>? GetLinkActivityStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveStatisticShareLink>?>? GetLinkActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveStatisticsData>?>? GetSharedFilesActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveStatisticsData>?>? GetActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveStatisticsData>?>? GetUserActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExportData>?>? GetSizeStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveStatisticsData>?>? GetSizeStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?>? CountTrashedItemAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? GrantItemAccessApplicationsAsync(long fileId, KDriveFileAccessApplicationsRequest payload, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveUserSummary>?>? ListDriveMemberUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? LockDriveMemberAsync(long userId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? PatchDriveMemberManagerAsync(long userId, KDrivePatchUserManagerRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserPreference>?>? PatchUserGlobalPreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? RemoveAllCategoriesFromItemAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? RemoveCategoryFromItemsAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? RemoveItemAccessTeamAsync(long fileId, long teamId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? SendItemCategoryAiFeedbackAsync(long fileId, long categoryId, KDriveCategoryAiFeedbackRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? SetDriveCategoryRightsAsync(KDriveCategoryRightsRequest rightsPayload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? SetCurrentItemVersionAsync(long fileId, KDriveSetCurrentVersionRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? SetItemAccessAsync(long fileId, KDriveSetFileAccessRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?>? StartKdriveImportJobAsync(KDriveStartKdriveImportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?>? StartOAuthImportJobAsync(KDriveStartOAuthImportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?>? StartSharelinkImportJobAsync(KDriveStartSharelinkImportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUploadResponse>?>? StartUploadSessionsBatchAsync(KDriveUploadSessionBatchRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?>? StartWebdavImportJobAsync(KDriveStartWebdavImportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? SyncParentItemAccessAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? UnlockDriveMemberAsync(long userId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? UpdateDriveCategoryAsync(long categoryId, string? name = null, string? color = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveInvitation>?>? UpdateInvitationAsync(long invitationId, KDriveUpdateInvitationRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserPreference>?>? UpdateUserDrivePreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?>? UpdateDriveAiSettingsAsync(KDriveDriveSettingsAiRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?>? UpdateDriveLinkSettingsAsync(KDriveDriveSettingsLinkRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?>? UpdateDriveOfficeSettingsAsync(KDriveDriveSettingsOfficeRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?>? UpdateDriveTrashSettingsAsync(KDriveDriveSettingsTrashRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveDetail>?>? UpdateDriveInfoAsync(KDriveUpdateDriveRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserSummary>?>? UpdateDriveMemberAsync(long userId, KDriveUpdateUserRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? UpdateItemAccessTeamAsync(long fileId, long teamId, KDriveFileAccessTeamUpdateRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? UpdateItemAccessUserAsync(long fileId, long userId, KDriveFileAccessUserUpdateRequest payload, CancellationToken ct = default);

    }
}
