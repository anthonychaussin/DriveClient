using System.Text;
using kDriveClient.Models;

namespace kDriveClientTests
{
    [TestClass()]
    public class KDriveFileTests
    {
        [TestMethod()]
        [DataRow(2000, 2)]
        [DataRow(2001, 3)]
        [DataRow(2500, 3)]
        [DataRow(500, 1)]
        public void SplitIntoChunksTest(int totalSize, int nbChunk)
        {
            var testContent = Encoding.UTF8.GetBytes(new string('A', totalSize));
            using var ms = new MemoryStream(testContent);

            var file = new KDriveFile
            {
                Name = "test.txt",
                DirectoryId = "dir1",
                DirectoryPath = "/test",
                Content = ms,
                CreatedAt = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LastModifiedAt = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            file.SplitIntoChunks(1000);
            Assert.AreEqual(nbChunk, file.Chunks.Count);
            Assert.AreEqual(totalSize, file.TotalSize);

            foreach (var chunk in file.Chunks)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(chunk.ChunkHash));
                Assert.AreEqual(chunk.Content.Length, chunk.ChunkSize);
            }

            Assert.IsFalse(string.IsNullOrWhiteSpace(file.TotalChunkHash));
        }
    }
}