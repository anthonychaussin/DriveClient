using kDriveClient.kDriveClient.Application.Upload;
using kDriveClient.kDriveClient.Interface;
using kDriveClient.Models;
using System.Text;
using System.Text.Json.Nodes;

namespace kDriveClientTests.kDriveClient
{
    [TestClass]
    public class KDriveUploadServiceTests
    {
        [TestMethod]
        public async Task UploadAsync_ShouldUseDirectUpload_WhenBelowDirectThreshold()
        {
            var transport = new RecordingTransport();
            var service = new KDriveUploadService(111, transport);
            await service.InitializeAsync(new KDriveUploadOptions
            {
                UseAutoChunkSize = false,
                ChunkSize = 1024,
                DirectUploadThresholdBytes = 2048
            });

            var file = BuildInMemoryFile("small.txt", 1024);

            await service.UploadAsync(file);

            Assert.AreEqual(1, transport.DirectUploadCalls);
            Assert.AreEqual(0, transport.UploadChunkCalls);
        }

        [TestMethod]
        public async Task UploadAsync_ShouldUseChunkedUpload_WhenAboveOneMegabyte()
        {
            var transport = new RecordingTransport();
            var service = new KDriveUploadService(111, transport);
            await service.InitializeAsync(new KDriveUploadOptions
            {
                UseAutoChunkSize = false,
                ChunkSize = 1024 * 512,
                DirectUploadThresholdBytes = 1024
            });

            var file = BuildInMemoryFile("large.bin", (1024 * 1024) + 1024);

            await service.UploadAsync(file);

            Assert.AreEqual(0, transport.DirectUploadCalls);
            Assert.AreEqual(1, transport.StartSessionCalls);
            Assert.IsTrue(transport.UploadChunkCalls >= 2);
            Assert.AreEqual(1, transport.CloseSessionCalls);
        }

        [TestMethod]
        public async Task UploadChunkedAsync_ShouldRespectParallelism()
        {
            var transport = new RecordingTransport { ChunkDelay = TimeSpan.FromMilliseconds(50) };
            var service = new KDriveUploadService(111, transport);
            await service.InitializeAsync(new KDriveUploadOptions
            {
                UseAutoChunkSize = false,
                ChunkSize = 256,
                DirectUploadThresholdBytes = 1,
                Parallelism = 3
            });

            var file = BuildInMemoryFile("parallel.bin", 256 * 8);
            await service.UploadChunkedAsync(file);

            Assert.AreEqual(8, transport.UploadChunkCalls);
            Assert.IsTrue(transport.MaxConcurrentChunks <= 3, $"Expected max concurrency <= 3, got {transport.MaxConcurrentChunks}");
            Assert.IsTrue(transport.MaxConcurrentChunks >= 2, $"Expected some parallelism, got {transport.MaxConcurrentChunks}");
        }

        [TestMethod]
        public async Task UploadChunkedAsync_ShouldCancelSession_OnFailure()
        {
            var transport = new RecordingTransport { FailOnChunkNumber = 2 };
            var service = new KDriveUploadService(111, transport);
            await service.InitializeAsync(new KDriveUploadOptions
            {
                UseAutoChunkSize = false,
                ChunkSize = 256,
                DirectUploadThresholdBytes = 1,
                Parallelism = 2
            });

            var file = BuildInMemoryFile("fail.bin", 256 * 4);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.UploadChunkedAsync(file));
            Assert.AreEqual(1, transport.CancelSessionCalls);
            Assert.AreEqual(0, transport.CloseSessionCalls);
        }

        [TestMethod]
        public async Task UploadChunkedAsync_ShouldReportProgress()
        {
            var transport = new RecordingTransport();
            var service = new KDriveUploadService(111, transport);
            var reports = new List<double>();
            service.Progress = new Progress<double>(reports.Add);

            await service.InitializeAsync(new KDriveUploadOptions
            {
                UseAutoChunkSize = false,
                ChunkSize = 256,
                DirectUploadThresholdBytes = 1,
                Parallelism = 2
            });

            var file = BuildInMemoryFile("progress.bin", 256 * 4);
            await service.UploadChunkedAsync(file);

            // Progress<T> posts asynchronously; give it a moment.
            await Task.Delay(50);
            Assert.IsTrue(reports.Count >= 1);
            Assert.IsTrue(reports.Max() <= 1.0);
            Assert.IsTrue(reports.Max() > 0);
        }

        private static KDriveFile BuildInMemoryFile(string name, int size)
        {
            var bytes = Encoding.UTF8.GetBytes(new string('A', size));
            return new KDriveFile
            {
                Name = name,
                DirectoryPath = "/Private",
                Content = new MemoryStream(bytes)
            };
        }

        private sealed class RecordingTransport : IKDriveTransport
        {
            private int _concurrent;

            public int DirectUploadCalls { get; private set; }
            public int StartSessionCalls { get; private set; }
            public int CloseSessionCalls { get; private set; }
            public int CancelSessionCalls { get; private set; }
            public int MaxConcurrentChunks { get; private set; }
            public TimeSpan ChunkDelay { get; set; }
            public int? FailOnChunkNumber { get; set; }

            private int _chunkCalls;
            public int UploadChunkCalls => _chunkCalls;

            public Task<KDriveUploadResponse> CloseSessionAsync(long driveId, string sessionId, string totalHashHex, CancellationToken ct, KDriveUploadHashAlgorithm algorithm = KDriveUploadHashAlgorithm.Sha256)
            {
                CloseSessionCalls++;
                return Task.FromResult(new KDriveUploadResponse { Name = "closed" });
            }

            public Task<JsonObject?> CancelSessionAsync(long driveId, string sessionId, CancellationToken ct)
            {
                CancelSessionCalls++;
                return Task.FromResult<JsonObject?>(new JsonObject());
            }

            public Task<KDriveUploadResponse> DirectUploadAsync(long driveId, KDriveFile file, CancellationToken ct)
            {
                DirectUploadCalls++;
                return Task.FromResult(new KDriveUploadResponse { Name = file.Name });
            }

            public Task<(string Token, string UploadUrl)> StartUploadSessionAsync(long driveId, KDriveFile file, CancellationToken ct)
            {
                StartSessionCalls++;
                return Task.FromResult(("token", "https://upload.example.com"));
            }

            public async Task<JsonObject?> UploadChunkAsync(string baseUrl, long driveId, string sessionId, KDriveChunk chunk, KDriveFile file, CancellationToken ct)
            {
                var current = Interlocked.Increment(ref _concurrent);
                MaxConcurrentChunks = Math.Max(MaxConcurrentChunks, current);
                try
                {
                    if (ChunkDelay > TimeSpan.Zero)
                        await Task.Delay(ChunkDelay, ct);

                    if (FailOnChunkNumber is int fail && chunk.ChunkNumber == fail)
                        throw new InvalidOperationException("simulated chunk failure");

                    Interlocked.Increment(ref _chunkCalls);
                    return new JsonObject();
                }
                finally
                {
                    Interlocked.Decrement(ref _concurrent);
                }
            }
        }
    }
}
