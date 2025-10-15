using kDriveClient.Helpers;
using kDriveClient.Models;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        public Int32 Parallelism { get; private set; } = 4;

        /// <summary>
        /// Rate limiter to control the rate of requests sent to the kDrive API according to their rate limits
        /// </summary>
        private RateLimiter RateLimiter { get; set; } = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
        {
            PermitLimit = 59,
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
        public Int32 DynamicChunkSizeBytes { get; private set; }

        /// <summary>
        /// Progress reporter for tracking upload progress.
        /// </summary>
        public IProgress<double>? Progress { get; set; }

        /// <summary>
        /// Constructs a new instance of the KDriveClient.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        public KDriveClient(string token, long driveId) : this(token, driveId, new KDriveClientOptions())
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with optional logging.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        /// <param name="logger">Logger</param>
        public KDriveClient(string token, long driveId, ILogger<KDriveClient>? logger) : this(token, driveId, new KDriveClientOptions(), logger)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with custom options.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        /// <param name="options">Options</param>
        public KDriveClient(string token, long driveId, KDriveClientOptions options) : this(token, driveId, options, null)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with custom HttpClient.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        /// <param name="httpClient">Custom HttpClient</param>
        public KDriveClient(string token, long driveId, HttpClient? httpClient) : this(token, driveId, new KDriveClientOptions(), null, httpClient)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with auto-chunking and parallelism options.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        /// <param name="options"></param>
        /// <param name="logger">Logger</param>
        public KDriveClient(string token, long driveId, KDriveClientOptions options, ILogger<KDriveClient>? logger) : this(token, driveId, options, logger, null)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with auto-chunking, parallelism, and custom HttpClient.
        /// </summary>
        /// <param name="token">Bearer token</param>
        /// <param name="driveId">Drive ID</param>
        /// <param name="options">Options</param>
        /// <param name="logger">Logger</param>
        /// <param name="httpClient">Custom HttpClient</param>
        public KDriveClient(string token, long driveId, KDriveClientOptions options, ILogger<KDriveClient>? logger, HttpClient? httpClient = null)
        {
            DriveId = driveId;
            Parallelism = options.Parallelism;
            Logger = logger;
            HttpClient = httpClient ?? new HttpClient { BaseAddress = new Uri("https://api.infomaniak.com") };
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("kDriveClient.NET/" + GetVersion());
            this.Logger?.LogInformation("KDriveClient initialized with Drive ID: {DriveId}", DriveId);

            if (options.UseAutoChunkSize)
            {
                this.Logger?.LogInformation("Auto chunking enabled, initializing upload strategy...");
                InitializeUploadStrategyAsync().GetAwaiter().GetResult();
                this.Logger?.LogInformation("Upload strategy initialized with direct upload threshold: {Threshold} bytes and dynamic chunk size: {ChunkSize} bytes", DirectUploadThresholdBytes, DynamicChunkSizeBytes);
            }
            else if (options.ChunkSize is null) // If autoChunk is disabled and no custom chunk size provided, use a default
            {
                DynamicChunkSizeBytes = 1024 * 1024; // Default to 1MB chunks
                this.Logger?.LogInformation("Using default chunk size: {ChunkSize} bytes", DynamicChunkSizeBytes);
            }
            else
            {
                DynamicChunkSizeBytes = (int)options.ChunkSize;
                this.Logger?.LogInformation("Using custom chunk size: {ChunkSize} bytes", DynamicChunkSizeBytes);
            }
        }

        /// <summary>
        /// Uploads a file to kDrive, automatically determining the upload strategy based on file size.
        /// Throws <see cref="ArgumentException"/> if the file is invalid.
        /// </summary>
        /// <param name="file">File to upload.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Upload response.</returns>
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
            int maxRetries = 3;
            int delay = 500;
            var lastException = default(Exception);
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    using var lease = await RateLimiter.AcquireAsync(1, ct);
                    if (!lease.IsAcquired)
                    {
                        if (lease.TryGetMetadata("RETRY_AFTER", out var obj) && obj is TimeSpan retryAfter)
                        {
                            Logger?.LogInformation("Rate limit reached. Retry-After {RetryAfter}", retryAfter);
                            await Task.Delay(retryAfter, ct);
                        }
                        else
                        {
                            Logger?.LogWarning("Rate limit exceeded for request: {RequestMethod} {RequestUri}", request.Method, request.RequestUri);
                            throw new HttpRequestException("Rate limit exceeded");
                        }
                    }

                    Logger?.LogInformation("Sending request: {RequestMethod} {RequestUri}", request.Method, request.RequestUri);
                    return await SendWithErrorHandlingAsync(request, ct);
                }
                catch (HttpRequestException ex) when (i < maxRetries - 1)
                {
                    await Task.Delay(delay, ct);
                    delay *= 2;
                    lastException = ex;
                }
            }
            throw new HttpRequestException("Maximum retry attempts exceeded", lastException);
        }

        /// <summary>
        /// Sends an HTTP request and handles errors by deserializing the response.
        /// </summary>
        /// <param name="request"><see cref="HttpRequestMessage"/> request</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns><see cref="HttpResponseMessage"/></returns>
        private async Task<HttpResponseMessage> SendWithErrorHandlingAsync(HttpRequestMessage request, CancellationToken ct = default)
        {
            var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

            return await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
        }

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        /// <returns>The verstion of the assembly</returns>
        [RequiresAssemblyFiles("Calls System.Reflection.Assembly.Location")]
        private static string GetVersion()
        {
            var asm = typeof(KDriveClient).Assembly;
            var infoVer = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (!string.IsNullOrWhiteSpace(infoVer)) return infoVer;
            var asmVer = asm.GetName().Version?.ToString();
            if (!string.IsNullOrWhiteSpace(asmVer)) return asmVer;
            try
            {
                var loc = asm.Location;
                if (!string.IsNullOrWhiteSpace(loc))
                {
                    var fvi = FileVersionInfo.GetVersionInfo(loc);
                    if (!string.IsNullOrWhiteSpace(fvi.FileVersion)) return fvi.FileVersion!;
                }
            }
            catch { }

            return "unknown";
        }
    }
}