using System.IO.Hashing;
using System.Text;

namespace kDriveClient.Models
{
    /// <summary>
    /// kDriveChunk represents a chunk of Data in a kDrive file.
    /// </summary>
    public class KDriveChunk(Memory<byte> content, int chunkNumber, string hash, KDriveUploadHashAlgorithm algorithm = KDriveUploadHashAlgorithm.Sha256) : IDisposable
    {
        /// <summary>
        /// ChunkHash is the hex digest of the chunk content (without algo prefix).
        /// </summary>
        public string ChunkHash { get; init; } = hash.ToLowerInvariant();

        /// <summary>Hash algorithm used for <see cref="ChunkHash"/>.</summary>
        public KDriveUploadHashAlgorithm HashAlgorithm { get; init; } = algorithm;

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
        public Memory<byte> Content { get; private set; } = content;

        /// <summary>API form <c>algo:hex</c>.</summary>
        public string ApiChunkHash => $"{ToApiPrefix(HashAlgorithm)}:{ChunkHash}";

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
            this.Content = Memory<byte>.Empty;
            GC.Collect();
        }

        /// <summary>
        /// Hashes content with the given algorithm.
        /// </summary>
        public static string GetChunkHash(byte[] content, KDriveUploadHashAlgorithm algorithm = KDriveUploadHashAlgorithm.Sha256)
        {
            return algorithm switch
            {
                KDriveUploadHashAlgorithm.XxHash3 => Convert.ToHexString(XxHash3.Hash(content)).ToLowerInvariant(),
                _ => Convert.ToHexString(SHA256.HashData(content)).ToLowerInvariant()
            };
        }

        /// <summary>Gets the SHA-256 hash of the given content.</summary>
        public static string GetChunkHash(byte[] content) => GetChunkHash(content, KDriveUploadHashAlgorithm.Sha256);

        /// <summary>
        /// Computes the total upload hash as the digest of concatenated chunk digests (API contract).
        /// </summary>
        public static string ComputeTotalChunkHash(IEnumerable<string> orderedChunkHashes, KDriveUploadHashAlgorithm algorithm)
        {
            ArgumentNullException.ThrowIfNull(orderedChunkHashes);
            using var ms = new MemoryStream();
            foreach (var hash in orderedChunkHashes)
            {
                var bytes = Encoding.UTF8.GetBytes(hash);
                ms.Write(bytes, 0, bytes.Length);
            }

            var payload = ms.ToArray();
            return GetChunkHash(payload, algorithm);
        }

        public static string ToApiPrefix(KDriveUploadHashAlgorithm algorithm) => algorithm switch
        {
            KDriveUploadHashAlgorithm.XxHash3 => "xxh3",
            _ => "sha256"
        };

        /// <summary>API form <c>algo:hex</c> for a total chunk hash.</summary>
        public static string ToApiHash(KDriveUploadHashAlgorithm algorithm, string hexDigest)
            => $"{ToApiPrefix(algorithm)}:{hexDigest.ToLowerInvariant()}";


        public override string ToString()
        {
            return $"ChunkNumber={ChunkNumber}, ChunkSize={ChunkSize}, Hash={ApiChunkHash}";
        }
    }
}
