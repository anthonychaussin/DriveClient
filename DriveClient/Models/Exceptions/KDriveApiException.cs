namespace kDriveClient.Models.Exceptions
{
    /// <summary>
    /// KDriveApiException is thrown when the kDrive API returns an error response.
    /// </summary>
    /// <param name="error">The error response from the kDrive API.</param>
    public class KDriveApiException(KDriveErrorResponse error) : Exception($"{error.Error.Code}: {error.Error.Description}")
    {
        /// <summary>
        /// Error contains the details of the error returned by the kDrive API.
        /// </summary>
        public KDriveErrorResponse Error { get; } = error;
    }
}