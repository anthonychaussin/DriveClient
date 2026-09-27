using kDriveClient.Models;
using kDriveClient.Models.Domain;

namespace kDriveClient.kDriveClient
{
    public partial class KDriveClient
    {
        public Task<KDrivePagedResponse<KDriveDriveSummary>?> GetAccessibleDrivesAsync(long accountId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetAccessibleDrivesAsync(accountId, query, ct);

        public Task<KDrivePagedResponse<KDriveUserSummary>?> GetDriveUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveUsersAsync(query, ct);

        public Task<KDrivePagedResponse<KDriveDriveUserSummary>?> GetUserDrivesAsync(long userId, long accountId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetUserDrivesAsync(userId, accountId, query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetFileDetailsAsync(long fileId, string? with = null, CancellationToken ct = default)
            => EndpointsService.GetFileDetailsAsync(fileId, with, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> ListDirectoryFilesAsync(long directoryId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.ListDirectoryFilesAsync(directoryId, query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.SearchFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetRecentFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetRecentFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetFavoriteFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFavoriteFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFavoriteFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.SearchFavoriteFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMySharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetMySharedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchMySharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.SearchMySharedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetSharedWithMeFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetSharedWithMeFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchSharedWithMeFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.SearchSharedWithMeFilesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveUuidResource>?> GetArchivedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetArchivedFilesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveUuidResource>?> BuildArchiveAsync(IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default)
            => EndpointsService.BuildArchiveAsync(fileIds, parentId, exceptFileIds, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLargestFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetLargestFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLastModifiedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetLastModifiedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMostVersionedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetMostVersionedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLinkedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetLinkedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchLinkedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.SearchLinkedFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetDropboxFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDropboxFilesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?> CreateDropboxAsync(string name, long? parentDirectoryId = null, CancellationToken ct = default)
            => EndpointsService.CreateDropboxAsync(name, parentDirectoryId, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchDropboxFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.SearchDropboxFilesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetFileByNameAsync(long fileId, string name, string? with = null, CancellationToken ct = default)
            => EndpointsService.GetFileByNameAsync(fileId, name, with, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> DuplicateFileAsync(long fileId, string? name = null, string? with = null, CancellationToken ct = default)
            => EndpointsService.DuplicateFileAsync(fileId, name, with, ct);

        public Task<KDriveResourceResponse<bool>?> UnlockFileAsync(long fileId, string? path = null, string? token = null, CancellationToken ct = default)
            => EndpointsService.UnlockFileAsync(fileId, path, token, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountDirectoryElementsAsync(long fileId, int? depth = null, CancellationToken ct = default)
            => EndpointsService.CountDirectoryElementsAsync(fileId, depth, ct);

        public Task<KDrivePagedResponse<KDriveFileVersion>?> GetFileVersionsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFileVersionsAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionAsync(long fileId, long versionId, string? with = null, CancellationToken ct = default)
            => EndpointsService.RestoreFileVersionAsync(fileId, versionId, with, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionToDirectoryAsync(long fileId, long versionId, long destinationDirectoryId, string? name = null, string? with = null, CancellationToken ct = default)
            => EndpointsService.RestoreFileVersionToDirectoryAsync(fileId, versionId, destinationDirectoryId, name, with, ct);

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetFileActivitiesAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFileActivitiesAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> ConvertFileAsync(long fileId, string? with = null, CancellationToken ct = default)
            => EndpointsService.ConvertFileAsync(fileId, with, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateDefaultFileAsync(long parentDirectoryId, string name, string type, string? with = null, CancellationToken ct = default)
            => EndpointsService.CreateDefaultFileAsync(parentDirectoryId, name, type, with, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateTeamDirectoryAsync(string name, string? color = null, bool? forAllUser = null, string? with = null, CancellationToken ct = default)
            => EndpointsService.CreateTeamDirectoryAsync(name, color, forAllUser, with, ct);

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetRootFilesActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetRootFilesActivitiesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetDriveActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveActivitiesAsync(query, ct);

        public Task<KDriveResourceResponse<List<KDriveFileActivity>>?> GetDriveActivitiesTotalAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveActivitiesTotalAsync(query, ct);

        public Task<KDriveResourceResponse<bool>?> WakeDriveAsync(CancellationToken ct = default)
            => EndpointsService.WakeDriveAsync(ct);

        public Task<KDriveNavigatorResponse<KDriveUserV3>?> GetDriveUsersV3Async(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveUsersV3Async(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchTrashFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.SearchTrashFilesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashedDirectoryFilesAsync(long trashedDirectoryId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetTrashedDirectoryFilesAsync(trashedDirectoryId, query, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashedDirectoryElementsAsync(long trashedDirectoryId, int? depth = null, CancellationToken ct = default)
            => EndpointsService.CountTrashedDirectoryElementsAsync(trashedDirectoryId, depth, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateDirectoryAsync(long parentDirectoryId, string name, string? color = null, bool? onlyForMe = null, string? relativePath = null, CancellationToken ct = default)
            => EndpointsService.CreateDirectoryAsync(parentDirectoryId, name, color, onlyForMe, relativePath, ct);

        public Task<KDriveResourceResponse<KDriveCancelResource>?> RenameFileAsync(long fileId, string name, CancellationToken ct = default)
            => EndpointsService.RenameFileAsync(fileId, name, ct);

        public Task<KDriveResourceResponse<KDriveCancelResource>?> MoveFileAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "error", CancellationToken ct = default)
            => EndpointsService.MoveFileAsync(fileId, destinationDirectoryId, name, conflict, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> CopyFileAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "rename", string? with = null, CancellationToken ct = default)
            => EndpointsService.CopyFileAsync(fileId, destinationDirectoryId, name, conflict, with, ct);

        public Task<KDriveResourceResponse<KDriveCancelResource>?> TrashFileAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.TrashFileAsync(fileId, ct);

        public Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetTrashAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetTrashFileAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetTrashFileAsync(fileId, query, ct);

        public Task<KDrivePagedResponse<KDriveDriveSummary>?> GetDrivesAsync(long accountId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDrivesAsync(accountId, query, ct);

        public Task<KDrivePagedResponse<KDriveUserSummary>?> GetUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetUsersAsync(query, ct);

        public Task<KDrivePagedResponse<KDriveDriveUserSummary>?> GetDrivesByUserAsync(long userId, long accountId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDrivesByUserAsync(userId, accountId, query, ct);

        public async Task<KDriveItemResult?> GetItemAsync(long fileId, string? with = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetItemAsync(fileId, with, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public async Task<KDriveItemPage?> GetItemsAsync(long directoryId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetItemsAsync(directoryId, query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> SearchItemsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.SearchItemsAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> SearchItemsAsync(KDriveSearchQuery query, CancellationToken ct = default)
        {
            var response = await EndpointsService.SearchItemsAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> GetRecentAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetRecentAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> GetFavoritesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetFavoritesAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> SearchFavoritesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.SearchFavoritesAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> GetMySharedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetMySharedAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> SearchMySharedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.SearchMySharedAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> GetSharedWithMeAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetSharedWithMeAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> SearchSharedWithMeAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.SearchSharedWithMeAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public Task<KDriveResourceResponse<KDriveUuidResource>?> GetArchivesAsync(IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default)
            => EndpointsService.GetArchivesAsync(fileIds, parentId, exceptFileIds, ct);

        public async Task<KDriveItemPage?> GetLargestAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetLargestAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> GetLastModifiedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetLastModifiedAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> GetMostVersionedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetMostVersionedAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> GetLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetLinksAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> SearchLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.SearchLinksAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> GetDropboxesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetDropboxesAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public Task<KDriveResourceResponse<KDriveDropbox>?> CreateDropboxItemAsync(string name, long? parentDirectoryId = null, CancellationToken ct = default)
            => EndpointsService.CreateDropboxItemAsync(name, parentDirectoryId, ct);

        public async Task<KDriveItemPage?> SearchDropboxesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.SearchDropboxesAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemResult?> FindItemByNameAsync(long fileId, string name, string? with = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.FindItemByNameAsync(fileId, name, with, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public async Task<KDriveItemResult?> DuplicateItemAsync(long fileId, string? name = null, string? with = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.DuplicateItemAsync(fileId, name, with, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public Task<KDriveResourceResponse<bool>?> UnlockItemAsync(long fileId, string? path = null, string? token = null, CancellationToken ct = default)
            => EndpointsService.UnlockItemAsync(fileId, path, token, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountItemsAsync(long fileId, int? depth = null, CancellationToken ct = default)
            => EndpointsService.CountItemsAsync(fileId, depth, ct);

        public Task<KDrivePagedResponse<KDriveFileVersion>?> GetItemVersionsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetItemVersionsAsync(fileId, query, ct);

        public async Task<KDriveItemResult?> RestoreItemVersionAsync(long fileId, long versionId, string? with = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.RestoreItemVersionAsync(fileId, versionId, with, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public async Task<KDriveItemResult?> RestoreItemVersionToDirectoryAsync(long fileId, long versionId, long destinationDirectoryId, string? name = null, string? with = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.RestoreItemVersionToDirectoryAsync(fileId, versionId, destinationDirectoryId, name, with, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetItemActivitiesAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetItemActivitiesAsync(fileId, query, ct);

        public async Task<KDriveItemResult?> ConvertItemAsync(long fileId, string? with = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.ConvertItemAsync(fileId, with, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public async Task<KDriveItemResult?> CreateItemFileAsync(long parentDirectoryId, string name, string type, string? with = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.CreateItemFileAsync(parentDirectoryId, name, type, with, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public async Task<KDriveItemResult?> CreateTeamFolderAsync(string name, string? color = null, bool? forAllUser = null, string? with = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.CreateTeamFolderAsync(name, color, forAllUser, with, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetItemsActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetItemsActivitiesAsync(query, ct);

        public Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetDriveActivityFeedAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveActivityFeedAsync(query, ct);

        public Task<KDriveResourceResponse<List<KDriveFileActivity>>?> GetDriveActivityTotalAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveActivityTotalAsync(query, ct);

        public Task<KDriveResourceResponse<bool>?> WakeAsync(CancellationToken ct = default)
            => EndpointsService.WakeAsync(ct);

        public Task<KDriveNavigatorResponse<KDriveUserV3>?> GetUsersV3Async(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetUsersV3Async(query, ct);

        public async Task<KDriveItemPage?> SearchTrashAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.SearchTrashAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemPage?> GetTrashChildrenAsync(long trashedDirectoryId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetTrashChildrenAsync(trashedDirectoryId, query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashChildrenCountAsync(long trashedDirectoryId, int? depth = null, CancellationToken ct = default)
            => EndpointsService.GetTrashChildrenCountAsync(trashedDirectoryId, depth, ct);

        public async Task<KDriveItemResult?> CreateFolderAsync(long parentDirectoryId, string name, string? color = null, bool? onlyForMe = null, string? relativePath = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.CreateFolderAsync(parentDirectoryId, name, color, onlyForMe, relativePath, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public Task<KDriveResourceResponse<KDriveCancelResource>?> RenameItemAsync(long fileId, string name, CancellationToken ct = default)
            => EndpointsService.RenameItemAsync(fileId, name, ct);

        public Task<KDriveResourceResponse<KDriveCancelResource>?> MoveItemAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "error", CancellationToken ct = default)
            => EndpointsService.MoveItemAsync(fileId, destinationDirectoryId, name, conflict, ct);

        public async Task<KDriveItemResult?> CopyItemAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "rename", string? with = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.CopyItemAsync(fileId, destinationDirectoryId, name, conflict, with, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

        public Task<KDriveResourceResponse<KDriveCancelResource>?> TrashItemAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.TrashItemAsync(fileId, ct);

        public async Task<KDriveItemPage?> GetTrashItemsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetTrashItemsAsync(query, ct).ConfigureAwait(false);
            return KDriveItemPage.From(response);
        }

        public async Task<KDriveItemResult?> GetTrashItemAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
        {
            var response = await EndpointsService.GetTrashItemAsync(fileId, query, ct).ConfigureAwait(false);
            return KDriveItemResult.From(response);
        }

    }
}
