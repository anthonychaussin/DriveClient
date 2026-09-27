using kDriveClient.Helpers;
using kDriveClient.kDriveClient.Application.Api;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace kDriveClient.kDriveClient.Infrastructure.Api
{
    public sealed class HttpKDriveApiGateway : IKDriveApiGateway
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sender;

        public HttpKDriveApiGateway(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sender)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }


        public async Task<T?> SendTypedAsync<T>(HttpMethod method, string path, object? body, JsonTypeInfo<T> typeInfo, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(method, path);
            if (body is not null)
            {
                request.Content = CreateJsonContent(body);
            }

            var response = await _sender(request, ct).ConfigureAwait(false);
            await using var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync(stream, typeInfo, ct).ConfigureAwait(false);
        }

        private static StringContent CreateJsonContent(object body)
        {
            var json = JsonSerializer.Serialize(body, body.GetType(), KDriveJsonContext.Default);
            return new StringContent(
                json,
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
        }
    }
}
