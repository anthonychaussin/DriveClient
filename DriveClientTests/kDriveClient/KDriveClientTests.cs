using kDriveClient.kDriveClient;
using kDriveClient.Models;
using Microsoft.Extensions.Logging;
using System.Net;

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
                Content = new StringContent("{\"result\":\"success\",\"data\":{\"file\":{\"id\":123,\"name\":\"example.txt\",\"path\":\"/Private/\",\"hash\":\"test\",\"mime_type\":\"text/text\"}}}")
            };

            var handler = new FakeHttpMessageHandler(response);
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<KDriveClient>();
            var client = new KDriveClient("token", 111, false, 4, logger, new HttpClient(handler) { BaseAddress = new Uri("https://api.infomaniak.com") });
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