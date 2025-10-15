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
            if (!response.IsSuccessStatusCode)
            {
                KDriveErrorResponse? error = null;
                try
                {
                    await using var stream = await response.Content.ReadAsStreamAsync(ct);
                    error = await JsonSerializer.DeserializeAsync(stream, KDriveJsonContext.Default.KDriveErrorResponse, cancellationToken: ct);
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
        }
    }
}