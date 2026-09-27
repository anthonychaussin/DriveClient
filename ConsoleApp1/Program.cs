using kDriveClient.kDriveClient;
using kDriveClient.Models;
using kDriveClient.Models.Domain;
using kDriveClient.Models.Exceptions;
using System.Diagnostics;
using System.Globalization;

// Local E2E sample. Credentials come from environment / user-secrets — never hardcode tokens.
var token = Environment.GetEnvironmentVariable("KDRIVE_TOKEN")
            ?? throw new InvalidOperationException("Set KDRIVE_TOKEN environment variable (or user-secrets) before running.");
var driveId = long.Parse(Environment.GetEnvironmentVariable("KDRIVE_DRIVE_ID") ?? "0", CultureInfo.InvariantCulture);
var testRootParentDirectoryId = long.Parse(Environment.GetEnvironmentVariable("KDRIVE_PARENT_DIR_ID") ?? "5", CultureInfo.InvariantCulture);
var accountIdEnv = Environment.GetEnvironmentVariable("KDRIVE_ACCOUNT_ID");
long? accountId = long.TryParse(accountIdEnv, out var aid) ? aid : null;
var testRootFolderName = "kDriveClient-E2E-Test";
var localInputFilePath = Path.Combine(AppContext.BaseDirectory, "example.txt");
var localTestDirectory = Path.Combine(AppContext.BaseDirectory, "test");
var cleanupRemoteAtEnd = true;

if (driveId <= 0 || testRootParentDirectoryId <= 0)
{
    Console.Error.WriteLine("Set KDRIVE_DRIVE_ID and optionally KDRIVE_PARENT_DIR_ID / KDRIVE_ACCOUNT_ID.");
    return;
}

if (!File.Exists(localInputFilePath))
{
    Console.Error.WriteLine($"Input file not found: {localInputFilePath}");
    return;
}

Directory.CreateDirectory(localTestDirectory);

var uploadOptions = new KDriveUploadOptions
{
    UseAutoChunkSize = false,
    ChunkSize = 1024 * 1024 * 8,
    Parallelism = 4
};

var client = await KDriveClient.CreateAsync(token, driveId, uploadOptions);
var results = new List<EndpointCheck>();
long? uploadedFileId = null;
long? runFolderId = null;
long? testRootFolderId = null;
var localDownloadedPath = Path.Combine(localTestDirectory, "downloaded.txt");

