namespace kDriveClient.Models
{
    /// <summary>
    /// KDriveClientOptions holds configuration options for the KDriveClient.
    /// </summary>
    public class KDriveClientOptions
    {
        /// <summary>
        /// Number of parallel chunk to upload.
        /// </summary>
        public int Parallelism { get; set; } = 4;

        /// <summary>
        /// Size of each chunk in bytes when performing chunked uploads.
        /// </summary>
        public int? ChunkSize { get; set; }

        /// <summary>
        /// Threshold in bytes to decide between direct upload and chunked upload.
        /// </summary>
        public long DirectUploadThresholdBytes { get; set; } = 1L * 1024 * 1024;

        /// <summary>
        /// Use automatic chunk size adjustment based on bandwidth.
        /// </summary>
        public bool UseAutoChunkSize { get; set; } = true;
    }
}