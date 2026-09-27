using System.Text;

namespace kDriveClient.Models
{
    /// <summary>
    /// Local upload payload: the file content and metadata to upload to kDrive.
    /// </summary>
    /// <remarks>
    /// This is <b>not</b> a remote file-system resource. After a successful upload, use the
    /// returned <see cref="KDriveUploadResponse"/> (and/or fetch a
    /// <see cref="Domain.KDriveRemoteFile"/> via browse/get APIs).
    /// For remote files and directories already stored on the drive, see
    /// <see cref="Domain.KDriveItem"/>, <see cref="Domain.KDriveRemoteFile"/>, and
    /// <see cref="Domain.KDriveDirectory"/>.
    /// </remarks>
    /// <seealso cref="Domain.KDriveRemoteFile"/>
    public class KDriveFile : IDisposable
    {
        /// <summary>
        /// CreatedAt is the timestamp when the file was created.
        /// </summary>
        public long CreatedAt { get; set; }

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
        public long LastModifiedAt { get; set; }

        /// <summary>
        /// In case of a symbolic link, this is the target of the link.
        /// </summary>
        public string? SymbolicLink { get; set; }

        /// <summary>
        /// TotalChunkHash is the SHA-256 hash of the entire file content, computed from all chunks.
        /// </summary>
        public string TotalChunkHash { get; set; } = string.Empty;

        /// <summary>
        /// TotalSize is the total size of the file in bytes.
        /// </summary>
        public long TotalSize { get; set; }

        /// <summary>
        /// In case of conflict with an existing file, it defines how to manage the conflict.
        /// </summary>
        public ConflictChoice ConflictChoice { get; set; } = ConflictChoice.Version;

        /// <summary>
        /// LocalPath is the local file system path to the file.
        /// </summary>
        public string LocalPath { get; set; } = string.Empty;

        /// <summary>
        /// Optional in-memory content stream.
        /// </summary>
        public Stream? Content { get; set; }

        /// <summary>
        /// File chunks used for chunked upload.
        /// </summary>
        public List<KDriveChunk> Chunks { get; set; } = [];

        /// <summary>
        /// Constructs an empty file model.
        /// </summary>
        public KDriveFile()
        {
        }

        /// <summary>
        /// Constructs a KDriveFile instance from a local file path and drive path.
        /// </summary>
        /// <param name="localPath">Local file system path to the file.</param>
        /// <param name="drivePath">Drive path where the file will be uploaded.</param>
        /// <param name="conflictChoice">Conflict resolution strategy.</param>
        /// <exception cref="ArgumentException">Argument is null or whitespace.</exception>
        public KDriveFile(string localPath, string drivePath, ConflictChoice conflictChoice = ConflictChoice.Rename)
        {
            if (string.IsNullOrWhiteSpace(localPath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(localPath));

            var fileInfo = new FileInfo(localPath);
            Name = fileInfo.Name;
            LocalPath = localPath;
            DirectoryPath = drivePath;
            TotalSize = fileInfo.Length;
            ConflictChoice = conflictChoice;
            LastModifiedAt = new DateTimeOffset(fileInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
            CreatedAt = new DateTimeOffset(fileInfo.CreationTimeUtc).ToUnixTimeSeconds();
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
        /// Convert enum to string for API.
        /// </summary>
        /// <returns>String representation of the conflict mode.</returns>
        public string ConvertConflictChoice()
        {
            return ConflictChoice switch
            {
                ConflictChoice.Version => "version",
                ConflictChoice.Error => "error",
                ConflictChoice.Rename => "rename",
                _ => "error"
            };
        }

        /// <summary>
        /// Hash algorithm used when splitting into chunks.
        /// </summary>
        public KDriveUploadHashAlgorithm HashAlgorithm { get; set; } = KDriveUploadHashAlgorithm.Sha256;

        /// <summary>
        /// Split file content into chunks and compute chunk hashes.
        /// </summary>
        /// <param name="chunkSize">Target chunk size in bytes.</param>
        public void SplitIntoChunks(int chunkSize)
            => SplitIntoChunks(chunkSize, HashAlgorithm);

        /// <summary>
        /// Split file content into chunks and compute chunk hashes.
        /// </summary>
        public void SplitIntoChunks(int chunkSize, KDriveUploadHashAlgorithm algorithm)
        {
            if (chunkSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunkSize), "Chunk size must be greater than zero.");

            HashAlgorithm = algorithm;
            Chunks.Clear();
            using var source = OpenReadableStream();
            var buffer = new byte[chunkSize];
            var index = 0;
            int bytesRead;
            using var totalHasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                var chunkData = bytesRead == buffer.Length ? [.. buffer] : buffer[..bytesRead];
                var chunkHash = KDriveChunk.GetChunkHash(chunkData, algorithm);
                totalHasher.AppendData(Encoding.UTF8.GetBytes(chunkHash));
                Chunks.Add(new KDriveChunk(chunkData, index++, chunkHash, algorithm));
            }

            TotalSize = source.Length;
            TotalChunkHash = Convert.ToHexString(totalHasher.GetHashAndReset()).ToLowerInvariant();
        }

        /// <summary>
        /// Create an in-memory virtual file.
        /// </summary>
        internal static KDriveFile CreateVirtualFile(string name, Memory<byte> buffer, string drivePath)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return new KDriveFile
            {
                Name = name,
                DirectoryPath = drivePath,
                Content = new MemoryStream(buffer.ToArray(), writable: false),
                TotalSize = buffer.Length,
                CreatedAt = now,
                LastModifiedAt = now,
                LocalPath = string.Empty
            };
        }

        /// <summary>
        /// Frees resources used by the KDriveFile instance.
        /// </summary>
        public void Dispose()
        {
            Content?.Dispose();
            foreach (var chunk in Chunks)
            {
                chunk.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        private Stream OpenReadableStream()
        {
            if (!string.IsNullOrWhiteSpace(LocalPath))
            {
                return new FileStream(LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            if (Content is null)
            {
                throw new InvalidOperationException("Either LocalPath or Content must be provided.");
            }

            if (Content.CanSeek)
            {
                Content.Position = 0;
            }

            return Content;
        }

        public override string ToString()
        {
            var source = !string.IsNullOrWhiteSpace(LocalPath) ? "LocalPath" : (Content is not null ? "Content" : "None");
            return $"Name={Name}, DirectoryId={DirectoryId}, DirectoryPath={DirectoryPath}, TotalSize={TotalSize}, Chunks={Chunks.Count}, Conflict={ConflictChoice}, Source={source}, LastModifiedAt={LastModifiedAt}, CreatedAt={CreatedAt}";
        }
    }
}
