using kDriveClient.Helpers;
using kDriveClient.kDriveClient.Domain.Common;
using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Application.Endpoints
{
    public sealed partial class KDriveEndpointsService
    {


























































        public Task<KDriveResourceResponse<KDriveCancelResource>?> RestoreTrashedFileAsync(long fileId, long destinationDirectoryId, CancellationToken ct = default)
        {
            var payload = new Dictionary<string, object?> { ["destination_directory_id"] = destinationDirectoryId };
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/trash/{fileId}/restore", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveCancelResource, ct);
        }

        public Task<KDriveResourceResponse<bool>?> EmptyTrashAsync(CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/trash", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> PermanentlyDeleteTrashedFileAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/trash/{fileId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> GetTrashCountAsync(CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/trash/count", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveDirectoryCount, ct);

        public Task<KDriveResourceResponse<bool>?> AddFavoriteAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/favorite", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveFavoriteAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/favorite", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> UndoAsync(string? cancelId = null, IEnumerable<string>? cancelIds = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/cancel", BuildCancelPayload(cancelId, cancelIds), KDriveJsonContext.Default.KDriveResourceResponseListKDriveFeedbackResource, ct);

        public Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> CheckFilesExistAsync(IEnumerable<long> ids, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(ids);
            var payload = new Dictionary<string, object?> { ["ids"] = ids.ToArray() };
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/exists", payload, KDriveJsonContext.Default.KDriveResourceResponseListKDriveFeedbackResource, ct);
        }

        public Task<KDriveResourceResponse<KDriveFileHash>?> GetFileHashAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/files/{fileId}/hash", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileHash, ct);

        public Task<KDriveResourceResponse<KDriveFileSizeInfo>?> GetFileSizesAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/files/{fileId}/sizes", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSizeInfo, ct);

        public Task<KDriveResourceResponse<KDriveTemporaryUrl>?> GetFileTemporaryUrlAsync(long fileId, int? durationSeconds = null, CancellationToken ct = default)
        {
            var query = durationSeconds is > 0
                ? new Dictionary<string, string?> { ["duration"] = durationSeconds.Value.ToString() }
                : null;
            return _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/temporary_url", query), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveTemporaryUrl, ct);
        }

        public Task<KDriveResourceResponse<bool>?> SetFileLastModifiedAsync(long fileId, long lastModifiedAt, CancellationToken ct = default)
        {
            var payload = new Dictionary<string, object?> { ["last_modified_at"] = lastModifiedAt };
            return _api.SendTypedAsync(HttpMethod.Post, $"/3/drive/{_driveId}/files/{fileId}/last-modified", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);
        }

        public Task<KDriveResourceResponse<List<KDriveExternalImport>>?> CopyFileToDriveAsync(long fileId, long sourceDriveId, long sourceFileId, CancellationToken ct = default)
        {
            var payload = new Dictionary<string, object?>
            {
                ["source_drive_id"] = sourceDriveId,
                ["source_file_id"] = sourceFileId
            };
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/copy-to-drive", payload, KDriveJsonContext.Default.KDriveResourceResponseListKDriveExternalImport, ct);
        }

        public Task<KDriveResourceResponse<KDriveShareLink>?> GetShareLinkAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/files/{fileId}/link", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveShareLink, ct);

        public Task<KDriveResourceResponse<KDriveShareLink>?> CreateShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/link", request, KDriveJsonContext.Default.KDriveResourceResponseKDriveShareLink, ct);
        }

        public Task<KDriveResourceResponse<bool>?> UpdateShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/files/{fileId}/link", request, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);
        }

        public Task<KDriveResourceResponse<bool>?> DeleteShareLinkAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/link", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDriveResourceResponse<bool>?> InviteShareLinkAsync(long fileId, IEnumerable<string>? emails = null, IEnumerable<long>? userIds = null, string? message = null, CancellationToken ct = default)
        {
            var payload = new Dictionary<string, object?>();
            if (emails is not null)
                payload["emails"] = emails.ToArray();
            if (userIds is not null)
                payload["user_ids"] = userIds.ToArray();
            if (message is not null)
                payload["message"] = message;
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/link/invite", payload, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);
        }

        public Task<KDrivePagedResponse<KDriveComment>?> GetFileCommentsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/files/{fileId}/comments", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveComment, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> GetFileCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/files/{fileId}/comments/{commentId}", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveComment, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> CreateFileCommentAsync(long fileId, string body, long? parentId = null, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(body);
            var payload = new Dictionary<string, object?> { ["body"] = body };
            if (parentId.HasValue)
                payload["parent_id"] = parentId.Value;
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/comments", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveComment, ct);
        }

        public Task<KDriveResourceResponse<KDriveComment>?> ReplyFileCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(body);
            var payload = new Dictionary<string, object?> { ["body"] = body };
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/files/{fileId}/comments/{commentId}", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveComment, ct);
        }

        public Task<KDriveResourceResponse<KDriveComment>?> UpdateFileCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(body);
            var payload = new Dictionary<string, object?> { ["body"] = body };
            return _api.SendTypedAsync(HttpMethod.Put, $"/2/drive/{_driveId}/files/{fileId}/comments/{commentId}", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveComment, ct);
        }

        public Task<KDriveResourceResponse<bool>?> DeleteFileCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Delete, $"/2/drive/{_driveId}/files/{fileId}/comments/{commentId}", null, KDriveJsonContext.Default.KDriveResourceResponseBoolean, ct);

        public Task<KDrivePagedResponse<KDriveCategory>?> GetCategoriesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/categories", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveCategory, ct);

        public Task<KDriveResourceResponse<KDriveCategory>?> CreateCategoryAsync(string name, string? color = null, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            var payload = new Dictionary<string, object?> { ["name"] = name };
            if (!string.IsNullOrWhiteSpace(color))
                payload["color"] = color;
            return _api.SendTypedAsync(HttpMethod.Post, $"/2/drive/{_driveId}/categories", payload, KDriveJsonContext.Default.KDriveResourceResponseKDriveCategory, ct);
        }

        public Task<KDriveResourceResponse<KDriveDropbox>?> GetFileDropboxAsync(long fileId, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/files/{fileId}/dropbox", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveDropbox, ct);

        public Task<KDriveResourceResponse<KDriveDriveDetail>?> GetDriveAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}", query?.ToDictionary()), null, KDriveJsonContext.Default.KDriveResourceResponseKDriveDriveDetail, ct);

        public Task<KDriveResourceResponse<KDriveDriveSettings>?> GetDriveSettingsAsync(CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, $"/2/drive/{_driveId}/settings", null, KDriveJsonContext.Default.KDriveResourceResponseKDriveDriveSettings, ct);

        public Task<KDrivePagedResponse<KDriveDriveInvitation>?> GetDriveInvitationsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => _api.SendTypedAsync(HttpMethod.Get, QueryStringBuilder.BuildPath($"/2/drive/{_driveId}/users/invitation", query?.ToDictionary()), null, KDriveJsonContext.Default.KDrivePagedResponseKDriveDriveInvitation, ct);

        public Task<KDriveResourceResponse<KDriveCancelResource>?> RestoreTrashItemAsync(long fileId, long destinationDirectoryId, CancellationToken ct = default)
            => RestoreTrashedFileAsync(fileId, destinationDirectoryId, ct);

        public Task<KDriveResourceResponse<bool>?> EmptyTrashItemsAsync(CancellationToken ct = default)
            => EmptyTrashAsync(ct);

        public Task<KDriveResourceResponse<bool>?> PurgeTrashItemAsync(long fileId, CancellationToken ct = default)
            => PermanentlyDeleteTrashedFileAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveDirectoryCount>?> CountTrashAsync(CancellationToken ct = default)
            => GetTrashCountAsync(ct);

        public Task<KDriveResourceResponse<bool>?> FavoriteItemAsync(long fileId, CancellationToken ct = default)
            => AddFavoriteAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> UnfavoriteItemAsync(long fileId, CancellationToken ct = default)
            => RemoveFavoriteAsync(fileId, ct);

        public Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> UndoOperationAsync(string? cancelId = null, IEnumerable<string>? cancelIds = null, CancellationToken ct = default)
            => UndoAsync(cancelId, cancelIds, ct);

        public Task<KDriveResourceResponse<List<KDriveFeedbackResource>>?> CheckItemsExistAsync(IEnumerable<long> ids, CancellationToken ct = default)
            => CheckFilesExistAsync(ids, ct);

        public Task<KDriveResourceResponse<KDriveFileHash>?> GetItemHashAsync(long fileId, CancellationToken ct = default)
            => GetFileHashAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveFileSizeInfo>?> GetItemSizesAsync(long fileId, CancellationToken ct = default)
            => GetFileSizesAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveTemporaryUrl>?> GetItemTemporaryUrlAsync(long fileId, int? durationSeconds = null, CancellationToken ct = default)
            => GetFileTemporaryUrlAsync(fileId, durationSeconds, ct);

        public Task<KDriveResourceResponse<bool>?> TouchItemLastModifiedAsync(long fileId, long lastModifiedAt, CancellationToken ct = default)
            => SetFileLastModifiedAsync(fileId, lastModifiedAt, ct);

        public Task<KDriveResourceResponse<KDriveShareLink>?> GetItemShareLinkAsync(long fileId, CancellationToken ct = default)
            => GetShareLinkAsync(fileId, ct);

        public Task<KDriveResourceResponse<KDriveShareLink>?> CreateItemShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default)
            => CreateShareLinkAsync(fileId, request, ct);

        /// <summary>Updates the public share link of a file (API v2).</summary>
        public Task<KDriveResourceResponse<bool>?> UpdateItemShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default)
            => UpdateShareLinkAsync(fileId, request, ct);

        /// <summary>Deletes the public share link of a file (API v2).</summary>
        public Task<KDriveResourceResponse<bool>?> DeleteItemShareLinkAsync(long fileId, CancellationToken ct = default)
            => DeleteShareLinkAsync(fileId, ct);

        /// <summary>Invites users/emails to a file share link (API v2).</summary>
        public Task<KDriveResourceResponse<bool>?> InviteItemShareLinkAsync(long fileId, IEnumerable<string>? emails = null, IEnumerable<long>? userIds = null, string? message = null, CancellationToken ct = default)
            => InviteShareLinkAsync(fileId, emails, userIds, message, ct);

        /// <summary>Lists comments on a file (API v2, one page).</summary>
        public Task<KDrivePagedResponse<KDriveComment>?> GetItemCommentsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetFileCommentsAsync(fileId, query, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> GetItemCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => GetFileCommentAsync(fileId, commentId, ct);

        /// <summary>Creates a comment on a file (API v2).</summary>
        public Task<KDriveResourceResponse<KDriveComment>?> CreateItemCommentAsync(long fileId, string body, long? parentId = null, CancellationToken ct = default)
            => CreateFileCommentAsync(fileId, body, parentId, ct);

        /// <summary>Replies to a comment on a file (API v2).</summary>
        public Task<KDriveResourceResponse<KDriveComment>?> ReplyItemCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default)
            => ReplyFileCommentAsync(fileId, commentId, body, ct);

        public Task<KDriveResourceResponse<KDriveComment>?> UpdateItemCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default)
            => UpdateFileCommentAsync(fileId, commentId, body, ct);

        public Task<KDriveResourceResponse<bool>?> DeleteItemCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => DeleteFileCommentAsync(fileId, commentId, ct);

        public Task<KDriveResourceResponse<bool>?> LikeItemCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => LikeFileCommentAsync(fileId, commentId, ct);

        public Task<KDriveResourceResponse<bool>?> UnlikeItemCommentAsync(long fileId, long commentId, CancellationToken ct = default)
            => UnlikeFileCommentAsync(fileId, commentId, ct);

        /// <summary>Lists drive categories (API v2, one page).</summary>
        public Task<KDrivePagedResponse<KDriveCategory>?> ListCategoriesAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetCategoriesAsync(query, ct);

        /// <summary>Creates a drive category (API v2).</summary>
        public Task<KDriveResourceResponse<KDriveCategory>?> CreateDriveCategoryAsync(string name, string? color = null, CancellationToken ct = default)
            => CreateCategoryAsync(name, color, ct);

        /// <summary>Tags a file with a category (API v2).</summary>
        public Task<KDriveResourceResponse<bool>?> TagItemAsync(long fileId, long categoryId, CancellationToken ct = default)
            => AddCategoryToFileAsync(fileId, categoryId, ct);

        /// <summary>Removes a category tag from a file (API v2).</summary>
        public Task<KDriveResourceResponse<bool>?> UntagItemAsync(long fileId, long categoryId, CancellationToken ct = default)
            => RemoveCategoryFromFileAsync(fileId, categoryId, ct);

        /// <summary>Gets the dropbox settings of a file (API v2).</summary>
        public Task<KDriveResourceResponse<KDriveDropbox>?> GetItemDropboxAsync(long fileId, CancellationToken ct = default)
            => GetFileDropboxAsync(fileId, ct);

        /// <summary>Enables a dropbox on a file (API v2).</summary>
        public Task<KDriveResourceResponse<KDriveDropbox>?> EnableItemDropboxAsync(long fileId, KDriveDropboxRequest? payload = null, CancellationToken ct = default)
            => CreateFileDropboxAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<KDriveDropbox>?> UpdateItemDropboxAsync(long fileId, KDriveDropboxRequest payload, CancellationToken ct = default)
            => UpdateFileDropboxAsync(fileId, payload, ct);

        /// <summary>Disables the dropbox on a file (API v2).</summary>
        public Task<KDriveResourceResponse<bool>?> DisableItemDropboxAsync(long fileId, CancellationToken ct = default)
            => DeleteFileDropboxAsync(fileId, ct);

        public Task<KDriveResourceResponse<bool>?> InviteItemDropboxAsync(long fileId, KDriveDropboxInviteRequest payload, CancellationToken ct = default)
            => InviteFileDropboxAsync(fileId, payload, ct);

        /// <summary>Gets access rights for a file (API v2).</summary>
        public Task<KDriveResourceResponse<KDriveFileAccess>?> GetItemAccessAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default)
            => GetFileAccessAsync(fileId, query, ct);

        /// <summary>Shares a file with users (API v2).</summary>
        public Task<KDriveResourceResponse<bool>?> ShareItemWithUsersAsync(long fileId, KDriveFileAccessUsersRequest payload, CancellationToken ct = default)
            => AddFileAccessUsersAsync(fileId, payload, ct);

        public Task<KDriveResourceResponse<bool>?> RemoveItemAccessUserAsync(long fileId, long userId, CancellationToken ct = default)
            => RemoveFileAccessUserAsync(fileId, userId, ct);

        /// <summary>Lists pending drive invitations (API v2, one page).</summary>
        public Task<KDrivePagedResponse<KDriveDriveInvitation>?> ListInvitationsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetDriveInvitationsAsync(query, ct);

        public Task<KDriveResourceResponse<bool>?> SendInvitationAsync(long invitationId, CancellationToken ct = default)
            => SendDriveInvitationAsync(invitationId, ct);

        /// <summary>Lists external imports (API v2, one page).</summary>
        public Task<KDrivePagedResponse<KDriveExternalImport>?> ListImportsAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetImportsAsync(query, ct);

        /// <summary>Gets drive details (API v2).</summary>
        public Task<KDriveResourceResponse<KDriveDriveDetail>?> GetDriveInfoAsync(KDriveListQuery? query = null, CancellationToken ct = default)
            => GetDriveAsync(query, ct);

        private static Dictionary<string, object?>? BuildCancelPayload(string? cancelId, IEnumerable<string>? cancelIds)
        {
            var payload = new Dictionary<string, object?>();
            if (!string.IsNullOrWhiteSpace(cancelId))
                payload["cancel_id"] = cancelId;
            if (cancelIds is not null)
                payload["cancel_ids"] = cancelIds.ToArray();
            return payload.Count == 0 ? null : payload;
        }
    }
}
