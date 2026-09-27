namespace kDriveClient.Models
{
    /// <summary>
    /// Options for file download requests (conversion, password-protected files).
    /// </summary>
    public sealed class KDriveDownloadOptions
    {
        /// <summary>
        /// On-the-fly conversion via the <c>as</c> query parameter (<c>pdf</c> or <c>text</c>).
        /// </summary>
        public string? ConvertAs { get; set; }

        /// <summary>
        /// Password for protected files (<c>x-kdrive-file-password</c> header).
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// When true, obtain a temporary URL first then download from it (files only).
        /// </summary>
        public bool UseTemporaryUrl { get; set; }

        /// <summary>
        /// Temporary URL lifetime in seconds (60–86400). Used when <see cref="UseTemporaryUrl"/> is true.
        /// </summary>
        public int? TemporaryUrlDurationSeconds { get; set; }

        /// <summary>
        /// Optional progress reporter (bytes received so far).
        /// </summary>
        public IProgress<long>? Progress { get; set; }

        /// <summary>
        /// Optional expected content hash (hex or <c>algo:hex</c>). Verified as SHA-256 of the downloaded bytes.
        /// </summary>
        public string? ExpectedHash { get; set; }
    }
}
