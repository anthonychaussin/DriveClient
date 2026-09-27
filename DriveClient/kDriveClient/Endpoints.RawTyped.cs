using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    public partial class KDriveClient
    {
        public Task<KDriveResourceResponse<bool>?>? LikeFileCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => EndpointsService.LikeFileCommentAsync(fileId, commentId, ct);

        public Task<KDriveResourceResponse<bool>?>? UnlikeFileCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => EndpointsService.UnlikeFileCommentAsync(fileId, commentId, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?>? CreateFileDropboxAsync(long fileId, KDriveDropboxRequest? payload = null, CancellationToken ct = default)
            => EndpointsService.CreateFileDropboxAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?>? UpdateFileDropboxAsync(long fileId, KDriveDropboxRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateFileDropboxAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? DeleteFileDropboxAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.DeleteFileDropboxAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?>? InviteFileDropboxAsync(long fileId, KDriveDropboxInviteRequest payload, CancellationToken ct = default)
            => EndpointsService.InviteFileDropboxAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveFileAccess>?>? GetFileAccessAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFileAccessAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<bool>?>? SetFileAccessAsync(long fileId, KDriveSetFileAccessRequest payload, CancellationToken ct = default)
            => EndpointsService.SetFileAccessAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveDetail>?>? UpdateDriveAsync(KDriveUpdateDriveRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?>? UpdateDriveSettingsAiAsync(KDriveDriveSettingsAiRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveSettingsAiAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?>? UpdateDriveSettingsLinkAsync(KDriveDriveSettingsLinkRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveSettingsLinkAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?>? UpdateDriveSettingsOfficeAsync(KDriveDriveSettingsOfficeRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveSettingsOfficeAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?>? UpdateDriveSettingsTrashAsync(KDriveDriveSettingsTrashRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveSettingsTrashAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveInvitation>?>? GetDriveInvitationAsync(long invitationId, CancellationToken ct = default)
            => EndpointsService.GetDriveInvitationAsync(invitationId, ct);

        public Task<KDriveResourceResponse<KDriveDriveInvitation>?>? UpdateDriveInvitationAsync(long invitationId, KDriveUpdateInvitationRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveInvitationAsync(invitationId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? DeleteDriveInvitationAsync(long invitationId, CancellationToken ct = default)
            => EndpointsService.DeleteDriveInvitationAsync(invitationId, ct);

        public Task<KDriveResourceResponse<bool>?>? SendDriveInvitationAsync(long invitationId, CancellationToken ct = default)
            => EndpointsService.SendDriveInvitationAsync(invitationId, ct);

        public Task<KDrivePagedResponse<KDriveExternalImport>?>? GetImportsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetImportsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?>? GetImportAsync(long importId, CancellationToken ct = default)
            => EndpointsService.GetImportAsync(importId, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?>? CancelImportAsync(long importId, CancellationToken ct = default)
            => EndpointsService.CancelImportAsync(importId, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?>? GetStatisticsActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetStatisticsActivitiesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?>? GetStatisticsSizesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetStatisticsSizesAsync(query, ct);

        public Task<KDrivePagedResponse<KDriveStatisticShareLink>?>? GetStatisticsActivitiesLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetStatisticsActivitiesLinksAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?>? GetStatisticsActivitiesLinksExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetStatisticsActivitiesLinksExportAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?>? GetStatisticsActivitiesUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetStatisticsActivitiesUsersAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?>? GetStatisticsActivitiesSharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetStatisticsActivitiesSharedFilesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?>? GetStatisticsActivitiesExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetStatisticsActivitiesExportAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?>? GetStatisticsSizesExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetStatisticsSizesExportAsync(query, ct);

        public Task<KDriveResourceResponse<List<KDriveOAuthImportDrive>>?>? GetOAuthImportDrivesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetOAuthImportDrivesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?>? StartOAuthImportAsync(KDriveStartOAuthImportRequest payload, CancellationToken ct = default)
            => EndpointsService.StartOAuthImportAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?>? StartKdriveImportAsync(KDriveStartKdriveImportRequest payload, CancellationToken ct = default)
            => EndpointsService.StartKdriveImportAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?>? StartWebdavImportAsync(KDriveStartWebdavImportRequest payload, CancellationToken ct = default)
            => EndpointsService.StartWebdavImportAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?>? StartSharelinkImportAsync(KDriveStartSharelinkImportRequest payload, CancellationToken ct = default)
            => EndpointsService.StartSharelinkImportAsync(payload, ct);

        public Task<KDriveResourceResponse<bool>?>? AddFileAccessUsersAsync(long fileId, KDriveFileAccessUsersRequest payload, CancellationToken ct = default)
            => EndpointsService.AddFileAccessUsersAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? UpdateFileAccessUserAsync(long fileId, long userId, KDriveFileAccessUserUpdateRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateFileAccessUserAsync(fileId, userId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? AddFileAccessTeamsAsync(long fileId, KDriveFileAccessTeamsRequest payload, CancellationToken ct = default)
            => EndpointsService.AddFileAccessTeamsAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? UpdateFileAccessTeamAsync(long fileId, long teamId, KDriveFileAccessTeamUpdateRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateFileAccessTeamAsync(fileId, teamId, payload, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessInvitation>?>? GetFileAccessInvitationsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFileAccessInvitationsAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<bool>?>? CreateFileAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsRequest payload, CancellationToken ct = default)
            => EndpointsService.CreateFileAccessInvitationsAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? CheckFileAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsCheckRequest payload, CancellationToken ct = default)
            => EndpointsService.CheckFileAccessInvitationsAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? CreateFileAccessRequestAsync(long fileId, KDriveCreateFileAccessRequestBody payload, CancellationToken ct = default)
            => EndpointsService.CreateFileAccessRequestAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? GrantFileAccessApplicationsAsync(long fileId, KDriveFileAccessApplicationsRequest payload, CancellationToken ct = default)
            => EndpointsService.GrantFileAccessApplicationsAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? CheckFileAccessAsync(long fileId, KDriveFileAccessCheckRequest payload, CancellationToken ct = default)
            => EndpointsService.CheckFileAccessAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? ForceFileAccessAsync(long fileId, KDriveFileAccessForceRequest payload, CancellationToken ct = default)
            => EndpointsService.ForceFileAccessAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? SyncParentFileAccessAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.SyncParentFileAccessAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?>? RemoveAllCategoriesFromFileAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.RemoveAllCategoriesFromFileAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?>? SendCategoryAiFeedbackAsync(long fileId, long categoryId, KDriveCategoryAiFeedbackRequest payload, CancellationToken ct = default)
            => EndpointsService.SendCategoryAiFeedbackAsync(fileId, categoryId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? AddCategoryOnFilesAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default)
            => EndpointsService.AddCategoryOnFilesAsync(categoryId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? RemoveCategoryFromFilesAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default)
            => EndpointsService.RemoveCategoryFromFilesAsync(categoryId, payload, ct);

        public Task<KDriveResourceResponse<KDriveFileVersion>?>? UpdateFileVersionV2Async(long fileId, long versionId, KDriveUpdateFileVersionRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateFileVersionV2Async(fileId, versionId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? SetCurrentFileVersionAsync(long fileId, KDriveSetCurrentVersionRequest payload, CancellationToken ct = default)
            => EndpointsService.SetCurrentFileVersionAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?>? RestoreFileVersionToDirectoryV2Async(long fileId, long versionId, long destinationDirectoryId, string? restoredName = null, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.RestoreFileVersionToDirectoryV2Async(fileId, versionId, destinationDirectoryId, restoredName, query, ct);

        public Task<KDriveResourceResponse<bool>?>? LockDriveUserAsync(long userId, CancellationToken ct = default)
            => EndpointsService.LockDriveUserAsync(userId, ct);

        public Task<KDriveResourceResponse<bool>?>? UnlockDriveUserAsync(long userId, CancellationToken ct = default)
            => EndpointsService.UnlockDriveUserAsync(userId, ct);

        public Task<KDriveResourceResponse<bool>?>? PatchDriveUserManagerAsync(long userId, KDrivePatchUserManagerRequest payload, CancellationToken ct = default)
            => EndpointsService.PatchDriveUserManagerAsync(userId, payload, ct);

        public Task<KDriveResourceResponse<bool>?>? CancelUploadByPathAsync(KDriveCancelUploadByPathRequest payload, CancellationToken ct = default)
            => EndpointsService.CancelUploadByPathAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveUploadResponse>?>? StartUploadSessionBatchV3Async(KDriveUploadSessionBatchRequest payload, CancellationToken ct = default)
            => EndpointsService.StartUploadSessionBatchV3Async(payload, ct);

        public Task<KDriveResourceResponse<KDriveUploadResponse>?>? FinishUploadSessionBatchV3Async(KDriveFinishUploadSessionBatchRequest payload, CancellationToken ct = default)
            => EndpointsService.FinishUploadSessionBatchV3Async(payload, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?>? GetActivityReportExportAsync(long reportId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetActivityReportExportAsync(reportId, query, ct);

        public Task<KDriveResourceResponse<KDriveUuidResource>?>? BuildShareLinkArchiveAsync(string sharelinkUuid, IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default)
            => EndpointsService.BuildShareLinkArchiveAsync(sharelinkUuid, fileIds, parentId, exceptFileIds, ct);

    }
}
