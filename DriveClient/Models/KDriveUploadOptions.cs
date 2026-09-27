namespace kDriveClient.Models
{
    /// <summary>
    /// KDriveUploadOptions holds configuration options for the KDriveClient.
    /// </summary>
    public sealed class KDriveUploadOptions
    {
        /// <summary>
        /// Number of parallel chunk to upload.
        /// </summary>
        public int Parallelism { get; set; } = 4;

        /// <summary>
        /// Size of each chunk in bytes when performing chunked uploads.
        /// </summary>
        public int ChunkSize { get; set; }

        /// <summary>
        /// Threshold in bytes to decide between direct upload and chunked upload.
        /// </summary>
        public long DirectUploadThresholdBytes { get; set; } = 1L * 1024 * 1024;

        /// <summary>
        /// Use automatic chunk size adjustment based on bandwidth.
        /// </summary>
        public bool UseAutoChunkSize { get; set; } = true;

        /// <summary>
        /// Hash algorithm used for chunked uploads (<c>sha256</c> or <c>xxh3</c>). Default: sha256.
        /// </summary>
        public KDriveUploadHashAlgorithm HashAlgorithm { get; set; } = KDriveUploadHashAlgorithm.Sha256;

        public override string ToString()
        {
            return $"Parallelism={Parallelism}, ChunkSize={ChunkSize}, DirectUploadThresholdBytes={DirectUploadThresholdBytes}, UseAutoChunkSize={UseAutoChunkSize}, HashAlgorithm={HashAlgorithm}";
        }
    }

    /// <summary>Supported upload content-hash algorithms (API <c>algo:hex</c> form).</summary>
    public enum KDriveUploadHashAlgorithm
    {
        /// <summary>SHA-256 (default, widely supported).</summary>
        Sha256 = 0,

        /// <summary>XXH3 — faster for large files when the API accepts it.</summary>
        XxHash3 = 1
    }
}