try
{
    if (accountId is > 0)
    {
        await RunEndpointAsync(results, "BootstrapAsync", async () =>
        {
            var ctx = await client.BootstrapAsync(accountId.Value, preferredDriveId: driveId);
            Console.WriteLine($"Bootstrap selected drive {ctx.DriveId} ({ctx.Drives.Count} drives).");
            return ctx;
        });
    }

    var testRoot = await client.GetOrCreateFolderAsync(testRootParentDirectoryId, testRootFolderName);
    testRootFolderId = testRoot.Id;

    var runFolder = await client.GetOrCreateFolderAsync(testRoot.Id, $"run-{DateTime.UtcNow:yyyyMMdd-HHmmss}");
    runFolderId = runFolder.Id;

    using var uploadFile = new KDriveFile(localInputFilePath, "/");
    uploadFile.DirectoryId = runFolderId.Value.ToString(CultureInfo.InvariantCulture);

    await RunEndpointNoResultAsync(results, "UploadAsync", async () =>
    {
        var uploaded = await client.UploadAsync(uploadFile);
        uploadedFileId = uploaded.Id;
        Console.WriteLine($"Uploaded file id: {uploaded.Id}");
    });

    if (uploadedFileId is null || runFolderId is null)
        throw new InvalidOperationException("Setup failed: uploaded file id or run folder id is missing.");

    var listQuery = new KDriveListQuery { Limit = 50, Includes = KDriveItemIncludes.Path };

    await RunEndpointNoResultAsync(results, "DownloadFileAsync", async () =>
    {
        await client.DownloadFileAsync(uploadedFileId.Value, localDownloadedPath);
    });

    await RunEndpointAsync(results, "GetItemAsync", () => client.GetItemAsync(uploadedFileId.Value, "path"));
    await RunEndpointAsync(results, "GetItemsAsync", () => client.GetItemsAsync(runFolderId.Value, listQuery));
    await RunEndpointAsync(results, "GetRecentAsync", () => client.GetRecentAsync(listQuery));
    await RunEndpointAsync(results, "GetFavoritesAsync", () => client.GetFavoritesAsync(listQuery));
    await RunEndpointAsync(results, "SearchItemsAsync", () => client.SearchItemsAsync(new KDriveSearchQuery { Query = "example", Limit = 20 }));
    await RunEndpointAsync(results, "GetTrashItemsAsync", () => client.GetTrashItemsAsync(listQuery));

    await RunEndpointNoResultAsync(results, "EnumerateItemsAsync", async () =>
    {
        var count = 0;
        await foreach (var _ in client.EnumerateItemsAsync(runFolderId.Value, listQuery, pageSize: 50))
            count++;
        Console.WriteLine($"Enumerated {count} items.");
    });

    var copy = await RunEndpointAsync(results, "CopyItemAsync", () =>
        client.CopyItemAsync(uploadedFileId.Value, runFolderId.Value, "copy-e2e.txt", conflict: "rename"));
    if (copy?.Item?.Id is long copyId)
        await RunEndpointAsync(results, "TrashItemAsync", () => client.TrashItemAsync(copyId));
}
finally
{
    PrintSummary(results);

    if (cleanupRemoteAtEnd)
    {
        if (uploadedFileId is not null)
            await SafeTrashAsync(client, uploadedFileId.Value, "uploaded file");
        if (runFolderId is not null)
            await SafeTrashAsync(client, runFolderId.Value, "run folder");
    }
}

static async Task<T?> RunEndpointAsync<T>(ICollection<EndpointCheck> results, string name, Func<Task<T>> action)
{
    var sw = Stopwatch.StartNew();
    try
    {
        var value = await action();
        sw.Stop();
        results.Add(new EndpointCheck(name, true, $"OK ({sw.ElapsedMilliseconds} ms)"));
        Console.WriteLine($"[OK] {name} ({sw.ElapsedMilliseconds} ms)");
        return value;
    }
    catch (Exception ex)
    {
        sw.Stop();
        var detail = BuildExceptionDetail(ex);
        results.Add(new EndpointCheck(name, false, detail));
        Console.WriteLine($"[ERR] {name} => {detail}");
        return default;
    }
}

static Task RunEndpointNoResultAsync(ICollection<EndpointCheck> results, string name, Func<Task> action)
    => RunEndpointAsync<object?>(results, name, async () => { await action(); return null; });

static void PrintSummary(IEnumerable<EndpointCheck> results)
{
    var entries = results.ToList();
    var ok = entries.Count(x => x.Success);
    Console.WriteLine();
    Console.WriteLine($"========== Endpoint Test Summary ==========");
    Console.WriteLine($"Total: {entries.Count} | Success: {ok} | Failed: {entries.Count - ok}");
    foreach (var item in entries.Where(x => !x.Success))
        Console.WriteLine($" - {item.Name}: {item.Detail}");
}

static string BuildExceptionDetail(Exception ex)
{
    if (ex is KDriveApiException apiEx)
        return $"{apiEx.Error.Error.Code}: {apiEx.Error.Error.Description}";
    return $"{ex.GetType().Name}: {ex.Message}";
}

static async Task SafeTrashAsync(KDriveClient client, long itemId, string label)
{
    try
    {
        await client.TrashItemAsync(itemId);
        Console.WriteLine($"Cleanup: trashed {label} (id: {itemId}).");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Cleanup warning for {label}: {BuildExceptionDetail(ex)}");
    }
}

readonly record struct EndpointCheck(string Name, bool Success, string Detail);
