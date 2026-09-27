using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public partial interface IKDriveEndpointsService
    {
        Task<KDriveResourceResponse<KDriveCancelResource>?> RestoreTrashedFileAsync(long fileId, long destinationDirectoryId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> EmptyTrashAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> PermanentlyDeleteTrashedFileAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashCountAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> AddFavoriteAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> RemoveFavoriteAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> UndoAsync(string? cancelId = null, IEnumerable<string>? cancelIds = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> CheckFilesExistAsync(IEnumerable<long> ids, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileHash>?> GetFileHashAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSizeInfo>?> GetFileSizesAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveTemporaryUrl>?> GetFileTemporaryUrlAsync(long fileId, int? durationSeconds = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> SetFileLastModifiedAsync(long fileId, long lastModifiedAt, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveExternalImport>>?> CopyFileToDriveAsync(long fileId, long sourceDriveId, long sourceFileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveShareLink>?> GetShareLinkAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveShareLink>?> CreateShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UpdateShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> DeleteShareLinkAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> InviteShareLinkAsync(long fileId, IEnumerable<string>? emails = null, IEnumerable<long>? userIds = null, string? message = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveComment>?> GetFileCommentsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveComment>?> GetFileCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveComment>?> CreateFileCommentAsync(long fileId, string body, long? parentId = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveComment>?> ReplyFileCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveComment>?> UpdateFileCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> DeleteFileCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveCategory>?> GetCategoriesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCategory>?> CreateCategoryAsync(string name, string? color = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDropbox>?> GetFileDropboxAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveDetail>?> GetDriveAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveSettings>?> GetDriveSettingsAsync(CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveDriveInvitation>?> GetDriveInvitationsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCancelResource>?> RestoreTrashItemAsync(long fileId, long destinationDirectoryId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> EmptyTrashItemsAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> PurgeTrashItemAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashAsync(CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> FavoriteItemAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UnfavoriteItemAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> UndoOperationAsync(string? cancelId = null, IEnumerable<string>? cancelIds = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> CheckItemsExistAsync(IEnumerable<long> ids, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileHash>?> GetItemHashAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileSizeInfo>?> GetItemSizesAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveTemporaryUrl>?> GetItemTemporaryUrlAsync(long fileId, int? durationSeconds = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> TouchItemLastModifiedAsync(long fileId, long lastModifiedAt, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveShareLink>?> GetItemShareLinkAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveShareLink>?> CreateItemShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UpdateItemShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> DeleteItemShareLinkAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> InviteItemShareLinkAsync(long fileId, IEnumerable<string>? emails = null, IEnumerable<long>? userIds = null, string? message = null, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveComment>?> GetItemCommentsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveComment>?> GetItemCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveComment>?> CreateItemCommentAsync(long fileId, string body, long? parentId = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveComment>?> ReplyItemCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveComment>?> UpdateItemCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> DeleteItemCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> LikeItemCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UnlikeItemCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveCategory>?> ListCategoriesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveCategory>?> CreateDriveCategoryAsync(string name, string? color = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> TagItemAsync(long fileId, long categoryId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> UntagItemAsync(long fileId, long categoryId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDropbox>?> GetItemDropboxAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDropbox>?> EnableItemDropboxAsync(long fileId, KDriveDropboxRequest? payload = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDropbox>?> UpdateItemDropboxAsync(long fileId, KDriveDropboxRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> DisableItemDropboxAsync(long fileId, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> InviteItemDropboxAsync(long fileId, KDriveDropboxInviteRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveFileAccess>?> GetItemAccessAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> ShareItemWithUsersAsync(long fileId, KDriveFileAccessUsersRequest payload, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> RemoveItemAccessUserAsync(long fileId, long userId, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveDriveInvitation>?> ListInvitationsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<bool>?> SendInvitationAsync(long invitationId, CancellationToken ct = default);

        Task<KDrivePagedResponse<KDriveExternalImport>?> ListImportsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        Task<KDriveResourceResponse<KDriveDriveDetail>?> GetDriveInfoAsync(KDriveListQuery? query = null, CancellationToken ct = default);

    }
}
