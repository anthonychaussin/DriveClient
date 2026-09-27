using kDriveClient.Helpers;
using kDriveClient.kDriveClient.Domain.Common;
using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public sealed partial class KDriveEndpointsService
    {
        public Task<KDriveResourceResponse<bool>?> LikeFileCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/comments/{commentId}/like", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> UnlikeFileCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/comments/{commentId}/unlike", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?> CreateFileDropboxAsync(long fileId, KDriveDropboxRequest? payload = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/dropbox", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveDropbox, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?> UpdateFileDropboxAsync(long fileId, KDriveDropboxRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/files/{fileId}/dropbox", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveDropbox, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteFileDropboxAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/dropbox", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> InviteFileDropboxAsync(long fileId, KDriveDropboxInviteRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/dropbox/invite", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveFileAccess>?> GetFileAccessAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/access", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileAccess, ct);

        public Task<KDriveResourceResponse<bool>?> SetFileAccessAsync(long fileId, KDriveSetFileAccessRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveDriveDetail>?> UpdateDriveAsync(KDriveUpdateDriveRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveDriveDetail, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsAiAsync(KDriveDriveSettingsAiRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/settings/ai", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveDriveSettings, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsLinkAsync(KDriveDriveSettingsLinkRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/settings/link", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveDriveSettings, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsOfficeAsync(KDriveDriveSettingsOfficeRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/settings/office", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveDriveSettings, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsTrashAsync(KDriveDriveSettingsTrashRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/settings/trash", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveDriveSettings, ct);

        public Task<KDriveResourceResponse<KDriveDriveInvitation>?> GetDriveInvitationAsync(long invitationId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/users/invitation/{invitationId}", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveDriveInvitation, ct);

        public Task<KDriveResourceResponse<KDriveDriveInvitation>?> UpdateDriveInvitationAsync(long invitationId, KDriveUpdateInvitationRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/users/invitation/{invitationId}", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveDriveInvitation, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteDriveInvitationAsync(long invitationId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/users/invitation/{invitationId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> SendDriveInvitationAsync(long invitationId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/users/invitation/{invitationId}/send", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDrivePagedResponse<KDriveExternalImport>?> GetImportsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/imports", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveExternalImport, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> GetImportAsync(long importId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/imports/{importId}", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveExternalImport, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> CancelImportAsync(long importId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/imports/{importId}/cancel", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveExternalImport, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/statistics/activities", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveStatisticsData, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsSizesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/statistics/sizes", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveStatisticsData, ct);

        public Task<KDrivePagedResponse<KDriveStatisticShareLink>?> GetStatisticsActivitiesLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/statistics/activities/links", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveStatisticShareLink, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?> GetStatisticsActivitiesLinksExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/statistics/activities/links/export", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsActivitiesUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/statistics/activities/users", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveStatisticsData, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsActivitiesSharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/statistics/activities/shared_files", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveStatisticsData, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?> GetStatisticsActivitiesExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/statistics/activities/export", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?> GetStatisticsSizesExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/statistics/sizes/export", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData, ct);

        public Task<KDriveResourceResponse<List<KDriveOAuthImportDrive>>?> GetOAuthImportDrivesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/imports/oauth/drives", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseListKDriveOAuthImportDrive, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartOAuthImportAsync(KDriveStartOAuthImportRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/imports/oauth", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveExternalImport, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartKdriveImportAsync(KDriveStartKdriveImportRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/imports/kdrive", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveExternalImport, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartWebdavImportAsync(KDriveStartWebdavImportRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/imports/webdav", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveExternalImport, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartSharelinkImportAsync(KDriveStartSharelinkImportRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/imports/sharelink", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveExternalImport, ct);

        public Task<KDriveResourceResponse<bool>?> AddFileAccessUsersAsync(long fileId, KDriveFileAccessUsersRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access/users", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> UpdateFileAccessUserAsync(long fileId, long userId, KDriveFileAccessUserUpdateRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/files/{fileId}/access/users/{userId}", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> AddFileAccessTeamsAsync(long fileId, KDriveFileAccessTeamsRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access/teams", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> UpdateFileAccessTeamAsync(long fileId, long teamId, KDriveFileAccessTeamUpdateRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/files/{fileId}/access/teams/{teamId}", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessInvitation>?> GetFileAccessInvitationsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/access/invitations", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveFileAccessInvitation, ct);

        public Task<KDriveResourceResponse<bool>?> CreateFileAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access/invitations", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> CheckFileAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsCheckRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access/invitations/check", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> CreateFileAccessRequestAsync(long fileId, KDriveCreateFileAccessRequestBody payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access/requests", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> GrantFileAccessApplicationsAsync(long fileId, KDriveFileAccessApplicationsRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access/applications", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> CheckFileAccessAsync(long fileId, KDriveFileAccessCheckRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access/check", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> ForceFileAccessAsync(long fileId, KDriveFileAccessForceRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access/force", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> SyncParentFileAccessAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/access/sync-parent", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveAllCategoriesFromFileAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/categories", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> SendCategoryAiFeedbackAsync(long fileId, long categoryId, KDriveCategoryAiFeedbackRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/categories/{categoryId}/ai-feedback", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> AddCategoryOnFilesAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/categories/{categoryId}", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveCategoryFromFilesAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/categories/{categoryId}", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveFileVersion>?> UpdateFileVersionV2Async(long fileId, long versionId, KDriveUpdateFileVersionRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/files/{fileId}/versions/{versionId}", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileVersion, ct);

        public Task<KDriveResourceResponse<bool>?> SetCurrentFileVersionAsync(long fileId, KDriveSetCurrentVersionRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/files/{fileId}/versions/current", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionToDirectoryV2Async(long fileId, long versionId, long destinationDirectoryId, string? restoredName = null, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var payload = new Dictionary<string, object?>();
            if (!string.IsNullOrWhiteSpace(restoredName))
                payload["restored_name"] = restoredName;
            return _api.SendTypedAsync(HttpMethod.Post, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/versions/{versionId}/restore/{destinationDirectoryId}", query?.ToDictionary()), payload.Count == 0 ? null : payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<bool>?> LockDriveUserAsync(long userId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/users/{userId}/lock", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> UnlockDriveUserAsync(long userId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/users/{userId}/unlock", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> PatchDriveUserManagerAsync(long userId, KDrivePatchUserManagerRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Patch, $"/2/drive/{_driveId}/users/{userId}/manager", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> CancelUploadByPathAsync(KDriveCancelUploadByPathRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/upload", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveUploadResponse>?> StartUploadSessionBatchV3Async(KDriveUploadSessionBatchRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/3/drive/{_driveId}/upload/session/batch/start", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveUploadResponse, ct);

        public Task<KDriveResourceResponse<KDriveUploadResponse>?> FinishUploadSessionBatchV3Async(KDriveFinishUploadSessionBatchRequest payload, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/3/drive/{_driveId}/upload/session/batch/finish", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveUploadResponse, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?> GetActivityReportExportAsync(long reportId, KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/activities/reports/{reportId}/export", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveExportData, ct);


        public Task<KDriveResourceResponse<KDriveUuidResource>?> BuildShareLinkArchiveAsync(string sharelinkUuid, IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sharelinkUuid);
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/app/{_driveId}/share/{sharelinkUuid}/archive", BuildArchivePayload(fileIds, parentId, exceptFileIds), KDriveJsonContext.Default.KDriveResourceResponseKDriveUuidResource, ct);
        }
    }
}
