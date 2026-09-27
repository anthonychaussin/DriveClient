using kDriveClient.Helpers;
using kDriveClient.kDriveClient.Interface;
using kDriveClient.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// HTTP transport for upload session operations, using a shared send pipeline.
    /// </summary>
    public sealed class KDriveHttpTransport : IKDriveTransport
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _send;
        private readonly ILogger? _logger;

        /// <summary>
        /// Creates a transport that sends through the provided send delegate (shared pipeline).
        /// </summary>
        public KDriveHttpTransport(
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send,
            ILogger? logger = null)
        {
            _send = send ?? throw new ArgumentNullException(nameof(send));
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<KDriveUploadResponse> DirectUploadAsync(long driveId, KDriveFile file, CancellationToken ct)
        {
            _logger?.LogInformation("Starting direct upload for file '{FileName}' with size {FileSize} bytes...", file.Name, file.TotalSize);
            Stream stream;
            if (!string.IsNullOrWhiteSpace(file.LocalPath))
            {
                stream = new FileStream(file.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else if (file.Content is not null)
            {
                if (file.Content.CanSeek)
                {
                    file.Content.Position = 0;
                }
                stream = file.Content;
            }
            else
            {
                throw new InvalidOperationException("Either LocalPath or Content must be provided for direct upload.");
            }

            using var req = new HttpRequestMessage(HttpMethod.Post, $"/3/drive/{driveId}/upload?" + string.Join("&", BuildUploadQueryParams(file, includeChunkMetadata: false).ToList().ConvertAll(e => $"{e.Key}={e.Value}")))
            {
                Content = new StreamContent(stream),
                Version = HttpVersion.Version20,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
            };

            using var response = await _send(req, ct).ConfigureAwait(false);
            try
            {
                _logger?.LogInformation("Direct upload for file '{FileName}' completed successfully.", file.Name);
                return KDriveJsonHelper.DeserializeUploadResponse(await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false));
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError(ex, "Failed to upload file '{FileName}' directly", file.Name);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<(string Token, string UploadUrl)> StartUploadSessionAsync(long driveId, KDriveFile file, CancellationToken ct)
        {
            using var content = new StringContent(JsonSerializer.Serialize(BuildUploadQueryParams(file, includeChunkMetadata: true), KDriveJsonContext.Default.Object));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using var req = new HttpRequestMessage(HttpMethod.Post, $"/3/drive/{driveId}/upload/session/start")
            {
                Content = content,
                Version = HttpVersion.Version20,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
            };

            _logger?.LogInformation("Starting upload session for file '{FileName}' with size {FileSize} bytes...", file.Name, file.TotalSize);

            using var response = await _send(req, ct).ConfigureAwait(false);
            try
            {
                _logger?.LogInformation("Upload session for file '{FileName}' started successfully.", file.Name);
                return KDriveJsonHelper.ParseStartSessionResponse(await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false));
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError(ex, "Failed to start upload session for file '{FileName}'", file.Name);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonObject?> UploadChunkAsync(string baseUrl, long driveId, string sessionId, KDriveChunk chunk, KDriveFile file, CancellationToken ct)
        {
            using var content = new ReadOnlyMemoryContent(chunk.Content);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using var req = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/3/drive/{driveId}/upload/session/{sessionId}/chunk?chunk_number={chunk.ChunkNumber + 1}&chunk_size={chunk.Content.Length}&chunk_hash={chunk.ApiChunkHash}")
            {
                Content = content,
                Version = HttpVersion.Version20,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
            };

            _logger?.LogInformation("Uploading chunk {ChunkNumber}/{TotalChunks} for file '{FileName}' with size {ChunkSize} bytes...",
                chunk.ChunkNumber + 1, file.Chunks.Count, file.Name, chunk.ChunkSize);

            using var response = await _send(req, ct).ConfigureAwait(false);
            try
            {
                return JsonNode.Parse(await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false)) as JsonObject;
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError(ex, "Failed to upload chunk {ChunkNumber} for file '{FileName}'", chunk.ChunkNumber + 1, file.Name);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<KDriveUploadResponse> CloseSessionAsync(long driveId, string sessionId, string totalHashHex, CancellationToken ct, KDriveUploadHashAlgorithm algorithm = KDriveUploadHashAlgorithm.Sha256)
        {
            using var content = new StringContent(JsonSerializer.Serialize(new { total_chunk_hash = KDriveChunk.ToApiHash(algorithm, totalHashHex) }, KDriveJsonContext.Default.Object));
            using var req = new HttpRequestMessage(HttpMethod.Post, $"/3/drive/{driveId}/upload/session/{sessionId}/finish") { Content = content };
            using var response = await _send(req, ct).ConfigureAwait(false);
            try
            {
                _logger?.LogInformation("Upload session with token '{SessionToken}' finished successfully.", sessionId);
                return KDriveJsonHelper.DeserializeUploadResponse(await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false));
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError(ex, "Failed to finish upload session with token '{SessionToken}'", sessionId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonObject?> CancelSessionAsync(long driveId, string sessionId, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(HttpMethod.Delete, $"/3/drive/{driveId}/upload/session/{sessionId}");
            using var response = await _send(req, ct).ConfigureAwait(false);
            try
            {
                _logger?.LogInformation("Upload session with token '{SessionToken}' cancelled successfully.", sessionId);
                return JsonNode.Parse(await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false)) as JsonObject;
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError(ex, "Failed to cancel upload session with token '{SessionToken}'", sessionId);
                throw;
            }
        }

        private static Dictionary<string, object> BuildUploadQueryParams(KDriveFile file, bool includeChunkMetadata)
        {
            var list = new Dictionary<string, object>
            {
                { "file_name", file.GetEscapedFileName() },
                { "total_size", file.TotalSize },
                { "conflict", file.ConvertConflictChoice() }
            };

            AddDirectoryParam(file, list);
            AddOptionalParam(list, "with", file.SymbolicLink);
            AddOptionalNumericParam(list, "created_at", file.CreatedAt);
            AddOptionalNumericParam(list, "last_modified_at", file.LastModifiedAt);
            if (includeChunkMetadata)
            {
                AddOptionalNumericParam(list, "total_chunks", file.Chunks.Count);
                AddOptionalChunkHash(list, file.TotalChunkHash, file.HashAlgorithm);
            }

            return list;
        }

        private static void AddDirectoryParam(KDriveFile file, Dictionary<string, object> list)
        {
            if (file.DirectoryId is not null)
                list.Add("directory_id", file.DirectoryId);
            else if (!string.IsNullOrWhiteSpace(file.DirectoryPath))
                list.Add("directory_path", file.DirectoryPath);
            else
                throw new ArgumentException("Either DirectoryId or DirectoryPath must be provided");
        }

        private static void AddOptionalParam(Dictionary<string, object> list, string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                list.Add(key, value);
        }

        private static void AddOptionalNumericParam(Dictionary<string, object> list, string key, long value)
        {
            if (value > 0)
                list.Add(key, value);
        }

        private static void AddOptionalChunkHash(Dictionary<string, object> list, string? hash, KDriveUploadHashAlgorithm algorithm)
        {
            if (!string.IsNullOrWhiteSpace(hash))
                list.Add("total_chunk_hash", KDriveChunk.ToApiHash(algorithm, hash));
        }
    }
}
