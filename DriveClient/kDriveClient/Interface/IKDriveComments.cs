using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// Comments on files and directories.
    /// </summary>
    public partial interface IKDriveComments
    {
        /// <summary>
        /// Gets file Comments.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveComment>?> GetFileCommentsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets file Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveComment>?> GetFileCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        /// <summary>
        /// Creates file Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="body">The body.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveComment>?> CreateFileCommentAsync(long fileId, string body, long? parentId = null, CancellationToken ct = default);

        /// <summary>
        /// Performs reply File Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="body">The body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveComment>?> ReplyFileCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default);

        /// <summary>
        /// Updates file Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="body">The body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveComment>?> UpdateFileCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default);

        /// <summary>
        /// Deletes file Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteFileCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        /// <summary>
        /// Gets item Comments.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveComment>?> GetItemCommentsAsync(long fileId, KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Gets item Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveComment>?> GetItemCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        /// <summary>
        /// Creates item Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="body">The body.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveComment>?> CreateItemCommentAsync(long fileId, string body, long? parentId = null, CancellationToken ct = default);

        /// <summary>
        /// Performs reply Item Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="body">The body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveComment>?> ReplyItemCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default);

        /// <summary>
        /// Updates item Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="body">The body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveComment>?> UpdateItemCommentAsync(long fileId, long commentId, string body, CancellationToken ct = default);

        /// <summary>
        /// Deletes item Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteItemCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        /// <summary>
        /// Performs like Item Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> LikeItemCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        /// <summary>
        /// Performs unlike Item Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UnlikeItemCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        /// <summary>
        /// Performs like File Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> LikeFileCommentAsync(long fileId, long commentId, CancellationToken ct = default);

        /// <summary>
        /// Performs unlike File Comment.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="commentId">The comment id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UnlikeFileCommentAsync(long fileId, long commentId, CancellationToken ct = default);

    }
}
