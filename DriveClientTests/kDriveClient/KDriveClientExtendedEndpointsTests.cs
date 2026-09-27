using kDriveClient.kDriveClient;
using kDriveClient.Models;
using System.Net;
using System.Text;

namespace kDriveClientTests.kDriveClient
{
    [TestClass]
    public class KDriveClientExtendedEndpointsTests
    {
        [TestMethod]
        public async Task RestoreTrashedFileTypedAsync_ShouldPostDestinationDirectoryId()
        {
            var json = """
            {"result":"success","data":{"cancel_id":"c-1","valid_until":1893456000}}
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.RestoreTrashedFileAsync(55, 100);

            Assert.IsNotNull(response?.Data);
            Assert.AreEqual("c-1", response.Data.CancelId);
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/111/trash/55/restore", handler.LastRequest.RequestUri!.PathAndQuery);
            StringAssert.Contains(handler.LastRequestBody!, "\"destination_directory_id\":100");
        }

        [TestMethod]
        public async Task EmptyTrashTypedAsync_ShouldDeleteTrashRoot()
        {
            var json = """{"result":"success","data":true}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.EmptyTrashAsync();

            Assert.IsTrue(response?.Data);
            Assert.AreEqual(HttpMethod.Delete, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/111/trash", handler.LastRequest.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetTrashCountTypedAsync_ShouldHitCountEndpoint()
        {
            var json = """{"result":"success","data":{"files":3,"directories":1,"total":4}}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetTrashCountAsync();

            Assert.AreEqual(4, response?.Data?.Total);
            Assert.AreEqual("/2/drive/111/trash/count", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task FavoriteItemAsync_ShouldPostFavoriteEndpoint()
        {
            var json = """{"result":"success","data":true}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            await client.FavoriteItemAsync(77);

            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/111/files/77/favorite", handler.LastRequest.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task CheckFilesExistTypedAsync_ShouldPostIds()
        {
            var json = """
            {"result":"success","data":[{"id":1,"result":"success","message":"ok"}]}
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CheckFilesExistAsync(new[] { 1L, 2L });

            Assert.AreEqual(1, response?.Data?.Count);
            StringAssert.Contains(handler.LastRequestBody!, "\"ids\"");
            Assert.AreEqual("/2/drive/111/files/exists", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetFileHashTypedAsync_ShouldDeserializeHash()
        {
            var json = """{"result":"success","data":{"hash":"xxh3:abc"}}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetFileHashAsync(5);

            Assert.AreEqual("xxh3:abc", response?.Data?.Hash);
            Assert.AreEqual("/2/drive/111/files/5/hash", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task CreateShareLinkTypedAsync_ShouldPostLinkBody()
        {
            var json = """
            {"result":"success","data":{"url":"https://kdrive.test/l/abc","file_id":9,"right":"public"}}
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CreateShareLinkAsync(9, new KDriveShareLinkRequest { Right = "public", CanDownload = true });

            Assert.AreEqual("public", response?.Data?.Right);
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            StringAssert.Contains(handler.LastRequestBody!, "\"right\":\"public\"");
        }

        [TestMethod]
        public async Task GetDriveTypedAsync_ShouldHitDriveInfoEndpoint()
        {
            var json = """{"result":"success","data":{"id":111,"name":"My Drive","used_size":50}}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetDriveAsync();

            Assert.AreEqual(111, response?.Data?.Id);
            Assert.AreEqual("/2/drive/111", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetFileAccessUsersTypedAsync_ShouldHitAccessUsersEndpoint()
        {
            var json = """{"result":"success","data":[],"items_per_page":100,"page":1,"pages":1,"total":0}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            await client.GetFileAccessUsersAsync(42);

            Assert.AreEqual("/2/drive/111/files/42/access/users", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task RemoveFileAccessUserTypedAsync_ShouldDeleteUserAccess()
        {
            var json = """{"result":"success","data":true}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.RemoveFileAccessUserAsync(42, 999);

            Assert.IsTrue(response?.Data);
            Assert.AreEqual(HttpMethod.Delete, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/111/files/42/access/users/999", handler.LastRequest.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetFileVersionsV2TypedAsync_ShouldHitV2VersionsList()
        {
            var json = """{"result":"success","data":[],"items_per_page":100,"page":1,"pages":1,"total":0}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            await client.GetFileVersionsV2Async(7);

            Assert.AreEqual("/2/drive/111/files/7/versions", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetGlobalPreferencesTypedAsync_ShouldHitPreferencesRoot()
        {
            var json = """{"result":"success","data":{"default_drive":111,"density":"comfortable"}}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetGlobalPreferencesAsync();

            Assert.AreEqual(111, response?.Data?.DefaultDrive);
            Assert.AreEqual("/2/drive/preferences", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetTrashedFileCountTypedAsync_ShouldHitTrashItemCount()
        {
            var json = """{"result":"success","data":{"files":1,"directories":0,"total":1}}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetTrashedFileCountAsync(88);

            Assert.AreEqual(1, response?.Data?.Total);
            Assert.AreEqual("/3/drive/111/trash/88/count", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task CancelUploadSessionTypedAsync_ShouldDeleteV3Session()
        {
            var json = """{"result":"success","data":true}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CancelUploadSessionAsync("sess-99");

            Assert.IsTrue(response?.Data);
            Assert.AreEqual(HttpMethod.Delete, handler.LastRequest!.Method);
            Assert.AreEqual("/3/drive/111/upload/session/sess-99", handler.LastRequest.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task BuildShareLinkArchiveTypedAsync_ShouldPostAppShareArchive()
        {
            var json = """{"result":"success","data":{"uuid":"arc-1"}}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.BuildShareLinkArchiveAsync("share-uuid-1", new[] { 10L });

            Assert.AreEqual("arc-1", response?.Data?.Uuid);
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            Assert.AreEqual("/2/app/111/share/share-uuid-1/archive", handler.LastRequest.RequestUri!.PathAndQuery);
            StringAssert.Contains(handler.LastRequestBody!, "\"file_ids\"");
        }

        [TestMethod]
        public async Task CreateFileDropboxTypedAsync_ShouldPostDropbox()
        {
            var json = """{"result":"success","data":{"id":1,"name":"Inbox","url":"https://kdrive.test/dbx"}}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CreateFileDropboxAsync(12, new KDriveDropboxRequest { Name = "Inbox" });

            Assert.AreEqual("Inbox", response?.Data?.Name);
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/111/files/12/dropbox", handler.LastRequest.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task LockDriveUserTypedAsync_ShouldPostLock()
        {
            var json = """{"result":"success","data":true}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.LockDriveUserAsync(501);

            Assert.IsTrue(response?.Data);
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/111/users/501/lock", handler.LastRequest.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task CancelUploadSessionsBatchAsync_ShouldDeleteWithTokensQuery()
        {
            var json = """{"result":"success","data":true}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            await client.CancelUploadSessionsBatchAsync(new[] { "tok-a", "tok-b" });

            Assert.AreEqual(HttpMethod.Delete, handler.LastRequest!.Method);
            StringAssert.Contains(handler.LastRequest.RequestUri!.PathAndQuery, "/3/drive/111/upload/session/batch");
            StringAssert.Contains(handler.LastRequest.RequestUri.PathAndQuery, "tokens=");
            StringAssert.Contains(handler.LastRequest.RequestUri.PathAndQuery, "tok-a");
            StringAssert.Contains(handler.LastRequest.RequestUri.PathAndQuery, "tok-b");
        }

        [TestMethod]
        public async Task UpdateCategoryTypedAsync_ShouldPutCategory()
        {
            var json = """{"result":"success","data":true}""";
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            await client.UpdateCategoryAsync(3, name: "Labels");

            Assert.AreEqual(HttpMethod.Put, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/111/categories/3", handler.LastRequest.RequestUri!.PathAndQuery);
            StringAssert.Contains(handler.LastRequestBody!, "\"name\":\"Labels\"");
        }

        [TestMethod]
        public async Task SearchItemsAsync_WithTypedSearchQuery_ShouldMapQueryParams()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":21,"name":"match.txt","type":"file"}],
              "cursor":"c2",
              "has_more":false,
              "response_at":1710000001
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.SearchItemsAsync(new KDriveSearchQuery
            {
                Query = "match",
                Limit = 25,
                DirectoryId = 9
            });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Items.Count);
            var url = handler.LastRequest!.RequestUri!.ToString();
            StringAssert.Contains(url, "/3/drive/111/files/search?");
            StringAssert.Contains(url, "query=match");
            StringAssert.Contains(url, "limit=25");
            StringAssert.Contains(url, "directory_id=9");
        }

        private static KDriveClient CreateClient(HttpMessageHandler handler)
        {
            var options = new KDriveUploadOptions { UseAutoChunkSize = false, ChunkSize = 1024 * 1024 };
            return new KDriveClient(
                "token",
                111,
                options,
                logger: null,
                httpClient: new HttpClient(handler) { BaseAddress = new Uri("https://api.infomaniak.com") });
        }

        private sealed class RecordingHandler(HttpResponseMessage response) : HttpMessageHandler
        {
            public HttpRequestMessage? LastRequest { get; private set; }
            public string? LastRequestBody { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                if (request.Content is not null)
                    LastRequestBody = await request.Content.ReadAsStringAsync(cancellationToken);
                return response;
            }
        }
    }
}
