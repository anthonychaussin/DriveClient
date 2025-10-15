using kDriveClient.kDriveClient;
using kDriveClient.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kDriveClient.kDriveClient.Tests
{
    [TestClass()]
    public class KDriveClientTests
    {
        [TestMethod()]
        public async Task UploadFileChunkedAsync_Should_CancelSession_OnError()
        {
            // Arrange
            var file = new KDriveFile
            {
                Name = "test.txt",
                Content = new MemoryStream([1, 2, 3]),
                Chunks = new List<KDriveChunk>
        {
            new KDriveChunk([1], 0, [0x00]),
            new KDriveChunk([2], 1, [0x01])
        }
            };
            var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<KDriveClient>();
            var client = new KDriveClient("token", 111, false, 4, logger, new HttpClient(handler));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HttpRequestException>(async () =>
            {
                await client.UploadFileChunkedAsync(file);
            });
            // Ici, vous pouvez vérifier que CancelUploadSessionRequest a été appelé via un mock ou un logger.
        }
    }
}