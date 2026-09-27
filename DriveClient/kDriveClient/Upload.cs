using kDriveClient.kDriveClient.Application.Upload;
using kDriveClient.Models;
using System.Text.Json.Nodes;

namespace kDriveClient.kDriveClient
{
    public partial class KDriveClient
    {
        /// <inheritdoc cref="IKDriveUploadService.UploadDirectAsync" />
        public async Task<KDriveUploadResponse> UploadFileDirectAsync(KDriveFile file, CancellationToken ct = default)
        {
            await EnsureUploadInitializedAsync(ct).ConfigureAwait(false);
            UploadService.Progress = Progress;
            return await UploadService.UploadDirectAsync(file, ct).ConfigureAwait(false);
        }

        /// <inheritdoc cref="IKDriveUploadService.UploadChunkedAsync" />
        public async Task<KDriveUploadResponse> UploadFileChunkedAsync(KDriveFile file, CancellationToken ct = default)
        {
            await EnsureUploadInitializedAsync(ct).ConfigureAwait(false);
            UploadService.Progress = Progress;
            return await UploadService.UploadChunkedAsync(file, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<KDriveUploadResponse>> UploadBatchAsync(
            IEnumerable<KDriveFile> files,
            string? conflict = null,
            long? directoryId = null,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(files);
            await EnsureUploadInitializedAsync(ct).ConfigureAwait(false);

            var list = files.ToList();
            if (list.Count == 0)
                return [];

            // Announce batch intent when multiple files share a destination (best-effort).
            if (list.Count > 1)
            {
                try
                {
                    var batchFiles = list.Select(f =>
                    {
                        if (f.Chunks.Count == 0)
                            f.SplitIntoChunks(UploadService.ChunkSizeBytes, f.HashAlgorithm);
                        return new KDriveUploadSessionBatchFile
                        {
                            Name = f.Name,
                            Size = f.TotalSize,
                            TotalSize = f.TotalSize,
                            DirectoryId = directoryId
                                ?? (long.TryParse(f.DirectoryId, out var id) ? id : null),
                            DirectoryPath = f.DirectoryPath,
                            Hash = string.IsNullOrEmpty(f.TotalChunkHash)
                                ? null
                                : $"{KDriveChunk.ToApiPrefix(f.HashAlgorithm)}:{f.TotalChunkHash}"
                        };
                    }).ToArray();

                    await StartUploadSessionsBatchAsync(new KDriveUploadSessionBatchRequest
                    {
                        Files = batchFiles,
                        Conflict = conflict,
                        DirectoryId = directoryId
                    }, ct).ConfigureAwait(false);
                }
                catch
                {
                    // Fall back to independent uploads below.
                }
            }

            var results = new List<KDriveUploadResponse>(list.Count);
            foreach (var file in list)
                results.Add(await UploadAsync(file, ct).ConfigureAwait(false));

            return results;
        }

        internal Task<(string SessionToken, string UploadUrl)> StartUploadSessionAsync(KDriveFile file, CancellationToken ct = default)
        {
            return UploadService.StartSessionAsync(file, ct);
        }

        internal Task<JsonObject?> CancelUploadSessionRequest(string sessionToken, CancellationToken ct = default)
        {
            return UploadService.CancelSessionAsync(sessionToken, ct);
        }
    }
}
