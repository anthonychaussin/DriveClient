using kDriveClient.kDriveClient;
using kDriveClient.Models;
using System.Net;
using System.Text;

namespace kDriveClientTests.kDriveClient
{
    [TestClass]
    public class KDriveClientSmartEndpointsTests
    {
        [TestMethod]
        public async Task EnsureDriveAwakeAsync_ShouldCallWakeOnlyOnceWithinInterval()
        {
            var handler = new SequenceHandler(
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
                },
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
                });

            var client = CreateClient(handler);
            var first = await client.EnsureDriveAwakeAsync(TimeSpan.FromMinutes(10));
            var second = await client.EnsureDriveAwakeAsync(TimeSpan.FromMinutes(10));

            Assert.AreEqual(true, first);
            Assert.AreEqual(true, second);
            Assert.AreEqual(1, handler.Requests.Count);
            Assert.AreEqual("/3/drive/111/wake", handler.Requests[0].RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetAllDriveUsersV3Async_ShouldPaginateOnCursor()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":1,"display_name":"A"}],"cursor":"u-c1","has_more":true,"response_at":1}
                """, Encoding.UTF8, "application/json")
            };
            var page2 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":2,"display_name":"B"}],"cursor":"u-c2","has_more":false,"response_at":2}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page1, page2);
            var client = CreateClient(handler);
            var users = await client.GetAllDriveUsersV3Async(pageSize: 1, maxItems: 10);

            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(1L, users[0].Id);
            Assert.AreEqual(2L, users[1].Id);
            Assert.AreEqual(3, handler.Requests.Count);
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "/3/drive/111/users?limit=1");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "/3/drive/111/users?");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "limit=1");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "cursor=u-c1");
        }

        [TestMethod]
        public async Task GetItemsAllAsync_ShouldPaginateOnCursor()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":1,"name":"a.txt","type":"file"}],"cursor":"c1","has_more":true,"response_at":1}
                """, Encoding.UTF8, "application/json")
            };
            var page2 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":2,"name":"b.txt","type":"file"}],"cursor":"c2","has_more":false,"response_at":2}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page1, page2);
            var client = CreateClient(handler);
            var items = await client.GetItemsAllAsync(20, pageSize: 1, maxItems: 10);

            Assert.AreEqual(2, items.Count);
            Assert.AreEqual("a.txt", items[0].Name);
            Assert.AreEqual("b.txt", items[1].Name);
            Assert.AreEqual(3, handler.Requests.Count);
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "/3/drive/111/files/20/files?limit=1");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "cursor=c1");
        }

        [TestMethod]
        public async Task SearchItemsAllAsync_WithTypedQuery_ShouldSendQueryAndPaginate()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":9,"name":"match.txt","type":"file"}],"has_more":false,"response_at":1}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page);
            var client = CreateClient(handler);
            var items = await client.SearchItemsAllAsync(new KDriveSearchQuery { Query = "match", DirectoryId = 5, Limit = 10 });

            Assert.AreEqual(1, items.Count);
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "/3/drive/111/files/search?");
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "query=match");
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "directory_id=5");
        }

        [TestMethod]
        public async Task FindTrashItemAsync_ShouldPreferExactName()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var search = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {
                  "result":"success",
                  "data":[
                    {"id":10,"name":"report-final.txt","type":"file","path":"/Trash/report-final.txt"},
                    {"id":11,"name":"report.txt","type":"file","path":"/Trash/reports/report.txt"},
                    {"id":12,"name":"report-old.txt","type":"file","path":"/Trash/report-old.txt"}
                  ],
                  "cursor":"t-c1",
                  "has_more":false,
                  "response_at":3
                }
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, search);
            var client = CreateClient(handler);
            var found = await client.FindTrashItemAsync("report.txt");

            Assert.IsNotNull(found);
            Assert.AreEqual(11L, found.Id);
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "/3/drive/111/files/search/trash");
        }

        [TestMethod]
        public async Task GetItemVersionsAllAsync_ShouldPaginateByPageNumber()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":11,"file_id":7,"name":"v1"},{"id":12,"file_id":7,"name":"v2"}],"page":1,"pages":2,"items_per_page":2,"total":3}
                """, Encoding.UTF8, "application/json")
            };
            var page2 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":13,"file_id":7,"name":"v3"}],"page":2,"pages":2,"items_per_page":2,"total":3}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page1, page2);
            var client = CreateClient(handler);
            var versions = await client.GetItemVersionsAllAsync(7, pageSize: 2, maxItems: 10);

            Assert.AreEqual(3, versions.Count);
            Assert.AreEqual(11L, versions[0].Id);
            Assert.AreEqual(13L, versions[2].Id);
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "/3/drive/111/files/7/versions?page=1&per_page=2");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "/3/drive/111/files/7/versions?page=2&per_page=2");
        }

        [TestMethod]
        public async Task FindItemVersionAsync_ShouldReturnLatestMatchingVersion()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {
                  "result":"success",
                  "data":[
                    {"id":21,"file_id":9,"name":"invoice-v1.pdf","last_modified_at":1700000000},
                    {"id":22,"file_id":9,"name":"invoice-v2.pdf","last_modified_at":1710000000},
                    {"id":23,"file_id":9,"name":"draft.txt","last_modified_at":1720000000}
                  ],
                  "page":1,"pages":1,"items_per_page":50,"total":3
                }
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page);
            var client = CreateClient(handler);
            var version = await client.FindItemVersionAsync(9, nameContains: "invoice");

            Assert.IsNotNull(version);
            Assert.AreEqual(22L, version.Id);
        }

        [TestMethod]
        public async Task RestoreLatestItemVersionAsync_ShouldResolveVersionThenRestore()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var versions = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":31,"file_id":10,"name":"a","last_modified_at":1700},{"id":32,"file_id":10,"name":"b","last_modified_at":1800}],"page":1,"pages":1,"items_per_page":50,"total":2}
                """, Encoding.UTF8, "application/json")
            };
            var restore = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":{"id":10,"name":"restored.txt","type":"file","path":"/Private/restored.txt"}}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, versions, restore);
            var client = CreateClient(handler);
            var restored = await client.RestoreLatestItemVersionAsync(10, with: "path");

            Assert.IsNotNull(restored);
            Assert.AreEqual(10L, restored.Item?.Id);
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "/3/drive/111/files/10/versions/32/restore?with=path");
        }

        [TestMethod]
        public async Task GetDriveActivityFeedAllAsync_ShouldPaginateCursor()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":1001,"action":"created","type":"file","created_at":1}],"cursor":"a-c1","has_more":true,"response_at":1}
                """, Encoding.UTF8, "application/json")
            };
            var page2 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":1002,"action":"renamed","type":"file","created_at":2}],"cursor":"a-c2","has_more":false,"response_at":2}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page1, page2);
            var client = CreateClient(handler);
            var activities = await client.GetDriveActivityFeedAllAsync(pageSize: 1, maxItems: 10);

            Assert.AreEqual(2, activities.Count);
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "/3/drive/111/activities?limit=1");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "/3/drive/111/activities?");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "limit=1");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "cursor=a-c1");
        }

        [TestMethod]
        public async Task FindDriveActivityAsync_ShouldPreferExactAction()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":1101,"action":"renamed","type":"file","created_at":10},{"id":1102,"action":"rename","type":"file","created_at":20}],"cursor":"x","has_more":false,"response_at":3}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page);
            var client = CreateClient(handler);
            var found = await client.FindDriveActivityAsync("rename");

            Assert.IsNotNull(found);
            Assert.AreEqual(1102L, found.Id);
        }

        [TestMethod]
        public async Task GetFileActivitiesAllAsync_ShouldUseFileActivitiesEndpoint()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":1201,"action":"updated","type":"file","created_at":30}],"cursor":"f-c1","has_more":false,"response_at":4}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page);
            var client = CreateClient(handler);
            var activities = await client.GetFileActivitiesAllAsync(77, pageSize: 10, maxItems: 100);

            Assert.AreEqual(1, activities.Count);
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "/3/drive/111/files/77/activities?limit=10");
        }

        [TestMethod]
        public async Task CreateItemCommentAsync_ShouldPostCommentBody()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":{"id":9,"body":"Looks good"}}""", Encoding.UTF8, "application/json")
            };
            var handler = new SequenceHandler(response);
            var client = CreateClient(handler);

            var created = await client.CreateItemCommentAsync(55, "Looks good");

            Assert.IsNotNull(created?.Data);
            Assert.AreEqual(9L, created.Data.Id);
            Assert.AreEqual(HttpMethod.Post, handler.Requests[0].Method);
            StringAssert.Contains(handler.Requests[0].RequestUri!.ToString(), "/2/drive/111/files/55/comments");
        }

        [TestMethod]
        public async Task UpdateItemShareLinkAsync_ShouldPutLink()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var handler = new SequenceHandler(response);
            var client = CreateClient(handler);

            var updated = await client.UpdateItemShareLinkAsync(55, new KDriveShareLinkRequest { CanDownload = false });

            Assert.AreEqual(true, updated?.Data);
            Assert.AreEqual(HttpMethod.Put, handler.Requests[0].Method);
            StringAssert.Contains(handler.Requests[0].RequestUri!.ToString(), "/2/drive/111/files/55/link");
        }

        [TestMethod]
        public async Task GetItemCommentsAllAsync_ShouldPaginatePages()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":1,"body":"a"}],"page":1,"pages":2,"items_per_page":1,"total":2}
                """, Encoding.UTF8, "application/json")
            };
            var page2 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":2,"body":"b"}],"page":2,"pages":2,"items_per_page":1,"total":2}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page1, page2);
            var client = CreateClient(handler);
            var comments = await client.GetItemCommentsAllAsync(55, pageSize: 1, maxItems: 10);

            Assert.AreEqual(2, comments.Count);
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "/2/drive/111/files/55/comments?page=1&per_page=1");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "/2/drive/111/files/55/comments?page=2&per_page=1");
        }

        [TestMethod]
        public async Task GetFavoritesAllAsync_ShouldPaginateOnCursor()
        {
            var wake = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var page1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":1,"name":"a.txt","type":"file"}],"cursor":"f1","has_more":true,"response_at":1}
                """, Encoding.UTF8, "application/json")
            };
            var page2 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                {"result":"success","data":[{"id":2,"name":"b.txt","type":"file"}],"cursor":"f2","has_more":false,"response_at":2}
                """, Encoding.UTF8, "application/json")
            };

            var handler = new SequenceHandler(wake, page1, page2);
            var client = CreateClient(handler);
            var favorites = await client.GetFavoritesAllAsync(pageSize: 1, maxItems: 10);

            Assert.AreEqual(2, favorites.Count);
            StringAssert.Contains(handler.Requests[1].RequestUri!.ToString(), "/3/drive/111/files/favorites?limit=1");
            StringAssert.Contains(handler.Requests[2].RequestUri!.ToString(), "cursor=f1");
        }

        [TestMethod]
        public async Task TagItemAsync_ShouldPostCategoryOnFile()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"result":"success","data":true}""", Encoding.UTF8, "application/json")
            };
            var handler = new SequenceHandler(response);
            var client = CreateClient(handler);

            var tagged = await client.TagItemAsync(55, 7);

            Assert.AreEqual(true, tagged?.Data);
            Assert.AreEqual(HttpMethod.Post, handler.Requests[0].Method);
            StringAssert.Contains(handler.Requests[0].RequestUri!.ToString(), "/2/drive/111/files/55/categories/7");
        }

        private static KDriveClient CreateClient(HttpMessageHandler handler)
        {
            var options = new KDriveUploadOptions
            {
                UseAutoChunkSize = false,
                ChunkSize = 1024 * 1024
            };

            return new KDriveClient(
                "token",
                111,
                options,
                logger: null,
                httpClient: new HttpClient(handler) { BaseAddress = new Uri("https://api.infomaniak.com") });
        }

        private sealed class SequenceHandler(params HttpResponseMessage[] responses) : HttpMessageHandler
        {
            private readonly Queue<HttpResponseMessage> _responses = new(responses);
            public List<HttpRequestMessage> Requests { get; } = [];

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Requests.Add(request);
                if (_responses.Count == 0)
                {
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("""{"result":"error","error":{"code":"no_mock","description":"No mock response configured"}}""", Encoding.UTF8, "application/json")
                    });
                }

                return Task.FromResult(_responses.Dequeue());
            }
        }
    }
}
