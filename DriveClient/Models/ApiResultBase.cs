using System.Text.Json;
using System.Text.Json.Serialization;

namespace kDriveClient.Models
{
    /// <summary>
    /// A base class for API result models, providing a common structure for responses.
    /// </summary>
    public abstract class ApiResultBase
    {
        /// <summary>
        /// Extra data that may be included in the response.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraData { get; set; }
    }
}
