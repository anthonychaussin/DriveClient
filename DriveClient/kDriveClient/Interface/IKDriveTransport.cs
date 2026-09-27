using kDriveClient.Models;
using System.Text.Json.Nodes;

namespace kDriveClient.kDriveClient.Interface
{
    /// <summary>
    /// Interface for kDriveTransport providing methods to manage file upload sessions.
    /// </summary>
    public interface IKDriveTransport
    {
        /// <summary>
        /// Starts an upload session for a file in the specified drive.
        /// </summary>
        /// <param name="driveId">Drive identifier where the file will be uploaded.</param>
        /// <param name="file">KDriveFile object containing file details.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Tuple containing the session token and upload URL.</returns>
        Task<(String Token, String UploadUrl)> StartUploadSessionAsync(long driveId, KDriveFile file, CancellationToken ct);

        /// <summary>
        /// Creates an HTTP request for uploading a file directly to KDrive.
        /// </summary>
        /// <param name="driveId">The ID of the drive where the file will be uploaded.</param>
        /// <param name="file">The KDriveFile object containing the file details to be uploaded.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>An HttpRequestMessage configured for the upload operation.</returns>
        Task<KDriveUploadResponse> DirectUploadAsync(long driveId, KDriveFile file, CancellationToken ct);

        /// <summary>
        /// Uploads a chunk of a file to the specified drive and session.
        /// </summary>
        /// <param name="baseUrl">Base URL for the upload session.</param>
        /// <param name="driveId">Drive identifier where the file is being uploaded.</param>
        /// <param name="sessionId">Session identifier for the upload session.</param>
        /// <param name="chunk">Chunk of the file to be uploaded.</param>
        /// <param name="file">File being uploaded.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>JsonObject representing the response from the server.</returns>
        Task<JsonObject?> UploadChunkAsync(string baseUrl, long driveId, string sessionId, KDriveChunk chunk, KDriveFile file, CancellationToken ct);

        /// <summary>
        /// Closes the upload session after all chunks have been uploaded.
        /// </summary>
        /// <param name="driveId">Drive identifier where the file is being uploaded.</param>
        /// <param name="sessionId">Session identifier for the upload session.</param>
        /// <param name="totalSha256Hex">Hexadecimal SHA-256 hash of the entire file content.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>KDriveUploadResponse representing the uploaded file details.</returns>
        Task<KDriveUploadResponse> CloseSessionAsync(long driveId, string sessionId, string totalHashHex, CancellationToken ct, KDriveUploadHashAlgorithm algorithm = KDriveUploadHashAlgorithm.Sha256);

        /// <summary>
        /// Cancels an ongoing upload session incase of errors or interruptions.
        /// </summary>
        /// <param name="driveId">Drive identifier where the file is being uploaded.</param>
        /// <param name="sessionId">Session identifier for the upload session.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>JsonObject representing the response from the server.</returns>
        Task<JsonObject?> CancelSessionAsync(Int64 driveId, String sessionId, CancellationToken ct);
    }

}
