namespace kDriveClient.Models
{
    /// <summary>
    /// A base class for API Result models, providing a common structure for responses.
    /// </summary>
    public abstract class ApiResultBase
    {
        /// <summary>
        /// Extra Data that may be included in the response.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraData { get; set; }
    }
}