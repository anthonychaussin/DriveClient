using kDriveClient.Models;
using kDriveClient.Models.Domain;

namespace kDriveClient.kDriveClient
{
    public partial interface IKDriveFiles
    {
        /// <summary>
        /// Gets file Details.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetFileDetailsAsync(long fileId, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Lists directory Files.
        /// </summary>
        /// <param name="directoryId">Directory id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> ListDirectoryFilesAsync(long directoryId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets recent Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetRecentFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets favorite Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetFavoriteFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches favorite Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFavoriteFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets archived Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUuidResource>?> GetArchivedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Builds archive.
        /// </summary>
        /// <param name="fileIds">The file ids.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="exceptFileIds">The except file ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUuidResource>?> BuildArchiveAsync(IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default);

        /// <summary>
        /// Gets largest Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLargestFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets last Modified Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLastModifiedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets most Versioned Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMostVersionedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets linked Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLinkedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches linked Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchLinkedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets file By Name.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="name">The name.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetFileByNameAsync(long fileId, string name, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Duplicates file.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="name">The name.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> DuplicateFileAsync(long fileId, string? name = null, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Unlocks file.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="path">The path.</param>
        /// <param name="token">The token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UnlockFileAsync(long fileId, string? path = null, string? token = null, CancellationToken ct = default);

        /// <summary>
        /// Counts directory Elements.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="depth">The depth.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountDirectoryElementsAsync(long fileId, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Gets file Versions.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileVersion>?> GetFileVersionsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Restores file Version.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionAsync(long fileId, long versionId, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Restores file Version To Directory.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="destinationDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionToDirectoryAsync(long fileId, long versionId, long destinationDirectoryId, string? name = null, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Gets file Activities.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetFileActivitiesAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Converts file.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> ConvertFileAsync(long fileId, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Creates default File.
        /// </summary>
        /// <param name="parentDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateDefaultFileAsync(long parentDirectoryId, string name, string type, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Creates team Directory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        /// <param name="forAllUser">The for all user.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateTeamDirectoryAsync(string name, string? color = null, bool? forAllUser = null, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Gets root Files Activities.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetRootFilesActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches trash Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchTrashFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets trashed Directory Files.
        /// </summary>
        /// <param name="trashedDirectoryId">Directory id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashedDirectoryFilesAsync(long trashedDirectoryId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Counts trashed Directory Elements.
        /// </summary>
        /// <param name="trashedDirectoryId">Directory id.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashedDirectoryElementsAsync(long trashedDirectoryId, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Creates directory.
        /// </summary>
        /// <param name="parentDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        /// <param name="onlyForMe">The only for me.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateDirectoryAsync(long parentDirectoryId, string name, string? color = null, bool? onlyForMe = null, string? relativePath = null, CancellationToken ct = default);

        /// <summary>
        /// Renames file.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="name">The name.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCancelResource>?> RenameFileAsync(long fileId, string name, CancellationToken ct = default);

        /// <summary>
        /// Moves file.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="destinationDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="conflict">The conflict.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCancelResource>?> MoveFileAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "error", CancellationToken ct = default);

        /// <summary>
        /// Copies file.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="destinationDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="conflict">The conflict.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CopyFileAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "rename", string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Moves to trash file.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCancelResource>?> TrashFileAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets trash.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets trash File.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetTrashFileAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> GetItemAsync(long fileId, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Gets items.
        /// </summary>
        /// <param name="directoryId">Directory id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetItemsAsync(long directoryId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches items.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> SearchItemsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches items.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> SearchItemsAsync(KDriveSearchQuery query, CancellationToken ct = default);

        /// <summary>
        /// Gets recent.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetRecentAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets favorites.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetFavoritesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches favorites.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> SearchFavoritesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets archives.
        /// </summary>
        /// <param name="fileIds">The file ids.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="exceptFileIds">The except file ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUuidResource>?> GetArchivesAsync(IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default);

        /// <summary>
        /// Gets largest.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetLargestAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets last Modified.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetLastModifiedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets most Versioned.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetMostVersionedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets links.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches links.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> SearchLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Finds item By Name.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="name">The name.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> FindItemByNameAsync(long fileId, string name, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Duplicates item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="name">The name.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> DuplicateItemAsync(long fileId, string? name = null, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Unlocks item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="path">The path.</param>
        /// <param name="token">The token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UnlockItemAsync(long fileId, string? path = null, string? token = null, CancellationToken ct = default);

        /// <summary>
        /// Counts items.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="depth">The depth.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountItemsAsync(long fileId, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Gets item Versions.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileVersion>?> GetItemVersionsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Restores item Version.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> RestoreItemVersionAsync(long fileId, long versionId, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Restores item Version To Directory.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="destinationDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> RestoreItemVersionToDirectoryAsync(long fileId, long versionId, long destinationDirectoryId, string? name = null, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Gets item Activities.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetItemActivitiesAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Converts item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> ConvertItemAsync(long fileId, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Creates item File.
        /// </summary>
        /// <param name="parentDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> CreateItemFileAsync(long parentDirectoryId, string name, string type, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Creates team Folder.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        /// <param name="forAllUser">The for all user.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> CreateTeamFolderAsync(string name, string? color = null, bool? forAllUser = null, string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Gets items Activities.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetItemsActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches trash.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> SearchTrashAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets trash Children.
        /// </summary>
        /// <param name="trashedDirectoryId">Directory id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetTrashChildrenAsync(long trashedDirectoryId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets trash Children Count.
        /// </summary>
        /// <param name="trashedDirectoryId">Directory id.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashChildrenCountAsync(long trashedDirectoryId, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Creates folder.
        /// </summary>
        /// <param name="parentDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        /// <param name="onlyForMe">The only for me.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> CreateFolderAsync(long parentDirectoryId, string name, string? color = null, bool? onlyForMe = null, string? relativePath = null, CancellationToken ct = default);

