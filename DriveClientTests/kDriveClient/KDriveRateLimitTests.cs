using kDriveClient.kDriveClient;
using kDriveClient.kDriveClient.Infrastructure.Api;
using kDriveClient.Models;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.RateLimiting;

namespace kDriveClientTests.kDriveClient
{
    [TestClass]
    public class KDriveRateLimitTests
    {
        [TestMethod]
        public async Task Transport_ShouldReacquirePermit_AfterRetryAfter()
        {
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """{"result":"success","data":{"file":{"id":1,"name":"ok.txt"}}}""",
                    Encoding.UTF8,
                    "application/json")
            });

            var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.infomaniak.com") };
            var pipeline = new KDriveHttpPipeline(http);
            var limiter = new ScriptedRateLimiter(TimeSpan.FromMilliseconds(120));
            pipeline.RateLimiter = limiter;
            var transport = new KDriveHttpTransport(pipeline.SendAsync);

            var file = new KDriveFile
            {
                Name = "ok.txt",
                DirectoryPath = "/Private",
                Content = new MemoryStream(Encoding.UTF8.GetBytes("payload"))
            };
            file.TotalChunkHash = "abcdef0123456789";
            file.Chunks.Add(new KDriveChunk(Encoding.UTF8.GetBytes("payload"), 0, "abcdef0123456789"));

            var sw = Stopwatch.StartNew();
            await transport.DirectUploadAsync(111, file, CancellationToken.None);
            sw.Stop();

            Assert.AreEqual(1, handler.RequestCount);
            Assert.AreEqual(2, limiter.AcquireCount);
            Assert.IsTrue(sw.ElapsedMilliseconds >= 100, $"Expected retry delay to be applied, elapsed={sw.ElapsedMilliseconds}ms");
            Assert.IsNotNull(handler.LastRequestUri);
            Assert.IsFalse(handler.LastRequestUri!.Contains("total_chunk_hash=", StringComparison.Ordinal));
            Assert.IsFalse(handler.LastRequestUri!.Contains("total_chunks=", StringComparison.Ordinal));
        }

        [TestMethod]
        public async Task ClientEndpoints_ShouldReacquirePermit_AfterRetryAfter()
        {
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """{"result":"success","data":[],"cursor":"c","has_more":false,"response_at":1710000000}""",
                    Encoding.UTF8,
                    "application/json")
            });

            var client = new KDriveClient(
                "token",
                111,
                new KDriveUploadOptions { UseAutoChunkSize = false, ChunkSize = 1024 * 1024 },
                logger: null,
                httpClient: new HttpClient(handler) { BaseAddress = new Uri("https://api.infomaniak.com") });

            var limiter = new ScriptedRateLimiter(TimeSpan.FromMilliseconds(120));
            SetNonPublicProperty(client, "RateLimiter", limiter);

            var sw = Stopwatch.StartNew();
            await client.GetRecentFilesAsync();
            sw.Stop();

            Assert.AreEqual(1, handler.RequestCount);
            Assert.AreEqual(2, limiter.AcquireCount);
            Assert.IsTrue(sw.ElapsedMilliseconds >= 100, $"Expected retry delay to be applied, elapsed={sw.ElapsedMilliseconds}ms");
        }

        [TestMethod]
        public async Task EndpointsAndUpload_ShouldShareSameRateLimiter()
        {
            var handler = new CountingOkHandler();
            var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.infomaniak.com") };
            var client = new KDriveClient(
                "token",
                111,
                new KDriveUploadOptions { UseAutoChunkSize = false, ChunkSize = 1024 * 1024 },
                logger: null,
                httpClient: http);

            var limiter = new CountingRateLimiter();
            SetNonPublicProperty(client, "RateLimiter", limiter);

            await client.GetRecentFilesAsync();

            var file = new KDriveFile
            {
                Name = "ok.txt",
                DirectoryPath = "/Private",
                Content = new MemoryStream(Encoding.UTF8.GetBytes("payload"))
            };
            await client.UploadFileDirectAsync(file);

            Assert.IsTrue(limiter.AcquireCount >= 2, $"Expected shared limiter to see both endpoint and upload, got {limiter.AcquireCount}");
            Assert.AreSame(
                GetPipeline(client).RateLimiter,
                limiter);
        }

        private static KDriveHttpPipeline GetPipeline(KDriveClient client)
        {
            var property = typeof(KDriveClient).GetProperty("Pipeline", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.IsNotNull(property);
            return (KDriveHttpPipeline)property.GetValue(client)!;
        }

        private static void SetNonPublicProperty(object target, string propertyName, object value)
        {
            var property = target.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.IsNotNull(property, $"Property '{propertyName}' not found on type '{target.GetType().FullName}'.");
            property.SetValue(target, value);
        }

        private sealed class RecordingHandler(HttpResponseMessage response) : HttpMessageHandler
        {
            public int RequestCount => _requestCount;
            public string? LastRequestUri { get; private set; }
            private int _requestCount;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Interlocked.Increment(ref _requestCount);
                LastRequestUri = request.RequestUri?.ToString();
                return Task.FromResult(response);
            }
        }

        private sealed class CountingOkHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var path = request.RequestUri?.AbsolutePath ?? "";
                string body;
                if (path.Contains("/upload", StringComparison.Ordinal))
                {
                    body = """{"result":"success","data":{"file":{"id":1,"name":"ok.txt"}}}""";
                }
                else
                {
                    body = """{"result":"success","data":[],"cursor":"c","has_more":false,"response_at":1710000000}""";
                }

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                });
            }
        }

        private sealed class CountingRateLimiter : RateLimiter
        {
            private int _acquireCount;
            public int AcquireCount => _acquireCount;

            public override TimeSpan? IdleDuration => null;

            protected override RateLimitLease AttemptAcquireCore(int permitCount) => throw new NotSupportedException();

            protected override ValueTask<RateLimitLease> AcquireAsyncCore(int permitCount, CancellationToken cancellationToken = default)
            {
                Interlocked.Increment(ref _acquireCount);
                return ValueTask.FromResult<RateLimitLease>(SuccessfulLease.Instance);
            }

            public override RateLimiterStatistics? GetStatistics() => null;

            protected override ValueTask DisposeAsyncCore() => ValueTask.CompletedTask;
        }

        private sealed class ScriptedRateLimiter(TimeSpan retryAfter) : RateLimiter
        {
            private int _acquireCount;

            public int AcquireCount => _acquireCount;

            public override TimeSpan? IdleDuration => null;

            protected override RateLimitLease AttemptAcquireCore(int permitCount)
            {
                throw new NotSupportedException();
            }

            protected override ValueTask<RateLimitLease> AcquireAsyncCore(int permitCount, CancellationToken cancellationToken = default)
            {
                var call = Interlocked.Increment(ref _acquireCount);
                if (call == 1)
                {
                    return ValueTask.FromResult<RateLimitLease>(new RetryAfterLease(retryAfter));
                }

                return ValueTask.FromResult<RateLimitLease>(SuccessfulLease.Instance);
            }

            public override RateLimiterStatistics? GetStatistics()
            {
                return null;
            }

            protected override ValueTask DisposeAsyncCore()
            {
                return ValueTask.CompletedTask;
            }
        }

        private sealed class SuccessfulLease : RateLimitLease
        {
            public static SuccessfulLease Instance { get; } = new();

            public override bool IsAcquired => true;

            public override IEnumerable<string> MetadataNames => [];

            public override bool TryGetMetadata(string metadataName, out object? metadata)
            {
                metadata = null;
                return false;
            }
        }

        private sealed class RetryAfterLease(TimeSpan retryAfter) : RateLimitLease
        {
            public override bool IsAcquired => false;

            public override IEnumerable<string> MetadataNames => ["RETRY_AFTER"];

            public override bool TryGetMetadata(string metadataName, out object? metadata)
            {
                if (metadataName == "RETRY_AFTER")
                {
                    metadata = retryAfter;
                    return true;
                }

                metadata = null;
                return false;
            }
        }
    }
}
