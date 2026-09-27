using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public partial interface IKDriveEndpointsService
    {
        Task<KDriveResourceResponse<bool>?> LikeFileCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UnlikeFileCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDropbox>?> CreateFileDropboxAsync(long fileId, KDriveDropboxRequest? payload = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDropbox>?> UpdateFileDropboxAsync(long fileId, KDriveDropboxRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> DeleteFileDropboxAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> InviteFileDropboxAsync(long fileId, KDriveDropboxInviteRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileAccess>?> GetFileAccessAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> SetFileAccessAsync(long fileId, KDriveSetFileAccessRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveDetail>?> UpdateDriveAsync(KDriveUpdateDriveRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsAiAsync(KDriveDriveSettingsAiRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsLinkAsync(KDriveDriveSettingsLinkRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsOfficeAsync(KDriveDriveSettingsOfficeRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsTrashAsync(KDriveDriveSettingsTrashRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveInvitation>?> GetDriveInvitationAsync(long invitationId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveInvitation>?> UpdateDriveInvitationAsync(long invitationId, KDriveUpdateInvitationRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> DeleteDriveInvitationAsync(long invitationId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> SendDriveInvitationAsync(long invitationId, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveExternalImport>?> GetImportsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?> GetImportAsync(long importId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?> CancelImportAsync(long importId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsSizesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveStatisticShareLink>?> GetStatisticsActivitiesLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExportData>?> GetStatisticsActivitiesLinksExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsActivitiesUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsActivitiesSharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExportData>?> GetStatisticsActivitiesExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExportData>?> GetStatisticsSizesExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveOAuthImportDrive>>?> GetOAuthImportDrivesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?> StartOAuthImportAsync(KDriveStartOAuthImportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?> StartKdriveImportAsync(KDriveStartKdriveImportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?> StartWebdavImportAsync(KDriveStartWebdavImportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExternalImport>?> StartSharelinkImportAsync(KDriveStartSharelinkImportRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> AddFileAccessUsersAsync(long fileId, KDriveFileAccessUsersRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UpdateFileAccessUserAsync(long fileId, long userId, KDriveFileAccessUserUpdateRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> AddFileAccessTeamsAsync(long fileId, KDriveFileAccessTeamsRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UpdateFileAccessTeamAsync(long fileId, long teamId, KDriveFileAccessTeamUpdateRequest payload, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileAccessInvitation>?> GetFileAccessInvitationsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> CreateFileAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> CheckFileAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsCheckRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> CreateFileAccessRequestAsync(long fileId, KDriveCreateFileAccessRequestBody payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> GrantFileAccessApplicationsAsync(long fileId, KDriveFileAccessApplicationsRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> CheckFileAccessAsync(long fileId, KDriveFileAccessCheckRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> ForceFileAccessAsync(long fileId, KDriveFileAccessForceRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> SyncParentFileAccessAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> RemoveAllCategoriesFromFileAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> SendCategoryAiFeedbackAsync(long fileId, long categoryId, KDriveCategoryAiFeedbackRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> AddCategoryOnFilesAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> RemoveCategoryFromFilesAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileVersion>?> UpdateFileVersionV2Async(long fileId, long versionId, KDriveUpdateFileVersionRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> SetCurrentFileVersionAsync(long fileId, KDriveSetCurrentVersionRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionToDirectoryV2Async(long fileId, long versionId, long destinationDirectoryId, string? restoredName = null, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> LockDriveUserAsync(long userId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UnlockDriveUserAsync(long userId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> PatchDriveUserManagerAsync(long userId, KDrivePatchUserManagerRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> CancelUploadByPathAsync(KDriveCancelUploadByPathRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUploadResponse>?> StartUploadSessionBatchV3Async(KDriveUploadSessionBatchRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUploadResponse>?> FinishUploadSessionBatchV3Async(KDriveFinishUploadSessionBatchRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveExportData>?> GetActivityReportExportAsync(long reportId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUuidResource>?> BuildShareLinkArchiveAsync(string sharelinkUuid, IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default);

    }
}
