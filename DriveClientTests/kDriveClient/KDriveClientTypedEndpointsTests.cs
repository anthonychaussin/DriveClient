using kDriveClient.kDriveClient;
using kDriveClient.Models;
using System.Net;
using System.Text;

namespace kDriveClientTests.kDriveClient
{
    [TestClass]
    public class KDriveClientTypedEndpointsTests
    {
        [TestMethod]
        public async Task GetAccessibleDrivesTypedAsync_ShouldDeserialize_AndIncludeAccountIdInQuery()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":10,"name":"Main Drive","size":1000,"used_size":100}],
              "total":1,
              "page":1,
              "pages":1,
              "items_per_page":10
            }
            """;

            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetAccessibleDrivesAsync(42);

            Assert.IsNotNull(response);
            Assert.AreEqual("success", response.Result);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual(10, response.Data[0].Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/2/drive?account_id=42");
            Assert.AreEqual(HttpMethod.Get, handler.LastRequest.Method);
        }

        [TestMethod]
        public async Task GetFileDetailsTypedAsync_ShouldDeserialize_FileItem()
        {
            var json = """
            {
              "result":"success",
              "data":{
                "id":99,
                "name":"report.pdf",
                "type":"file",
                "path":"/Private/report.pdf",
                "size":12345,
                "mime_type":"application/pdf"
              }
            }
            """;

            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetFileDetailsAsync(99, "path");

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(99, response.Data.Id);
            Assert.AreEqual("report.pdf", response.Data.Name);
            Assert.AreEqual("application/pdf", response.Data.MimeType);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/99?with=path");
        }

        [TestMethod]
        public async Task RenameFileTypedAsync_ShouldSendJsonBody_AndDeserializeCancelResource()
        {
            var json = """
            {
              "result":"success",
              "data":{"cancel_id":"abc-123","valid_until":1893456000}
            }
            """;

            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.RenameFileAsync(777, "new-name.txt");

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual("abc-123", response.Data.CancelId);
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/111/files/777/rename", handler.LastRequest.RequestUri!.PathAndQuery);

            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"name\":\"new-name.txt\"");
        }

        [TestMethod]
        public async Task GetTrashTypedAsync_ShouldDeserializeNavigatorFields()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":11,"name":"old.docx","type":"file"}],
              "cursor":"next-cursor",
              "has_more":true,
              "response_at":1710000000
            }
            """;

            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetTrashAsync(new KDriveListQuery { Limit = 50 });

            Assert.IsNotNull(response);
            Assert.AreEqual("next-cursor", response.Cursor);
            Assert.AreEqual(true, response.HasMore);
            Assert.AreEqual(1710000000L, response.ResponseAt);
            Assert.AreEqual(1, response.Data.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/trash?limit=50");
        }

