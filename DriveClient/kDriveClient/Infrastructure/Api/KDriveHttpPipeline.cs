using kDriveClient.Helpers;
using kDriveClient.kDriveClient.Domain.Common;
using System.Threading.RateLimiting;

namespace kDriveClient.kDriveClient.Infrastructure.Api
{
    /// <summary>
    /// Shared HTTP send pipeline with a single rate limiter and retry policy for all kDrive API calls.
    /// </summary>
    public sealed class KDriveHttpPipeline
    {
        private readonly HttpClient _http;
        private readonly ILogger? _logger;

        /// <summary>
        /// Rate limiter shared by endpoints and upload transport.
        /// </summary>
        public RateLimiter RateLimiter { get; set; } = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
        {
            PermitLimit = KDriveApiLimits.RequestsPerMinute,
            Window = TimeSpan.FromMinutes(1),
            AutoReplenishment = true
        });

        /// <summary>
        /// Creates a new pipeline bound to the given <see cref="HttpClient"/>.
        /// </summary>
        public KDriveHttpPipeline(HttpClient http, ILogger? logger = null)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _logger = logger;
        }

        /// <summary>
        /// Sends an HTTP request with rate limiting and retries.
        /// </summary>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct = default)
        {
            const int maxRetries = 3;
            var delay = 500;
            Exception? lastException = null;

            for (var i = 0; i < maxRetries; i++)
            {
                try
                {
                    using var lease = await RateLimiter.AcquireAsync(1, ct).ConfigureAwait(false);
                    if (!lease.IsAcquired)
                    {
                        if (lease.TryGetMetadata("RETRY_AFTER", out var obj) && obj is TimeSpan retryAfter)
                        {
                            _logger?.LogInformation("Rate limit reached. Retry-After {RetryAfter}", retryAfter);
                            await Task.Delay(retryAfter, ct).ConfigureAwait(false);
                            i--;
                            continue;
                        }

                        _logger?.LogWarning(
                            "Rate limit exceeded for request: {RequestMethod} {RequestUri}",
                            request.Method,
                            request.RequestUri);
                        throw new HttpRequestException("Rate limit exceeded");
                    }

                    _logger?.LogInformation(
                        "Sending request: {RequestMethod} {RequestUri}",
                        request.Method,
                        request.RequestUri);

                    var response = await _http
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct)
                        .ConfigureAwait(false);

                    return await KDriveJsonHelper.DeserializeResponseAsync(response, ct).ConfigureAwait(false);
                }
                catch (HttpRequestException ex) when (i < maxRetries - 1)
                {
                    await Task.Delay(delay, ct).ConfigureAwait(false);
                    delay *= 2;
                    lastException = ex;
                }
            }

            throw new HttpRequestException("Maximum retry attempts exceeded", lastException);
        }
    }
}
