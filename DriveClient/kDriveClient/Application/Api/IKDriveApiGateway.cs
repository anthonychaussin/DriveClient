using System.Text.Json.Serialization.Metadata;

namespace kDriveClient.kDriveClient.Application.Api
{
    public interface IKDriveApiGateway
    {

        Task<T?> SendTypedAsync<T>(HttpMethod method, string path, object? body, JsonTypeInfo<T> typeInfo, CancellationToken ct);
    }
}
