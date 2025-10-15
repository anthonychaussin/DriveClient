---

## 🚀 **kDriveClient SDK**

![NuGet](https://img.shields.io/nuget/v/kDriveClient.svg)
![NuGet Downloads](https://img.shields.io/nuget/dt/kDriveClient.svg)
![License: MIT](https://img.shields.io/badge/license-MIT-green.svg)

`kDriveClient` is a modern C# SDK to simplify file upload and download with **Infomaniak kDrive API**, including:

* Automatic direct or chunked upload
* Dynamic chunk size based on your actual bandwidth
* Download with redirect support
* Built-in rate limiting (60 requests/min)
* Strong error handling (deserialized API errors)
* Native .NET logging support

---

### 📦 **Installation**

```bash
dotnet add package kDriveClient
dotnet add package Microsoft.Extensions.Logging.Console
```

---

### ⚡ **Usage**

#### Upload (automatic direct/chunked selection)

```csharp
using Microsoft.Extensions.Logging;

var logger = LoggerFactory.Create(builder => builder.AddConsole())
                          .CreateLogger<KDriveClient>();

var client = new KDriveClient("your_token", "your_drive_id", logger: logger);
var file = new KDriveFile
{
    Name = "example.txt",
    DirectoryPath = "/Private/test",
    Content = File.OpenRead("example.txt"),
    CreatedAt = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
    LastModifiedAt = (int)new FileInfo("example.txt").LastWriteTimeUtc.Subtract(DateTime.UnixEpoch).TotalSeconds
};

try
{
    var response = await client.UploadAsync(file);
    Console.WriteLine($"Uploaded file ID: {response.Id}");
}
catch (KDriveApiException ex)
{
    Console.WriteLine($"API error: {ex.Error}");
}
```

---

#### Download

```csharp
// Into memory
var stream = await client.DownloadFileAsync("file_id");

Console.WriteLine($"File downloaded");

using var fs = File.Create("downloaded.txt");
await stream.CopyToAsync(fs);

Console.WriteLine($"File saved to {fs.Name}");

// Directly into a file
using var outFile = File.Create("downloaded_direct.txt");
await client.DownloadFileAsync("file_id", outFile);

Console.WriteLine($"File downloaded and saved to {fs.Name}");
```

---

### ⚙ **Advanced features**

✅ **Custom parallelism**

```csharp
var client = new KDriveClient("your_token", "your_drive_id", new KDriveClientOptions { Parallelism = 8});
```

✅ **Inject your own `HttpClient` (for testing/mocking)**

```csharp
var fakeHttpClient = new HttpClient(new FakeHandler()) { BaseAddress = new Uri("https://api.infomaniak.com") };
var client = new KDriveClient("your_token", "your_drive_id", httpClient: fakeHttpClient);
```

✅ **Built-in error object**

```csharp
catch (KDriveApiException ex)
{
    Console.WriteLine($"Error: {ex.Error.Result}, Code: {ex.Error.Error.Code}, Description: {ex.Error.Error.Description}");
}
```

---

### ✅ **Features**

* Automatic direct or chunked upload mode based on speed test
* Dynamic chunk size calculation
* Download with redirect support
* Rate limit of 60 requests/min
* Deserialized error responses (no manual parsing)
* Native logger support (Microsoft.Extensions.Logging)
* Testable: inject your own `HttpClient`

---

### 🤝 **Contributing**

PRs and issues welcome! Let’s improve this SDK together.

---

### 📄 **License**

MIT

---