using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// File and archive download operations.
    /// </summary>
    public interface IKDriveDownload
    {
        /// <summary>
        /// Downloads a file to a temporary stream (deleted on dispose).
        /// </summary>
        Task<Stream> DownloadFileAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Downloads a file to a temporary stream with conversion / password / temporary-url options.
        /// </summary>
        Task<Stream> DownloadFileAsync(long fileId, KDriveDownloadOptions? options, CancellationToken ct = default);

        /// <summary>
        /// Downloads a file to the given path.
        /// </summary>
        Task DownloadFileAsync(long fileId, string filePath, CancellationToken ct = default);

        /// <summary>
        /// Downloads a file to the given path with conversion / password / temporary-url options.
        /// </summary>
        Task DownloadFileAsync(long fileId, string filePath, KDriveDownloadOptions? options, CancellationToken ct = default);

        /// <summary>
        /// Downloads a built archive ZIP by its UUID.
        /// </summary>
        Task<Stream> DownloadArchiveAsync(string archiveUuid, CancellationToken ct = default);

        /// <summary>
        /// Downloads a share-link archive ZIP.
        /// </summary>
        Task<Stream> DownloadShareLinkArchiveAsync(string sharelinkUuid, string archiveUuid, CancellationToken ct = default);

        /// <summary>
        /// Downloads a file preview stream.
        /// </summary>
        Task<Stream> DownloadFilePreviewAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Downloads a file thumbnail stream.
        /// </summary>
        Task<Stream> DownloadFileThumbnailAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Downloads a trashed item thumbnail stream.
        /// </summary>
        Task<Stream> DownloadTrashThumbnailAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Downloads a specific file version stream (v2).
        /// </summary>
        Task<Stream> DownloadFileVersionAsync(long fileId, long versionId, CancellationToken ct = default);
    }
}
