using kDriveClient.Models.Exceptions;

namespace kDriveClient.Helpers
{
    /// <summary>
    /// KDriveErrorHandler provides methods to handle API errors from kDrive.
    /// </summary>
    public static class KDriveErrorHandler
    {
        /// <summary>
        /// HandleApiErrorAsync checks the HTTP response for errors and throws a KDriveApiException if an error is found.
        /// </summary>
        /// <param name="response">Response from the kDrive API</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Task</returns>
        /// <exception cref="KDriveApiException">KDriveApiException is thrown when an error is found in the response</exception>
        public static async Task HandleApiErrorAsync(HttpResponseMessage response, CancellationToken ct)
        {
            if (response.IsSuccessStatusCode)
                return;

            KDriveErrorResponse? error = null;
            try
            {
                await using var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
                error = await JsonSerializer.DeserializeAsync(stream, KDriveJsonContext.Default.KDriveErrorResponse, cancellationToken: ct)
                    .ConfigureAwait(false);
            }
            catch
            {
                // Fall through to generic failure below.
            }

            if (error is not null && !string.IsNullOrWhiteSpace(error.Error?.Code))
                throw new KDriveApiException(error);

            throw new KDriveApiException(new KDriveErrorResponse
            {
                Result = "error",
                Error = new KDriveErrorDetail
                {
                    Code = $"http_{(int)response.StatusCode}",
                    Description = response.ReasonPhrase ?? response.StatusCode.ToString()
                }
            });
        }

        /// <summary>
        /// Throws when a typed envelope reports <c>result == "error"</c> (even on HTTP 200).
        /// </summary>
        public static void ThrowIfEnvelopeError(string? result, string? code = null, string? description = null)
        {
            if (!string.Equals(result, "error", StringComparison.OrdinalIgnoreCase))
                return;

            throw new KDriveApiException(new KDriveErrorResponse
            {
                Result = "error",
                Error = new KDriveErrorDetail
                {
                    Code = string.IsNullOrWhiteSpace(code) ? "api_error" : code,
                    Description = description ?? "The API returned result=error."
                }
            });
        }
    }
}
