using kDriveClient.Models;
using kDriveClient.Models.Domain;
using kDriveClient.Models.Exceptions;
using System.Runtime.CompilerServices;

namespace kDriveClient.kDriveClient
{
    public partial class KDriveClient
    {
        private readonly SemaphoreSlim _wakeSemaphore = new(1, 1);
        private DateTimeOffset _lastWakeAt = DateTimeOffset.MinValue;

        /// <summary>
        /// Wakes the drive if needed, with a minimum interval between wake calls (default 2 minutes).
        /// </summary>
        /// <param name="minInterval">Minimum time between wake API calls. Defaults to 2 minutes.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><see langword="true"/> when the drive is awake (or already considered awake).</returns>
        /// <exception cref="Models.Exceptions.KDriveApiException">Thrown when the wake API returns an error.</exception>
        public async Task<bool> EnsureDriveAwakeAsync(TimeSpan? minInterval = null, CancellationToken ct = default)
        {
            var interval = minInterval ?? TimeSpan.FromMinutes(2);
            var now = DateTimeOffset.UtcNow;
            if ((now - _lastWakeAt) < interval)
                return true;

            await _wakeSemaphore.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                now = DateTimeOffset.UtcNow;
                if ((now - _lastWakeAt) < interval)
                    return true;

                var wake = await WakeDriveAsync(ct).ConfigureAwait(false);
                var ok = wake?.Data ?? false;
                if (ok)
                    _lastWakeAt = DateTimeOffset.UtcNow;

                return ok;
            }
            finally
            {
                _wakeSemaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<KDriveBootstrapContext> BootstrapAsync(long accountId, long? preferredDriveId = null, CancellationToken ct = default)
        {
            if (accountId <= 0)
                throw new ArgumentOutOfRangeException(nameof(accountId));

            var page = await GetAccessibleDrivesAsync(accountId, new KDriveListQuery { Limit = 200 }, ct).ConfigureAwait(false);
            var drives = page?.Data ?? [];
            if (drives.Count == 0)
                throw new InvalidOperationException($"No drives found for account {accountId}.");

            KDriveDriveSummary? selected = null;
            if (preferredDriveId is > 0)
                selected = drives.FirstOrDefault(d => d.Id == preferredDriveId.Value);

            selected ??= drives.FirstOrDefault(d => d.InMaintenance != true) ?? drives[0];

            if (selected.Id != DriveId)
                RebindDriveId(selected.Id);

            return new KDriveBootstrapContext
            {
                AccountId = accountId,
                DriveId = selected.Id,
                Drive = selected,
                Drives = drives
            };
        }

        /// <inheritdoc />
        public async Task<KDriveDirectory> GetOrCreateFolderAsync(long parentDirectoryId, string name, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            name = name.Trim();

            var existing = await FindDirectoryByNameAsync(parentDirectoryId, name, ct).ConfigureAwait(false);
            if (existing is not null)
                return existing;

            try
            {
                var created = await CreateFolderAsync(parentDirectoryId, name, ct: ct).ConfigureAwait(false);
                if (created?.Item is KDriveDirectory dir)
                    return dir;
                if (created?.Item is not null)
                    return KDriveDirectory.FromDto(created.Item.Source ?? new KDriveFileSystemItem
                    {
                        Id = created.Item.Id,
                        Name = created.Item.Name,
                        Type = "dir",
                        Path = created.Item.Path,
                        ParentId = created.Item.ParentId
                    });
            }
            catch (KDriveApiException ex) when (IsAlreadyExistsError(ex))
            {
                // Race: another client created the folder.
            }

            existing = await FindDirectoryByNameAsync(parentDirectoryId, name, ct).ConfigureAwait(false);
            if (existing is not null)
                return existing;

            throw new InvalidOperationException($"Folder '{name}' already exists under {parentDirectoryId} but could not be resolved.");
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<KDriveItem> EnumerateItemsAsync(
            long directoryId,
            KDriveListQuery? query = null,
            int pageSize = 200,
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            string? cursor = null;

            while (true)
            {
                var q = CloneListQuery(query);
                q.Limit = pageSize;
                q.Cursor = cursor;

                var page = await GetItemsAsyncDto(directoryId, q, ct).ConfigureAwait(false);
                if (page is null || page.Data.Count == 0)
                    yield break;

                foreach (var item in KDriveItem.FromMany(page.Data))
                    yield return item;

                if (page.HasMore != true || string.IsNullOrWhiteSpace(page.Cursor))
                    yield break;

                cursor = page.Cursor;
            }
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<KDriveItem> EnumerateSearchAsync(
            KDriveSearchQuery query,
            int pageSize = 200,
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentException.ThrowIfNullOrWhiteSpace(query.Query);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            string? cursor = null;

            while (true)
            {
                var q = CloneListQuery(query);
                if (!string.IsNullOrWhiteSpace(query.Query))
                    q.Extra["query"] = query.Query;
                if (query.DirectoryId.HasValue)
                    q.Extra["directory_id"] = query.DirectoryId.Value.ToString();
                q.Limit = pageSize;
                q.Cursor = cursor;

                var page = await SearchItemsAsyncDto(q, ct).ConfigureAwait(false);
                if (page is null || page.Data.Count == 0)
                    yield break;

                foreach (var item in KDriveItem.FromMany(page.Data))
                    yield return item;

                if (page.HasMore != true || string.IsNullOrWhiteSpace(page.Cursor))
                    yield break;

                cursor = page.Cursor;
            }
        }

        /// <inheritdoc />
        public IAsyncEnumerable<KDriveItem> EnumerateFavoritesAsync(KDriveListQuery? query = null, int pageSize = 200, CancellationToken ct = default)
            => EnumerateByCursorAsync((q, token) => EndpointsService.GetFavoritesAsync(q, token), query, pageSize, ct);

        /// <inheritdoc />
        public IAsyncEnumerable<KDriveItem> EnumerateTrashAsync(KDriveListQuery? query = null, int pageSize = 200, CancellationToken ct = default)
            => EnumerateByCursorAsync((q, token) => EndpointsService.GetTrashAsync(q, token), query, pageSize, ct);

        /// <inheritdoc />
        public IAsyncEnumerable<KDriveItem> EnumerateSharedAsync(KDriveListQuery? query = null, int pageSize = 200, CancellationToken ct = default)
            => EnumerateByCursorAsync((q, token) => EndpointsService.GetSharedWithMeAsync(q, token), query, pageSize, ct);

        /// <inheritdoc />
        public async Task<Stream> BuildAndDownloadArchiveAsync(
            IEnumerable<long> fileIds,
            long? parentId = null,
            IEnumerable<long>? exceptFileIds = null,
            TimeSpan? timeout = null,
            TimeSpan? interval = null,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(fileIds);
            var ids = fileIds as IList<long> ?? fileIds.ToList();
            if (ids.Count == 0 && parentId is null)
                throw new ArgumentException("Provide at least one file id or a parent directory id.", nameof(fileIds));

            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            var built = await BuildArchiveAsync(ids, parentId, exceptFileIds, ct).ConfigureAwait(false);
            var uuid = built?.Data?.Uuid;
            if (string.IsNullOrWhiteSpace(uuid))
                throw new InvalidOperationException("Archive build did not return a UUID.");

            var deadline = DateTimeOffset.UtcNow + (timeout ?? TimeSpan.FromMinutes(10));
            var delay = interval ?? TimeSpan.FromSeconds(2);
            Exception? lastError = null;

            while (true)
            {
                ct.ThrowIfCancellationRequested();
                try
                {
                    return await DownloadArchiveAsync(uuid, ct).ConfigureAwait(false);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    lastError = ex;
                    if (DateTimeOffset.UtcNow >= deadline)
                        throw new TimeoutException($"Timed out waiting for archive {uuid} to become available.", lastError);

                    await Task.Delay(delay, ct).ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc />
        public async Task<KDriveExternalImport> WaitForImportCompleteAsync(
            long importId,
            TimeSpan? timeout = null,
            TimeSpan? interval = null,
            CancellationToken ct = default)
        {
            var result = await WaitForAsyncResultAsync(
                async token =>
                {
                    var response = await GetImportJobAsync(importId, token).ConfigureAwait(false);
                    return response?.Data;
                },
                import =>
                {
                    var status = import.Status ?? string.Empty;
                    return status is not ("done" or "failed" or "canceled");
                },
                timeout,
                interval,
                ct).ConfigureAwait(false);

            if (result is null)
                throw new InvalidOperationException($"Import job {importId} was not found.");

            return result;
        }

        /// <inheritdoc />
        public async Task<KDriveExternalImport> CopyBetweenDrivesAsync(
            long destinationDirectoryId,
            long sourceDriveId,
            long sourceFileId,
            TimeSpan? timeout = null,
            TimeSpan? interval = null,
            CancellationToken ct = default)
        {
            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            var started = await CopyFileToDriveAsync(destinationDirectoryId, sourceDriveId, sourceFileId, ct).ConfigureAwait(false);
            var job = started?.Data?.FirstOrDefault();
            if (job?.Id is null or <= 0)
                throw new InvalidOperationException("Copy-to-drive did not return an import job id.");

            return await WaitForImportCompleteAsync(job.Id.Value, timeout, interval, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<T?> WaitForAsyncResultAsync<T>(
            Func<CancellationToken, Task<T?>> poll,
            Func<T, bool> isPending,
            TimeSpan? timeout = null,
            TimeSpan? interval = null,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(poll);
            ArgumentNullException.ThrowIfNull(isPending);

            var deadline = DateTimeOffset.UtcNow + (timeout ?? TimeSpan.FromMinutes(5));
            var delay = interval ?? TimeSpan.FromSeconds(2);

            while (true)
            {
                ct.ThrowIfCancellationRequested();
                var value = await poll(ct).ConfigureAwait(false);
                if (value is null || !isPending(value))
                    return value;

                if (DateTimeOffset.UtcNow >= deadline)
                    throw new TimeoutException("Timed out waiting for asynchronous kDrive operation.");

                await Task.Delay(delay, ct).ConfigureAwait(false);
            }
        }

        private async Task<KDriveDirectory?> FindDirectoryByNameAsync(long parentDirectoryId, string name, CancellationToken ct)
        {
            await foreach (var item in EnumerateItemsAsync(parentDirectoryId, new KDriveListQuery { Limit = 200 }, 200, ct).ConfigureAwait(false))
            {
                if (item.IsDirectory &&
                    string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase) &&
                    item is KDriveDirectory directory)
                {
                    return directory;
                }
            }

            return null;
        }

        private static bool IsAlreadyExistsError(KDriveApiException ex)
        {
            var code = ex.Error.Error.Code ?? string.Empty;
            return code.Contains("already_exists", StringComparison.OrdinalIgnoreCase)
                   || code.Contains("destination_already_exists", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Aggregates all drive users via v3 cursor pagination.
        /// </summary>
        public async Task<IReadOnlyList<KDriveUserV3>> GetAllDriveUsersV3Async(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxItems);

            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            var users = new List<KDriveUserV3>(Math.Min(maxItems, pageSize));
            string? cursor = null;

            while (users.Count < maxItems)
            {
                var q = CloneListQuery(query);
                q.Limit = pageSize;
                q.Cursor = cursor;

                var page = await GetDriveUsersV3Async(q, ct).ConfigureAwait(false);
                if (page is null)
                    break;

                if (page.Data.Count > 0)
                {
                    var remaining = maxItems - users.Count;
                    users.AddRange(page.Data.Take(remaining));
                }

                if (page.HasMore != true || string.IsNullOrWhiteSpace(page.Cursor))
                    break;

                cursor = page.Cursor;
            }

            return users;
        }

        /// <summary>
        /// Searches all trashed items matching <paramref name="queryText"/> across cursor pages.
        /// </summary>
        public async Task<IReadOnlyList<KDriveItem>> SearchTrashAllAsync(string queryText, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(queryText);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxItems);

            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            var items = new List<KDriveItem>(Math.Min(maxItems, pageSize));
            string? cursor = null;

            while (items.Count < maxItems)
            {
                var q = CloneListQuery(query);
                q.Limit = pageSize;
                q.Cursor = cursor;
                q.Extra["query"] = queryText;

                var page = await SearchTrashFilesAsync(q, ct).ConfigureAwait(false);
                if (page is null)
                    break;

                if (page.Data.Count > 0)
                {
                    var remaining = maxItems - items.Count;
                    items.AddRange(KDriveItem.FromMany(page.Data.Take(remaining)));
                }

                if (page.HasMore != true || string.IsNullOrWhiteSpace(page.Cursor))
                    break;

                cursor = page.Cursor;
            }

            return items;
        }

        /// <summary>
        /// Lists all children of a trashed directory across cursor pages.
        /// </summary>
        public async Task<IReadOnlyList<KDriveItem>> GetTrashChildrenAllAsync(long trashedDirectoryId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxItems);

            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            var items = new List<KDriveItem>(Math.Min(maxItems, pageSize));
            string? cursor = null;

            while (items.Count < maxItems)
            {
                var q = CloneListQuery(query);
                q.Limit = pageSize;
                q.Cursor = cursor;

                var page = await GetTrashedDirectoryFilesAsync(trashedDirectoryId, q, ct).ConfigureAwait(false);
                if (page is null)
                    break;

                if (page.Data.Count > 0)
                {
                    var remaining = maxItems - items.Count;
                    items.AddRange(KDriveItem.FromMany(page.Data.Take(remaining)));
                }

                if (page.HasMore != true || string.IsNullOrWhiteSpace(page.Cursor))
                    break;

                cursor = page.Cursor;
            }

            return items;
        }

        /// <summary>
        /// Finds the best matching trashed item by name/path ranking.
        /// </summary>
        public async Task<KDriveItem?> FindTrashItemAsync(string queryText, long? trashedDirectoryId = null, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(queryText);
            var trimmed = queryText.Trim();

            IReadOnlyList<KDriveItem> candidates = trashedDirectoryId.HasValue
                ? await GetTrashChildrenAllAsync(trashedDirectoryId.Value, query, pageSize, maxItems, ct).ConfigureAwait(false)
                : await SearchTrashAllAsync(trimmed, query, pageSize, maxItems, ct).ConfigureAwait(false);

            if (candidates.Count == 0)
                return null;

            var best = candidates
                .OrderBy(item => MatchRank(item, trimmed))
                .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();

            return best is not null && MatchRank(best, trimmed) < 4 ? best : null;
        }

        /// <summary>
        /// Lists all versions of a file across v2 page pagination.
        /// </summary>
        /// <param name="fileId">File id (not a directory).</param>
        public async Task<IReadOnlyList<KDriveFileVersion>> GetItemVersionsAllAsync(long fileId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxItems);

            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            var versions = new List<KDriveFileVersion>(Math.Min(maxItems, pageSize));
            var pageNumber = 1;

            while (versions.Count < maxItems)
            {
                var q = CloneListQuery(query);
                q.Extra["page"] = pageNumber.ToString();
                q.Extra["per_page"] = pageSize.ToString();

                var page = await GetFileVersionsAsync(fileId, q, ct).ConfigureAwait(false);
                if (page is null || page.Data.Count == 0)
                    break;

                var remaining = maxItems - versions.Count;
                versions.AddRange(page.Data.Take(remaining));

                var reachedLastPage = page.Pages.HasValue && page.Page.HasValue && page.Page.Value >= page.Pages.Value;
                if (reachedLastPage || page.Data.Count < pageSize)
                    break;

                pageNumber++;
            }

            return versions;
        }

        /// <summary>
        /// Finds a file version, optionally filtering by name substring (most recent match).
        /// </summary>
        public async Task<KDriveFileVersion?> FindItemVersionAsync(long fileId, string? nameContains = null, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            var versions = await GetItemVersionsAllAsync(fileId, query, pageSize, maxItems, ct).ConfigureAwait(false);
            if (versions.Count == 0)
                return null;

            IEnumerable<KDriveFileVersion> filtered = versions;
            if (!string.IsNullOrWhiteSpace(nameContains))
            {
                var term = nameContains.Trim();
                filtered = filtered.Where(v => (v.Name ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            return filtered
                .OrderByDescending(v => v.LastModifiedAt ?? v.UpdatedAt ?? v.CreatedAt ?? 0)
                .ThenByDescending(v => v.Id ?? 0)
                .FirstOrDefault();
        }

        /// <summary>
        /// Restores the latest version of a file in place.
        /// </summary>
        public async Task<KDriveItemResult?> RestoreLatestItemVersionAsync(long fileId, string? with = null, KDriveListQuery? versionsQuery = null, CancellationToken ct = default)
        {
            var version = await FindItemVersionAsync(fileId, query: versionsQuery, ct: ct).ConfigureAwait(false);
            if (version?.Id is null)
                return null;

            var response = await RestoreItemVersionAsync(fileId, version.Id.Value, with, ct).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Restores the latest version of a file into a destination directory.
        /// </summary>
        public async Task<KDriveItemResult?> RestoreLatestItemVersionToDirectoryAsync(long fileId, long destinationDirectoryId, string? restoredName = null, string? with = null, KDriveListQuery? versionsQuery = null, CancellationToken ct = default)
        {
            var version = await FindItemVersionAsync(fileId, query: versionsQuery, ct: ct).ConfigureAwait(false);
            if (version?.Id is null)
                return null;

            var response = await RestoreItemVersionToDirectoryAsync(fileId, version.Id.Value, destinationDirectoryId, restoredName, with, ct).ConfigureAwait(false);
            return response;
        }

        /// <summary>Aggregates all activities for a file via cursor pagination.</summary>
        public Task<IReadOnlyList<KDriveFileActivity>> GetFileActivitiesAllAsync(long fileId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
            => GetAllActivitiesByCursorAsync((q, token) => GetFileActivitiesAsync(fileId, q, token), query, pageSize, maxItems, ct);

        /// <summary>Aggregates root/items activities via cursor pagination.</summary>
        public Task<IReadOnlyList<KDriveFileActivity>> GetItemsActivitiesAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
            => GetAllActivitiesByCursorAsync((q, token) => GetRootFilesActivitiesAsync(q, token), query, pageSize, maxItems, ct);

        /// <summary>Aggregates the drive activity feed via cursor pagination.</summary>
        public Task<IReadOnlyList<KDriveFileActivity>> GetDriveActivityFeedAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
            => GetAllActivitiesByCursorAsync((q, token) => GetDriveActivitiesAsync(q, token), query, pageSize, maxItems, ct);

        /// <summary>Finds a drive activity matching <paramref name="queryText"/> by ranking.</summary>
        public async Task<KDriveFileActivity?> FindDriveActivityAsync(string queryText, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(queryText);
            var needle = queryText.Trim();
            var activities = await GetDriveActivityFeedAllAsync(query, pageSize, maxItems, ct).ConfigureAwait(false);
            return RankActivities(activities, needle);
        }

        /// <summary>Finds a file activity matching <paramref name="queryText"/> by ranking.</summary>
        public async Task<KDriveFileActivity?> FindFileActivityAsync(long fileId, string queryText, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(queryText);
            var needle = queryText.Trim();
            var activities = await GetFileActivitiesAllAsync(fileId, query, pageSize, maxItems, ct).ConfigureAwait(false);
            return RankActivities(activities, needle);
        }

        /// <summary>
        /// Lists all children of a directory by following v3 cursor pages.
        /// </summary>
        /// <param name="directoryId">Parent directory id.</param>
        /// <param name="query">Optional typed list filters/includes.</param>
        /// <param name="pageSize">Page size per request.</param>
        /// <param name="maxItems">Hard cap on aggregated items.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Domain <see cref="KDriveItem"/> list (files and directories).</returns>
        public async Task<IReadOnlyList<KDriveItem>> GetItemsAllAsync(long directoryId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            var dtos = await GetAllItemsByCursorAsync((q, token) => GetItemsAsyncDto(directoryId, q, token), query, pageSize, maxItems, ct).ConfigureAwait(false);
            return KDriveItem.FromMany(dtos);
        }

        /// <summary>
        /// Searches all matching items by following v3 cursor pages.
        /// </summary>
        public async Task<IReadOnlyList<KDriveItem>> SearchItemsAllAsync(string queryText, long? directoryId = null, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(queryText);
            var q = CloneListQuery(query);
            q.Extra["query"] = queryText.Trim();
            if (directoryId.HasValue)
                q.Extra["directory_id"] = directoryId.Value.ToString();

            var dtos = await GetAllItemsByCursorAsync((dict, token) => SearchItemsAsyncDto(dict, token), q, pageSize, maxItems, ct).ConfigureAwait(false);
            return KDriveItem.FromMany(dtos);
        }

        /// <summary>
        /// Searches all matching items using a typed <see cref="KDriveSearchQuery"/>.
        /// </summary>
        public Task<IReadOnlyList<KDriveItem>> SearchItemsAllAsync(KDriveSearchQuery query, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentException.ThrowIfNullOrWhiteSpace(query.Query);
            return SearchItemsAllAsync(query.Query!, query.DirectoryId, query, pageSize, maxItems, ct);
        }

        /// <summary>
        /// Lists all comments on a file by following v2 page pagination.
        /// </summary>
        public Task<IReadOnlyList<KDriveComment>> GetItemCommentsAllAsync(long fileId, KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
            => GetAllByPageAsync((q, token) => GetItemCommentsAsync(fileId, q, token), query, pageSize, maxItems, ct);

        /// <summary>
        /// Lists all dropbox items by following v3 cursor pages.
        /// </summary>
        public async Task<IReadOnlyList<KDriveItem>> GetDropboxesAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            var dtos = await GetAllItemsByCursorAsync((q, token) => EndpointsService.GetDropboxesAsync(q, token), query, pageSize, maxItems, ct).ConfigureAwait(false);
            return KDriveItem.FromMany(dtos);
        }

        /// <summary>
        /// Lists all favorite items by following v3 cursor pages.
        /// </summary>
        public async Task<IReadOnlyList<KDriveItem>> GetFavoritesAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
        {
            var dtos = await GetAllItemsByCursorAsync((q, token) => EndpointsService.GetFavoritesAsync(q, token), query, pageSize, maxItems, ct).ConfigureAwait(false);
            return KDriveItem.FromMany(dtos);
        }

        /// <summary>
        /// Lists all drive categories by following v2 page pagination.
        /// </summary>
        public Task<IReadOnlyList<KDriveCategory>> ListCategoriesAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
            => GetAllByPageAsync((q, token) => ListCategoriesAsync(q, token), query, pageSize, maxItems, ct);

        /// <summary>
        /// Lists all drive invitations by following v2 page pagination.
        /// </summary>
        public Task<IReadOnlyList<KDriveDriveInvitation>> ListInvitationsAllAsync(KDriveListQuery? query = null, int pageSize = 200, int maxItems = 5000, CancellationToken ct = default)
            => GetAllByPageAsync((q, token) => ListInvitationsAsync(q, token), query, pageSize, maxItems, ct);

        private Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetItemsAsyncDto(long directoryId, KDriveListQuery? query, CancellationToken ct)
            => EndpointsService.GetItemsAsync(directoryId, query, ct);

        private Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchItemsAsyncDto(KDriveListQuery? query, CancellationToken ct)
            => EndpointsService.SearchItemsAsync(query, ct);

        private async IAsyncEnumerable<KDriveItem> EnumerateByCursorAsync(
            Func<KDriveListQuery, CancellationToken, Task<KDriveNavigatorResponse<KDriveFileSystemItem>?>> fetchPage,
            KDriveListQuery? query,
            int pageSize,
            [EnumeratorCancellation] CancellationToken ct)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            string? cursor = null;

            while (true)
            {
                var q = CloneListQuery(query);
                q.Limit = pageSize;
                q.Cursor = cursor;

                var page = await fetchPage(q, ct).ConfigureAwait(false);
                if (page is null || page.Data.Count == 0)
                    yield break;

                foreach (var item in KDriveItem.FromMany(page.Data))
                    yield return item;

                if (page.HasMore != true || string.IsNullOrWhiteSpace(page.Cursor))
                    yield break;

                cursor = page.Cursor;
            }
        }

        private async Task<IReadOnlyList<KDriveFileSystemItem>> GetAllItemsByCursorAsync(
            Func<KDriveListQuery, CancellationToken, Task<KDriveNavigatorResponse<KDriveFileSystemItem>?>> fetchPage,
            KDriveListQuery? query,
            int pageSize,
            int maxItems,
            CancellationToken ct)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxItems);

            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            var items = new List<KDriveFileSystemItem>(Math.Min(maxItems, pageSize));
            string? cursor = null;

            while (items.Count < maxItems)
            {
                var q = CloneListQuery(query);
                q.Limit = pageSize;
                q.Cursor = cursor;

                var page = await fetchPage(q, ct).ConfigureAwait(false);
                if (page is null)
                    break;

                if (page.Data.Count > 0)
                {
                    var remaining = maxItems - items.Count;
                    items.AddRange(page.Data.Take(remaining));
                }

                if (page.HasMore != true || string.IsNullOrWhiteSpace(page.Cursor))
                    break;

                cursor = page.Cursor;
            }

            return items;
        }

        private static int MatchRank(KDriveItem item, string search)
        {
            var name = item.Name ?? string.Empty;
            if (string.Equals(name, search, StringComparison.OrdinalIgnoreCase))
                return 0;
            if (name.StartsWith(search, StringComparison.OrdinalIgnoreCase))
                return 1;
            if (name.Contains(search, StringComparison.OrdinalIgnoreCase))
                return 2;
            if ((item.Path ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase))
                return 3;
            return 4;
        }

        private static KDriveListQuery CloneListQuery(KDriveListQuery? query)
            => query?.Clone() ?? new KDriveListQuery();

        private async Task<IReadOnlyList<KDriveFileActivity>> GetAllActivitiesByCursorAsync(
            Func<KDriveListQuery, CancellationToken, Task<KDriveNavigatorResponse<KDriveFileActivity>?>> fetchPage,
            KDriveListQuery? query,
            int pageSize,
            int maxItems,
            CancellationToken ct)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxItems);

            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            var activities = new List<KDriveFileActivity>(Math.Min(maxItems, pageSize));
            string? cursor = null;

            while (activities.Count < maxItems)
            {
                var q = CloneListQuery(query);
                q.Limit = pageSize;
                q.Cursor = cursor;

                var page = await fetchPage(q, ct).ConfigureAwait(false);
                if (page is null)
                    break;

                if (page.Data.Count > 0)
                {
                    var remaining = maxItems - activities.Count;
                    activities.AddRange(page.Data.Take(remaining));
                }

                if (page.HasMore != true || string.IsNullOrWhiteSpace(page.Cursor))
                    break;

                cursor = page.Cursor;
            }

            return activities;
        }

        private static KDriveFileActivity? RankActivities(IReadOnlyList<KDriveFileActivity> activities, string needle)
        {
            if (activities.Count == 0)
                return null;

            var best = activities
                .OrderBy(a => ActivityRank(a, needle))
                .ThenByDescending(a => a.CreatedAt ?? 0)
                .ThenByDescending(a => a.Id ?? 0)
                .FirstOrDefault();

            return best is not null && ActivityRank(best, needle) < 4 ? best : null;
        }

        private static int ActivityRank(KDriveFileActivity activity, string needle)
        {
            var action = activity.Action ?? string.Empty;
            var type = activity.Type ?? string.Empty;

            if (string.Equals(action, needle, StringComparison.OrdinalIgnoreCase))
                return 0;
            if (action.StartsWith(needle, StringComparison.OrdinalIgnoreCase))
                return 1;
            if (action.Contains(needle, StringComparison.OrdinalIgnoreCase) || type.Contains(needle, StringComparison.OrdinalIgnoreCase))
                return 2;
            if ((activity.ExtraData?.Count ?? 0) > 0 && activity.ExtraData!.Keys.Any(k => k.Contains(needle, StringComparison.OrdinalIgnoreCase)))
                return 3;

            return 4;
        }

        private async Task<IReadOnlyList<TItem>> GetAllByPageAsync<TItem>(
            Func<KDriveListQuery, CancellationToken, Task<KDrivePagedResponse<TItem>?>> fetchPage,
            KDriveListQuery? query,
            int pageSize,
            int maxItems,
            CancellationToken ct)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxItems);

            await EnsureDriveAwakeAsync(ct: ct).ConfigureAwait(false);
            var items = new List<TItem>(Math.Min(maxItems, pageSize));
            var pageNumber = 1;

            while (items.Count < maxItems)
            {
                var q = CloneListQuery(query);
                q.Extra["page"] = pageNumber.ToString();
                q.Extra["per_page"] = pageSize.ToString();

                var page = await fetchPage(q, ct).ConfigureAwait(false);
                if (page is null || page.Data.Count == 0)
                    break;

                var remaining = maxItems - items.Count;
                items.AddRange(page.Data.Take(remaining));

                var reachedLastPage = page.Pages.HasValue && page.Page.HasValue && page.Page.Value >= page.Pages.Value;
                if (reachedLastPage || page.Data.Count < pageSize)
                    break;

                pageNumber++;
            }

            return items;
        }
    }
}
