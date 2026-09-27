using kDriveClient.Helpers;
using kDriveClient.kDriveClient.Application.Api;
using kDriveClient.Models.Exceptions;
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
            var bytes = await response.Content.ReadAsByteArrayAsync(ct).ConfigureAwait(false);
            ThrowIfEnvelopeError(bytes);
            await using var stream = new MemoryStream(bytes, writable: false);
            return await JsonSerializer.DeserializeAsync(stream, typeInfo, ct).ConfigureAwait(false);
        }

        private static void ThrowIfEnvelopeError(byte[] bytes)
        {
            if (bytes.Length == 0)
                return;

            try
            {
                using var doc = JsonDocument.Parse(bytes);
                if (!doc.RootElement.TryGetProperty("result", out var resultProp))
                    return;

                var result = resultProp.GetString();
                if (!string.Equals(result, "error", StringComparison.OrdinalIgnoreCase))
                    return;

                var error = JsonSerializer.Deserialize(bytes, KDriveJsonContext.Default.KDriveErrorResponse);
                if (error is not null)
                    throw new KDriveApiException(error);

                KDriveErrorHandler.ThrowIfEnvelopeError(result);
            }
            catch (KDriveApiException)
            {
                throw;
            }
            catch (JsonException)
            {
                // Non-JSON payloads (e.g. binary) are fine.
            }
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
