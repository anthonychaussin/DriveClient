## kDriveClient SDK

![NuGet](https://img.shields.io/nuget/v/kDriveClient.svg)
![NuGet Downloads](https://img.shields.io/nuget/dt/kDriveClient.svg)
![License: MIT](https://img.shields.io/badge/license-MIT-green.svg)

`kDriveClient` is a modern C# SDK for the **Infomaniak kDrive API**:

* Automatic direct or **parallel chunked** upload (bandwidth-aware)
* Download, browse, search, trash, share links, and more
* Prefer **API v3** when both v2 and v3 exist; some features remain v2-only in the OpenAPI
* Single shared rate limiter (60 requests/min) for endpoints and uploads
* Domain interfaces (`IKDriveUpload`, `IKDriveFiles`, `IKDriveShares`, …) + DI (`AddKDriveClient`)
* Domain models (`KDriveItem` / `KDriveRemoteFile` / `KDriveDirectory`, rights enums) + typed queries
* Strong error handling and .NET logging

**Recommended surface:** typed `*Async` methods and business aliases (`GetItemAsync`, `CreateFolderAsync`, `CreateItemCommentAsync`, `TagItemAsync`, …) plus smart helpers (`GetItemsAllAsync`, `GetFavoritesAllAsync`, …). Prefer these over legacy `*V2*` routes when a v3 equivalent exists.

---

### Concepts

| Concept | C# type | Notes |
|---------|---------|--------|
| Remote file | `KDriveRemoteFile` (: `KDriveItem`) | A file already stored on the drive |
| Remote directory | `KDriveDirectory` (: `KDriveItem`) | A folder; API paths often still say `file_id` |
| Local upload payload | `KDriveFile` | Stream/path to **upload** — not a remote resource |
| OpenAPI transport DTOs | `KDriveApi*` in `Models/Generated` | Generated from OpenAPI schemas prefixed `91ac10ff_*` (FileV3, DirectoryV3, Drive, ShareLink, …) |
| Unified file/dir payload | `KDriveFileSystemItem` (: `KDriveApiFileSystemItem`) | v3 list/get transport shape; typed `Capabilities` / `ShareLink` / `Categories` when `with=` is set |
| List / search filters | `KDriveListQuery`, `KDriveSearchQuery` | Prefer over raw dictionaries; use `Includes` (`KDriveItemIncludes`) |
| Access right | `KDriveRight` | `Read`, `Write`, `Manage`, `Public`, … |
| Visibility | `KDriveVisibility` | Private / shared / team space, … |
| Collaborative ACL | `KDriveAccess` / `KDriveFileAccess` | Who can read/write inside the drive |
| Public share link | `KDriveShareLink` / `KDriveApiShareLink` | URL outsiders can open (`AccessRight` typed); OpenAPI DTO used on items with `with=sharelink` |
| Dropbox | `KDriveDropbox` / `KDriveApiDropbox` | Upload inbox on a folder — different from a share link |

Aliases such as `GetItemAsync` / `GetItemsAsync` / `GetItemsAllAsync` return domain types (`KDriveItemResult`, `KDriveItemPage`, `IReadOnlyList<KDriveItem>`). Low-level OpenAPI-named methods may still return transport DTOs (`KDriveFileSystemItem`) when you need the raw JSON shape. Domain items expose OpenAPI relation fields (`Capabilities`, `ShareLink`, `Categories`, `CreatedBy`, …) mapped from the generated `91ac10ff_*` models.

---

### Installation

```bash
dotnet add package kDriveClient
```

Optional logging provider for console samples:

```bash
dotnet add package Microsoft.Extensions.Logging.Console
```

---

### Usage

#### Upload (automatic direct/chunked selection)

Prefer `CreateAsync` when auto chunk sizing is enabled (avoids sync bandwidth probe in the constructor):

```csharp
using Microsoft.Extensions.Logging;
using kDriveClient.kDriveClient;
using kDriveClient.Models;

var logger = LoggerFactory.Create(builder => builder.AddConsole())
                          .CreateLogger<KDriveClient>();

var client = await KDriveClient.CreateAsync("your_token", your_drive_id, logger: logger);
var file = new KDriveFile
{
    Name = "example.txt",
    DirectoryPath = "/Private/test",
    Content = File.OpenRead("example.txt"),
    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
    LastModifiedAt = new DateTimeOffset(new FileInfo("example.txt").LastWriteTimeUtc).ToUnixTimeSeconds()
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
// Stream (temp file backed)
await using var stream = await client.DownloadFileAsync(fileId);
await using var fs = File.Create("downloaded.txt");
await stream.CopyToAsync(fs);

// Directly to a path
await client.DownloadFileAsync(fileId, "downloaded_direct.txt");
```

---

#### Browse a folder (v3)

```csharp
using kDriveClient.Models;
using kDriveClient.Models.Domain;

var page = await client.GetItemsAsync(directoryId, new KDriveListQuery
{
    Limit = 50,
    Includes = KDriveItemIncludes.Path
});

foreach (var item in page?.Items ?? [])
{
    if (item is KDriveDirectory dir)
        Console.WriteLine($"dir  {dir.Id} {dir.Name}");
    else if (item is KDriveRemoteFile file)
        Console.WriteLine($"file {file.Id} {file.Name} ({file.MimeType})");
}
```

Load every page automatically:

```csharp
var all = await client.GetItemsAllAsync(directoryId, pageSize: 100);
```

---

#### Search (v3)

```csharp
var results = await client.SearchItemsAsync(new KDriveSearchQuery
{
    Query = "report",
    DirectoryId = directoryId,
    Limit = 25
});
```

```csharp
var allMatches = await client.SearchItemsAllAsync("report", directoryId: directoryId);
```

---

#### Create, rename, move, trash (v3 / v2 where required)

```csharp
var folder = await client.CreateFolderAsync(parentDirectoryId, "Projects");
var folderId = folder!.Item!.Id;

await client.RenameItemAsync(folderId, "Projects-2026");
await client.MoveItemAsync(fileId, folderId);
await client.TrashItemAsync(fileId);

// Restore from trash (v2-only in OpenAPI)
await client.RestoreTrashItemAsync(fileId, destinationDirectoryId: folderId);
```

---

#### Favorites and share links (v2-only)

```csharp
using kDriveClient.Models;
using kDriveClient.Models.Domain;

await client.FavoriteItemAsync(fileId);

var link = await client.CreateItemShareLinkAsync(fileId, new KDriveShareLinkRequest
{
    AccessRight = KDriveRight.Public,
    CanDownload = true
});
Console.WriteLine(link?.Data?.Url);

var existing = await client.GetItemShareLinkAsync(fileId);
await client.UpdateItemShareLinkAsync(fileId, new KDriveShareLinkRequest { CanDownload = false });
await client.InviteItemShareLinkAsync(fileId, emails: new[] { "user@example.com" });
await client.DeleteItemShareLinkAsync(fileId);
```

---

#### Comments, categories, dropbox, access (v2)

```csharp
await client.CreateItemCommentAsync(fileId, "Looks good");
await client.ReplyItemCommentAsync(fileId, commentId, "Thanks");
var comments = await client.GetItemCommentsAllAsync(fileId);

var category = await client.CreateDriveCategoryAsync("Urgent", color: "#ff0000");
await client.TagItemAsync(fileId, category!.Data!.Id!.Value);

await client.EnableItemDropboxAsync(fileId);
await client.ShareItemWithUsersAsync(fileId, new KDriveFileAccessUsersRequest
{
    UserIds = new[] { collaboratorUserId },
    AccessRight = KDriveRight.Write,
    Message = "Please review"
});
```

---

#### Smart pagination helpers

```csharp
var users = await client.GetAllDriveUsersV3Async(pageSize: 100);
var trashHits = await client.SearchTrashAllAsync("invoice");
var versions = await client.GetItemVersionsAllAsync(fileId);
var favorites = await client.GetFavoritesAllAsync();
var dropboxes = await client.GetDropboxesAllAsync();
var categories = await client.ListCategoriesAllAsync();
```

---

### Which API should I call?

| Goal | Prefer | Avoid unless needed |
|------|--------|---------------------|
| Day-to-day files | `GetItemAsync`, `GetItemsAsync`, `CreateFolderAsync`, `TrashItemAsync`, … | Low-level OpenAPI verb names when an alias exists |
| Share / comments / tags | `CreateItemShareLinkAsync`, `CreateItemCommentAsync`, `TagItemAsync`, … | — |
| Auto pagination / search | `GetItemsAllAsync`, `SearchItemsAllAsync`, `EnumerateItemsAsync`, `GetOrCreateFolderAsync`, `BootstrapAsync` | Manual cursor loops |
| Upload | `UploadAsync`, `UploadBatchAsync` | Manual session/chunk calls |
| Download | `DownloadFileAsync` (+ `KDriveDownloadOptions` for `as`/password/temporary URL) | — |
| Admin / uncommon ops | Business aliases (`ListInvitationsAsync`, `GetActivityStatisticsAsync`, …) | — |
| Legacy duplicate routes | — | `*V2*` (obsolete when v3 exists) |

When both v2 and v3 exist (versions, activities, upload cancel, trash child count), public aliases use **v3**. The two OpenAPI v2-only cancel routes (`DELETE /2/.../upload/session/{token}` and `.../batch`) are intentionally not exposed; use the v3 cancel APIs instead.

**Model layers:** generated OpenAPI DTOs (`Models/Generated/KDriveApi*`) are the transport source of truth; `KDriveItem` / `KDriveItemPage` / `KDriveItemResult` are the stable domain surface returned by business aliases.

---

### Dependency injection

```csharp
using kDriveClient.Extensions;

services.AddKDriveClient(options =>
{
    options.Token = "your_token";
    options.DriveId = your_drive_id;
    options.Upload = new KDriveUploadOptions { Parallelism = 8 };
});

// Inject IKDriveClient, IKDriveUpload, IKDriveFiles, IKDriveShares, …
```

Domain interfaces (all implemented by `KDriveClient`):

* `IKDriveUpload` / `IKDriveDownload` — file transfer
* `IKDriveFiles` — browse, CRUD, trash, versions
* `IKDriveShares` — share links, ACL, dropbox
* `IKDriveComments` / `IKDriveCategories`
* `IKDriveDrive` — drive admin, users, stats, imports
* `IKDriveSmart` — pagination / wake helpers
* `IKDriveClient` — aggregate of the above

---

### Advanced features

**Custom upload options / parallelism**

`Parallelism` controls how many chunks are uploaded concurrently (and feeds auto chunk-size estimation):

```csharp
var client = await KDriveClient.CreateAsync(
    "your_token",
    your_drive_id,
    new KDriveUploadOptions { Parallelism = 8, UseAutoChunkSize = true });

client.Progress = new Progress<double>(p => Console.WriteLine($"{p:P0}"));
```

**Inject your own `HttpClient` (testing)**

```csharp
var http = new HttpClient(new FakeHandler()) { BaseAddress = new Uri("https://api.infomaniak.com") };
var client = new KDriveClient("your_token", your_drive_id, httpClient: http);
```

**API errors**

```csharp
catch (KDriveApiException ex)
{
    Console.WriteLine($"Error: {ex.Error.Result}, Code: {ex.Error.Error.Code}, Description: {ex.Error.Error.Description}");
}
```

---

### Features

* Automatic direct or parallel chunked upload based on speed test (optional `KDriveUploadHashAlgorithm.XxHash3`)
* Dynamic chunk size calculation (auto min 1 MiB; prefer direct under ~100 MiB)
* Batch upload orchestration via `UploadBatchAsync`
* Single shared HTTP pipeline + rate limit (60 req/min)
* Typed endpoints with business aliases and domain interfaces
* Smart helpers: cursor aggregation, `EnumerateItemsAsync`, `GetOrCreateFolderAsync`, `BootstrapAsync`, async polling
* Download with options (`as=pdf|text`, password, temporary URL) and temp-stream cleanup
* `CreateAsync` factory and `AddKDriveClient` DI registration
* Deserialized error responses
* Native logger support (`Microsoft.Extensions.Logging`)
* Testable via custom `HttpClient`

The `ConsoleApp1` sample is an optional E2E harness. Set `KDRIVE_TOKEN`, `KDRIVE_DRIVE_ID`, and optionally `KDRIVE_ACCOUNT_ID` / `KDRIVE_PARENT_DIR_ID` — never commit tokens.

---

### Breaking changes (2.0.0)

* Removed `KDriveFileUploader` (use `UploadAsync` / `UploadFileChunkedAsync`)
* Removed Newtonsoft.Json dependency
* Renamed `KDriveUploadResponseWraper` → `KDriveUploadResponseWrapper`
* Sync constructor no longer blocks on bandwidth probe when `UseAutoChunkSize` is true (lazy on first upload); prefer `CreateAsync`

---

### Contributing

PRs and issues welcome.

---

### License

MIT

---
