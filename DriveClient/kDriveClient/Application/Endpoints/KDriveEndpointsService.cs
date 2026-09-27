using kDriveClient.Helpers;
using kDriveClient.kDriveClient.Application.Api;
using kDriveClient.kDriveClient.Domain.Common;
using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public sealed partial class KDriveEndpointsService : IKDriveEndpointsService
    {
        private long _driveId;
        private readonly IKDriveApiGateway _api;
        private readonly object _driveIdLock = new();

        public KDriveEndpointsService(long driveId, IKDriveApiGateway api)
        {
            _driveId = driveId;
            _api = api;
        }

        /// <summary>Updates the drive id used for subsequent endpoint calls (thread-safe).</summary>
        public void SetDriveId(long driveId)
        {
            if (driveId <= 0)
                throw new ArgumentOutOfRangeException(nameof(driveId));
            lock (_driveIdLock)
                _driveId = driveId;
        }

















































        public Task<KDrivePagedResponse<KDriveDriveSummary>?> GetAccessibleDrivesAsync(long accountId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var q = query?.ToDictionary() ?? new Dictionary<string, string?>();
            q["account_id"] = accountId.ToString();
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath("/2/drive", q), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveDriveSummary, ct);
        }

        public Task<KDrivePagedResponse<KDriveUserSummary>?> GetDriveUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath("/2/drive/users", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveUserSummary, ct);
        }

        public Task<KDrivePagedResponse<KDriveDriveUserSummary>?> GetUserDrivesAsync(long userId, long accountId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var q = query?.ToDictionary() ?? new Dictionary<string, string?>();
            q["account_id"] = accountId.ToString();
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/users/{userId}/drives", q), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveDriveUserSummary, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetFileDetailsAsync(long fileId, string? with = null, CancellationToken ct = default)
        {
            var q = BuildWithQuery(with);
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}", q), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> ListDirectoryFilesAsync(long directoryId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{directoryId}/files", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/search", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetRecentFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/recents", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetFavoriteFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/favorites", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFavoriteFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/search/favorites", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMySharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/my_shared", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchMySharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/search/my_shared", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetSharedWithMeFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/shared_with_me", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchSharedWithMeFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/search/shared_with_me", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        [Obsolete("Use BuildArchiveAsync instead. Archives are created via POST /3/drive/{driveId}/files/archives, not listed via GET.")]
        public Task<KDriveResourceResponse<KDriveUuidResource>?> GetArchivedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => BuildArchiveAsync(null, null, null, ct);

        public Task<KDriveResourceResponse<KDriveUuidResource>?> BuildArchiveAsync(IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Post, $"/3/drive/{_driveId}/files/archives", BuildArchivePayload(fileIds, parentId, exceptFileIds), KDriveJsonContext.Default.KDriveResourceResponseKDriveUuidResource, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLargestFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/largest", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLastModifiedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/last_modified", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMostVersionedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/most_versions", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLinkedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/links", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchLinkedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/search/links", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetDropboxFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/dropboxes", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveDropbox>?> CreateDropboxAsync(string name, long? parentDirectoryId = null, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            var payload = new Dictionary<string, object?> { ["name"] = name };
            if (parentDirectoryId.HasValue)
                payload["parent_directory_id"] = parentDirectoryId.Value;
            return _api.SendTypedAsync(HttpMethod.Post, $"/3/drive/{_driveId}/files/dropboxes", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveDropbox, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchDropboxFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/search/dropboxes", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetFileByNameAsync(long fileId, string name, string? with = null, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            var query = BuildWithQuery(with) ?? new Dictionary<string, string?>();
            query["name"] = name;
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/name", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> DuplicateFileAsync(long fileId, string? name = null, string? with = null, CancellationToken ct = default)
        {
            var query = BuildWithQuery(with);
            var payload = string.IsNullOrWhiteSpace(name) ? null : new Dictionary<string, object?> { ["name"] = name };
            return _api.SendTypedAsync(HttpMethod.Post, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/duplicate", query?.ToDictionary()), payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<bool>?> UnlockFileAsync(long fileId, string? path = null, string? token = null, CancellationToken ct = default)
        {
            var query = new Dictionary<string, string?>();
            if (!string.IsNullOrWhiteSpace(path))
                query["path"] = path;
            if (!string.IsNullOrWhiteSpace(token))
                query["token"] = token;
            return _api.SendTypedAsync(HttpMethod.Delete, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/lock", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);
        }

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountDirectoryElementsAsync(long fileId, int? depth = null, CancellationToken ct = default)
        {
            Dictionary<string, string?>? query = depth.HasValue
                ? new Dictionary<string, string?> { ["depth"] = depth.Value.ToString() }
                : null;

            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/count", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveDirectoryCount, ct);
        }

        public Task<KDrivePagedResponse<KDriveFileVersion>?> GetFileVersionsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/versions", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveFileVersion, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionAsync(long fileId, long versionId, string? with = null, CancellationToken ct = default)
        {
            var query = BuildWithQuery(with);
            return _api.SendTypedAsync(HttpMethod.Post, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/versions/{versionId}/restore", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionToDirectoryAsync(long fileId, long versionId, long destinationDirectoryId, string? name = null, string? with = null, CancellationToken ct = default)
        {
            var query = BuildWithQuery(with);
            var payload = string.IsNullOrWhiteSpace(name) ? null : new Dictionary<string, object?> { ["name"] = name };
            return _api.SendTypedAsync(HttpMethod.Post, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/versions/{versionId}/restore/{destinationDirectoryId}", query?.ToDictionary()), payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetFileActivitiesAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/activities", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileActivity, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> ConvertFileAsync(long fileId, string? with = null, CancellationToken ct = default)
        {
            var query = BuildWithQuery(with);
            return _api.SendTypedAsync(HttpMethod.Post, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/convert", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateDefaultFileAsync(long parentDirectoryId, string name, string type, string? with = null, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(type);
            var query = BuildWithQuery(with);
            var payload = new Dictionary<string, object?>
            {
                ["name"] = name,
                ["type"] = type
            };
            return _api.SendTypedAsync(HttpMethod.Post, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{parentDirectoryId}/file", query?.ToDictionary()), payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateTeamDirectoryAsync(string name, string? color = null, bool? forAllUser = null, string? with = null, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            var query = BuildWithQuery(with);
            var payload = new Dictionary<string, object?>
            {
                ["name"] = name
            };
            if (!string.IsNullOrWhiteSpace(color))
                payload["color"] = color;
            if (forAllUser.HasValue)
                payload["for_all_user"] = forAllUser.Value;

            return _api.SendTypedAsync(HttpMethod.Post, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/team_directory", query?.ToDictionary()), payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetRootFilesActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/activities", NormalizeWithQueryIfPresent(query)), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileActivity, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetDriveActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/activities", NormalizeWithQueryIfPresent(query)), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileActivity, ct);
        }

        public Task<KDriveResourceResponse<List<KDriveFileActivity>>?> GetDriveActivitiesTotalAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/activities/total", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseListKDriveFileActivity, ct);
        }

        public Task<KDriveResourceResponse<bool>?> WakeDriveAsync(CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Post, $"/3/drive/{_driveId}/wake", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveUserV3>?> GetDriveUsersV3Async(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/users", NormalizeWithQueryIfPresent(query)), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveUserV3, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchTrashFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/search/trash", NormalizeWithQueryIfPresent(query)), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashedDirectoryFilesAsync(long trashedDirectoryId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/trash/{trashedDirectoryId}/files", NormalizeWithQueryIfPresent(query)), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashedDirectoryElementsAsync(long trashedDirectoryId, int? depth = null, CancellationToken ct = default)
        {
            Dictionary<string, string?>? query = depth.HasValue
                ? new Dictionary<string, string?> { ["depth"] = depth.Value.ToString() }
                : null;

            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/trash/{trashedDirectoryId}/count", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveDirectoryCount, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateDirectoryAsync(long parentDirectoryId, string name, string? color = null, bool? onlyForMe = null, string? relativePath = null, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            return _api.SendTypedAsync(HttpMethod.Post, $"/3/drive/{_driveId}/files/{parentDirectoryId}/directory", BuildDirectoryPayload(name, color, onlyForMe, relativePath), KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveCancelResource>?> RenameFileAsync(long fileId, string name, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            var payload = new Dictionary<string, object?> { ["name"] = name };
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/rename", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveCancelResource, ct);
        }

        public Task<KDriveResourceResponse<KDriveCancelResource>?> MoveFileAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "error", CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Post, $"/3/drive/{_driveId}/files/{fileId}/move/{destinationDirectoryId}", BuildMoveOrCopyPayload(name, conflict), KDriveJsonContext.Default.KDriveResourceResponseKDriveCancelResource, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CopyFileAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "rename", string? with = null, CancellationToken ct = default)
        {
            var query = BuildWithQuery(with);
            return _api.SendTypedAsync(HttpMethod.Post, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/files/{fileId}/copy/{destinationDirectoryId}", query?.ToDictionary()), BuildMoveOrCopyPayload(name, conflict), KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveCancelResource>?> TrashFileAsync(long fileId, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveCancelResource, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/trash", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveNavigatorResponseKDriveFileSystemItem, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetTrashFileAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/3/drive/{_driveId}/trash/{fileId}", NormalizeWithQueryIfPresent(query)), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem, ct);
        }

        public Task<KDrivePagedResponse<KDriveDriveSummary>?> GetDrivesAsync(long accountId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetAccessibleDrivesAsync(accountId, query, ct);

        public Task<KDrivePagedResponse<KDriveUserSummary>?> GetUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetDriveUsersAsync(query, ct);

        public Task<KDrivePagedResponse<KDriveDriveUserSummary>?> GetDrivesByUserAsync(long userId, long accountId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetUserDrivesAsync(userId, accountId, query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetItemAsync(long fileId, string? with = null, CancellationToken ct = default)
            => GetFileDetailsAsync(fileId, with, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetItemsAsync(long directoryId, KDriveListQuery? query = null, CancellationToken ct = default)
            => ListDirectoryFilesAsync(directoryId, query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchItemsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => SearchFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchItemsAsync(KDriveSearchQuery query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);
            return SearchFilesAsync(query, ct);
        }

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetRecentAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetRecentFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetFavoritesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetFavoriteFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFavoritesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => SearchFavoriteFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMySharedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetMySharedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchMySharedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => SearchMySharedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetSharedWithMeAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetSharedWithMeFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchSharedWithMeAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => SearchSharedWithMeFilesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveUuidResource>?> GetArchivesAsync(IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default)
            => BuildArchiveAsync(fileIds, parentId, exceptFileIds, ct);

        internal static object? BuildArchivePayload(IEnumerable<long>? fileIds, long? parentId, IEnumerable<long>? exceptFileIds)
            => KDriveArchiveRequest.From(fileIds, parentId, exceptFileIds);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLargestAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetLargestFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLastModifiedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetLastModifiedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMostVersionedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetMostVersionedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetLinkedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => SearchLinkedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetDropboxesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetDropboxFilesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?> CreateDropboxItemAsync(string name, long? parentDirectoryId = null, CancellationToken ct = default)
            => CreateDropboxAsync(name, parentDirectoryId, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchDropboxesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => SearchDropboxFilesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> FindItemByNameAsync(long fileId, string name, string? with = null, CancellationToken ct = default)
            => GetFileByNameAsync(fileId, name, with, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> DuplicateItemAsync(long fileId, string? name = null, string? with = null, CancellationToken ct = default)
            => DuplicateFileAsync(fileId, name, with, ct);

        public Task<KDriveResourceResponse<bool>?> UnlockItemAsync(long fileId, string? path = null, string? token = null, CancellationToken ct = default)
            => UnlockFileAsync(fileId, path, token, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountItemsAsync(long fileId, int? depth = null, CancellationToken ct = default)
            => CountDirectoryElementsAsync(fileId, depth, ct);

        public Task<KDrivePagedResponse<KDriveFileVersion>?> GetItemVersionsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetFileVersionsAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreItemVersionAsync(long fileId, long versionId, string? with = null, CancellationToken ct = default)
            => RestoreFileVersionAsync(fileId, versionId, with, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreItemVersionToDirectoryAsync(long fileId, long versionId, long destinationDirectoryId, string? name = null, string? with = null, CancellationToken ct = default)
            => RestoreFileVersionToDirectoryAsync(fileId, versionId, destinationDirectoryId, name, with, ct);

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetItemActivitiesAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetFileActivitiesAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> ConvertItemAsync(long fileId, string? with = null, CancellationToken ct = default)
            => ConvertFileAsync(fileId, with, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateItemFileAsync(long parentDirectoryId, string name, string type, string? with = null, CancellationToken ct = default)
            => CreateDefaultFileAsync(parentDirectoryId, name, type, with, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateTeamFolderAsync(string name, string? color = null, bool? forAllUser = null, string? with = null, CancellationToken ct = default)
            => CreateTeamDirectoryAsync(name, color, forAllUser, with, ct);

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetItemsActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetRootFilesActivitiesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetDriveActivityFeedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetDriveActivitiesAsync(query, ct);

        public Task<KDriveResourceResponse<List<KDriveFileActivity>>?> GetDriveActivityTotalAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetDriveActivitiesTotalAsync(query, ct);

        public Task<KDriveResourceResponse<bool>?> WakeAsync(CancellationToken ct = default)
            => WakeDriveAsync(ct);

        public Task<KDriveNavigatorResponse<KDriveUserV3>?> GetUsersV3Async(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetDriveUsersV3Async(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchTrashAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => SearchTrashFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashChildrenAsync(long trashedDirectoryId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetTrashedDirectoryFilesAsync(trashedDirectoryId, query, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashChildrenCountAsync(long trashedDirectoryId, int? depth = null, CancellationToken ct = default)
            => CountTrashedDirectoryElementsAsync(trashedDirectoryId, depth, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateFolderAsync(long parentDirectoryId, string name, string? color = null, bool? onlyForMe = null, string? relativePath = null, CancellationToken ct = default)
            => CreateDirectoryAsync(parentDirectoryId, name, color, onlyForMe, relativePath, ct);

        public Task<KDriveResourceResponse<KDriveCancelResource>?> RenameItemAsync(long fileId, string name, CancellationToken ct = default)
            => RenameFileAsync(fileId, name, ct);

        public Task<KDriveResourceResponse<KDriveCancelResource>?> MoveItemAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "error", CancellationToken ct = default)
            => MoveFileAsync(fileId, destinationDirectoryId, name, conflict, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CopyItemAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "rename", string? with = null, CancellationToken ct = default)
            => CopyFileAsync(fileId, destinationDirectoryId, name, conflict, with, ct);

        public Task<KDriveResourceResponse<KDriveCancelResource>?> TrashItemAsync(long fileId, CancellationToken ct = default)
            => TrashFileAsync(fileId, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashItemsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetTrashAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetTrashItemAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetTrashFileAsync(fileId, query, ct);

        private static Dictionary<string, object?> BuildDirectoryPayload(string name, string? color, bool? onlyForMe, string? relativePath)
        {
            var payload = new Dictionary<string, object?> { ["name"] = name };
            if (!string.IsNullOrWhiteSpace(color))
                payload["color"] = color;
            if (onlyForMe.HasValue)
                payload["only_for_me"] = onlyForMe.Value;
            if (!string.IsNullOrWhiteSpace(relativePath))
                payload["relative_path"] = relativePath;
            return payload;
        }

        private static Dictionary<string, object?> BuildMoveOrCopyPayload(string? name, string conflict)
        {
            var normalizedConflict = NormalizeConflict(conflict);
            var payload = new Dictionary<string, object?> { ["conflict"] = normalizedConflict };
            if (!string.IsNullOrWhiteSpace(name))
                payload["name"] = name;
            return payload;
        }

        private static readonly HashSet<string> AllowedWithValues = new(StringComparer.OrdinalIgnoreCase)
        {
            "capabilities",
            "dropbox",
            "dropbox.capabilities",
            "external_import",
            "rewind",
            "supported_by",
            "version",
            "conversion_capabilities",
            "hash",
            "path",
            "sorted_name",
            "parents.path",
            "parents.sorted_name",
            "users",
            "teams",
            "is_favorite",
            "sharelink",
            "sharelink.views",
            "lock",
            "activity",
            "parents",
            "parents.capabilities",
            "parents.users",
            "parents.teams",
            "categories",
            "categories.category",
            "etag"
        };

        private static Dictionary<string, string?>? BuildWithQuery(string? with)
        {
            if (string.IsNullOrWhiteSpace(with))
                return null;

            return new Dictionary<string, string?> { ["with"] = NormalizeWithValue(with, nameof(with)) };
        }

        private static IDictionary<string, string?>? NormalizeWithQueryIfPresent(KDriveListQuery? query)
        {
            if (query is null)
                return null;

            var normalized = query.ToDictionary();
            if (normalized.TryGetValue("with", out var withValue) && !string.IsNullOrWhiteSpace(withValue))
            {
                normalized["with"] = NormalizeWithValue(withValue, "query[\"with\"]");
            }

            return normalized;
        }

        private static string NormalizeWithValue(string with, string paramName)
        {
            var includes = with
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (includes.Count == 0)
                throw new ArgumentException("The 'with' parameter must contain at least one include.", paramName);

            var invalid = includes.Where(x => !AllowedWithValues.Contains(x)).ToList();
            if (invalid.Count > 0)
            {
                var allowed = string.Join(", ", AllowedWithValues.OrderBy(x => x, StringComparer.OrdinalIgnoreCase));
                throw new ArgumentException($"Invalid 'with' value(s): {string.Join(", ", invalid)}. Allowed values: {allowed}", paramName);
            }

            return string.Join(",", includes);
        }

        private static string NormalizeConflict(string conflict)
        {
            if (string.IsNullOrWhiteSpace(conflict))
                return "error";

            var normalized = conflict.Trim().ToLowerInvariant();
            if (normalized is "error" or "rename" or "version")
                return normalized;

            throw new ArgumentException("Invalid conflict value. Allowed values: error, rename, version.", nameof(conflict));
        }
    }
}
