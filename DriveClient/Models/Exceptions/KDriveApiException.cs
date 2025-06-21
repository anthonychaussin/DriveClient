namespace kDriveClient.Models.Exceptions
{
    /// KDriveApiException constructor initializes a new instance of the KDriveApiException class with the specified error details.
    /// </remarks>
    /// <param name="error"></param>
    public class KDriveApiException(KDriveErrorResponse error) : Exception($"{error.Error.Code}: {error.Error.Description}")
    {
        /// <summary>
        /// Error contains the details of the error returned by the kDrive API.
        /// </summary>
        public KDriveErrorResponse Error { get; } = error;
    }
}
