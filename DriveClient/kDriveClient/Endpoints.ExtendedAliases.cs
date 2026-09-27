using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    public partial class KDriveClient
    {
        public Task<KDriveResourceResponse<bool>?> AddCategoryOnItemsAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default)
            => EndpointsService.AddCategoryOnItemsAsync(categoryId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> AddItemAccessTeamsAsync(long fileId, KDriveFileAccessTeamsRequest payload, CancellationToken ct = default)
            => EndpointsService.AddItemAccessTeamsAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveUuidResource>?> CreateShareLinkArchiveAsync(string sharelinkUuid, IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default)
            => EndpointsService.CreateShareLinkArchiveAsync(sharelinkUuid, fileIds, parentId, exceptFileIds, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> CancelImportJobAsync(long importId, CancellationToken ct = default)
            => EndpointsService.CancelImportJobAsync(importId, ct);

        public Task<KDriveResourceResponse<bool>?> CancelUploadAtPathAsync(KDriveCancelUploadByPathRequest payload, CancellationToken ct = default)
            => EndpointsService.CancelUploadAtPathAsync(payload, ct);

        public Task<KDriveResourceResponse<bool>?> CheckItemAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsCheckRequest payload, CancellationToken ct = default)
            => EndpointsService.CheckItemAccessInvitationsAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> CheckItemAccessAsync(long fileId, KDriveFileAccessCheckRequest payload, CancellationToken ct = default)
            => EndpointsService.CheckItemAccessAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<List<KDriveExternalImport>>?> CopyItemToDriveAsync(long fileId, long sourceDriveId, long sourceFileId, CancellationToken ct = default)
            => EndpointsService.CopyItemToDriveAsync(fileId, sourceDriveId, sourceFileId, ct);

        public Task<KDriveResourceResponse<KDriveActivityReport>?> CreateDriveActivityReportAsync(KDriveCreateActivityReportRequest payload, CancellationToken ct = default)
            => EndpointsService.CreateDriveActivityReportAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveUserSummary>?> AddDriveUserAsync(KDriveCreateUserRequest payload, CancellationToken ct = default)
            => EndpointsService.AddDriveUserAsync(payload, ct);

        public Task<KDriveResourceResponse<bool>?> CreateItemAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsRequest payload, CancellationToken ct = default)
            => EndpointsService.CreateItemAccessInvitationsAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> CreateItemAccessRequestAsync(long fileId, KDriveCreateFileAccessRequestBody payload, CancellationToken ct = default)
            => EndpointsService.CreateItemAccessRequestAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> DeclineAccessRequestAsync(long requestId, CancellationToken ct = default)
            => EndpointsService.DeclineAccessRequestAsync(requestId, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteDriveActivityReportAsync(long reportId, CancellationToken ct = default)
            => EndpointsService.DeleteDriveActivityReportAsync(reportId, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteAllItemVersionsAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.DeleteAllItemVersionsAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveDriveCategoryAsync(long categoryId, CancellationToken ct = default)
            => EndpointsService.RemoveDriveCategoryAsync(categoryId, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteInvitationAsync(long invitationId, CancellationToken ct = default)
            => EndpointsService.DeleteInvitationAsync(invitationId, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveDriveUserAsync(long userId, CancellationToken ct = default)
            => EndpointsService.RemoveDriveUserAsync(userId, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> DeleteImportJobAsync(long importId, CancellationToken ct = default)
            => EndpointsService.DeleteImportJobAsync(importId, ct);

        public Task<KDriveResourceResponse<bool>?> ClearImportsHistoryAsync(CancellationToken ct = default)
            => EndpointsService.ClearImportsHistoryAsync(ct);

        public Task<KDriveResourceResponse<KDriveUploadResponse>?> FinishUploadSessionsBatchAsync(KDriveFinishUploadSessionBatchRequest payload, CancellationToken ct = default)
            => EndpointsService.FinishUploadSessionsBatchAsync(payload, ct);

        public Task<KDriveResourceResponse<bool>?> ForceItemAccessAsync(long fileId, KDriveFileAccessForceRequest payload, CancellationToken ct = default)
            => EndpointsService.ForceItemAccessAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?> GetDriveActivityReportExportAsync(long reportId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveActivityReportExportAsync(reportId, query, ct);

        public Task<KDriveResourceResponse<KDriveActivityReport>?> GetDriveActivityReportAsync(long reportId, CancellationToken ct = default)
            => EndpointsService.GetDriveActivityReportAsync(reportId, ct);

        public Task<KDrivePagedResponse<KDriveActivityReport>?> GetDriveActivityReportsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveActivityReportsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveCategoryPermission>?> GetDriveCategoryRightsAsync(CancellationToken ct = default)
            => EndpointsService.GetDriveCategoryRightsAsync(ct);

        public Task<KDriveResourceResponse<KDriveFileAccessRequest>?> GetAccessRequestAsync(long requestId, CancellationToken ct = default)
            => EndpointsService.GetAccessRequestAsync(requestId, ct);

        public Task<KDriveResourceResponse<KDriveDriveInvitation>?> GetInvitationAsync(long invitationId, CancellationToken ct = default)
            => EndpointsService.GetInvitationAsync(invitationId, ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> GetUserDrivePreferencesAsync(CancellationToken ct = default)
            => EndpointsService.GetUserDrivePreferencesAsync(ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> GetDriveSettingsInfoAsync(CancellationToken ct = default)
            => EndpointsService.GetDriveSettingsInfoAsync(ct);

        public Task<KDriveResourceResponse<KDriveUserSummary>?> GetDriveMemberAsync(long userId, CancellationToken ct = default)
            => EndpointsService.GetDriveMemberAsync(userId, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessInvitation>?> GetItemAccessInvitationsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetItemAccessInvitationsAsync(fileId, query, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessRequest>?> GetItemAccessRequestsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetItemAccessRequestsAsync(fileId, query, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessTeam>?> GetItemAccessTeamsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetItemAccessTeamsAsync(fileId, query, ct);

        public Task<KDrivePagedResponse<KDriveFileAccessUser>?> GetItemAccessUsersAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetItemAccessUsersAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> GetUserGlobalPreferencesAsync(CancellationToken ct = default)
            => EndpointsService.GetUserGlobalPreferencesAsync(ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> GetImportJobAsync(long importId, CancellationToken ct = default)
            => EndpointsService.GetImportJobAsync(importId, ct);

        public Task<KDriveResourceResponse<List<KDriveOAuthImportDrive>>?> ListOAuthImportDrivesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.ListOAuthImportDrivesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?> GetActivityStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetActivityStatisticsExportAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?> GetLinkActivityStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetLinkActivityStatisticsExportAsync(query, ct);

        public Task<KDrivePagedResponse<KDriveStatisticShareLink>?> GetLinkActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetLinkActivityStatisticsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetSharedFilesActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetSharedFilesActivityStatisticsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetActivityStatisticsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetUserActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetUserActivityStatisticsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveExportData>?> GetSizeStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetSizeStatisticsExportAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveStatisticsData>?> GetSizeStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetSizeStatisticsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashedItemAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.CountTrashedItemAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> GrantItemAccessApplicationsAsync(long fileId, KDriveFileAccessApplicationsRequest payload, CancellationToken ct = default)
            => EndpointsService.GrantItemAccessApplicationsAsync(fileId, payload, ct);

        public Task<KDrivePagedResponse<KDriveUserSummary>?> ListDriveMemberUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.ListDriveMemberUsersAsync(query, ct);

        public Task<KDriveResourceResponse<bool>?> LockDriveMemberAsync(long userId, CancellationToken ct = default)
            => EndpointsService.LockDriveMemberAsync(userId, ct);

        public Task<KDriveResourceResponse<bool>?> PatchDriveMemberManagerAsync(long userId, KDrivePatchUserManagerRequest payload, CancellationToken ct = default)
            => EndpointsService.PatchDriveMemberManagerAsync(userId, payload, ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> PatchUserGlobalPreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default)
            => EndpointsService.PatchUserGlobalPreferencesAsync(payload, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveAllCategoriesFromItemAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.RemoveAllCategoriesFromItemAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveCategoryFromItemsAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default)
            => EndpointsService.RemoveCategoryFromItemsAsync(categoryId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveItemAccessTeamAsync(long fileId, long teamId, CancellationToken ct = default)
            => EndpointsService.RemoveItemAccessTeamAsync(fileId, teamId, ct);

        public Task<KDriveResourceResponse<bool>?> SendItemCategoryAiFeedbackAsync(long fileId, long categoryId, KDriveCategoryAiFeedbackRequest payload, CancellationToken ct = default)
            => EndpointsService.SendItemCategoryAiFeedbackAsync(fileId, categoryId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> SetDriveCategoryRightsAsync(KDriveCategoryRightsRequest rightsPayload, CancellationToken ct = default)
            => EndpointsService.SetDriveCategoryRightsAsync(rightsPayload, ct);

        public Task<KDriveResourceResponse<bool>?> SetCurrentItemVersionAsync(long fileId, KDriveSetCurrentVersionRequest payload, CancellationToken ct = default)
            => EndpointsService.SetCurrentItemVersionAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> SetItemAccessAsync(long fileId, KDriveSetFileAccessRequest payload, CancellationToken ct = default)
            => EndpointsService.SetItemAccessAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartKdriveImportJobAsync(KDriveStartKdriveImportRequest payload, CancellationToken ct = default)
            => EndpointsService.StartKdriveImportJobAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartOAuthImportJobAsync(KDriveStartOAuthImportRequest payload, CancellationToken ct = default)
            => EndpointsService.StartOAuthImportJobAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartSharelinkImportJobAsync(KDriveStartSharelinkImportRequest payload, CancellationToken ct = default)
            => EndpointsService.StartSharelinkImportJobAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveUploadResponse>?> StartUploadSessionsBatchAsync(KDriveUploadSessionBatchRequest payload, CancellationToken ct = default)
            => EndpointsService.StartUploadSessionsBatchAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveExternalImport>?> StartWebdavImportJobAsync(KDriveStartWebdavImportRequest payload, CancellationToken ct = default)
            => EndpointsService.StartWebdavImportJobAsync(payload, ct);

        public Task<KDriveResourceResponse<bool>?> SyncParentItemAccessAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.SyncParentItemAccessAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> UnlockDriveMemberAsync(long userId, CancellationToken ct = default)
            => EndpointsService.UnlockDriveMemberAsync(userId, ct);

        public Task<KDriveResourceResponse<bool>?> UpdateDriveCategoryAsync(long categoryId, string? name = null, string? color = null, CancellationToken ct = default)
            => EndpointsService.UpdateDriveCategoryAsync(categoryId, name, color, ct);

        public Task<KDriveResourceResponse<KDriveDriveInvitation>?> UpdateInvitationAsync(long invitationId, KDriveUpdateInvitationRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateInvitationAsync(invitationId, payload, ct);

        public Task<KDriveResourceResponse<KDriveUserPreference>?> UpdateUserDrivePreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateUserDrivePreferencesAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveAiSettingsAsync(KDriveDriveSettingsAiRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveAiSettingsAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveLinkSettingsAsync(KDriveDriveSettingsLinkRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveLinkSettingsAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveOfficeSettingsAsync(KDriveDriveSettingsOfficeRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveOfficeSettingsAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveTrashSettingsAsync(KDriveDriveSettingsTrashRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveTrashSettingsAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveDriveDetail>?> UpdateDriveInfoAsync(KDriveUpdateDriveRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveInfoAsync(payload, ct);

        public Task<KDriveResourceResponse<KDriveUserSummary>?> UpdateDriveMemberAsync(long userId, KDriveUpdateUserRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateDriveMemberAsync(userId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> UpdateItemAccessTeamAsync(long fileId, long teamId, KDriveFileAccessTeamUpdateRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateItemAccessTeamAsync(fileId, teamId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> UpdateItemAccessUserAsync(long fileId, long userId, KDriveFileAccessUserUpdateRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateItemAccessUserAsync(fileId, userId, payload, ct);

    }
}
