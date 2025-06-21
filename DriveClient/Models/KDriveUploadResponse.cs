namespace kDriveClient.Models
{
    /// <summary>
    /// KDrive upload response wrapper.
    /// </summary>
    public class KDriveUploadResponseWraper : ApiResultBase
    {
        /// <summary>
        /// Result of the upload operation. ("success" or "error")
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// Data containing the details of the uploaded file.
        /// </summary>
        public KDriveUploadResponse Data { get; set; }
    }

    /// <summary>
    /// Kdrive upload response model.
    /// </summary>
    public class KDriveUploadResponse : ApiResultBase
    {
        /// <summary>
        /// File ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// File name.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// File path.
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// File directory ID.
        /// </summary>
        public int DirectoryId { get; set; }
        /// <summary>
        /// File size in bytes.
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Creation timestamps of file.
        /// </summary>
        public int CreatedAt { get; set; }
        /// <summary>
        /// Last modified timestamps of file.
        /// </summary>
        public int LastModifiedAt { get; set; }
        /// <summary>
        /// File MIME type.
        /// </summary>
        public required string Mime_Type { get; set; }
        /// <summary>
        /// File hash.
        /// </summary>
        public string? Hash { get; set; }
    }
}