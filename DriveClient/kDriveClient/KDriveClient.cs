using kDriveClient.kDriveClient.Application.Endpoints;
using kDriveClient.kDriveClient.Application.Upload;
using kDriveClient.kDriveClient.Infrastructure.Api;
using kDriveClient.kDriveClient.Interface;
using kDriveClient.Models;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.RateLimiting;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// A client for interacting with the kDrive API, providing methods for uploading and downloading files.
    /// </summary>
    public partial class KDriveClient : IKDriveClient
    {
        private ILogger<KDriveClient>? Logger { get; set; }

        /// <summary>
        /// Current drive id used for API calls. Updated by <see cref="RebindDriveId"/> / <see cref="BootstrapAsync"/>.
        /// </summary>
        public long DriveId { get; private set; }

        private HttpClient HttpClient { get; set; }

        private KDriveHttpPipeline Pipeline { get; set; }

        /// <summary>
        /// Number of parallel chunk uploads.
        /// </summary>
        public Int32 Parallelism { get; private set; } = 4;

        private IKDriveUploadService UploadService { get; set; }

        private IKDriveEndpointsService EndpointsService { get; set; }

        /// <summary>
        /// Shared rate limiter (endpoints + uploads). Exposed for tests.
        /// </summary>
        private RateLimiter RateLimiter
        {
            get => Pipeline.RateLimiter;
            set => Pipeline.RateLimiter = value;
        }

        private Int64 DirectUploadThresholdBytes { get; set; }

        /// <summary>
        /// Dynamic chunk size in bytes after strategy initialization.
        /// </summary>
        public Int32 DynamicChunkSizeBytes { get; private set; }

        /// <summary>
        /// Progress reporter for tracking upload progress (0.0–1.0).
        /// </summary>
        public IProgress<double>? Progress { get; set; }

        private KDriveUploadOptions _uploadOptions = new();
        private readonly SemaphoreSlim _initLock = new(1, 1);
        private bool _uploadInitialized;

        /// <summary>
        /// Constructs a new instance of the KDriveClient.
        /// </summary>
        public KDriveClient(string token, long driveId) : this(token, driveId, new KDriveUploadOptions())
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with optional logging.
        /// </summary>
        public KDriveClient(string token, long driveId, ILogger<KDriveClient>? logger) : this(token, driveId, new KDriveUploadOptions(), logger)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with custom options.
        /// </summary>
        public KDriveClient(string token, long driveId, KDriveUploadOptions options) : this(token, driveId, options, null)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with custom HttpClient.
        /// </summary>
        public KDriveClient(string token, long driveId, HttpClient? httpClient) : this(token, driveId, new KDriveUploadOptions(), null, httpClient)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with auto-chunking and parallelism options.
        /// </summary>
        public KDriveClient(string token, long driveId, KDriveUploadOptions options, ILogger<KDriveClient>? logger) : this(token, driveId, options, logger, null)
        { }

        /// <summary>
        /// Constructs a new instance of the KDriveClient with auto-chunking, parallelism, and custom HttpClient.
        /// Upload strategy probing is deferred until the first upload (or <see cref="CreateAsync"/>).
        /// </summary>
        public KDriveClient(string token, long driveId, KDriveUploadOptions options, ILogger<KDriveClient>? logger, HttpClient? httpClient = null)
        {
            DriveId = driveId;
            Parallelism = options.Parallelism;
            _uploadOptions = options;
            Logger = logger;
            HttpClient = httpClient ?? new HttpClient { BaseAddress = new Uri("https://api.infomaniak.com") };
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("kDriveClient.NET/" + GetVersion());
            Logger?.LogInformation("KDriveClient initialized with Drive ID: {DriveId}", DriveId);

            Pipeline = new KDriveHttpPipeline(HttpClient, Logger);
            var apiGateway = new HttpKDriveApiGateway((request, cancellationToken) => SendAsync(request, cancellationToken));
            UploadService = new KDriveUploadService(DriveId, new KDriveHttpTransport(SendAsync, Logger), Logger);
            EndpointsService = new KDriveEndpointsService(DriveId, apiGateway);

            if (!options.UseAutoChunkSize)
            {
                UploadService.InitializeAsync(options).GetAwaiter().GetResult();
                ApplyUploadStrategyState();
                _uploadInitialized = true;
            }
            else
            {
                // Defaults until CreateAsync / first upload probes bandwidth.
                DirectUploadThresholdBytes = options.DirectUploadThresholdBytes > 0
                    ? options.DirectUploadThresholdBytes
                    : 1024L * 1024;
                DynamicChunkSizeBytes = options.ChunkSize > 0 ? options.ChunkSize : 1024 * 1024;
            }
        }

        /// <summary>
        /// Creates and fully initializes a client (including optional bandwidth probe).
        /// Preferred over the sync constructor when <see cref="KDriveUploadOptions.UseAutoChunkSize"/> is true.
        /// </summary>
        public static async Task<KDriveClient> CreateAsync(
            string token,
            long driveId,
            KDriveUploadOptions? options = null,
            ILogger<KDriveClient>? logger = null,
            HttpClient? httpClient = null,
            CancellationToken ct = default)
        {
            options ??= new KDriveUploadOptions();
            var client = new KDriveClient(token, driveId, options, logger, httpClient);
            await client.EnsureUploadInitializedAsync(ct).ConfigureAwait(false);
            return client;
        }

        /// <summary>
        /// Rebinds the client to another drive id (endpoints + upload). Thread-safe.
        /// Prefer calling after <see cref="BootstrapAsync"/> when the selected drive differs from construction.
        /// </summary>
        public void RebindDriveId(long driveId)
        {
            if (driveId <= 0)
                throw new ArgumentOutOfRangeException(nameof(driveId));

            DriveId = driveId;
            if (EndpointsService is KDriveEndpointsService endpoints)
                endpoints.SetDriveId(driveId);
            if (UploadService is KDriveUploadService upload)
                upload.SetDriveId(driveId);
            Logger?.LogInformation("KDriveClient rebound to Drive ID: {DriveId}", driveId);
        }

        /// <summary>
        /// Uploads a file to kDrive, automatically determining the upload strategy based on file size.
        /// </summary>
        public async Task<KDriveUploadResponse> UploadAsync(KDriveFile file, CancellationToken ct = default)
        {
            await EnsureUploadInitializedAsync(ct).ConfigureAwait(false);
            UploadService.Progress = Progress;
            return await UploadService.UploadAsync(file, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an HTTP request through the shared pipeline.
        /// </summary>
        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct = default)
        {
            return Pipeline.SendAsync(request, ct);
        }

        private async Task EnsureUploadInitializedAsync(CancellationToken ct)
        {
            if (_uploadInitialized)
                return;

            await _initLock.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                if (_uploadInitialized)
                    return;

                await UploadService.InitializeAsync(_uploadOptions, ct).ConfigureAwait(false);
                ApplyUploadStrategyState();
                _uploadInitialized = true;
                Logger?.LogInformation(
                    "Upload strategy initialized with direct upload threshold: {Threshold} bytes and dynamic chunk size: {ChunkSize} bytes",
                    DirectUploadThresholdBytes,
                    DynamicChunkSizeBytes);
            }
            finally
            {
                _initLock.Release();
            }
        }

        private void ApplyUploadStrategyState()
        {
            DirectUploadThresholdBytes = UploadService.DirectUploadThresholdBytes;
            DynamicChunkSizeBytes = UploadService.ChunkSizeBytes;
            Parallelism = _uploadOptions.Parallelism;
        }

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
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
