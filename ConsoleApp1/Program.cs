using kDriveClient.kDriveClient;
using kDriveClient.Models;

var client = new KDriveClient("toekn", 0000);
var file = new KDriveFile
{
    Name = "example.txt",
    DirectoryPath = "/Private/test",
    Content = File.OpenRead("example.txt"),
    CreatedAt = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
    LastModifiedAt = (int)new FileInfo("example.txt").LastWriteTimeUtc.Subtract(DateTime.UnixEpoch).TotalSeconds
};

var response = await client.UploadAsync(file);
Console.WriteLine($"Uploaded file ID: {response.Id}");

var stream = await client.DownloadFileAsync(response.Id);

Console.WriteLine($"File downloaded");

using var fs = File.Create("downloaded.txt");
await stream.CopyToAsync(fs);

Console.WriteLine($"File saved to {fs.Name}");

using var outFile = File.Create("downloaded_direct.txt");
await client.DownloadFileAsync(response.Id, outFile);

Console.WriteLine($"File downloaded and saved to {fs.Name}");