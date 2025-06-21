namespace kDriveClient.Models.Exceptions
{
    /// <summary>
    /// KDriveErrorResponse represents the error response from the kDrive API.
    /// </summary>
    public class KDriveErrorResponse : ApiResultBase
    {
        /// <summary>
        /// Result indicates the type of error.
        /// </summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// KDriveErrorDetail contains detailed information about the error.
        /// </summary>
        public KDriveErrorDetail Error { get; set; } = new();

        /// <summary>
        /// ToString provides a string representation of the KDriveErrorResponse.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Result: {Result}, Code: {Error?.Code}, Description: {Error?.Description}";
        }
    }
}