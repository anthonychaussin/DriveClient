using System.Text;

namespace kDriveClient.Models
{
    /// <summary>
    /// KDriveFile represents a file in the kDrive system.
    /// </summary>
    public class KDriveFile
    {
        private string _totalChunkHash;
        /// <summary>
        /// CreatedAt is the timestamp when the file was created.
        /// </summary>
        public int CreatedAt { get; set; }

        /// <summary>
        /// DirectoryId is the unique identifier for the directory containing this file.
        /// </summary>
        public string? DirectoryId { get; set; }

        /// <summary>
        /// DirectoryPath is the path to the directory containing this file.
        /// </summary>
        public string? DirectoryPath { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// LastModifiedAt is the timestamp when the file was last modified.
        /// </summary>
        public int LastModifiedAt { get; set; }

        /// <summary>
        /// In case of a symbolic link, this is the target of the link.
        /// </summary>
        public string? SymbolicLink { get; set; }

        /// <summary>
        /// TotalChunkHash is the SHA-256 hash of the entire file content, computed from all chunks.
        /// </summary>
        public string TotalChunkHash
        {
            get
            {
                if(this._totalChunkHash is null)
                {
                    this._totalChunkHash = Convert.ToHexString(
                        SHA256.HashData(this.Content));
                }

                return this._totalChunkHash;
            }
        }

        /// <summary>
        /// TotalSize is the total size of the file in bytes, calculated as the sum of all chunk sizes.
        /// </summary>
        public long TotalSize => this.Chunks.Sum(c => c.ChunkSize);

        /// <summary>
        /// Chunks is a list of KDriveChunk objects representing the file's content split into chunks.
        /// </summary>
        public List<KDriveChunk> Chunks { get; set; } = [];

        /// <summary>
        /// Content is a stream representing the file's content.
        /// </summary>
        public required Stream Content { get; init; }

        /// <summary>
        /// In case of conflict with an existing file, it define how to manage the conflict
        /// </summary>
        public ConflictChoice ConflictChoice { get; set; } = ConflictChoice.Version;

        /// <summary>
        /// Splits the file content into chunks of the specified size.
        /// </summary>
        /// <param name="chunkSize">Define the size of each chunk (except the last one)</param>
        public void SplitIntoChunks(int chunkSize)
        {
            var buffer = new byte[chunkSize];
            int chunkNumber = 0;
            int bytesRead;

            this.Content.Position = 0;

            while ((bytesRead = this.Content.Read(buffer, 0, chunkSize)) > 0)
            {
                byte[] content = [.. buffer.Take(bytesRead)];
                this.Chunks.Add(new KDriveChunk(content, chunkNumber++, SHA256.HashData(content)));
            }

            this.Content.Position = 0;
        }

        /// <summary>
        /// Escapes the file name to ensure it is safe for use in URLs.
        /// </summary>
        /// <returns>Escaped string</returns>
        public string GetEscapedFileName()
        {
            return Uri.EscapeDataString(Name.Replace("/", ":"));
        }

        /// <summary>
        /// Convert enum to string for api
        /// </summary>
        /// <returns>String representation of </returns>
        public string ConvertConflictChoice()
        {
            return this.ConflictChoice switch
            {
                ConflictChoice.Version => "version",
                ConflictChoice.Error => "error",
                ConflictChoice.Rename => "rename",
                _ => "error"
            };
        }
    }
}