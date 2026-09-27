using kDriveClient.Models;

namespace kDriveClient.Extensions
{
    /// <summary>
    /// Options for registering <see cref="kDriveClient.kDriveClient.KDriveClient"/> in DI.
    /// </summary>
    public sealed class KDriveClientOptions
    {
        /// <summary>
        /// Bearer API token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Target drive id.
        /// </summary>
        public long DriveId { get; set; }

        /// <summary>
        /// API base address (defaults to Infomaniak).
        /// </summary>
        public Uri BaseAddress { get; set; } = new("https://api.infomaniak.com");

        /// <summary>
        /// Upload strategy options.
        /// </summary>
        public KDriveUploadOptions Upload { get; set; } = new();

        /// <summary>
        /// Named <see cref="HttpClient"/> registration name.
        /// </summary>
        public string HttpClientName { get; set; } = "kDrive";
    }
}
