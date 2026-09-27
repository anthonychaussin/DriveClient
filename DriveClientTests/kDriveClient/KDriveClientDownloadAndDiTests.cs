using kDriveClient.Extensions;
using kDriveClient.Helpers;
using kDriveClient.kDriveClient;
using kDriveClient.Models;
using kDriveClient.Models.Domain;
using kDriveClient.Models.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;

namespace kDriveClientTests.kDriveClient
{
    [TestClass]
    public class KDriveClientDownloadAndDiTests
    {
        [TestMethod]
        public async Task DownloadFileAsync_ShouldRequestConvertAndPassword()
        {
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(Encoding.UTF8.GetBytes("hello"))
            });
            var client = CreateClient(handler);

            await using var stream = await client.DownloadFileAsync(42, new KDriveDownloadOptions
            {
                ConvertAs = "pdf",
                Password = "secret"
            });

            Assert.IsNotNull(stream);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/2/drive/111/files/42/download?as=pdf");
            Assert.IsTrue(handler.LastRequest.Headers.Contains("x-kdrive-file-password"));
        }

        [TestMethod]
        public async Task DownloadFileAsync_ToPath_ShouldWriteBytes()
        {
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(Encoding.UTF8.GetBytes("payload"))
            });
            var client = CreateClient(handler);
            var path = Path.Combine(Path.GetTempPath(), $"kdrive-dl-{Guid.NewGuid():N}.txt");

            try
            {
                await client.DownloadFileAsync(7, path);
                Assert.AreEqual("payload", await File.ReadAllTextAsync(path));
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [TestMethod]
        public async Task AddKDriveClient_ShouldResolveInterfaces()
        {
            var services = new ServiceCollection();
            services.AddKDriveClient(o =>
            {
                o.Token = "token";
                o.DriveId = 111;
                o.Upload = new KDriveUploadOptions { UseAutoChunkSize = false, ChunkSize = 1024 };
            });

            await using var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IKDriveClient>();
            Assert.IsNotNull(client);
            Assert.IsNotNull(provider.GetRequiredService<IKDriveFiles>());
            Assert.IsNotNull(provider.GetRequiredService<IKDriveDownload>());
        }

        [TestMethod]
        public async Task ApiError_ShouldThrowKDriveApiException()
        {
            var json = """{"result":"error","error":{"code":"forbidden_error","description":"nope"}}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
            var client = CreateClient(handler);

            var ex = await Assert.ThrowsAsync<KDriveApiException>(() => client.GetItemAsync(1));
            Assert.AreEqual("forbidden_error", ex.Error.Error.Code);
        }

        [TestMethod]
        public async Task GetOrCreateFolderAsync_ShouldReturnExistingDirectory()
        {
            var wake = """{"result":"success","data":true}""";
            var list = """
            {"result":"success","data":[{"id":9,"name":"Projects","type":"dir"}],"cursor":null,"has_more":false}
            """;
            var handler = new SequenceHandler(
                Ok(wake),
                Ok(list));
            var client = CreateClient(handler);

            var folder = await client.GetOrCreateFolderAsync(5, "Projects");
            Assert.AreEqual(9L, folder.Id);
            Assert.AreEqual("Projects", folder.Name);
        }

        [TestMethod]
        public async Task BootstrapAsync_ShouldSelectPreferredDrive()
        {
            var json = """
            {"result":"success","data":[{"id":10,"name":"A"},{"id":111,"name":"B"}],"page":1,"pages":1}
            """;
            var handler = new RecordingHandler(Ok(json));
            var client = CreateClient(handler);

            var ctx = await client.BootstrapAsync(99, preferredDriveId: 111);
            Assert.AreEqual(111L, ctx.DriveId);
            Assert.AreEqual(111L, client.DriveId);
            Assert.AreEqual(2, ctx.Drives.Count);
        }

        [TestMethod]
        public async Task BootstrapAsync_ShouldRebindDriveId()
        {
            var json = """
            {"result":"success","data":[{"id":222,"name":"Other"}],"page":1,"pages":1}
            """;
            var handler = new RecordingHandler(Ok(json));
            var client = CreateClient(handler);
            Assert.AreEqual(111L, client.DriveId);

            var ctx = await client.BootstrapAsync(99);
            Assert.AreEqual(222L, ctx.DriveId);
            Assert.AreEqual(222L, client.DriveId);
        }

        [TestMethod]
        public async Task EnvelopeErrorOnHttp200_ShouldThrow()
        {
            var json = """{"result":"error","error":{"code":"conflict_error","description":"nope"}}""";
            var handler = new RecordingHandler(Ok(json));
            var client = CreateClient(handler);

            var ex = await Assert.ThrowsAsync<KDriveApiException>(() => client.GetItemAsync(1));
            Assert.AreEqual("conflict_error", ex.Error.Error.Code);
        }

        [TestMethod]
        public async Task NonJsonHttpError_ShouldStillThrow()
        {
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent("bad gateway", Encoding.UTF8, "text/plain"),
                ReasonPhrase = "Bad Gateway"
            });
            var client = CreateClient(handler);

            var ex = await Assert.ThrowsAsync<KDriveApiException>(() => client.DownloadFileAsync(1));
            Assert.AreEqual("http_502", ex.Error.Error.Code);
        }

        [TestMethod]
        public async Task DownloadFileAsync_ShouldReportProgressAndVerifyHash()
        {
            var payload = Encoding.UTF8.GetBytes("hello-hash");
            var expected = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(payload)).ToLowerInvariant();
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(payload)
            });
            var client = CreateClient(handler);
            long reported = 0;

            await using var stream = await client.DownloadFileAsync(42, new KDriveDownloadOptions
            {
                Progress = new Progress<long>(n => reported = n),
                ExpectedHash = expected
            });

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            Assert.AreEqual(payload.Length, ms.Length);
            await Task.Delay(30);
            Assert.AreEqual(payload.Length, reported);
        }

        [TestMethod]
        public async Task WaitForImportCompleteAsync_ShouldStopOnDone()
        {
            var queued = Ok("""{"result":"success","data":{"id":9,"status":"in_progress"}}""");
            var done = Ok("""{"result":"success","data":{"id":9,"status":"done"}}""");
            var handler = new SequenceHandler(queued, done);
            var client = CreateClient(handler);

            var job = await client.WaitForImportCompleteAsync(9, timeout: TimeSpan.FromSeconds(5), interval: TimeSpan.FromMilliseconds(1));
            Assert.AreEqual("done", job.Status);
            Assert.AreEqual(2, handler.Requests.Count);
        }

        [TestMethod]
        public async Task BuildAndDownloadArchiveAsync_ShouldRetryUntilZipReady()
        {
            var build = Ok("""{"result":"success","data":{"uuid":"arch-1"}}""");
            var notReady = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent("""{"result":"error","error":{"code":"not_ready","description":"wait"}}""", Encoding.UTF8, "application/json")
            };
            var zip = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(Encoding.UTF8.GetBytes("PKZIP"))
            };
            var wake = Ok("""{"result":"success","data":true}""");
            var handler = new SequenceHandler(wake, build, notReady, zip);
            var client = CreateClient(handler);

            await using var stream = await client.BuildAndDownloadArchiveAsync(
                [1, 2],
                timeout: TimeSpan.FromSeconds(5),
                interval: TimeSpan.FromMilliseconds(1));
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            Assert.AreEqual("PKZIP", Encoding.UTF8.GetString(ms.ToArray()));
        }

        [TestMethod]
        public void FormatIncludes_ShouldEmitExtendedWithValues()
        {
            var with = KDriveEnumFormatting.FormatIncludes(
                KDriveItemIncludes.Hash | KDriveItemIncludes.Etag | KDriveItemIncludes.Activity | KDriveItemIncludes.Lock);
            Assert.IsNotNull(with);
            CollectionAssert.AreEquivalent(new[] { "hash", "etag", "activity", "lock" }, with!.Split(','));
        }

        [TestMethod]
        public async Task WaitForAsyncResultAsync_ShouldStopWhenNotPending()
        {
            var client = CreateClient(new RecordingHandler(Ok("""{"result":"success","data":true}""")));
            var calls = 0;
            var result = await client.WaitForAsyncResultAsync(
                _ =>
                {
                    calls++;
                    return Task.FromResult<string?>(calls < 2 ? "asynchronous" : "success");
                },
                value => value == "asynchronous",
                timeout: TimeSpan.FromSeconds(5),
                interval: TimeSpan.FromMilliseconds(1));

            Assert.AreEqual("success", result);
            Assert.AreEqual(2, calls);
        }

        [TestMethod]
        public void DomainMapping_FileV3_ShouldMapScalarsWithoutRoundTrip()
        {
            var dto = new global::kDriveClient.Models.Generated.KDriveApiFileV3
            {
                Id = 5,
                Name = "a.txt",
                Type = "file",
                Size = 12,
                MimeType = "text/plain"
            };

            var item = KDriveDomainMapping.ToItem(dto);
            Assert.IsNotNull(item);
            Assert.AreEqual(5L, item!.Id);
            Assert.AreEqual("a.txt", item.Name);
            Assert.AreEqual(12L, item.Size);
        }

        static HttpResponseMessage Ok(string json) => new(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        static KDriveClient CreateClient(HttpMessageHandler handler)
        {
            var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.infomaniak.com/") };
            return new KDriveClient("token", 111, new KDriveUploadOptions { UseAutoChunkSize = false, ChunkSize = 1024 }, null, http);
        }

        private sealed class RecordingHandler(HttpResponseMessage response) : HttpMessageHandler
        {
            public HttpRequestMessage? LastRequest { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                return Task.FromResult(response);
            }
        }

        private sealed class SequenceHandler(params HttpResponseMessage[] responses) : HttpMessageHandler
        {
            private int _i;
            public List<HttpRequestMessage> Requests { get; } = [];

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Requests.Add(request);
                var response = responses[Math.Min(_i, responses.Length - 1)];
                _i++;
                return Task.FromResult(response);
            }
        }
    }
}
