using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// High-level file upload operations.
    /// </summary>
    /// <remarks>
    /// Pass a local <see cref="KDriveFile"/> upload payload (stream/path). After upload,
    /// browse APIs return domain <see cref="Models.Domain.KDriveRemoteFile"/> instances —
    /// do not confuse the two types.
    /// </remarks>
    public interface IKDriveUpload
    {
        /// <summary>
        /// Number of parallel chunk uploads.
        /// </summary>
        int Parallelism { get; }

        /// <summary>
        /// Dynamic chunk size in bytes after strategy initialization.
        /// </summary>
        int DynamicChunkSizeBytes { get; }

        /// <summary>
        /// Progress reporter for tracking upload progress (0.0–1.0).
        /// </summary>
        IProgress<double>? Progress { get; set; }

        /// <summary>
        /// Uploads a file, choosing direct or chunked strategy automatically.
        /// </summary>
        Task<KDriveUploadResponse> UploadAsync(KDriveFile file, CancellationToken ct = default);

        /// <summary>
        /// Uploads a file in a single request.
        /// </summary>
        Task<KDriveUploadResponse> UploadFileDirectAsync(KDriveFile file, CancellationToken ct = default);

        /// <summary>
        /// Uploads a file using a chunked upload session (parallel when configured).
        /// </summary>
        Task<KDriveUploadResponse> UploadFileChunkedAsync(KDriveFile file, CancellationToken ct = default);

        /// <summary>
        /// Uploads multiple files via a v3 batch upload session (start → per-file chunked upload → batch finish).
        /// </summary>
        Task<IReadOnlyList<KDriveUploadResponse>> UploadBatchAsync(IEnumerable<KDriveFile> files, string? conflict = null, long? directoryId = null, CancellationToken ct = default);
    }
}
