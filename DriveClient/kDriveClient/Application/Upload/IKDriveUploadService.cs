using kDriveClient.Models;
using System.Text.Json.Nodes;

namespace kDriveClient.kDriveClient.Application.Upload
{
    public interface IKDriveUploadService
    {
        int ChunkSizeBytes { get; }

        long DirectUploadThresholdBytes { get; }

        IProgress<double>? Progress { get; set; }

        Task InitializeAsync(KDriveUploadOptions options, CancellationToken ct = default);

        Task<KDriveUploadResponse> UploadAsync(KDriveFile file, CancellationToken ct = default);

        Task<KDriveUploadResponse> UploadDirectAsync(KDriveFile file, CancellationToken ct = default);

        Task<KDriveUploadResponse> UploadChunkedAsync(KDriveFile file, CancellationToken ct = default);

        Task<(string SessionToken, string UploadUrl)> StartSessionAsync(KDriveFile file, CancellationToken ct = default);

        Task<JsonObject?> CancelSessionAsync(string sessionToken, CancellationToken ct = default);
    }
}
