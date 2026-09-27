using kDriveClient.Models;
using System.Net;
using System.Net.Http.Headers;

namespace kDriveClient.kDriveClient
{
    internal static class KDriveRequestFactory
    {
        public static HttpRequestMessage CreateDownloadRequest(long driveId, long fileId, string? convertAs = null, string? password = null)
        {
            var path = $"/2/drive/{driveId}/files/{fileId}/download";
            if (!string.IsNullOrWhiteSpace(convertAs))
                path += $"?as={Uri.EscapeDataString(convertAs)}";

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            if (!string.IsNullOrEmpty(password))
                request.Headers.TryAddWithoutValidation("x-kdrive-file-password", password);

            return request;
        }

        public static HttpRequestMessage CreateArchiveDownloadRequest(long driveId, string archiveUuid)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"/2/drive/{driveId}/files/archives/{archiveUuid}");
        }

        public static HttpRequestMessage CreateShareLinkArchiveDownloadRequest(long driveId, string sharelinkUuid, string archiveUuid)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"/2/app/{driveId}/share/{sharelinkUuid}/archive/{archiveUuid}/download");
        }

        public static HttpRequestMessage CreateFilePreviewRequest(long driveId, long fileId)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"/2/drive/{driveId}/files/{fileId}/preview");
        }

        public static HttpRequestMessage CreateFileThumbnailRequest(long driveId, long fileId)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"/2/drive/{driveId}/files/{fileId}/thumbnail");
        }

        public static HttpRequestMessage CreateTrashThumbnailRequest(long driveId, long fileId)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"/2/drive/{driveId}/trash/{fileId}/thumbnail");
        }

        public static HttpRequestMessage CreateFileVersionDownloadRequest(long driveId, long fileId, long versionId)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"/2/drive/{driveId}/files/{fileId}/versions/{versionId}/download");
        }

        public static HttpRequestMessage CreateChunkUploadRequest(string baseUrl, string sessionToken, long driveId, KDriveChunk chunk)
        {
            var content = new ReadOnlyMemoryContent(chunk.Content);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var relativePath =
                $"/3/drive/{driveId}/upload/session/{sessionToken}/chunk?chunk_number={chunk.ChunkNumber + 1}&chunk_size={chunk.ChunkSize}&chunk_hash={chunk.ApiChunkHash}";

            return new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}{relativePath}")
            {
                Content = content,
                Version = HttpVersion.Version20,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
            };
        }

        public static HttpRequestMessage CreateUploadSessionFinishRequest(long driveId, string sessionToken)
        {
            return new HttpRequestMessage(HttpMethod.Post, $"/3/drive/{driveId}/upload/session/{sessionToken}/finish");
        }
    }
}
