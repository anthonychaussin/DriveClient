using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    public partial class KDriveClient
    {
        public Task<KDriveResourceResponse<KDriveUserPreference>?> GetGlobalPreferencesAsync(CancellationToken ct = default)
            => EndpointsService.GetGlobalPreferencesAsync(ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> PatchGlobalPreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default)
            => EndpointsService.PatchGlobalPreferencesAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> GetDrivePreferencesAsync(CancellationToken ct = default)
            => EndpointsService.GetDrivePreferencesAsync(ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> UpdateDrivePreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDrivePreferencesAsync(payload, ct);

        public Task<KDriveNavigatorResponse<KDriveActivityV2>?> GetDriveActivitiesV2Async(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveActivitiesV2Async(query, ct);

        public Task<KDrivePagedResponse<KDriveActivityReport>?> GetActivityReportsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetActivityReportsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveActivityReport>?> CreateActivityReportAsync(KDriveCreateActivityReportRequest payload, CancellationToken ct = default)
            => EndpointsService.CreateActivityReportAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveActivityReport>?> GetActivityReportAsync(long reportId, CancellationToken ct = default)
            => EndpointsService.GetActivityReportAsync(reportId, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteActivityReportAsync(long reportId, CancellationToken ct = default)
            => EndpointsService.DeleteActivityReportAsync(reportId, ct);

        public Task<KDriveResourceResponse<KDriveFileAccessRequest>?> GetDriveAccessRequestAsync(long requestId, CancellationToken ct = default)
            => EndpointsService.GetDriveAccessRequestAsync(requestId, ct);

        public Task<KDriveResourceResponse<bool>?> DeclineDriveAccessRequestAsync(long requestId, CancellationToken ct = default)
            => EndpointsService.DeclineDriveAccessRequestAsync(requestId, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessUser>?> GetFileAccessUsersAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFileAccessUsersAsync(fileId, query, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessTeam>?> GetFileAccessTeamsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFileAccessTeamsAsync(fileId, query, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessRequest>?> GetFileAccessRequestsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFileAccessRequestsAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveFileAccessUserAsync(long fileId, long userId, CancellationToken ct = default)
            => EndpointsService.RemoveFileAccessUserAsync(fileId, userId, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveFileAccessTeamAsync(long fileId, long teamId, CancellationToken ct = default)
            => EndpointsService.RemoveFileAccessTeamAsync(fileId, teamId, ct);

        public Task<KDrivePagedResponse<KDriveFileVersion>?> GetFileVersionsV2Async(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFileVersionsV2Async(fileId, query, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteAllFileVersionsAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.DeleteAllFileVersionsAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveFileVersion>?> GetFileVersionV2Async(long fileId, long versionId, CancellationToken ct = default)
            => EndpointsService.GetFileVersionV2Async(fileId, versionId, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteFileVersionV2Async(long fileId, long versionId, CancellationToken ct = default)
            => EndpointsService.DeleteFileVersionV2Async(fileId, versionId, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionV2Async(long fileId, long versionId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.RestoreFileVersionV2Async(fileId, versionId, query, ct);

        public Task<KDrivePagedResponse<KDriveUserSummary>?> ListDriveUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.ListDriveUsersAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveUserSummary>?> GetDriveUserAsync(long userId, CancellationToken ct = default)
            => EndpointsService.GetDriveUserAsync(userId, ct);

        public Task<KDriveResourceResponse<KDriveUserSummary>?> CreateDriveUserAsync(KDriveCreateUserRequest payload, CancellationToken ct = default)
            => EndpointsService.CreateDriveUserAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveUserSummary>?> UpdateDriveUserAsync(long userId, KDriveUpdateUserRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveUserAsync(userId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteDriveUserAsync(long userId, CancellationToken ct = default)
            => EndpointsService.DeleteDriveUserAsync(userId, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashedFileCountV2Async(long fileId, CancellationToken ct = default)
            => EndpointsService.GetTrashedFileCountV2Async(fileId, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashedFileCountAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.GetTrashedFileCountAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashedFileCountV3Async(long fileId, CancellationToken ct = default)
            => EndpointsService.GetTrashedFileCountV3Async(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> CancelUploadSessionAsync(string sessionToken, CancellationToken ct = default)
            => EndpointsService.CancelUploadSessionAsync(sessionToken, ct);

        public Task<KDriveResourceResponse<bool>?> CancelUploadSessionsBatchAsync(IEnumerable<string> sessionTokens, CancellationToken ct = default)
            => EndpointsService.CancelUploadSessionsBatchAsync(sessionTokens, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> DeleteImportAsync(long importId, CancellationToken ct = default)
            => EndpointsService.DeleteImportAsync(importId, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteImportsHistoryAsync(CancellationToken ct = default)
            => EndpointsService.DeleteImportsHistoryAsync(ct);

        public Task<KDriveResourceResponse<bool>?> UpdateCategoryAsync(long categoryId, string? name = null, string? color = null, CancellationToken ct = default)
            => EndpointsService.UpdateCategoryAsync(categoryId, name, color, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteCategoryAsync(long categoryId, CancellationToken ct = default)
            => EndpointsService.DeleteCategoryAsync(categoryId, ct);

        public Task<KDriveResourceResponse<KDriveCategoryPermission>?> GetCategoryRightsAsync(CancellationToken ct = default)
            => EndpointsService.GetCategoryRightsAsync(ct);

        public Task<KDriveResourceResponse<bool>?> SetCategoryRightsAsync(KDriveCategoryRightsRequest rightsPayload, CancellationToken ct = default)
            => EndpointsService.SetCategoryRightsAsync(rightsPayload, ct);

        public Task<KDriveResourceResponse<bool>?> AddCategoryToFileAsync(long fileId, long categoryId, CancellationToken ct = default)
            => EndpointsService.AddCategoryToFileAsync(fileId, categoryId, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveCategoryFromFileAsync(long fileId, long categoryId, CancellationToken ct = default)
            => EndpointsService.RemoveCategoryFromFileAsync(fileId, categoryId, ct);

    }
}
