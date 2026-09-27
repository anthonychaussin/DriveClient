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
        /// Optional Infomaniak account id (useful for <c>BootstrapAsync</c> / multi-drive flows).
        /// </summary>
        public long? AccountId { get; set; }

        /// <summary>
        /// API base address (must be absolute; defaults to Infomaniak).
        /// </summary>
        public Uri BaseAddress { get; set; } = new("https://api.infomaniak.com");

        /// <summary>
        /// Upload strategy options.
        /// </summary>
        public KDriveUploadOptions Upload { get; set; } = new();

        /// <summary>
        /// Named <see cref="HttpClient"/> registration name.
        /// When using a custom name, ensure that named client is registered (default registration uses <c>kDrive</c>).
        /// </summary>
        public string HttpClientName { get; set; } = "kDrive";
    }
}
