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
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// Data containing the details of the uploaded file.
        /// </summary>
        public KDriveUploadDataResponse? Data { get; set; }
    }

    /// <summary>
    /// KDrive upload data response model.
    /// </summary>
    public class KDriveUploadDataResponse : ApiResultBase
    {
        /// <summary>
        /// KDrive file upload response details.
        /// </summary>
        public KDriveUploadResponse? File { get; set; }

        /// <summary>
        /// Token for the uploaded request.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Status of the upload operation.
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Additional message regarding the upload operation.
        /// </summary>
        public string? Message { get; set; }
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
        public string Name { get; set; } = string.Empty;

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
        public string Mime_Type { get; set; } = string.Empty;

        /// <summary>
        /// File hash.
        /// </summary>
        public string? Hash { get; set; }

        /// <summary>
        /// File or folder type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Current status of the file.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Visibility of the file.
        /// </summary>
        public string Visibility { get; set; }

        /// <summary>
        /// Drive ID associated with the file.
        /// </summary>
        public int Drive_id { get; set; }

        /// <summary>
        /// Depth of the file in the directory structure.
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Creator user ID.
        /// </summary>
        public int Created_by { get; set; }

        /// <summary>
        /// Timestamp when the file was created.
        /// </summary>
        public int Created_at { get; set; }

        /// <summary>
        /// Timestamp when the file was added to the drive.
        /// </summary>
        public int Added_at { get; set; }

        /// <summary>
        /// Timestamp when the file was last modified.
        /// </summary>
        public int Last_modified_at { get; set; }

        /// <summary>
        /// Timestamp when the file was last edited.
        /// </summary>
        public int Last_modified_by { get; set; }

        /// <summary>
        /// Timestamp of last file version.
        /// </summary>
        public int Revised_at { get; set; }

        /// <summary>
        /// Timestamp when the file was last updated.
        /// </summary>
        public int Updated_at { get; set; }

        /// <summary>
        /// Id of parent directory.
        /// </summary>
        public int Parent_id { get; set; }

        /// <summary>
        /// Extension type of the file.
        /// </summary>
        public string? Extension_type { get; set; }

        /// <summary>
        /// Antivirus scan status of the file.
        /// </summary>
        public string? Scan_status { get; set; }
    }
}