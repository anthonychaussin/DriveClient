using kDriveClient.Models;
using kDriveClient.Models.Domain;
using kDriveClient.Models.Exceptions;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// High-level helpers that compose pagination, wake, and find operations.
    /// </summary>
    /// <remarks>
    /// Prefer these methods for application code. File-system helpers return domain
    /// <see cref="KDriveItem"/> instances (<see cref="KDriveRemoteFile"/> /
    /// <see cref="KDriveDirectory"/>) instead of raw transport DTOs.
    /// </remarks>
    public interface IKDriveSmart
    {
        /// <summary>
        /// Ensures the drive is awake, throttling wake calls to at most once per interval.
        /// </summary>
        /// <param name="minInterval">Minimum time between wake API calls. Defaults to 2 minutes.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><see langword="true"/> when the drive is considered awake.</returns>
        /// <exception cref="KDriveApiException">Thrown when the wake API returns an error.</exception>
        Task<bool> EnsureDriveAwakeAsync(TimeSpan? minInterval = null, CancellationToken ct = default);

        /// <summary>Loads all drive users (v3) by following cursor pages.</summary>
        /// <param name="query">Optional typed filters.</param>
        /// <param name="pageSize">Items requested per page.</param>
        /// <param name="maxItems">Maximum aggregated users.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<IReadOnlyList<KDriveUserV3>> GetAllDriveUsersV3Async(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Searches all trashed items matching the text across cursor pages.</summary>
        Task<IReadOnlyList<KDriveItem>> SearchTrashAllAsync(string queryText, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Lists all children of a trashed directory across cursor pages.</summary>
        Task<IReadOnlyList<KDriveItem>> GetTrashChildrenAllAsync(long trashedDirectoryId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Finds the best matching trashed item by name/path ranking.</summary>
        Task<KDriveItem?> FindTrashItemAsync(string queryText, long? trashedDirectoryId = null, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Lists all versions of a file across page pagination.</summary>
        /// <param name="fileId">Remote file id (directories do not have versions).</param>
        Task<IReadOnlyList<KDriveFileVersion>> GetItemVersionsAllAsync(long fileId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Finds a file version, optionally filtering by name substring.</summary>
        Task<KDriveFileVersion?> FindItemVersionAsync(long fileId, string? nameContains = null, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Restores the latest version of a file in place.</summary>
        /// <returns>Domain item result, or null when no version exists.</returns>
        Task<KDriveItemResult?> RestoreLatestItemVersionAsync(long fileId, string? with = null, KDriveListQuery? versionsQuery = null, CancellationToken ct = default);

        /// <summary>Restores the latest version of a file into a destination directory.</summary>
        Task<KDriveItemResult?> RestoreLatestItemVersionToDirectoryAsync(long fileId, long destinationDirectoryId, string? restoredName = null, string? with = null, KDriveListQuery? versionsQuery = null, CancellationToken ct = default);

        /// <summary>Aggregates all activities for a file via cursor pagination.</summary>
        Task<IReadOnlyList<KDriveFileActivity>> GetFileActivitiesAllAsync(long fileId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Aggregates root/items activities via cursor pagination.</summary>
        Task<IReadOnlyList<KDriveFileActivity>> GetItemsActivitiesAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Aggregates the drive activity feed via cursor pagination.</summary>
        Task<IReadOnlyList<KDriveFileActivity>> GetDriveActivityFeedAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Finds a drive activity matching the text by ranking.</summary>
        Task<KDriveFileActivity?> FindDriveActivityAsync(string queryText, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Finds a file activity matching the text by ranking.</summary>
        Task<KDriveFileActivity?> FindFileActivityAsync(long fileId, string queryText, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>
        /// Lists all children of a directory by following v3 cursor pages.
        /// </summary>
        /// <param name="directoryId">Parent directory id.</param>
        /// <param name="query">Optional typed list filters and <see cref="KDriveItemIncludes"/>.</param>
        /// <param name="pageSize">Page size per request.</param>
        /// <param name="maxItems">Hard cap on aggregated items.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Domain items (<see cref="KDriveRemoteFile"/> / <see cref="KDriveDirectory"/>).</returns>
        /// <seealso cref="IKDriveFiles.GetItemsAsync"/>
        Task<IReadOnlyList<KDriveItem>> GetItemsAllAsync(long directoryId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Searches all matching items by following v3 cursor pages.</summary>
        Task<IReadOnlyList<KDriveItem>> SearchItemsAllAsync(string queryText, long? directoryId = null, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Searches all matching items using a typed <see cref="KDriveSearchQuery"/>.</summary>
        Task<IReadOnlyList<KDriveItem>> SearchItemsAllAsync(KDriveSearchQuery query, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Lists all comments on a file by following v2 page pagination.</summary>
        Task<IReadOnlyList<KDriveComment>> GetItemCommentsAllAsync(long fileId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Lists all dropbox items by following v3 cursor pages.</summary>
        Task<IReadOnlyList<KDriveItem>> GetDropboxesAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Lists all favorite items by following v3 cursor pages.</summary>
        Task<IReadOnlyList<KDriveItem>> GetFavoritesAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Lists all drive categories by following v2 page pagination.</summary>
        Task<IReadOnlyList<KDriveCategory>> ListCategoriesAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>Lists all drive invitations by following v2 page pagination.</summary>
        Task<IReadOnlyList<KDriveDriveInvitation>> ListInvitationsAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default);

        /// <summary>
        /// Discovers accessible drives for an account and optionally picks one (first non-maintenance, or matching <paramref name="preferredDriveId"/>).
        /// When the selected drive differs from the current <see cref="KDriveClient.DriveId"/>, the client is rebound via <see cref="KDriveClient.RebindDriveId"/>.
        /// </summary>
        Task<KDriveBootstrapContext> BootstrapAsync(long accountId, long? preferredDriveId = null, CancellationToken ct = default);

        /// <summary>
        /// Gets an existing folder by name under <paramref name="parentDirectoryId"/>, or creates it.
        /// Treats <c>destination_already_exists</c> / <c>file_already_exists_error</c> as success and resolves the existing id.
        /// </summary>
        Task<KDriveDirectory> GetOrCreateFolderAsync(long parentDirectoryId, string name, CancellationToken ct = default);

        /// <summary>
        /// Enumerates directory children page by page via v3 cursor pagination.
        /// </summary>
        IAsyncEnumerable<KDriveItem> EnumerateItemsAsync(long directoryId, KDriveListQuery? query = null, int pageSize = 200, CancellationToken ct = default);

        /// <summary>
        /// Enumerates search results page by page via v3 cursor pagination.
        /// </summary>
        IAsyncEnumerable<KDriveItem> EnumerateSearchAsync(KDriveSearchQuery query, int pageSize = 200, CancellationToken ct = default);

        /// <summary>Enumerates favorite items via v3 cursor pagination.</summary>
        IAsyncEnumerable<KDriveItem> EnumerateFavoritesAsync(KDriveListQuery? query = null, int pageSize = 200, CancellationToken ct = default);

        /// <summary>Enumerates trash items via v3 cursor pagination.</summary>
        IAsyncEnumerable<KDriveItem> EnumerateTrashAsync(KDriveListQuery? query = null, int pageSize = 200, CancellationToken ct = default);

        /// <summary>Enumerates shared-with-me items via v3 cursor pagination.</summary>
        IAsyncEnumerable<KDriveItem> EnumerateSharedAsync(KDriveListQuery? query = null, int pageSize = 200, CancellationToken ct = default);

        /// <summary>
        /// Builds a ZIP archive for the given files (or directory) and downloads it when ready.
        /// </summary>
        Task<Stream> BuildAndDownloadArchiveAsync(
            IEnumerable<long> fileIds,
            long? parentId = null,
            IEnumerable<long>? exceptFileIds = null,
            TimeSpan? timeout = null,
            TimeSpan? interval = null,
            CancellationToken ct = default);

        /// <summary>
        /// Polls an external import job until status is <c>done</c>, <c>failed</c>, or <c>canceled</c>.
        /// </summary>
        Task<KDriveExternalImport> WaitForImportCompleteAsync(
            long importId,
            TimeSpan? timeout = null,
            TimeSpan? interval = null,
            CancellationToken ct = default);

        /// <summary>
        /// Copies a file from another drive into <paramref name="destinationDirectoryId"/> and waits for the import job.
        /// </summary>
        Task<KDriveExternalImport> CopyBetweenDrivesAsync(
            long destinationDirectoryId,
            long sourceDriveId,
            long sourceFileId,
            TimeSpan? timeout = null,
            TimeSpan? interval = null,
            CancellationToken ct = default);

        /// <summary>
        /// Polls until <paramref name="isPending"/> returns false (or timeout).
        /// Use for API envelopes where <c>result == "asynchronous"</c>.
        /// </summary>
        Task<T?> WaitForAsyncResultAsync<T>(
            Func<CancellationToken, Task<T?>> poll,
            Func<T, bool> isPending,
            TimeSpan? timeout = null,
            TimeSpan? interval = null,
            CancellationToken ct = default);
    }
}
