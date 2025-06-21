using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// Interface for kDriveClient providing methods to upload and download files.
    /// </summary>
    public interface IKDriveClient
    {
        /// <summary>
        /// Uploads a file directly to kDrive in one request (for small files).
        /// </summary>
        /// <param name="file"><see cref="KDriveFile"/></param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <rurns><see cref="KDriveUploadResponse"/></returns>
        Task<KDriveUploadResponse> UploadAsync(KDriveFile file, CancellationToken ct = default);

        /// <summary>
        /// Downloads a file from kDrive by its ID.
        /// </summary>
        /// <param name="fileId">File ID.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Stream containing the file data.</returns>
        Task<Stream> DownloadFileAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Downloads a file from kDrive by its ID and writes it directly to a destination stream.
        /// </summary>
        /// <param name="fileId">File ID.</param>
        /// <param name="destination">Destination stream to write the file data.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task DownloadFileAsync(long fileId, Stream destination, CancellationToken ct = default);
    }
}