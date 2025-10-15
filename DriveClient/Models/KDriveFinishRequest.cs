namespace kDriveClient.Models
{
    /// <summary>
    /// Represents the request body for finishing an upload session.
    /// </summary>
    public class KDriveFinishRequest
    {
        /// <summary>
        /// The total chunk hash of the uploaded file.
        /// </summary>
        [JsonPropertyName("total_chunk_hash")]
        public string TotalChunkHash { get; set; } = string.Empty;
    }
}