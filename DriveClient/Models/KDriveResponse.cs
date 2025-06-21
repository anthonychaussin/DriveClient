namespace kDriveClient.Models
{
    /// <summary>
    /// KDriveResponse represents the response structure from the kDrive API.
    /// </summary>
    public class KDriveResponse : ApiResultBase
    {
        /// <summary>
        /// Result of the API call, typically "success" or "error".
        /// </summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// Data contains the actual response data, which can vary based on the API endpoint.
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = [];
    }
}