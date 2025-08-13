using kDriveClient.Helpers;
using kDriveClient.Models;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.RateLimiting;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// A client for interacting with the kDrive API, providing methods for uploading and downloading files,
    /// </summary>
    public partial class KDriveClient : IKDriveClient
    {
        /// <summary>
        /// Logger for logging information, warnings, and errors.
        /// </summary>
        private ILogger<KDriveClient>? Logger { get; set; }

        /// <summary>
        /// The ID of the drive to which files will be uploaded or from which files will be downloaded.
        /// </summary>
        private Int64 DriveId { get; set; }

        /// <summary>
        /// HttpClient used to send requests to the kDrive API.
        /// </summary>
        private HttpClient HttpClient { get; set; }

        /// <summary>
        /// Number of parallel threads to use for chunked uploads.
        /// </summary>
        private Int32 Parallelism { get; set; } = 4;

        /// <summary>
        /// Rate limiter to control the rate of requests sent to the kDrive API according to their rate limits
        /// </summary>
        private RateLimiter RateLimiter { get; set; } = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
        {
            PermitLimit = 60,
            Window = TimeSpan.FromMinutes(1),
            AutoReplenishment = true
        });

        /// <summary>
        /// Direct upload threshold in bytes. Files smaller than this size will be uploaded directly.
        /// </summary>
        private Int64 DirectUploadThresholdBytes { get; set; }

        /// <summary>
        /// Dynamic chunk size in bytes. This is determined based on the speed test and is used for chunked uploads.
        /// </summary>
        private Int32 DynamicChunkSizeBytes { get; set; }

        /// <summary>
        /// Progress reporter for tracking upload progress.
        /// </summary>
        public IProgress<double>? Progress { get; set; }

        /// <summary>
        /// Constructs a new instance of the KDriveClient.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        public KDriveClient(string token, long driveId) : this(token, driveId, true, 4, null)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with optional logging.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        /// <param name="logger">Logger</param>
        public KDriveClient(string token, long driveId, ILogger<KDriveClient>? logger) : this(token, driveId, true, 4, logger)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with auto-chunking and parallelism options.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        /// <param name="autoChunk">Choose if we should make a speed test to optimize chunks</param>
        /// <param name="parallelism">Number of parrallels threads</param>
        /// <param name="logger">Logger</param>
        public KDriveClient(string token, long driveId, bool autoChunk, int parallelism, ILogger<KDriveClient>? logger) : this(token, driveId, autoChunk, parallelism, logger, null)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with auto-chunking, parallelism, and custom HttpClient.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        /// <param name="autoChunk">Choose if we should make a speed test to optimize chunks</param>
        /// <param name="parallelism">Number of parrallels threads</param>
        /// <param name="logger">Logger</param>
        /// <param name="httpClient">Custome HttpClient</param>
        public KDriveClient(string token, long driveId, bool autoChunk, int parallelism, ILogger<KDriveClient>? logger, HttpClient? httpClient = null)
        {
            DriveId = driveId;
            Parallelism = parallelism;
            Logger = logger;
            string version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "unknown";
            HttpClient = httpClient ?? new HttpClient { BaseAddress = new Uri("https://api.infomaniak.com") };
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("kDriveClient.NET/version");
            this.Logger?.LogInformation("KDriveClient initialized with Drive ID: {DriveId}", DriveId);
            if (autoChunk)
            {
                this.Logger?.LogInformation("Auto chunking enabled, initializing upload strategy...");
                InitializeUploadStrategyAsync().GetAwaiter().GetResult();
                this.Logger?.LogInformation("Upload strategy initialized with direct upload threshold: {Threshold} bytes and dynamic chunk size: {ChunkSize} bytes", DirectUploadThresholdBytes, DynamicChunkSizeBytes);
            }
        }

        /// <summary>
        /// Uploads a file to kDrive, automatically determining the upload strategy based on file size.
        /// </summary>
        /// <param name="file"><see cref="KDriveFile"/> to upload</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns><see cref="KDriveUploadResponse"/> of your uploaded file</returns>
        public async Task<KDriveUploadResponse> UploadAsync(KDriveFile file, CancellationToken ct = default)
        {
            file.SplitIntoChunks(this.DynamicChunkSizeBytes);
            if (file.TotalSize <= 1L * 1024 * 1024 || file.TotalSize <= DirectUploadThresholdBytes)
            {
                this.Logger?.LogInformation("File size {FileSize} bytes is below direct upload threshold {Threshold} bytes, uploading directly.", file.TotalSize, DirectUploadThresholdBytes);
                return await UploadFileDirectAsync(file, ct);
            }
            else
            {
                this.Logger?.LogInformation("File size {FileSize} bytes exceeds direct upload threshold {Threshold} bytes, uploading in chunks.", file.TotalSize, DirectUploadThresholdBytes);
                return await UploadFileChunkedAsync(file, ct);
            }
        }

        /// <summary>
        /// Sends an HTTP request to the kDrive API.
        /// </summary>
        /// <param name="request"><see cref="HttpRequestMessage"/> request</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns><see cref="HttpResponseMessage"/></returns>
        /// <exception cref="HttpRequestException"></exception>
        protected virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct = default)
        {
            if (!(await RateLimiter.AcquireAsync(1, ct)).IsAcquired)
            {
                Logger?.LogWarning("Rate limit exceeded for request: {RequestMethod} {RequestUri}", request.Method, request.RequestUri);
                throw new HttpRequestException("Rate limit exceeded");
            }

            Logger?.LogInformation("Sending request: {RequestMethod} {RequestUri}", request.Method, request.RequestUri);
            return await SendWithErrorHandlingAsync(request, ct);
        }

        /// <summary>
        /// Sends an HTTP request and handles errors by deserializing the response.
        /// </summary>
        /// <param name="request"><see cref="HttpRequestMessage"/> request</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns><see cref="HttpResponseMessage"/></returns>
        private async Task<HttpResponseMessage> SendWithErrorHandlingAsync(HttpRequestMessage request, CancellationToken ct = default)
        {
            var response = await HttpClient.SendAsync(request, ct);

            return await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
        }
    }
}
