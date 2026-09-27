namespace kDriveClient.kDriveClient.Domain.Upload
{
    /// <summary>
    /// Domain model describing how uploads should be split between direct and chunked paths.
    /// </summary>
    public sealed class UploadStrategy
    {
        private const long OneMegabyte = 1L * 1024 * 1024;

        public UploadStrategy(int chunkSizeBytes, long directUploadThresholdBytes)
        {
            if (chunkSizeBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunkSizeBytes), "Chunk size must be greater than zero.");
            if (directUploadThresholdBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(directUploadThresholdBytes), "Direct upload threshold must be greater than zero.");

            ChunkSizeBytes = chunkSizeBytes;
            DirectUploadThresholdBytes = directUploadThresholdBytes;
        }

        public int ChunkSizeBytes { get; }

        public long DirectUploadThresholdBytes { get; }

        public bool ShouldUseDirectUpload(long fileSizeBytes)
        {
            return fileSizeBytes <= OneMegabyte || fileSizeBytes <= DirectUploadThresholdBytes;
        }
    }
}
