using kDriveClient.kDriveClient;
using kDriveClient.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using Moq;

namespace kDriveClientTests.kDriveClient
{
    [TestClass()]
    public class KDriveClientTests
    {
        [TestMethod()]
        public async Task UploadDirect_Should_ConstructProperRequest()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":{"file":{"id":123,"name":"example.txt","path":"/Private","hash":"test","mime_type":"text/text", "visibility": "private_folder", "status": "ok", "type": "file"}}}""")
            };

            var handler = new FakeHttpMessageHandler(response);
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<KDriveClient>();
            var client = new KDriveClient("token", 111, new KDriveClientOptions(), logger, new HttpClient(handler) { BaseAddress = new Uri("https://api.infomaniak.com") });
            var file = new KDriveFile
            {
                Name = "example.txt",
                DirectoryPath = "/documents",
                Content = new MemoryStream([1, 2, 3])
            };

            var result = await client.UploadAsync(file);

            Assert.AreEqual(123, result.Id);
            Assert.IsNotNull(handler.LastRequest);
            StringAssert.Contains(handler.LastRequest.RequestUri!.ToString(), "/upload?");
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest.Method);
        }

        [TestMethod()]
        public async Task UploadFileChunkedAsync_Should_CancelSession_OnError()
        {
            var file = new KDriveFile
            {
                Name = "test.txt",
                Content = new MemoryStream([1, 2, 3]),
                Chunks =
                [
                    new KDriveChunk([1], 0, [0x00]),
                    new KDriveChunk([2], 1, [0x01])
                ]
            };
            var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<KDriveClient>();
            var client = new KDriveClient("token", 111, new KDriveClientOptions(), logger, new HttpClient(handler));

            await Assert.ThrowsExceptionAsync<HttpRequestException>(async () =>
            {
                await client.UploadFileChunkedAsync(file);
            });
        }

        [TestMethod()]
        public async Task UploadFileChunkedAsync_Should_UploadChunks_And_FinishSession()
        {
            var file = new KDriveFile
            {
                Name = "test.txt",
                Content = new MemoryStream([1, 2, 3]),
                Chunks =
                [
                    new KDriveChunk([1], 0, [0x00]),
                    new KDriveChunk([2], 1, [0x01])
                ]
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":{"file":{"id":123,"name":"test.txt"}}}""")
            };
            var handler = new FakeHttpMessageHandler(response);
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<KDriveClient>();
            var client = new KDriveClient("token", 111, new KDriveClientOptions(), logger, new HttpClient(handler));
            var result = await client.UploadFileChunkedAsync(file);

            Assert.AreEqual(123, result.Id);
            Assert.AreEqual("test.txt", result.Name);
        }

        [TestMethod]
        public void UploadAsync_Should_UseCustomOptions()
        {
            var options = new KDriveClientOptions { Parallelism = 8, ChunkSize = 512 * 1024 };
            var logger = new Mock<ILogger<KDriveClient>>().Object;
            var client = new KDriveClient("token", 123, options, logger);
            Assert.AreEqual(8, client.Parallelism);
            Assert.AreEqual(512 * 1024, client.DynamicChunkSizeBytes);
        }
    }


    public class FakeHttpMessageHandler(HttpResponseMessage fakeResponse) : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        private readonly HttpResponseMessage _fakeResponse = fakeResponse;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(_fakeResponse);
        }
    }

}