using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public partial interface IKDriveEndpointsService
    {
        Task<KDrivePagedResponse<KDriveDriveSummary>?> GetAccessibleDrivesAsync(long accountId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveUserSummary>?> GetDriveUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveDriveUserSummary>?> GetUserDrivesAsync(long userId, long accountId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetFileDetailsAsync(long fileId, string? with = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> ListDirectoryFilesAsync(long directoryId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetRecentFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetFavoriteFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFavoriteFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMySharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchMySharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetSharedWithMeFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchSharedWithMeFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUuidResource>?> GetArchivedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUuidResource>?> BuildArchiveAsync(IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLargestFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLastModifiedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMostVersionedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLinkedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchLinkedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetDropboxFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDropbox>?> CreateDropboxAsync(string name, long? parentDirectoryId = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchDropboxFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetFileByNameAsync(long fileId, string name, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> DuplicateFileAsync(long fileId, string? name = null, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UnlockFileAsync(long fileId, string? path = null, string? token = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountDirectoryElementsAsync(long fileId, int? depth = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileVersion>?> GetFileVersionsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionAsync(long fileId, long versionId, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreFileVersionToDirectoryAsync(long fileId, long versionId, long destinationDirectoryId, string? name = null, string? with = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetFileActivitiesAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> ConvertFileAsync(long fileId, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateDefaultFileAsync(long parentDirectoryId, string name, string type, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateTeamDirectoryAsync(string name, string? color = null, bool? forAllUser = null, string? with = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetRootFilesActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetDriveActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveFileActivity>>?> GetDriveActivitiesTotalAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> WakeDriveAsync(CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveUserV3>?> GetDriveUsersV3Async(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchTrashFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashedDirectoryFilesAsync(long trashedDirectoryId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashedDirectoryElementsAsync(long trashedDirectoryId, int? depth = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateDirectoryAsync(long parentDirectoryId, string name, string? color = null, bool? onlyForMe = null, string? relativePath = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCancelResource>?> RenameFileAsync(long fileId, string name, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCancelResource>?> MoveFileAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "error", CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CopyFileAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "rename", string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCancelResource>?> TrashFileAsync(long fileId, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetTrashFileAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveDriveSummary>?> GetDrivesAsync(long accountId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveUserSummary>?> GetUsersAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveDriveUserSummary>?> GetDrivesByUserAsync(long userId, long accountId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetItemAsync(long fileId, string? with = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetItemsAsync(long directoryId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchItemsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchItemsAsync(KDriveSearchQuery query, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetRecentAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetFavoritesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchFavoritesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMySharedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchMySharedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetSharedWithMeAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchSharedWithMeAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveUuidResource>?> GetArchivesAsync(IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLargestAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLastModifiedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMostVersionedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchLinksAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetDropboxesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDropbox>?> CreateDropboxItemAsync(string name, long? parentDirectoryId = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchDropboxesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> FindItemByNameAsync(long fileId, string name, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> DuplicateItemAsync(long fileId, string? name = null, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UnlockItemAsync(long fileId, string? path = null, string? token = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountItemsAsync(long fileId, int? depth = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveFileVersion>?> GetItemVersionsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreItemVersionAsync(long fileId, long versionId, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> RestoreItemVersionToDirectoryAsync(long fileId, long versionId, long destinationDirectoryId, string? name = null, string? with = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetItemActivitiesAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> ConvertItemAsync(long fileId, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateItemFileAsync(long parentDirectoryId, string name, string type, string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateTeamFolderAsync(string name, string? color = null, bool? forAllUser = null, string? with = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetItemsActivitiesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileActivity>?> GetDriveActivityFeedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveFileActivity>>?> GetDriveActivityTotalAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> WakeAsync(CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveUserV3>?> GetUsersV3Async(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchTrashAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashChildrenAsync(long trashedDirectoryId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashChildrenCountAsync(long trashedDirectoryId, int? depth = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CreateFolderAsync(long parentDirectoryId, string name, string? color = null, bool? onlyForMe = null, string? relativePath = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCancelResource>?> RenameItemAsync(long fileId, string name, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCancelResource>?> MoveItemAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "error", CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> CopyItemAsync(long fileId, long destinationDirectoryId, string? name = null, string conflict = "rename", string? with = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCancelResource>?> TrashItemAsync(long fileId, CancellationToken ct = default);

        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetTrashItemsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSystemItem>?> GetTrashItemAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

    }
}
