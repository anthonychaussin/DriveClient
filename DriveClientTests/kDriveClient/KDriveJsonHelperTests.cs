using kDriveClient.Helpers;

namespace kDriveClientTests.kDriveClient
{
    [TestClass]
    public class KDriveJsonHelperTests
    {
        [TestMethod]
        public void DeserializeUploadResponse_ShouldParse_WrapperWithFile()
        {
            var json = """
                       {"result":"success","data":{"file":{"id":101,"name":"a.txt","directory_id":1,"size":12,"type":"file","status":"ok","visibility":"is_visible"}}}
                       """;

            var response = KDriveJsonHelper.DeserializeUploadResponse(json);

            Assert.AreEqual(101, response.Id);
            Assert.AreEqual("a.txt", response.Name);
        }

        [TestMethod]
        public void DeserializeUploadResponse_ShouldParse_ResourceDataDirect()
        {
            var json = """
                       {"result":"success","data":{"id":202,"name":"b.txt","directory_id":2,"size":34,"type":"file","status":"ok","visibility":"is_visible"}}
                       """;

            var response = KDriveJsonHelper.DeserializeUploadResponse(json);

            Assert.AreEqual(202, response.Id);
            Assert.AreEqual("b.txt", response.Name);
        }

        [TestMethod]
        public void DeserializeUploadResponse_ShouldParse_RawUploadObject()
        {
            var json = """
                       {"id":303,"name":"c.txt","directory_id":3,"size":56,"type":"file","status":"ok","visibility":"is_visible"}
                       """;

            var response = KDriveJsonHelper.DeserializeUploadResponse(json);

            Assert.AreEqual(303, response.Id);
            Assert.AreEqual("c.txt", response.Name);
        }
    }
}
