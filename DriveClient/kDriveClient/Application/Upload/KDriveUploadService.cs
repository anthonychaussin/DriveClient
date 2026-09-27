using kDriveClient.kDriveClient.Domain.Common;
using kDriveClient.kDriveClient.Domain.Upload;
using kDriveClient.kDriveClient.Interface;
using kDriveClient.Models;
using System.Text;
using System.Text.Json.Nodes;

namespace kDriveClient.kDriveClient.Application.Upload
{
    /// <summary>
    /// Application service orchestrating upload flows using domain strategy and transport abstractions.
    /// </summary>
    public sealed class KDriveUploadService : IKDriveUploadService
    {
        private const int DefaultChunkSizeBytes = 1024 * 1024;
        /// <summary>Minimum auto-chunk size (1 MiB). Prefer direct upload below ~100 MiB.</summary>
        private const int MinChunkBytes = 1024 * 1024;
        private const long MaxChunkBytes = 1L * 1024 * 1024 * 1024;
        private const double Safety = 1.10;

        private readonly long _driveId;
        private readonly IKDriveTransport _transport;
        private readonly ILogger? _logger;
        private UploadStrategy _strategy = new(DefaultChunkSizeBytes, 1L * 1024 * 1024);
        private int _parallelism = 4;
        private KDriveUploadHashAlgorithm _hashAlgorithm = KDriveUploadHashAlgorithm.Sha256;

        public KDriveUploadService(long driveId, IKDriveTransport transport, ILogger? logger = null)
        {
            _driveId = driveId;
            _transport = transport;
            _logger = logger;
        }

        public int ChunkSizeBytes => _strategy.ChunkSizeBytes;

        public long DirectUploadThresholdBytes => _strategy.DirectUploadThresholdBytes;

        public IProgress<double>? Progress { get; set; }

        public async Task InitializeAsync(KDriveUploadOptions options, CancellationToken ct = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            _parallelism = Math.Max(1, options.Parallelism);
            _hashAlgorithm = options.HashAlgorithm;

            if (options.UseAutoChunkSize)
            {
                _logger?.LogInformation("Auto chunking enabled, probing optimal upload strategy.");
                _strategy = await MeasureStrategyAsync(_parallelism, ct).ConfigureAwait(false);
                return;
            }

            var chunkSize = options.ChunkSize > 0 ? options.ChunkSize : DefaultChunkSizeBytes;
            var threshold = options.DirectUploadThresholdBytes > 0
                ? options.DirectUploadThresholdBytes
                : (long)(chunkSize * 1.5);

            _strategy = new UploadStrategy(chunkSize, threshold);
        }

        public async Task<KDriveUploadResponse> UploadAsync(KDriveFile file, CancellationToken ct = default)
        {
            file.HashAlgorithm = _hashAlgorithm;
            file.SplitIntoChunks(_strategy.ChunkSizeBytes, _hashAlgorithm);
            if (_strategy.ShouldUseDirectUpload(file.TotalSize))
            {
                _logger?.LogInformation("Uploading '{FileName}' using direct strategy.", file.Name);
                return await UploadDirectAsync(file, ct).ConfigureAwait(false);
            }

            _logger?.LogInformation("Uploading '{FileName}' using chunked strategy (parallelism={Parallelism}).", file.Name, _parallelism);
            return await UploadChunkedAsync(file, ct).ConfigureAwait(false);
        }

        public async Task<KDriveUploadResponse> UploadDirectAsync(KDriveFile file, CancellationToken ct = default)
        {
            var response = await _transport.DirectUploadAsync(_driveId, file, ct).ConfigureAwait(false);
            Progress?.Report(1.0);
            return response;
        }