        [TestMethod]
        public async Task GetDriveUsersTypedAsync_ShouldDeserializePagedUsers()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":7,"display_name":"John Doe","email":"john@x.com"}],
              "total":1,
              "page":1,
              "pages":1,
              "items_per_page":10
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetDriveUsersAsync();

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual(7, response.Data[0].Id);
            Assert.AreEqual(HttpMethod.Get, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/users", handler.LastRequest.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetUserDrivesTypedAsync_ShouldDeserializePagedDriveUsers_AndIncludeAccountId()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":8,"display_name":"Jane","drive_id":111,"drive_name":"Main"}],
              "total":1,
              "page":1,
              "pages":1,
              "items_per_page":10
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetUserDrivesAsync(8, 42);

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual(111, response.Data[0].DriveId);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/2/drive/users/8/drives?account_id=42");
        }

        [TestMethod]
        public async Task ListDirectoryFilesTypedAsync_ShouldDeserializeNavigatorItems()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":20,"name":"folder","type":"dir"}],
              "cursor":"c1",
              "has_more":false,
              "response_at":1710000000
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.ListDirectoryFilesAsync(20, new KDriveListQuery { Limit = 25 });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual("folder", response.Data[0].Name);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/20/files?limit=25");
        }

        [TestMethod]
        public async Task SearchFilesTypedAsync_ShouldDeserializeNavigatorItems_AndQuery()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":21,"name":"match.txt","type":"file"}],
              "cursor":"c2",
              "has_more":true,
              "response_at":1710000001
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.SearchFilesAsync(new KDriveListQuery { Extra = { ["query"] = "match" } });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual(21, response.Data[0].Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/search?query=match");
        }

        [TestMethod]
        public async Task GetRecentFilesTypedAsync_ShouldDeserializeNavigatorItems()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":22,"name":"recent.doc","type":"file"}],
              "cursor":"c3",
              "has_more":false,
              "response_at":1710000002
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetRecentFilesAsync();

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual("recent.doc", response.Data[0].Name);
            Assert.AreEqual("/3/drive/111/files/recents", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetFavoriteFilesTypedAsync_ShouldDeserializeNavigatorItems()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":23,"name":"favorite.doc","type":"file"}],
              "cursor":"c4",
              "has_more":false,
              "response_at":1710000003
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetFavoriteFilesAsync(new KDriveListQuery { Limit = 10 });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual("favorite.doc", response.Data[0].Name);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/favorites?limit=10");
        }

        [TestMethod]
        public async Task SearchFavoriteFilesTypedAsync_ShouldDeserializeNavigatorItems_AndQuery()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":24,"name":"favorite-match.doc","type":"file"}],
              "cursor":"c5",
              "has_more":true,
              "response_at":1710000004
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.SearchFavoriteFilesAsync(new KDriveListQuery { Extra = { ["query"] = "favorite" } });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual(24, response.Data[0].Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/search/favorites?query=favorite");
        }

        [TestMethod]
        public async Task GetMySharedAsync_Alias_ShouldUseMySharedEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":25,"name":"shared-by-me.txt","type":"file"}],
              "cursor":"c6",
              "has_more":false,
              "response_at":1710000005
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetMySharedAsync();

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Items.Count);
            Assert.AreEqual("/3/drive/111/files/my_shared", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task SearchMySharedAsync_Alias_ShouldUseSearchMySharedEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":26,"name":"shared-by-me-match.txt","type":"file"}],
              "cursor":"c7",
              "has_more":true,
              "response_at":1710000006
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.SearchMySharedAsync(new KDriveListQuery { Extra = { ["query"] = "match" } });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Items.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/search/my_shared?query=match");
        }

        [TestMethod]
        public async Task GetSharedWithMeFilesTypedAsync_ShouldDeserializeNavigatorItems()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":27,"name":"shared-with-me.txt","type":"file"}],
              "cursor":"c8",
              "has_more":false,
              "response_at":1710000007
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetSharedWithMeFilesAsync();

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual("/3/drive/111/files/shared_with_me", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task SearchSharedWithMeAsync_Alias_ShouldUseSearchSharedWithMeEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":28,"name":"shared-with-me-match.txt","type":"file"}],
              "cursor":"c9",
              "has_more":true,
              "response_at":1710000008
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.SearchSharedWithMeAsync(new KDriveListQuery { Extra = { ["query"] = "shared" } });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Items.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/search/shared_with_me?query=shared");
        }

        [TestMethod]
        public async Task GetArchivesAsync_Alias_ShouldUseArchivesEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":{"uuid":"00000000-e89b-12d3-a456-426614174000"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetArchivesAsync(new[] { 1L, 2L });

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual("00000000-e89b-12d3-a456-426614174000", response.Data.Uuid);
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            Assert.AreEqual("/3/drive/111/files/archives", handler.LastRequest!.RequestUri!.PathAndQuery);
            StringAssert.Contains(handler.LastRequestBody!, "\"file_ids\"");
        }

        [TestMethod]
        public async Task GetLargestFilesTypedAsync_ShouldUseLargestEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":30,"name":"huge.bin","type":"file"}],
              "cursor":"c11",
              "has_more":false,
              "response_at":1710000010
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetLargestFilesAsync(new KDriveListQuery { Limit = 5 });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/largest?limit=5");
        }

        [TestMethod]
        public async Task GetLastModifiedAsync_Alias_ShouldUseLastModifiedEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":31,"name":"last-modified.txt","type":"file"}],
              "cursor":"c12",
              "has_more":false,
              "response_at":1710000011
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetLastModifiedAsync();

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Items.Count);
            Assert.AreEqual("/3/drive/111/files/last_modified", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetMostVersionedFilesTypedAsync_ShouldUseMostVersionsEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":32,"name":"many-versions.docx","type":"file"}],
              "cursor":"c13",
              "has_more":false,
              "response_at":1710000012
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetMostVersionedFilesAsync();

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual("/3/drive/111/files/most_versions", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetLinksAsync_Alias_ShouldUseLinksEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":33,"name":"linked-file.txt","type":"file"}],
              "cursor":"c14",
              "has_more":false,
              "response_at":1710000013
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetLinksAsync();

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Items.Count);
            Assert.AreEqual("/3/drive/111/files/links", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task SearchLinkedFilesTypedAsync_ShouldUseSearchLinksEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":34,"name":"linked-match.txt","type":"file"}],
              "cursor":"c15",
              "has_more":true,
              "response_at":1710000014
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.SearchLinkedFilesAsync(new KDriveListQuery { Extra = { ["query"] = "link" } });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/search/links?query=link");
        }

        [TestMethod]
        public async Task GetDropboxesAsync_Alias_ShouldUseDropboxesEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":35,"name":"dropbox-folder","type":"dir"}],
              "cursor":"c16",
              "has_more":false,
              "response_at":1710000015
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetDropboxesAsync();

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Items.Count);
            Assert.AreEqual("/3/drive/111/files/dropboxes", handler.LastRequest!.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task SearchDropboxesAsync_Alias_ShouldUseSearchDropboxesEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":36,"name":"dropbox-match","type":"dir"}],
              "cursor":"c17",
              "has_more":true,
              "response_at":1710000016
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.SearchDropboxesAsync(new KDriveListQuery { Extra = { ["query"] = "drop" } });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Items.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/search/dropboxes?query=drop");
        }

        [TestMethod]
        public async Task GetFileByNameTypedAsync_ShouldUseV3NameEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":101,"name":"match.txt","type":"file","path":"/Private/match.txt"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetFileByNameAsync(5, "match.txt", "path");

            Assert.IsNotNull(response);
            Assert.AreEqual(101, response.Data?.Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/5/name?with=path&name=match.txt");
        }

        [TestMethod]
        public async Task DuplicateFileTypedAsync_ShouldUseV3DuplicateEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":102,"name":"copy.txt","type":"file","path":"/Private/copy.txt"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.DuplicateFileAsync(42, "copy.txt", "path");

            Assert.IsNotNull(response);
            Assert.AreEqual(102, response.Data?.Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/42/duplicate?with=path");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"name\":\"copy.txt\"");
        }

        [TestMethod]
        public async Task UnlockFileTypedAsync_ShouldUseV3UnlockEndpoint_AndDeserializeBool()
        {
            var json = """
            {
              "result":"success",
              "data":true
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.UnlockFileAsync(99, "/Private/locked.txt", "token123");

            Assert.IsNotNull(response);
            Assert.AreEqual(true, response.Data);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/99/lock?path=%2FPrivate%2Flocked.txt&token=token123");
            Assert.AreEqual(HttpMethod.Delete, handler.LastRequest.Method);
        }

        [TestMethod]
        public async Task CountDirectoryElementsTypedAsync_ShouldUseV3CountEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":{"files":3,"directories":2,"total":5}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CountDirectoryElementsAsync(7, 2);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(5, response.Data.Total);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/7/count?depth=2");
        }

        [TestMethod]
        public async Task GetFileVersionsTypedAsync_ShouldUseV3VersionsEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":1,"file_id":77,"name":"v1.txt","size":10},{"id":2,"file_id":77,"name":"v2.txt","size":11}],
              "total":2,
              "page":1,
              "pages":1,
              "items_per_page":50
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetFileVersionsAsync(77, new KDriveListQuery { Extra = { ["page"] = "1", ["per_page"] = "50" } });

            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Data.Count);
            Assert.AreEqual(2, response.Total);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/77/versions?page=1&per_page=50");
        }

        [TestMethod]
        public async Task RestoreFileVersionTypedAsync_ShouldUseV3RestoreEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":88,"name":"restored.txt","type":"file","path":"/Private/restored.txt"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.RestoreFileVersionAsync(10, 4, "path");

            Assert.IsNotNull(response);
            Assert.AreEqual(88, response.Data?.Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/10/versions/4/restore?with=path");
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest.Method);
        }

        [TestMethod]
        public async Task RestoreFileVersionToDirectoryTypedAsync_ShouldUseV3RestoreToDirectoryEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":89,"name":"restored-copy.txt","type":"file","path":"/Private/Target/restored-copy.txt"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.RestoreFileVersionToDirectoryAsync(10, 4, 9, "restored-copy.txt", "path");

            Assert.IsNotNull(response);
            Assert.AreEqual(89, response.Data?.Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/10/versions/4/restore/9?with=path");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"name\":\"restored-copy.txt\"");
        }

        [TestMethod]
        public async Task GetItemActivitiesAsync_Alias_ShouldUseV3ActivitiesEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":501,"type":"file","action":"created","created_at":1710000100}],
              "cursor":"activities-c1",
              "has_more":false,
              "response_at":1710000101
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetItemActivitiesAsync(33, new KDriveListQuery { Limit = 10 });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual("created", response.Data[0].Action);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/33/activities?limit=10");
        }

        [TestMethod]
        public async Task ConvertFileTypedAsync_ShouldUseV3ConvertEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":200,"name":"converted.pdf","type":"file","path":"/Private/converted.pdf"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.ConvertFileAsync(20, with: "path");

            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.Data?.Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/20/convert?with=path");
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest.Method);
        }

        [TestMethod]
        public async Task CreateDefaultFileTypedAsync_ShouldUseV3FileEndpoint_AndSendBody()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":201,"name":"note","type":"file","path":"/Private/note"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CreateDefaultFileAsync(5, "note", "doc", with: "path");

            Assert.IsNotNull(response);
            Assert.AreEqual(201, response.Data?.Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/5/file?with=path");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"name\":\"note\"");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"type\":\"doc\"");
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest.Method);
        }

        [TestMethod]
        public async Task CreateTeamDirectoryTypedAsync_ShouldUseV3TeamDirectoryEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":202,"name":"Team Shared","type":"dir","path":"/Team Shared"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CreateTeamDirectoryAsync("Team Shared", "#00aa00", true, with: "path");

            Assert.IsNotNull(response);
            Assert.AreEqual(202, response.Data?.Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/team_directory?with=path");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"name\":\"Team Shared\"");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"for_all_user\":true");
        }

        [TestMethod]
        public async Task GetRootFilesActivitiesTypedAsync_ShouldUseV3FilesActivitiesEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":601,"type":"file","action":"updated","created_at":1710000200}],
              "cursor":"fa-c1",
              "has_more":true,
              "response_at":1710000201
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetRootFilesActivitiesAsync(new KDriveListQuery { Limit = 10 });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/activities?limit=10");
        }

        [TestMethod]
        public async Task GetDriveActivitiesTypedAsync_ShouldUseV3DriveActivitiesEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":602,"type":"file","action":"moved","created_at":1710000202}],
              "cursor":"da-c1",
              "has_more":false,
              "response_at":1710000203
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetDriveActivitiesAsync(new KDriveListQuery { Limit = 5 });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/activities?limit=5");
        }

        [TestMethod]
        public async Task GetDriveActivitiesTotalTypedAsync_ShouldDeserializeResourceList()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":603,"type":"file","action":"deleted","created_at":1710000204}]
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetDriveActivitiesTotalAsync(new KDriveListQuery { Extra = { ["from"] = "1710000000" } });

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(1, response.Data.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/activities/total?from=1710000000");
        }

        [TestMethod]
        public async Task WakeDriveTypedAsync_ShouldUseV3WakeEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":true
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.WakeDriveAsync();

            Assert.IsNotNull(response);
            Assert.AreEqual(true, response.Data);
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            Assert.AreEqual("/3/drive/111/wake", handler.LastRequest.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetDriveUsersV3TypedAsync_ShouldDeserializeNavigatorUsers()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":41,"display_name":"Alice","email":"alice@x.com","status":"active","type":"user"}],
              "cursor":"u1",
              "has_more":false,
              "response_at":1710000300
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetDriveUsersV3Async(new KDriveListQuery { Limit = 20 });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual(41, response.Data[0].Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/users?limit=20");
        }

        [TestMethod]
        public async Task SearchTrashFilesTypedAsync_ShouldUseV3SearchTrashEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":501,"name":"trashed.txt","type":"file","path":"/Trash/trashed.txt"}],
              "cursor":"t1",
              "has_more":false,
              "response_at":1710000301
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.SearchTrashFilesAsync(new KDriveListQuery { Extra = { ["query"] = "trash" } });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/search/trash?query=trash");
        }

        [TestMethod]
        public async Task GetTrashedDirectoryFilesTypedAsync_ShouldUseV3TrashChildrenEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":[{"id":502,"name":"child.doc","type":"file","path":"/Trash/Folder/child.doc"}],
              "cursor":"t2",
              "has_more":false,
              "response_at":1710000302
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetTrashedDirectoryFilesAsync(77, new KDriveListQuery { Limit = 50 });

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/trash/77/files?limit=50");
        }

        [TestMethod]
        public async Task CountTrashedDirectoryElementsTypedAsync_ShouldUseV3TrashCountEndpoint()
        {
            var json = """
            {
              "result":"success",
              "data":{"files":4,"directories":1,"total":5}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CountTrashedDirectoryElementsAsync(77, depth: 2);

            Assert.IsNotNull(response);
            Assert.AreEqual(5, response.Data?.Total);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/trash/77/count?depth=2");
        }

        [TestMethod]
        public async Task CreateDirectoryTypedAsync_ShouldSendBody_AndDeserializeResource()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":30,"name":"NewFolder","type":"dir","path":"/Private/NewFolder"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CreateDirectoryAsync(5, "NewFolder", "#0098ff", true, "nested");

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(30, response.Data.Id);
            Assert.AreEqual(HttpMethod.Post, handler.LastRequest!.Method);
            Assert.AreEqual("/3/drive/111/files/5/directory", handler.LastRequest.RequestUri!.PathAndQuery);
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"name\":\"NewFolder\"");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"color\":\"#0098ff\"");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"only_for_me\":true");
        }

        [TestMethod]
        public async Task MoveFileTypedAsync_ShouldSendBody_AndDeserializeCancelResource()
        {
            var json = """
            {
              "result":"success",
              "data":{"cancel_id":"move-1","valid_until":1893456001}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.MoveFileAsync(50, 60, "renamed.txt", "rename");

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual("move-1", response.Data.CancelId);
            Assert.AreEqual("/3/drive/111/files/50/move/60", handler.LastRequest!.RequestUri!.PathAndQuery);
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"conflict\":\"rename\"");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"name\":\"renamed.txt\"");
        }

        [TestMethod]
        public async Task CopyFileTypedAsync_ShouldSendBody_AndDeserializeResource()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":70,"name":"copy.txt","type":"file","path":"/Private/copy.txt"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.CopyFileAsync(55, 66, "copy.txt", "version", "path");

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(70, response.Data.Id);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/files/55/copy/66?with=path");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"conflict\":\"version\"");
            StringAssert.Contains(handler.LastRequestBody ?? string.Empty, "\"name\":\"copy.txt\"");
        }

        [TestMethod]
        public async Task TrashFileTypedAsync_ShouldUseDelete_AndDeserializeCancelResource()
        {
            var json = """
            {
              "result":"success",
              "data":{"cancel_id":"trash-1","valid_until":1893456002}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.TrashFileAsync(88);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual("trash-1", response.Data.CancelId);
            Assert.AreEqual(HttpMethod.Delete, handler.LastRequest!.Method);
            Assert.AreEqual("/2/drive/111/files/88", handler.LastRequest.RequestUri!.PathAndQuery);
        }

        [TestMethod]
        public async Task GetTrashFileTypedAsync_ShouldDeserializeResource()
        {
            var json = """
            {
              "result":"success",
              "data":{"id":99,"name":"deleted.txt","type":"file","path":"/Trash/deleted.txt"}
            }
            """;
            var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var client = CreateClient(handler);
            var response = await client.GetTrashFileAsync(99, new KDriveListQuery { With = "path" });

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual("deleted.txt", response.Data.Name);
            StringAssert.Contains(handler.LastRequest!.RequestUri!.ToString(), "/3/drive/111/trash/99?with=path");
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

        private sealed class RecordingHandler(HttpResponseMessage response) : HttpMessageHandler
        {
            public HttpRequestMessage? LastRequest { get; private set; }
            public string? LastRequestBody { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                if (request.Content is not null)
                {
                    LastRequestBody = await request.Content.ReadAsStringAsync(cancellationToken);
                }
                return response;
            }
        }
    }
}
