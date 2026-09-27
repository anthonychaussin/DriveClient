using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    public partial class KDriveClient
    {
        public Task<KDriveResourceResponse<KDriveCancelResource>?> RestoreTrashedFileAsync(long fileId, long destinationDirectoryId, CancellationToken ct = default)
            => EndpointsService.RestoreTrashedFileAsync(fileId, destinationDirectoryId, ct);

        public Task<KDriveResourceResponse<bool>?> EmptyTrashAsync(CancellationToken ct = default)
            => EndpointsService.EmptyTrashAsync(ct);

        public Task<KDriveResourceResponse<bool>?> PermanentlyDeleteTrashedFileAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.PermanentlyDeleteTrashedFileAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashCountAsync(CancellationToken ct = default)
            => EndpointsService.GetTrashCountAsync(ct);

        public Task<KDriveResourceResponse<bool>?> AddFavoriteAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.AddFavoriteAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveFavoriteAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.RemoveFavoriteAsync(fileId, ct);

        public Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> UndoAsync(string? cancelId = null, IEnumerable<string>? cancelIds = null, CancellationToken ct = default)
            => EndpointsService.UndoAsync(cancelId, cancelIds, ct);

        public Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> CheckFilesExistAsync(IEnumerable<long> ids, CancellationToken ct = default)
            => EndpointsService.CheckFilesExistAsync(ids, ct);

        public Task<KDriveResourceResponse<KDriveFileHash>?> GetFileHashAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.GetFileHashAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveFileSizeInfo>?> GetFileSizesAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.GetFileSizesAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveTemporaryUrl>?> GetFileTemporaryUrlAsync(long fileId, int? durationSeconds = null, CancellationToken ct = default)
            => EndpointsService.GetFileTemporaryUrlAsync(fileId, durationSeconds, ct);

        public Task<KDriveResourceResponse<bool>?> SetFileLastModifiedAsync(long fileId, long lastModifiedAt, CancellationToken ct = default)
            => EndpointsService.SetFileLastModifiedAsync(fileId, lastModifiedAt, ct);

        public Task<KDriveResourceResponse<List<KDriveExternalImport>>?> CopyFileToDriveAsync(long fileId, long sourceDriveId, long sourceFileId, CancellationToken ct = default)
            => EndpointsService.CopyFileToDriveAsync(fileId, sourceDriveId, sourceFileId, ct);

        public Task<KDriveResourceResponse<KDriveShareLink>?> GetShareLinkAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.GetShareLinkAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveShareLink>?> CreateShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default)
            => EndpointsService.CreateShareLinkAsync(fileId, request, ct);

        public Task<KDriveResourceResponse<bool>?> UpdateShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default)
            => EndpointsService.UpdateShareLinkAsync(fileId, request, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteShareLinkAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.DeleteShareLinkAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> InviteShareLinkAsync(long fileId, IEnumerable<string>? emails = null, IEnumerable<long>? userIds = null, string? message = null, CancellationToken ct = default)
            => EndpointsService.InviteShareLinkAsync(fileId, emails, userIds, message, ct);

        public Task<KDrivePagedResponse<KDriveComment>?> GetFileCommentsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetFileCommentsAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> GetFileCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => EndpointsService.GetFileCommentAsync(fileId, commentId, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> CreateFileCommentAsync(long fileId, string body, long? parentId = null, CancellationToken ct = default)
            => EndpointsService.CreateFileCommentAsync(fileId, body, parentId, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> ReplyFileCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default)
            => EndpointsService.ReplyFileCommentAsync(fileId, commentId, body, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> UpdateFileCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default)
            => EndpointsService.UpdateFileCommentAsync(fileId, commentId, body, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteFileCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => EndpointsService.DeleteFileCommentAsync(fileId, commentId, ct);

        public Task<KDrivePagedResponse<KDriveCategory>?> GetCategoriesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetCategoriesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveCategory>?> CreateCategoryAsync(string name, string? color = null, CancellationToken ct = default)
            => EndpointsService.CreateCategoryAsync(name, color, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?> GetFileDropboxAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.GetFileDropboxAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveDriveDetail>?> GetDriveAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> GetDriveSettingsAsync(CancellationToken ct = default)
            => EndpointsService.GetDriveSettingsAsync(ct);

        public Task<KDrivePagedResponse<KDriveDriveInvitation>?> GetDriveInvitationsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveInvitationsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveCancelResource>?> RestoreTrashItemAsync(long fileId, long destinationDirectoryId, CancellationToken ct = default)
            => EndpointsService.RestoreTrashItemAsync(fileId, destinationDirectoryId, ct);

        public Task<KDriveResourceResponse<bool>?> EmptyTrashItemsAsync(CancellationToken ct = default)
            => EndpointsService.EmptyTrashItemsAsync(ct);

        public Task<KDriveResourceResponse<bool>?> PurgeTrashItemAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.PurgeTrashItemAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashAsync(CancellationToken ct = default)
            => EndpointsService.CountTrashAsync(ct);

        public Task<KDriveResourceResponse<bool>?> FavoriteItemAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.FavoriteItemAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> UnfavoriteItemAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.UnfavoriteItemAsync(fileId, ct);

        public Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> UndoOperationAsync(string? cancelId = null, IEnumerable<string>? cancelIds = null, CancellationToken ct = default)
            => EndpointsService.UndoOperationAsync(cancelId, cancelIds, ct);

        public Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> CheckItemsExistAsync(IEnumerable<long> ids, CancellationToken ct = default)
            => EndpointsService.CheckItemsExistAsync(ids, ct);

        public Task<KDriveResourceResponse<KDriveFileHash>?> GetItemHashAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.GetItemHashAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveFileSizeInfo>?> GetItemSizesAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.GetItemSizesAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveTemporaryUrl>?> GetItemTemporaryUrlAsync(long fileId, int? durationSeconds = null, CancellationToken ct = default)
            => EndpointsService.GetItemTemporaryUrlAsync(fileId, durationSeconds, ct);

        public Task<KDriveResourceResponse<bool>?> TouchItemLastModifiedAsync(long fileId, long lastModifiedAt, CancellationToken ct = default)
            => EndpointsService.TouchItemLastModifiedAsync(fileId, lastModifiedAt, ct);

        public Task<KDriveResourceResponse<KDriveShareLink>?> GetItemShareLinkAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.GetItemShareLinkAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveShareLink>?> CreateItemShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default)
            => EndpointsService.CreateItemShareLinkAsync(fileId, request, ct);

        public Task<KDriveResourceResponse<bool>?> UpdateItemShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default)
            => EndpointsService.UpdateItemShareLinkAsync(fileId, request, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteItemShareLinkAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.DeleteItemShareLinkAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> InviteItemShareLinkAsync(long fileId, IEnumerable<string>? emails = null, IEnumerable<long>? userIds = null, string? message = null, CancellationToken ct = default)
            => EndpointsService.InviteItemShareLinkAsync(fileId, emails, userIds, message, ct);

        public Task<KDrivePagedResponse<KDriveComment>?> GetItemCommentsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetItemCommentsAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> GetItemCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => EndpointsService.GetItemCommentAsync(fileId, commentId, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> CreateItemCommentAsync(long fileId, string body, long? parentId = null, CancellationToken ct = default)
            => EndpointsService.CreateItemCommentAsync(fileId, body, parentId, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> ReplyItemCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default)
            => EndpointsService.ReplyItemCommentAsync(fileId, commentId, body, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> UpdateItemCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default)
            => EndpointsService.UpdateItemCommentAsync(fileId, commentId, body, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteItemCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => EndpointsService.DeleteItemCommentAsync(fileId, commentId, ct);

        public Task<KDriveResourceResponse<bool>?> LikeItemCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => EndpointsService.LikeItemCommentAsync(fileId, commentId, ct);

        public Task<KDriveResourceResponse<bool>?> UnlikeItemCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => EndpointsService.UnlikeItemCommentAsync(fileId, commentId, ct);

        public Task<KDrivePagedResponse<KDriveCategory>?> ListCategoriesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.ListCategoriesAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveCategory>?> CreateDriveCategoryAsync(string name, string? color = null, CancellationToken ct = default)
            => EndpointsService.CreateDriveCategoryAsync(name, color, ct);

        public Task<KDriveResourceResponse<bool>?> TagItemAsync(long fileId, long categoryId, CancellationToken ct = default)
            => EndpointsService.TagItemAsync(fileId, categoryId, ct);

        public Task<KDriveResourceResponse<bool>?> UntagItemAsync(long fileId, long categoryId, CancellationToken ct = default)
            => EndpointsService.UntagItemAsync(fileId, categoryId, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?> GetItemDropboxAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.GetItemDropboxAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?> EnableItemDropboxAsync(long fileId, KDriveDropboxRequest? payload = null, CancellationToken ct = default)
            => EndpointsService.EnableItemDropboxAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?> UpdateItemDropboxAsync(long fileId, KDriveDropboxRequest payload, CancellationToken ct = default)
            => EndpointsService.UpdateItemDropboxAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> DisableItemDropboxAsync(long fileId, CancellationToken ct = default)
            => EndpointsService.DisableItemDropboxAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> InviteItemDropboxAsync(long fileId, KDriveDropboxInviteRequest payload, CancellationToken ct = default)
            => EndpointsService.InviteItemDropboxAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveFileAccess>?> GetItemAccessAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetItemAccessAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<bool>?> ShareItemWithUsersAsync(long fileId, KDriveFileAccessUsersRequest payload, CancellationToken ct = default)
            => EndpointsService.ShareItemWithUsersAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveItemAccessUserAsync(long fileId, long userId, CancellationToken ct = default)
            => EndpointsService.RemoveItemAccessUserAsync(fileId, userId, ct);

        public Task<KDrivePagedResponse<KDriveDriveInvitation>?> ListInvitationsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.ListInvitationsAsync(query, ct);

        public Task<KDriveResourceResponse<bool>?> SendInvitationAsync(long invitationId, CancellationToken ct = default)
            => EndpointsService.SendInvitationAsync(invitationId, ct);

        public Task<KDrivePagedResponse<KDriveExternalImport>?> ListImportsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.ListImportsAsync(query, ct);

        public Task<KDriveResourceResponse<KDriveDriveDetail>?> GetDriveInfoAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => EndpointsService.GetDriveInfoAsync(query, ct);

    }
}
