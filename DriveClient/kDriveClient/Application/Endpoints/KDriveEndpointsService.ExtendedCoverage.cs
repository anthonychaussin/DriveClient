using kDriveClient.Helpers;
using kDriveClient.kDriveClient.Domain.Common;
using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public sealed partial class KDriveEndpointsService
    {





































































        public Task<KDriveResourceResponse<KDriveUserPreference>?> GetGlobalPreferencesAsync(CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, "/2/drive/preferences", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveUserPreference, ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> PatchGlobalPreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Patch, "/2/drive/preferences", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveUserPreference, ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> GetDrivePreferencesAsync(CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/preferences", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveUserPreference, ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> UpdateDrivePreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/preferences", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveUserPreference, ct);

        [Obsolete("Use GetDriveActivitiesAsync / GetDriveActivityFeedAsync (v3).")]
        public Task<KDriveNavigatorResponse<KDriveActivityV2>?> GetDriveActivitiesV2Async(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/activities", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveActivityV2, ct);

        public Task<KDrivePagedResponse<KDriveActivityReport>?> GetActivityReportsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/activities/reports", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveActivityReport, ct);

        public Task<KDriveResourceResponse<KDriveActivityReport>?> CreateActivityReportAsync(KDriveCreateActivityReportRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/activities/reports", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveActivityReport, ct);

        public Task<KDriveResourceResponse<KDriveActivityReport>?> GetActivityReportAsync(long reportId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/activities/reports/{reportId}", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveActivityReport, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteActivityReportAsync(long reportId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/activities/reports/{reportId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveFileAccessRequest>?> GetDriveAccessRequestAsync(long requestId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/access/requests/{requestId}", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileAccessRequest, ct);

        public Task<KDriveResourceResponse<bool>?> DeclineDriveAccessRequestAsync(long requestId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/access/requests/{requestId}/decline", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessUser>?> GetFileAccessUsersAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/access/users", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveFileAccessUser, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessTeam>?> GetFileAccessTeamsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/access/teams", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveFileAccessTeam, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessRequest>?> GetFileAccessRequestsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/access/requests", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveFileAccessRequest, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveFileAccessUserAsync(long fileId, long userId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/access/users/{userId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveFileAccessTeamAsync(long fileId, long teamId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/access/teams/{teamId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        [Obsolete("Use GetFileVersionsAsync / GetItemVersionsAsync (v3).")]
        public Task<KDrivePagedResponse<KDriveFileVersion>?> GetFileVersionsV2Async(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/versions", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveFileVersion, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteAllFileVersionsAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/versions", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveFileVersion>?> GetFileVersionV2Async(long fileId, long versionId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/files/{fileId}/versions/{versionId}", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileVersion, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteFileVersionV2Async(long fileId, long versionId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/versions/{versionId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        [Obsolete("Use RestoreFileVersionAsync / RestoreItemVersionAsync (v3).")]
        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionV2Async(long fileId, long versionId, KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/versions/{versionId}/restore", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);

        public Task<KDrivePagedResponse<KDriveUserSummary>?> ListDriveUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/users", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveUserSummary, ct);

        public Task<KDriveResourceResponse<KDriveUserSummary>?> GetDriveUserAsync(long userId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/users/{userId}", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveUserSummary, ct);

        public Task<KDriveResourceResponse<KDriveUserSummary>?> CreateDriveUserAsync(KDriveCreateUserRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/users", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveUserSummary, ct);

        public Task<KDriveResourceResponse<KDriveUserSummary>?> UpdateDriveUserAsync(long userId, KDriveUpdateUserRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/users/{userId}", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveUserSummary, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteDriveUserAsync(long userId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/users/{userId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        [Obsolete("Use GetTrashedFileCountAsync (v3).")]
        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashedFileCountV2Async(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/trash/{fileId}/count", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveDirectoryCount, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashedFileCountAsync(long fileId, CancellationToken ct = default)
            => GetTrashedFileCountV3Async(fileId, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashedFileCountV3Async(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/3/drive/{_driveId}/trash/{fileId}/count", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveDirectoryCount, ct);

        public Task<KDriveResourceResponse<bool>?> CancelUploadSessionAsync(string sessionToken, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sessionToken);
            return _api.SendTypedAsync(HttpMethod.Delete, $"/3/drive/{_driveId}/upload/session/{sessionToken}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);
        }

        public Task<KDriveResourceResponse<bool>?> CancelUploadSessionsBatchAsync(IEnumerable<string> sessionTokens, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(sessionTokens);
            var query = new Dictionary<string, string?> { ["tokens"] = string.Join(",", sessionTokens) };
            return _api.SendTypedAsync(HttpMethod.Delete, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/upload/session/batch", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);
        }

        public Task<KDriveResourceResponse<KDriveExternalImport>?> DeleteImportAsync(long importId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/imports/{importId}", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveExternalImport, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteImportsHistoryAsync(CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/imports", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> UpdateCategoryAsync(long categoryId, string? name = null, string? color = null, CancellationToken ct = default)
        {
            var payload = new Dictionary<string, object?>();
            if (!string.IsNullOrWhiteSpace(name))
                payload["name"] = name;
            if (!string.IsNullOrWhiteSpace(color))
                payload["color"] = color;
            return _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/categories/{categoryId}", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);
        }

        public Task<KDriveResourceResponse<bool>?> DeleteCategoryAsync(long categoryId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/categories/{categoryId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveCategoryPermission>?> GetCategoryRightsAsync(CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/categories/rights", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveCategoryPermission, ct);

        public Task<KDriveResourceResponse<bool>?> SetCategoryRightsAsync(KDriveCategoryRightsRequest rightsPayload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/categories/rights", rightsPayload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> AddCategoryToFileAsync(long fileId, long categoryId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/categories/{categoryId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveCategoryFromFileAsync(long fileId, long categoryId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/categories/{categoryId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);
    }
}