        /// <summary>
        /// Renames item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="name">The name.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCancelResource>?> RenameItemAsync(long fileId, string name, CancellationToken ct = default);

        /// <summary>
        /// Moves item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="destinationDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="conflict">The conflict.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCancelResource>?> MoveItemAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "error", CancellationToken ct = default);

        /// <summary>
        /// Copies item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="destinationDirectoryId">Directory id.</param>
        /// <param name="name">The name.</param>
        /// <param name="conflict">The conflict.</param>
        /// <param name="with">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> CopyItemAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "rename", string? with = null, CancellationToken ct = default);

        /// <summary>
        /// Moves to trash item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCancelResource>?> TrashItemAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets trash Items.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetTrashItemsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets trash Item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemResult?> GetTrashItemAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Restores trashed File.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="destinationDirectoryId">Directory id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCancelResource>?> RestoreTrashedFileAsync(long fileId, long destinationDirectoryId, CancellationToken ct = default);

        /// <summary>
        /// Empties trash.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> EmptyTrashAsync(CancellationToken ct = default);

        /// <summary>
        /// Permanently deletes delete Trashed File.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> PermanentlyDeleteTrashedFileAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets trash Count.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashCountAsync(CancellationToken ct = default);

        /// <summary>
        /// Adds favorite.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> AddFavoriteAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Removes favorite.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveFavoriteAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Performs undo.
        /// </summary>
        /// <param name="cancelId">The cancel id.</param>
        /// <param name="cancelIds">The cancel ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> UndoAsync(string? cancelId = null, IEnumerable<string>? cancelIds = null, CancellationToken ct = default);

        /// <summary>
        /// Checks files Exist.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> CheckFilesExistAsync(IEnumerable<long> ids, CancellationToken ct = default);

        /// <summary>
        /// Gets file Hash.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileHash>?> GetFileHashAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets file Sizes.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSizeInfo>?> GetFileSizesAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets file Temporary Url.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveTemporaryUrl>?> GetFileTemporaryUrlAsync(long fileId, int? durationSeconds = null, CancellationToken ct = default);

        /// <summary>
        /// Performs set File Last Modified.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="lastModifiedAt">The last modified at.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SetFileLastModifiedAsync(long fileId, long lastModifiedAt, CancellationToken ct = default);

        /// <summary>
        /// Restores trash Item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="destinationDirectoryId">Directory id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCancelResource>?> RestoreTrashItemAsync(long fileId, long destinationDirectoryId, CancellationToken ct = default);

        /// <summary>
        /// Empties trash Items.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> EmptyTrashItemsAsync(CancellationToken ct = default);

        /// <summary>
        /// Performs purge Trash Item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> PurgeTrashItemAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Counts trash.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashAsync(CancellationToken ct = default);

        /// <summary>
        /// Marks as favorite item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> FavoriteItemAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Performs unfavorite Item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UnfavoriteItemAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Performs undo Operation.
        /// </summary>
        /// <param name="cancelId">The cancel id.</param>
        /// <param name="cancelIds">The cancel ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> UndoOperationAsync(string? cancelId = null, IEnumerable<string>? cancelIds = null, CancellationToken ct = default);

        /// <summary>
        /// Checks items Exist.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> CheckItemsExistAsync(IEnumerable<long> ids, CancellationToken ct = default);

        /// <summary>
        /// Gets item Hash.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileHash>?> GetItemHashAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets item Sizes.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSizeInfo>?> GetItemSizesAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets item Temporary Url.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveTemporaryUrl>?> GetItemTemporaryUrlAsync(long fileId, int? durationSeconds = null, CancellationToken ct = default);

        /// <summary>
        /// Performs touch Item Last Modified.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="lastModifiedAt">The last modified at.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> TouchItemLastModifiedAsync(long fileId, long lastModifiedAt, CancellationToken ct = default);

        /// <summary>
        /// Deletes all Item Versions.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteAllItemVersionsAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Counts trashed Item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashedItemAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Performs set Current Item Version.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SetCurrentItemVersionAsync(long fileId, KDriveSetCurrentVersionRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets file Versions V2.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileVersion>?> GetFileVersionsV2Async(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Deletes all File Versions.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteAllFileVersionsAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets file Version V2.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileVersion>?> GetFileVersionV2Async(long fileId, long versionId, CancellationToken ct = default);

        /// <summary>
        /// Deletes file Version V2.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteFileVersionV2Async(long fileId, long versionId, CancellationToken ct = default);

        /// <summary>
        /// Restores file Version V2.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionV2Async(long fileId, long versionId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets trashed File Count V2.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashedFileCountV2Async(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets trashed File Count.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashedFileCountAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets trashed File Count V3.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashedFileCountV3Async(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Updates file Version V2.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileVersion>?> UpdateFileVersionV2Async(long fileId, long versionId, KDriveUpdateFileVersionRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs set Current File Version.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SetCurrentFileVersionAsync(long fileId, KDriveSetCurrentVersionRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Restores file Version To Directory V2.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="destinationDirectoryId">Directory id.</param>
        /// <param name="restoredName">The restored name.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionToDirectoryV2Async(long fileId, long versionId, long destinationDirectoryId, string? restoredName = null, KDriveListQuery? query = null, CancellationToken ct = default);

    }
}
