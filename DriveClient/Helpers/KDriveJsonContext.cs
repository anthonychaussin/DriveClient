using kDriveClient.Models;
using kDriveClient.Models.Exceptions;
namespace kDriveClient.Helpers
{
    /// <summary>
    /// KDriveJsonContext is a custom JSON serialization context for the kDrive client.
    /// </summary>
    [JsonSerializable(typeof(KDriveErrorResponse))]
    [JsonSerializable(typeof(KDriveErrorDetail))]
    [JsonSerializable(typeof(KDriveResponse))]
    [JsonSerializable(typeof(KDriveFile))]
    [JsonSerializable(typeof(KDriveChunk))]
    [JsonSerializable(typeof(KDriveUploadResponseWraper))]
    [JsonSerializable(typeof(KDriveFinishRequest))]
    [JsonSerializable(typeof(KDriveUploadDataResponse))]
    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
    public partial class KDriveJsonContext : JsonSerializerContext
    {
    }
}