        public async Task<KDriveUploadResponse> UploadChunkedAsync(KDriveFile file, CancellationToken ct = default)
        {
            if (file.Chunks.Count == 0)
                file.SplitIntoChunks(_strategy.ChunkSizeBytes, _hashAlgorithm);

            var algorithm = file.Chunks.FirstOrDefault()?.HashAlgorithm ?? _hashAlgorithm;
            var (sessionToken, uploadUrl) = await StartSessionAsync(file, ct).ConfigureAwait(false);

            // Hash order must follow chunk index, independent of upload completion order.
            using var totalHasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            foreach (var chunk in file.Chunks.OrderBy(c => c.ChunkNumber))
                totalHasher.AppendData(Encoding.UTF8.GetBytes(chunk.ChunkHash));

            var totalHashHex = Convert.ToHexString(totalHasher.GetHashAndReset()).ToLowerInvariant();

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var uploadCt = linkedCts.Token;
            using var gate = new SemaphoreSlim(_parallelism, _parallelism);
            var inFlight = new List<Task>(_parallelism);
            long bytesCompleted = 0;
            var totalBytes = Math.Max(1L, file.TotalSize);

            try
            {
                foreach (var chunk in file.Chunks)
                {
                    uploadCt.ThrowIfCancellationRequested();
                    await gate.WaitAsync(uploadCt).ConfigureAwait(false);

                    var captured = chunk;
                    var task = UploadChunkWithProgressAsync(
                        uploadUrl,
                        sessionToken,
                        captured,
                        file,
                        gate,
                        () =>
                        {
                            var done = Interlocked.Add(ref bytesCompleted, captured.Content.Length);
                            Progress?.Report(Math.Min(1.0, (double)done / totalBytes));
                        },
                        uploadCt);

                    inFlight.Add(task);

                    if (inFlight.Count >= _parallelism)
                    {
                        var finished = await Task.WhenAny(inFlight).ConfigureAwait(false);
                        inFlight.Remove(finished);
                        await finished.ConfigureAwait(false);
                    }
                }

                await Task.WhenAll(inFlight).ConfigureAwait(false);

                return await _transport
                    .CloseSessionAsync(_driveId, sessionToken, totalHashHex, ct, algorithm)
                    .ConfigureAwait(false);
            }
            catch
            {
                try
                {
                    linkedCts.Cancel();
                }
                catch
                {
                    // ignore
                }

                try
                {
                    await Task.WhenAll(inFlight).ConfigureAwait(false);
                }
                catch
                {
                    // swallow aggregate from in-flight cancellations
                }

                await CancelSessionAsync(sessionToken, CancellationToken.None).ConfigureAwait(false);
                throw;
            }
        }

        private async Task UploadChunkWithProgressAsync(
            string uploadUrl,
            string sessionToken,
            KDriveChunk chunk,
            KDriveFile file,
            SemaphoreSlim gate,
            Action onSuccess,
            CancellationToken ct)
        {
            try
            {
                await _transport.UploadChunkAsync(uploadUrl, _driveId, sessionToken, chunk, file, ct).ConfigureAwait(false);
                onSuccess();
            }
            finally
            {
                gate.Release();
            }
        }

        public async Task<(string SessionToken, string UploadUrl)> StartSessionAsync(KDriveFile file, CancellationToken ct = default)
        {
            var (token, uploadUrl) = await _transport.StartUploadSessionAsync(_driveId, file, ct).ConfigureAwait(false);
            return (token, uploadUrl);
        }

        public Task<JsonObject?> CancelSessionAsync(string sessionToken, CancellationToken ct = default)
        {
            return _transport.CancelSessionAsync(_driveId, sessionToken, ct);
        }

        private async Task<UploadStrategy> MeasureStrategyAsync(int parallelism, CancellationToken ct)
        {
            var buffer = new byte[1024 * 1024];
            RandomNumberGenerator.Fill(buffer);
            using var testFile = KDriveFile.CreateVirtualFile("speedtest.dat", buffer, "/Private");
            testFile.SplitIntoChunks(buffer.Length);

            var (sessionToken, uploadUrl) = await _transport.StartUploadSessionAsync(_driveId, testFile, ct).ConfigureAwait(false);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await _transport.UploadChunkAsync(uploadUrl, _driveId, sessionToken, testFile.Chunks.First(), testFile, ct).ConfigureAwait(false);
            }
            finally
            {
                stopwatch.Stop();
                await _transport.CancelSessionAsync(_driveId, sessionToken, ct).ConfigureAwait(false);
            }

            var bytesPerSecond = buffer.Length / Math.Max(0.001, stopwatch.Elapsed.TotalSeconds);
            var target = Math.Min(
                Math.Max((long)Math.Ceiling((long)Math.Ceiling(parallelism * bytesPerSecond * 60.0 / KDriveApiLimits.RequestsPerMinute) * Safety), MinChunkBytes),
                MaxChunkBytes);

            return new UploadStrategy((int)target, (long)(target * 1.5));
        }
    }
}
