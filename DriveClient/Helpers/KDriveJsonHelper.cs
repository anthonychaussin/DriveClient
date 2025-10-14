using kDriveClient.Models;
using kDriveClient.Models.Exceptions;

namespace kDriveClient.Helpers
{
    /// <summary>
    /// KDriveJsonHelper provides methods to deserialize JSON responses from kDrive API.
    /// </summary>
    public static class KDriveJsonHelper
    {
        /// <summary>
        /// Deserializes a JSON string into a KDriveResponse object.
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <returns>KDriveResponse object</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static KDriveUploadResponse DeserializeUploadResponse(string json)
        {
            return JsonSerializer.Deserialize(json, KDriveJsonContext.Default.KDriveUploadResponseWraper)?.Data?.File ?? throw new InvalidOperationException("Failed to parse upload response");
        }

        /// <summary>
        /// Parses the start session response JSON to extract the token and upload URL.
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <returns>BaseUrl and Toekn as a tuple</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static (string Token, string UploadUrl) ParseStartSessionResponse(string json)
        {
            var root = JsonSerializer.Deserialize(json, KDriveJsonContext.Default.KDriveResponse);
            if (root == null || root.Data == null)
            {
                throw new InvalidOperationException("Start session response is null or missing Data");
            }

            if (!root.Data.TryGetValue("token", out object? tokenProp) || !root.Data.TryGetValue("upload_url", out object? urlProp))
            {
                throw new InvalidOperationException("Start session response is missing required properties");
            }

            return (
                tokenProp?.ToString() ?? throw new InvalidOperationException("Token is null"),
                urlProp?.ToString() ?? throw new InvalidOperationException("UploadUrl is null"));
        }

        /// <summary>
        /// Deserializes the response from the kDrive API and throws an exception if the response indicates an error.
        /// </summary>
        /// <param name="response">The HTTP response message to deserialize.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized HTTP response message.</returns>
        /// <exception cref="KDriveApiException">Thrown when the response indicates an error.</exception>
        public static async Task<HttpResponseMessage> DeserializeResponseAsync(HttpResponseMessage response, CancellationToken ct)
        {
            if (!response.IsSuccessStatusCode)
            {
                KDriveErrorResponse? error = null;
                try
                {
                    await using var stream = await response.Content.ReadAsStreamAsync(ct);
                    error = await JsonSerializer.DeserializeAsync( stream, KDriveJsonContext.Default.KDriveErrorResponse, cancellationToken: ct);
                }
                catch
                {
                    response.EnsureSuccessStatusCode();
                }

                if (error is not null)
                {
                    throw new KDriveApiException(error);
                }
            }

            return response;
        }
    }
}