using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// Drive administration: users, settings, activities, imports, and statistics.
    /// </summary>
    public partial interface IKDriveDrive
    {
        /// <summary>
        /// Gets drive Users.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveUserSummary>?> GetDriveUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets user Drives.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveDriveUserSummary>?> GetUserDrivesAsync(long userId, long accountId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Activities.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetDriveActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Activities Total.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveFileActivity>>?> GetDriveActivitiesTotalAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Performs wake Drive.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> WakeDriveAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets drive Users V3.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveUserV3>?> GetDriveUsersV3Async(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drives.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveDriveSummary>?> GetDrivesAsync(long accountId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets users.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveUserSummary>?> GetUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drives By User.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveDriveUserSummary>?> GetDrivesByUserAsync(long userId, long accountId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Activity Feed.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetDriveActivityFeedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Activity Total.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveFileActivity>>?> GetDriveActivityTotalAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Performs wake.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> WakeAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets users V3.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveUserV3>?> GetUsersV3Async(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Copies file To Drive.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="sourceDriveId">The source drive id.</param>
        /// <param name="sourceFileId">The source file id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveExternalImport>>?> CopyFileToDriveAsync(long fileId, long sourceDriveId, long sourceFileId, CancellationToken ct = default);

        /// <summary>
        /// Gets drive.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveDetail>?> GetDriveAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Settings.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> GetDriveSettingsAsync(CancellationToken ct = default);

        /// <summary>
        /// Lists imports.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveExternalImport>?> ListImportsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Info.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveDetail>?> GetDriveInfoAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Cancels import Job.
        /// </summary>
        /// <param name="importId">The import id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> CancelImportJobAsync(long importId, CancellationToken ct = default);

        /// <summary>
        /// Cancels upload At Path.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CancelUploadAtPathAsync(KDriveCancelUploadByPathRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Copies item To Drive.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="sourceDriveId">The source drive id.</param>
        /// <param name="sourceFileId">The source file id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveExternalImport>>?> CopyItemToDriveAsync(long fileId, long sourceDriveId, long sourceFileId, CancellationToken ct = default);

        /// <summary>
        /// Creates drive Activity Report.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveActivityReport>?> CreateDriveActivityReportAsync(KDriveCreateActivityReportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Adds drive User.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserSummary>?> AddDriveUserAsync(KDriveCreateUserRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Deletes drive Activity Report.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteDriveActivityReportAsync(long reportId, CancellationToken ct = default);

        /// <summary>
        /// Removes drive User.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveDriveUserAsync(long userId, CancellationToken ct = default);

        /// <summary>
        /// Deletes import Job.
        /// </summary>
        /// <param name="importId">The import id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> DeleteImportJobAsync(long importId, CancellationToken ct = default);

        /// <summary>
        /// Clears imports History.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> ClearImportsHistoryAsync(CancellationToken ct = default);

        /// <summary>
        /// Finishes upload Sessions Batch.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUploadResponse>?> FinishUploadSessionsBatchAsync(KDriveFinishUploadSessionBatchRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Activity Report Export.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExportData>?> GetDriveActivityReportExportAsync(long reportId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Activity Report.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveActivityReport>?> GetDriveActivityReportAsync(long reportId, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Activity Reports.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveActivityReport>?> GetDriveActivityReportsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets user Drive Preferences.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserPreference>?> GetUserDrivePreferencesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets drive Settings Info.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> GetDriveSettingsInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets drive Member.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserSummary>?> GetDriveMemberAsync(long userId, CancellationToken ct = default);

        /// <summary>
        /// Gets user Global Preferences.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserPreference>?> GetUserGlobalPreferencesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets import Job.
        /// </summary>
        /// <param name="importId">The import id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> GetImportJobAsync(long importId, CancellationToken ct = default);

        /// <summary>
        /// Lists o Auth Import Drives.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveOAuthImportDrive>>?> ListOAuthImportDrivesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets activity Statistics Export.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExportData>?> GetActivityStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets link Activity Statistics Export.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExportData>?> GetLinkActivityStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets link Activity Statistics.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveStatisticShareLink>?> GetLinkActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets activity Statistics.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets user Activity Statistics.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetUserActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets size Statistics Export.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExportData>?> GetSizeStatisticsExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets size Statistics.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetSizeStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Lists drive Member Users.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveUserSummary>?> ListDriveMemberUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Performs lock Drive Member.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> LockDriveMemberAsync(long userId, CancellationToken ct = default);

        /// <summary>
        /// Performs patch Drive Member Manager.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> PatchDriveMemberManagerAsync(long userId, KDrivePatchUserManagerRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs patch User Global Preferences.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserPreference>?> PatchUserGlobalPreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs start Kdrive Import Job.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> StartKdriveImportJobAsync(KDriveStartKdriveImportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs start O Auth Import Job.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> StartOAuthImportJobAsync(KDriveStartOAuthImportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs start Upload Sessions Batch.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUploadResponse>?> StartUploadSessionsBatchAsync(KDriveUploadSessionBatchRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs start Webdav Import Job.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> StartWebdavImportJobAsync(KDriveStartWebdavImportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Unlocks drive Member.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UnlockDriveMemberAsync(long userId, CancellationToken ct = default);

        /// <summary>
        /// Updates user Drive Preferences.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserPreference>?> UpdateUserDrivePreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Ai Settings.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveAiSettingsAsync(KDriveDriveSettingsAiRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Link Settings.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveLinkSettingsAsync(KDriveDriveSettingsLinkRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Office Settings.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveOfficeSettingsAsync(KDriveDriveSettingsOfficeRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Trash Settings.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveTrashSettingsAsync(KDriveDriveSettingsTrashRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Info.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveDetail>?> UpdateDriveInfoAsync(KDriveUpdateDriveRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Member.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserSummary>?> UpdateDriveMemberAsync(long userId, KDriveUpdateUserRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets global Preferences.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserPreference>?> GetGlobalPreferencesAsync(CancellationToken ct = default);

        /// <summary>
        /// Performs patch Global Preferences.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserPreference>?> PatchGlobalPreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Preferences.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserPreference>?> GetDrivePreferencesAsync(CancellationToken ct = default);

        /// <summary>
        /// Updates drive Preferences.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserPreference>?> UpdateDrivePreferencesAsync(KDriveUserPreferencesRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Activities V2.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveActivityV2>?> GetDriveActivitiesV2Async(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets activity Reports.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveActivityReport>?> GetActivityReportsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Creates activity Report.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveActivityReport>?> CreateActivityReportAsync(KDriveCreateActivityReportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets activity Report.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveActivityReport>?> GetActivityReportAsync(long reportId, CancellationToken ct = default);

        /// <summary>
        /// Deletes activity Report.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteActivityReportAsync(long reportId, CancellationToken ct = default);

        /// <summary>
        /// Lists drive Users.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveUserSummary>?> ListDriveUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets drive User.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserSummary>?> GetDriveUserAsync(long userId, CancellationToken ct = default);

        /// <summary>
        /// Creates drive User.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserSummary>?> CreateDriveUserAsync(KDriveCreateUserRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive User.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUserSummary>?> UpdateDriveUserAsync(long userId, KDriveUpdateUserRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Deletes drive User.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteDriveUserAsync(long userId, CancellationToken ct = default);

        /// <summary>
        /// Cancels upload Session.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CancelUploadSessionAsync(string sessionToken, CancellationToken ct = default);

        /// <summary>
        /// Cancels upload Sessions Batch.
        /// </summary>
        /// <param name="sessionTokens">The session tokens.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CancelUploadSessionsBatchAsync(IEnumerable<string> sessionTokens, CancellationToken ct = default);

        /// <summary>
        /// Deletes import.
        /// </summary>
        /// <param name="importId">The import id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> DeleteImportAsync(long importId, CancellationToken ct = default);

        /// <summary>
        /// Deletes imports History.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteImportsHistoryAsync(CancellationToken ct = default);

        /// <summary>
        /// Updates drive.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveDetail>?> UpdateDriveAsync(KDriveUpdateDriveRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Settings Ai.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsAiAsync(KDriveDriveSettingsAiRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Settings Link.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsLinkAsync(KDriveDriveSettingsLinkRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Settings Office.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsOfficeAsync(KDriveDriveSettingsOfficeRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Settings Trash.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveSettings>?> UpdateDriveSettingsTrashAsync(KDriveDriveSettingsTrashRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets imports.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveExternalImport>?> GetImportsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets import.
        /// </summary>
        /// <param name="importId">The import id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> GetImportAsync(long importId, CancellationToken ct = default);

        /// <summary>
        /// Cancels import.
        /// </summary>
        /// <param name="importId">The import id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> CancelImportAsync(long importId, CancellationToken ct = default);

        /// <summary>
        /// Gets statistics Activities.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets statistics Sizes.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsSizesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets statistics Activities Links.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveStatisticShareLink>?> GetStatisticsActivitiesLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets statistics Activities Links Export.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExportData>?> GetStatisticsActivitiesLinksExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets statistics Activities Users.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsActivitiesUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets statistics Activities Export.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExportData>?> GetStatisticsActivitiesExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets statistics Sizes Export.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExportData>?> GetStatisticsSizesExportAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets o Auth Import Drives.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveOAuthImportDrive>>?> GetOAuthImportDrivesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Performs start O Auth Import.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> StartOAuthImportAsync(KDriveStartOAuthImportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs start Kdrive Import.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> StartKdriveImportAsync(KDriveStartKdriveImportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs start Webdav Import.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> StartWebdavImportAsync(KDriveStartWebdavImportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs lock Drive User.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> LockDriveUserAsync(long userId, CancellationToken ct = default);

        /// <summary>
        /// Unlocks drive User.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UnlockDriveUserAsync(long userId, CancellationToken ct = default);

        /// <summary>
        /// Performs patch Drive User Manager.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> PatchDriveUserManagerAsync(long userId, KDrivePatchUserManagerRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Cancels upload By Path.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CancelUploadByPathAsync(KDriveCancelUploadByPathRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs start Upload Session Batch V3.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUploadResponse>?> StartUploadSessionBatchV3Async(KDriveUploadSessionBatchRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Finishes upload Session Batch V3.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUploadResponse>?> FinishUploadSessionBatchV3Async(KDriveFinishUploadSessionBatchRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets activity Report Export.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExportData>?> GetActivityReportExportAsync(long reportId, KDriveListQuery? query = null, CancellationToken ct = default);

    }
}
