using kDriveClient.Models;
using kDriveClient.Models.Exceptions;
using System.Text.Json;

namespace kDriveClient.Helpers
{
    /// <summary>
    /// KDriveJsonHelper provides methods to deserialize JSON responses from kDrive API.
    /// </summary>
    public static class KDriveJsonHelper
    {
        /// <summary>
        /// JSON serializer options used for deserialization.
        /// </summary>
        private static readonly JsonSerializerOptions _defaultOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Deserializes a JSON string into a KDriveResponse object.
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <returns>KDriveResponse object</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static KDriveUploadResponse DeserializeUploadResponse(string json)
        {
            return JsonSerializer.Deserialize<KDriveUploadResponseWraper>(json, _defaultOptions)?.Data ?? throw new InvalidOperationException("Failed to parse upload response");
        }

        /// <summary>
        /// Parses the start session response JSON to extract the token and upload URL.
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <returns>BaseUrl and Toekn as a tuple</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static (string Token, string UploadUrl) ParseStartSessionResponse(string json)
        {
            var root = JsonSerializer.Deserialize<KDriveResponse>(json, _defaultOptions);
            if (root == null || root.data == null)
            {
                throw new InvalidOperationException("Start session response is null or missing data");
            }

            if (!root.data.TryGetValue("token", out object? tokenProp) || !root.data.TryGetValue("upload_url", out object? urlProp))
            {
                throw new InvalidOperationException("Start session response is missing required properties");
            }

            return (
                tokenProp?.ToString() ?? throw new InvalidOperationException("Token is null"),
                urlProp?.ToString() ?? throw new InvalidOperationException("UploadUrl is null"));
        }

        public static async Task<HttpResponseMessage> DeserializeResponseAsync(HttpResponseMessage response, CancellationToken ct)
        {
            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(ct);

                KDriveErrorResponse? error = null;
                try
                {
                    error = JsonSerializer.Deserialize<KDriveErrorResponse>(json, _defaultOptions);
                }
                catch
                {
                    response.EnsureSuccessStatusCode();
                }

                if (error != null)
                {
                    throw new KDriveApiException(error);
                }
            }

            return response;
        }
    }
}