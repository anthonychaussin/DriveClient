using kDriveClient.Models;
using kDriveClient.Models.Domain;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// Share links, collaborative ACL, dropboxes, and invitation operations.
    /// </summary>
    /// <remarks>
    /// Distinguish:
    /// <list type="bullet">
    /// <item><description><see cref="KDriveShareLink"/> — public URL outsiders can open.</description></item>
    /// <item><description><see cref="KDriveAccess"/> / file access APIs — who can collaborate inside the drive.</description></item>
    /// <item><description><see cref="KDriveDropbox"/> — upload inbox on a folder.</description></item>
    /// </list>
    /// </remarks>
    public partial interface IKDriveShares
    {
        /// <summary>
        /// Gets accessible Drives.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveDriveSummary>?> GetAccessibleDrivesAsync(long accountId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets my Shared Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetMySharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches my Shared Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchMySharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets shared With Me Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetSharedWithMeFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches shared With Me Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchSharedWithMeFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets dropbox Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> GetDropboxFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Creates dropbox.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parentDirectoryId">Directory id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDropbox>?> CreateDropboxAsync(string name, long? parentDirectoryId = null, CancellationToken ct = default);

        /// <summary>
        /// Searches dropbox Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveNavigatorResponse<KDriveFileSystemItem>?> SearchDropboxFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets my Shared.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetMySharedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches my Shared.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> SearchMySharedAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets shared With Me.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetSharedWithMeAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Searches shared With Me.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> SearchSharedWithMeAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets dropboxes.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> GetDropboxesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Creates dropbox Item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parentDirectoryId">Directory id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDropbox>?> CreateDropboxItemAsync(string name, long? parentDirectoryId = null, CancellationToken ct = default);

        /// <summary>
        /// Searches dropboxes.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveItemPage?> SearchDropboxesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveShareLink>?> GetShareLinkAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Creates share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="request">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveShareLink>?> CreateShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default);

        /// <summary>
        /// Updates share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="request">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UpdateShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default);

        /// <summary>
        /// Deletes share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteShareLinkAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Invites share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="emails">The emails.</param>
        /// <param name="userIds">The user ids.</param>
        /// <param name="message">The message.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> InviteShareLinkAsync(long fileId, IEnumerable<string>? emails = null, IEnumerable<long>? userIds = null, string? message = null, CancellationToken ct = default);

        /// <summary>
        /// Gets file Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDropbox>?> GetFileDropboxAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Invitations.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveDriveInvitation>?> GetDriveInvitationsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets item Share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveShareLink>?> GetItemShareLinkAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Creates item Share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="request">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveShareLink>?> CreateItemShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default);

        /// <summary>
        /// Updates item Share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="request">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UpdateItemShareLinkAsync(long fileId, KDriveShareLinkRequest request, CancellationToken ct = default);

        /// <summary>
        /// Deletes item Share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteItemShareLinkAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Invites item Share Link.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="emails">The emails.</param>
        /// <param name="userIds">The user ids.</param>
        /// <param name="message">The message.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> InviteItemShareLinkAsync(long fileId, IEnumerable<string>? emails = null, IEnumerable<long>? userIds = null, string? message = null, CancellationToken ct = default);

        /// <summary>
        /// Gets item Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDropbox>?> GetItemDropboxAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Enables item Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDropbox>?> EnableItemDropboxAsync(long fileId, KDriveDropboxRequest? payload = null, CancellationToken ct = default);

        /// <summary>
        /// Updates item Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDropbox>?> UpdateItemDropboxAsync(long fileId, KDriveDropboxRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Disables item Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DisableItemDropboxAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Invites item Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> InviteItemDropboxAsync(long fileId, KDriveDropboxInviteRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets item Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileAccess>?> GetItemAccessAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Shares item With Users.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> ShareItemWithUsersAsync(long fileId, KDriveFileAccessUsersRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Removes item Access User.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveItemAccessUserAsync(long fileId, long userId, CancellationToken ct = default);

        /// <summary>
        /// Lists invitations.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveDriveInvitation>?> ListInvitationsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Sends invitation.
        /// </summary>
        /// <param name="invitationId">The invitation id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SendInvitationAsync(long invitationId, CancellationToken ct = default);

        /// <summary>
        /// Adds item Access Teams.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> AddItemAccessTeamsAsync(long fileId, KDriveFileAccessTeamsRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Creates share Link Archive.
        /// </summary>
        /// <param name="sharelinkUuid">The sharelink uuid.</param>
        /// <param name="fileIds">The file ids.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="exceptFileIds">The except file ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUuidResource>?> CreateShareLinkArchiveAsync(string sharelinkUuid, IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default);

        /// <summary>
        /// Checks item Access Invitations.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CheckItemAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsCheckRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Checks item Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CheckItemAccessAsync(long fileId, KDriveFileAccessCheckRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Creates item Access Invitations.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CreateItemAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Creates item Access Request.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CreateItemAccessRequestAsync(long fileId, KDriveCreateFileAccessRequestBody payload, CancellationToken ct = default);

        /// <summary>
        /// Declines access Request.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeclineAccessRequestAsync(long requestId, CancellationToken ct = default);

        /// <summary>
        /// Deletes invitation.
        /// </summary>
        /// <param name="invitationId">The invitation id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteInvitationAsync(long invitationId, CancellationToken ct = default);

        /// <summary>
        /// Forces item Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> ForceItemAccessAsync(long fileId, KDriveFileAccessForceRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets access Request.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileAccessRequest>?> GetAccessRequestAsync(long requestId, CancellationToken ct = default);

        /// <summary>
        /// Gets invitation.
        /// </summary>
        /// <param name="invitationId">The invitation id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveInvitation>?> GetInvitationAsync(long invitationId, CancellationToken ct = default);

        /// <summary>
        /// Gets item Access Invitations.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileAccessInvitation>?> GetItemAccessInvitationsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets item Access Requests.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileAccessRequest>?> GetItemAccessRequestsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets item Access Teams.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileAccessTeam>?> GetItemAccessTeamsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets item Access Users.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileAccessUser>?> GetItemAccessUsersAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets shared Files Activity Statistics.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetSharedFilesActivityStatisticsAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Performs grant Item Access Applications.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> GrantItemAccessApplicationsAsync(long fileId, KDriveFileAccessApplicationsRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Removes item Access Team.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="teamId">The team id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveItemAccessTeamAsync(long fileId, long teamId, CancellationToken ct = default);

        /// <summary>
        /// Performs set Item Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SetItemAccessAsync(long fileId, KDriveSetFileAccessRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs start Sharelink Import Job.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> StartSharelinkImportJobAsync(KDriveStartSharelinkImportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs sync Parent Item Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SyncParentItemAccessAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Updates invitation.
        /// </summary>
        /// <param name="invitationId">The invitation id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveInvitation>?> UpdateInvitationAsync(long invitationId, KDriveUpdateInvitationRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates item Access Team.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="teamId">The team id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UpdateItemAccessTeamAsync(long fileId, long teamId, KDriveFileAccessTeamUpdateRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates item Access User.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="userId">The user id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UpdateItemAccessUserAsync(long fileId, long userId, KDriveFileAccessUserUpdateRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Access Request.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileAccessRequest>?> GetDriveAccessRequestAsync(long requestId, CancellationToken ct = default);

        /// <summary>
        /// Declines drive Access Request.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeclineDriveAccessRequestAsync(long requestId, CancellationToken ct = default);

        /// <summary>
        /// Gets file Access Users.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileAccessUser>?> GetFileAccessUsersAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets file Access Teams.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileAccessTeam>?> GetFileAccessTeamsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets file Access Requests.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileAccessRequest>?> GetFileAccessRequestsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Removes file Access User.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="userId">The user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveFileAccessUserAsync(long fileId, long userId, CancellationToken ct = default);

        /// <summary>
        /// Removes file Access Team.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="teamId">The team id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveFileAccessTeamAsync(long fileId, long teamId, CancellationToken ct = default);

        /// <summary>
        /// Creates file Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDropbox>?> CreateFileDropboxAsync(long fileId, KDriveDropboxRequest? payload = null, CancellationToken ct = default);

        /// <summary>
        /// Updates file Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDropbox>?> UpdateFileDropboxAsync(long fileId, KDriveDropboxRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Deletes file Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteFileDropboxAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Invites file Dropbox.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> InviteFileDropboxAsync(long fileId, KDriveDropboxInviteRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets file Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveFileAccess>?> GetFileAccessAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Performs set File Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SetFileAccessAsync(long fileId, KDriveSetFileAccessRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Invitation.
        /// </summary>
        /// <param name="invitationId">The invitation id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveInvitation>?> GetDriveInvitationAsync(long invitationId, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Invitation.
        /// </summary>
        /// <param name="invitationId">The invitation id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveDriveInvitation>?> UpdateDriveInvitationAsync(long invitationId, KDriveUpdateInvitationRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Deletes drive Invitation.
        /// </summary>
        /// <param name="invitationId">The invitation id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteDriveInvitationAsync(long invitationId, CancellationToken ct = default);

        /// <summary>
        /// Sends drive Invitation.
        /// </summary>
        /// <param name="invitationId">The invitation id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SendDriveInvitationAsync(long invitationId, CancellationToken ct = default);

        /// <summary>
        /// Gets statistics Activities Shared Files.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveStatisticsData>?> GetStatisticsActivitiesSharedFilesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Performs start Sharelink Import.
        /// </summary>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveExternalImport>?> StartSharelinkImportAsync(KDriveStartSharelinkImportRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Adds file Access Users.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> AddFileAccessUsersAsync(long fileId, KDriveFileAccessUsersRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates file Access User.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="userId">The user id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UpdateFileAccessUserAsync(long fileId, long userId, KDriveFileAccessUserUpdateRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Adds file Access Teams.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> AddFileAccessTeamsAsync(long fileId, KDriveFileAccessTeamsRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Updates file Access Team.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="teamId">The team id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UpdateFileAccessTeamAsync(long fileId, long teamId, KDriveFileAccessTeamUpdateRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Gets file Access Invitations.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveFileAccessInvitation>?> GetFileAccessInvitationsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Creates file Access Invitations.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CreateFileAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Checks file Access Invitations.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CheckFileAccessInvitationsAsync(long fileId, KDriveFileAccessInvitationsCheckRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Creates file Access Request.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CreateFileAccessRequestAsync(long fileId, KDriveCreateFileAccessRequestBody payload, CancellationToken ct = default);

        /// <summary>
        /// Performs grant File Access Applications.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> GrantFileAccessApplicationsAsync(long fileId, KDriveFileAccessApplicationsRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Checks file Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> CheckFileAccessAsync(long fileId, KDriveFileAccessCheckRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Forces file Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> ForceFileAccessAsync(long fileId, KDriveFileAccessForceRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs sync Parent File Access.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SyncParentFileAccessAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Builds share Link Archive.
        /// </summary>
        /// <param name="sharelinkUuid">The sharelink uuid.</param>
        /// <param name="fileIds">The file ids.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="exceptFileIds">The except file ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveUuidResource>?> BuildShareLinkArchiveAsync(string sharelinkUuid, IEnumerable<long>? fileIds = null, long? parentId = null, IEnumerable<long>? exceptFileIds = null, CancellationToken ct = default);

    }
}
