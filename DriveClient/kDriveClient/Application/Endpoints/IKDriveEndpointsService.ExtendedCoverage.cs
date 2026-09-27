using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public partial interface IKDriveEndpointsService
    {
        Task<KDriveResourceResponse<KDriveUserPreference>?>? GetGlobalPreferencesAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserPreference>?>? PatchGlobalPreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserPreference>?>? GetDrivePreferencesAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserPreference>?>? UpdateDrivePreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveActivityV2>?>? GetDriveActivitiesV2Async(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveActivityReport>?>? GetActivityReportsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveActivityReport>?>? CreateActivityReportAsync(KDriveCreateActivityReportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveActivityReport>?>? GetActivityReportAsync(long reportId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeleteActivityReportAsync(long reportId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileAccessRequest>?>? GetDriveAccessRequestAsync(long requestId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeclineDriveAccessRequestAsync(long requestId, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileAccessUser>?>? GetFileAccessUsersAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileAccessTeam>?>? GetFileAccessTeamsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileAccessRequest>?>? GetFileAccessRequestsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? RemoveFileAccessUserAsync(long fileId, long userId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? RemoveFileAccessTeamAsync(long fileId, long teamId, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileVersion>?>? GetFileVersionsV2Async(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeleteAllFileVersionsAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileVersion>?>? GetFileVersionV2Async(long fileId, long versionId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeleteFileVersionV2Async(long fileId, long versionId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?>? RestoreFileVersionV2Async(long fileId, long versionId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveUserSummary>?>? ListDriveUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserSummary>?>? GetDriveUserAsync(long userId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserSummary>?>? CreateDriveUserAsync(KDriveCreateUserRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUserSummary>?>? UpdateDriveUserAsync(long userId, KDriveUpdateUserRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeleteDriveUserAsync(long userId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?>? GetTrashedFileCountV2Async(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?>? GetTrashedFileCountAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?>? GetTrashedFileCountV3Async(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? CancelUploadSessionAsync(string sessionToken, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? CancelUploadSessionsBatchAsync(IEnumerable<string> sessionTokens, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?>? DeleteImportAsync(long importId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeleteImportsHistoryAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? UpdateCategoryAsync(long categoryId, string? name = null, string? color = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? DeleteCategoryAsync(long categoryId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCategoryPermission>?>? GetCategoryRightsAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? SetCategoryRightsAsync(KDriveCategoryRightsRequest rightsPayload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? AddCategoryToFileAsync(long fileId, long categoryId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?>? RemoveCategoryFromFileAsync(long fileId, long categoryId, CancellationToken ct = default);

    }
}
