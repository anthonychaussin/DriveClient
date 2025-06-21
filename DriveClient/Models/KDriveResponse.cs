using System.Text.Json;
using System.Text.Json.Serialization;

namespace kDriveClient.Models
{
    public class KDriveResponse
    {
        public string result { get; set; }
        public Dictionary<string, object> data { get;set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraData { get; set; }
    }
}