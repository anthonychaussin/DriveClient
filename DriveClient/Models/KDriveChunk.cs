namespace kDriveClient.Models
{
    /// <summary>
    /// kDriveChunk represents a chunk of Data in a kDrive file.
    /// </summary>
    public class KDriveChunk(byte[] content, int chunkNumber, byte[] hash) : IDisposable
    {
        /// <summary>
        /// ChunkHash is the SHA-256 hash of the chunk content.
        /// </summary>
        public string ChunkHash { get; init; } = Convert.ToHexString(hash);

        /// <summary>
        /// ChunkNumber is the sequential number of the chunk in the file.
        /// </summary>
        public int ChunkNumber { get; init; } = chunkNumber;

        /// <summary>
        /// ChunkSize is the size of the chunk in bytes.
        /// </summary>
        public int ChunkSize { get; set; } = content.Length;

        /// <summary>
        /// Content is the actual byte content of the chunk.
        /// </summary>
        public byte[]? Content { get; private set; } = content;

        /// <summary>
        /// Frees resources used by the KDriveChunk.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Deletes the content of the chunk to free memory.
        /// </summary>
        internal void Clean()
        {
            this.Content = null;
            GC.Collect();
        }
    }
}