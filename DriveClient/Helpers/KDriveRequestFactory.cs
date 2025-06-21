using kDriveClient.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace kDriveClient.Helpers
{
    public static class KDriveRequestFactory
    {
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

        public static HttpRequestMessage CreateStartSessionRequest(long driveId, KDriveFile file)
        {
            var queryParams = JsonSerializer.Serialize(BuildUploadQueryParams(file));
            var content = new StringContent(queryParams);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return new HttpRequestMessage(HttpMethod.Post, $"/3/drive/{driveId}/upload/session/start")
            {
                Content = content
            };
        }

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

        public static HttpRequestMessage CreateFinishSessionRequest(long driveId, string sessionToken, string totalChunkHash)
        {
            var content = new StringContent(JsonSerializer.Serialize(new
            {
                total_chunk_hash = $"sha256:{totalChunkHash.ToLowerInvariant()}"
            }));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return new HttpRequestMessage(HttpMethod.Post, $"/3/drive/{driveId}/upload/session/{sessionToken}/finish")
            {
                Content = content
            };
        }

        public static HttpRequestMessage CreateCancelSessionRequest(long driveId, string sessionToken)
        {
            return new HttpRequestMessage(HttpMethod.Delete, $"/2/drive/{driveId}/upload/session/{sessionToken}");
        }

        public static HttpRequestMessage CreateDownloadRequest(long driveId, long fileId)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"/2/drive/{driveId}/files/{fileId}/download");
        }

        private static Dictionary<string, object> BuildUploadQueryParams(KDriveFile file)
        {
            var list = new Dictionary<string, object>
            {
                { "file_name", file.GetEscapedFileName() },
                { "total_size", file.TotalSize },
                {"conflict", "rename" }
            };
            

            AddDirectoryParam(file, list);
            AddOptionalParam(list, "with", file.SymbolicLink);
            AddOptionalNumericParam(list, "created_at", file.CreatedAt);
            AddOptionalNumericParam(list, "last_modified_at", file.LastModifiedAt);
            AddOptionalNumericParam(list, "total_chunks", file.Chunks.Count);
            AddOptionalChunkHash(list, file.TotalChunkHash);

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

        private static void AddOptionalNumericParam(Dictionary<string, object> list, string key, int value)
        {
            if (value > 0)
                list.Add(key, value);
        }

        private static void AddOptionalChunkHash(Dictionary<string, object> list, string? hash)
        {
            if (!string.IsNullOrWhiteSpace(hash))
                list.Add("total_chunk_hash", $"sha256:{hash.ToLowerInvariant()}");
        }
    }
}
