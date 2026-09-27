using System.Text.Json;
using kDriveClient.Helpers;
using kDriveClient.Models;
using kDriveClient.Models.Domain;
using kDriveClient.Models.Generated;

namespace kDriveClientTests.kDriveClient
{
    [TestClass]
    public class KDriveOpenApiPropsTests
    {
        private const string FileWithIncludesJson = """
            {
              "result": "success",
              "data": {
                "id": 42,
                "name": "report.pdf",
                "path": "/Private/report.pdf",
                "type": "file",
                "status": "ok",
                "visibility": "is_in_private_space",
                "drive_id": 7,
                "parent_id": 1,
                "depth": 2,
                "size": 1024,
                "mime_type": "application/pdf",
                "created_by": 99,
                "created_at": 1700000000,
                "updated_at": 1700001000,
                "last_modified_at": 1700001000,
                "scan_status": "ok",
                "capabilities": {
                  "can_read": true,
                  "can_write": true,
                  "can_share": false,
                  "can_show": true,
                  "can_use_favorite": true,
                  "can_become_sharelink": true
                },
                "sharelink": {
                  "url": "https://kdrive.infomaniak.com/app/share/xxxx/yyyy",
                  "file_id": 42,
                  "right": "public",
                  "created_by": 99,
                  "created_at": 1700000000,
                  "updated_at": 1700000000,
                  "valid_until": null,
                  "access_blocked": false,
                  "capabilities": {
                    "can_edit": false,
                    "can_see_stats": true,
                    "can_see_info": true,
                    "can_download": true,
                    "can_comment": false,
                    "can_request_access": false
                  }
                },
                "categories": [
                  {
                    "category_id": 5,
                    "added_at": 1700000000,
                    "user_validation": "CORRECT",
                    "is_generated_by_ai": false,
                    "user_id": 99,
                    "category": {
                      "id": 5,
                      "name": "Contracts",
                      "color": "#112233"
                    }
                  }
                ]
              }
            }
            """;

        [TestMethod]
        public void ResourceResponse_WithCapabilitiesShareLinkCategories_DeserializesTypedProps()
        {
            var response = JsonSerializer.Deserialize(
                FileWithIncludesJson,
                KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem);

            Assert.IsNotNull(response);
            Assert.AreEqual("success", response!.Result);
            Assert.IsNotNull(response.Data);

            var item = response.Data!;
            Assert.AreEqual(42, item.Id);
            Assert.AreEqual("report.pdf", item.Name);

            Assert.IsNotNull(item.Capabilities);
            Assert.IsTrue(item.Capabilities!.CanRead);
            Assert.IsTrue(item.Capabilities.CanWrite);
            Assert.IsFalse(item.Capabilities.CanShare);

            Assert.IsNotNull(item.ShareLink);
            Assert.AreEqual("https://kdrive.infomaniak.com/app/share/xxxx/yyyy", item.ShareLink!.Url);
            Assert.AreEqual("public", item.ShareLink.Right);
            Assert.IsNotNull(item.ShareLink.Capabilities);
            Assert.IsTrue(item.ShareLink.Capabilities!.CanDownload);

            Assert.IsNotNull(item.Categories);
            Assert.AreEqual(1, item.Categories!.Count);
            Assert.AreEqual(5, item.Categories[0].CategoryId);
            Assert.AreEqual("Contracts", item.Categories[0].Category?.Name);

            // Known OpenAPI fields must not only live in ExtraData
            Assert.IsTrue(
                item.ExtraData is null
                || (!item.ExtraData.ContainsKey("capabilities")
                    && !item.ExtraData.ContainsKey("sharelink")
                    && !item.ExtraData.ContainsKey("categories")));
        }

        [TestMethod]
        public void KDriveItem_From_MapsOpenApiRelationProps()
        {
            var response = JsonSerializer.Deserialize(
                FileWithIncludesJson,
                KDriveJsonContext.Default.KDriveResourceResponseKDriveFileSystemItem);

            var domain = KDriveItem.From(response!.Data);
            Assert.IsNotNull(domain);
            Assert.IsInstanceOfType(domain, typeof(KDriveRemoteFile));
            Assert.AreEqual(42, domain!.Id);
            Assert.AreEqual(99, domain.CreatedBy);
            Assert.AreEqual("ok", domain.ScanStatus);

            Assert.IsNotNull(domain.Capabilities);
            Assert.IsTrue(domain.Capabilities!.CanRead);
            Assert.IsNotNull(domain.ShareLink);
            Assert.AreEqual(42, domain.ShareLink!.FileId);
            Assert.AreEqual(1, domain.Categories.Count);
            Assert.AreEqual(5, domain.Categories[0].CategoryId);
            Assert.AreEqual("Contracts", domain.Categories[0].Category?.Name);
        }

        [TestMethod]
        public void OpenApiFileV3_RoundTripsThroughDomainMapping()
        {
            var api = new KDriveApiFileV3
            {
                Id = 10,
                Name = "a.txt",
                Type = "file",
                Capabilities = new KDriveApiFileV3Capabilities { CanRead = true, CanWrite = false },
                ShareLink = new KDriveApiShareLink { Url = "https://example.test/s", FileId = 10, Right = "public" },
                Categories =
                [
                    new KDriveApiFileCategory
                    {
                        CategoryId = 1,
                        Category = new KDriveApiCategory { Id = 1, Name = "Tag" }
                    }
                ]
            };

            var transport = KDriveDomainMapping.ToFileSystemItem(api);
            Assert.AreEqual(10, transport.Id);
            Assert.IsNotNull(transport.Capabilities);
            Assert.IsTrue(transport.Capabilities!.CanRead);
            Assert.IsNotNull(transport.ShareLink);
            Assert.AreEqual(1, transport.Categories?.Count);

            var item = KDriveDomainMapping.ToItem(api);
            Assert.IsNotNull(item);
            Assert.AreEqual(10, item!.Id);
            Assert.IsNotNull(item.Capabilities);
            Assert.IsNotNull(item.ShareLink);
            Assert.AreEqual(1, item.Categories.Count);
            Assert.AreEqual(1, item.Categories[0].CategoryId);
            Assert.AreEqual("Tag", item.Categories[0].Category?.Name);
        }

        [TestMethod]
        public void DirectoryV3_DeserializesDropboxColor()
        {
            var json = """
                {
                  "id": 3,
                  "name": "Inbox",
                  "type": "dir",
                  "color": "#ffaa00",
                  "capabilities": { "can_read": true, "can_write": true },
                  "dropbox": { "id": 9, "url": "https://example.test/drop", "name": "Drop" }
                }
                """;

            var dir = JsonSerializer.Deserialize(json, KDriveOpenApiJsonContext.Default.KDriveApiDirectoryV3);
            Assert.IsNotNull(dir);
            Assert.AreEqual("#ffaa00", dir!.Color);
            Assert.IsNotNull(dir.Capabilities);
            Assert.IsNotNull(dir.Dropbox);
            Assert.AreEqual(9, dir.Dropbox!.Id);

            var domain = KDriveDomainMapping.ToItem(dir);
            Assert.IsInstanceOfType(domain, typeof(KDriveDirectory));
            Assert.AreEqual("#ffaa00", domain!.Color);
            Assert.IsNotNull(domain.Dropbox);
        }
    }
}
