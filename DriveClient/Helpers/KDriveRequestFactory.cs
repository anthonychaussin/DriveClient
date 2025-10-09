using kDriveClient.Models;
using System.Net.Http.Headers;

namespace kDriveClient.Helpers
{
    /// <summary>
    /// KDriveRequestFactory is a static class that provides methods to create HTTP requests for various operations related to KDrive files.
    /// </summary>
    public static class KDriveRequestFactory
    {
        /// <summary>
        /// Creates an HTTP request for uploading a file directly to KDrive.
        /// </summary>
        /// <param name="driveId">The ID of the drive where the file will be uploaded.</param>
        /// <param name="file">The KDriveFile object containing the file details to be uploaded.</param>
        /// <returns>An HttpRequestMessage configured for the upload operation.</returns>
        public static HttpRequestMessage CreateUploadDirectRequest(long driveId, KDriveFile file)
        {
            var url = $"/3/drive/{driveId}/upload?" + string.Join("&", BuildUploadQueryParams(file).ToList().ConvertAll(e => $"{e.Key}={e.Value}"));
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StreamContent(new MemoryStream(file.Chunks.First().Content))
            };

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            request.Content.Headers.ContentLength = file.TotalSize;

            return request;
        }

        /// <summary>
        /// Creates an HTTP request to start a file upload session in KDrive.
        /// </summary>
        /// <param name="driveId">The ID of the drive where the file will be uploaded.</param>
        /// <param name="file">The KDriveFile object containing the file details to be uploaded.</param>
        /// <returns>An HttpRequestMessage configured to start the upload session.</returns>
        public static HttpRequestMessage CreateStartSessionRequest(long driveId, KDriveFile file)
        {
            var content = new StringContent(JsonSerializer.Serialize(BuildUploadQueryParams(file), KDriveJsonContext.Default.Object));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return new HttpRequestMessage(HttpMethod.Post, $"/3/drive/{driveId}/upload/session/start")
            {
                Content = content
            };
        }

        /// <summary>
        /// Creates an HTTP request to upload a chunk of a file to KDrive.
        /// </summary>
        /// <param name="baseUrl">The base URL of the KDrive API.</param>
        /// <param name="token">The session token for the upload session.</param>
        /// <param name="driveId">The ID of the drive where the file is being uploaded.</param>
        /// <param name="chunk">The KDriveChunk object containing the chunk data to be uploaded.</param>
        /// <returns>An HttpRequestMessage configured for the chunk upload operation.</returns>
        public static HttpRequestMessage CreateChunkUploadRequest(string baseUrl, string token, long driveId, KDriveChunk chunk)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/3/drive/{driveId}/upload/session/{token}/chunk?chunk_number={chunk.ChunkNumber + 1}&chunk_size={chunk.ChunkSize}&chunk_hash=sha256:{chunk.ChunkHash.ToLowerInvariant()}")
            {
                Content = new ByteArrayContent(chunk.Content)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            request.Content.Headers.ContentLength = chunk.ChunkSize;

            return request;
        }

        /// <summary>
        /// Creates an HTTP request to finish an upload session in KDrive.
        /// </summary>
        /// <param name="driveId">The ID of the drive where the file is being uploaded.</param>
        /// <param name="sessionToken">The session token for the upload session.</param>
        /// <param name="totalChunkHash">The SHA-256 hash of the entire file content.</param>
        /// <returns>An HttpRequestMessage configured to finish the upload session.</returns>
        public static HttpRequestMessage CreateFinishSessionRequest(long driveId, string sessionToken, string totalChunkHash)
        {
            var finishRequest = new KDriveFinishRequest
            {
                TotalChunkHash = $"sha256:{totalChunkHash.ToLowerInvariant()}"
            };

            var content = new StringContent(JsonSerializer.Serialize(finishRequest, KDriveJsonContext.Default.KDriveFinishRequest));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return new HttpRequestMessage(HttpMethod.Post, $"/3/drive/{driveId}/upload/session/{sessionToken}/finish")
            {
                Content = content
            };
        }

        /// <summary>
        /// Creates an HTTP request to cancel an upload session in KDrive.
        /// </summary>
        /// <param name="driveId">The ID of the drive where the file is being uploaded.</param>
        /// <param name="sessionToken">The session token for the upload session.</param>
        /// <returns>An HttpRequestMessage configured to cancel the upload session.</returns>
        public static HttpRequestMessage CreateCancelSessionRequest(long driveId, string sessionToken)
        {
            return new HttpRequestMessage(HttpMethod.Delete, $"/2/drive/{driveId}/upload/session/{sessionToken}");
        }

        /// <summary>
        /// Creates an HTTP request to download a file from KDrive.
        /// </summary>
        /// <param name="driveId">The ID of the drive where the file is being uploaded.</param>
        /// <param name="fileId">The ID of the file to be downloaded.</param>
        /// <returns>An HttpRequestMessage configured for the download operation.</returns>
        public static HttpRequestMessage CreateDownloadRequest(long driveId, long fileId)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"/2/drive/{driveId}/files/{fileId}/download");
        }

        /// <summary>
        /// Builds the query parameters for the file upload request.
        /// </summary>
        /// <param name="file">The KDriveFile object containing the file details.</param>
        /// <returns>A dictionary containing the query parameters for the upload request.</returns>
        private static Dictionary<string, object> BuildUploadQueryParams(KDriveFile file)
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
            AddOptionalNumericParam(list, "total_chunks", file.Chunks.Count);
            AddOptionalChunkHash(list, file.TotalChunkHash);

            return list;
        }

        /// <summary>
        /// Add the directory parameter to the upload request.
        /// </summary>
        /// <param name="file">The KDriveFile object containing the file details.</param>
        /// <param name="list">The dictionary to which the directory parameter will be added.</param>
        /// <exception cref="ArgumentException">Thrown if neither DirectoryId nor DirectoryPath is provided.</exception>
        private static void AddDirectoryParam(KDriveFile file, Dictionary<string, object> list)
        {
            if (file.DirectoryId is not null)
                list.Add("directory_id", file.DirectoryId);
            else if (!string.IsNullOrWhiteSpace(file.DirectoryPath))
                list.Add("directory_path", file.DirectoryPath);
            else
                throw new ArgumentException("Either DirectoryId or DirectoryPath must be provided");
        }

        /// <summary>
        /// Add an optional parameter to the upload request if the value is not null or whitespace.
        /// </summary>
        /// <param name="list">The dictionary to which the parameter will be added.</param>
        /// <param name="key">The key for the parameter.</param>"
        /// <param name="value">The value for the parameter.</param>
        private static void AddOptionalParam(Dictionary<string, object> list, string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                list.Add(key, value);
        }

        /// <summary>
        /// Add an optional numeric parameter to the upload request if the value is greater than zero.
        /// </summary>
        /// <param name="list">The dictionary to which the parameter will be added.</param>
        /// <param name="key">The key for the parameter.</param>"
        /// <param name="value">The value for the parameter.</param>
        private static void AddOptionalNumericParam(Dictionary<string, object> list, string key, int value)
        {
            if (value > 0)
                list.Add(key, value);
        }

        /// <summary>
        /// Add the total chunk hash to the upload request if it is not null or whitespace.
        /// </summary>
        /// <param name="list">The dictionary to which the parameter will be added.</param>
        /// <param name="hash">The SHA-256 hash of the entire file content.</param>
        private static void AddOptionalChunkHash(Dictionary<string, object> list, string? hash)
        {
            if (!string.IsNullOrWhiteSpace(hash))
                list.Add("total_chunk_hash", $"sha256:{hash.ToLowerInvariant()}");
        }
    }
}