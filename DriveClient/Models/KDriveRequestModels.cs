namespace kDriveClient.Models
{
    /// <summary>Body for archive creation (drive files or share-link archive).</summary>
    public class KDriveArchiveRequest
    {
        /// <summary>File ids to include (required without parent_id).</summary>
        [JsonPropertyName("file_ids")]
        public long[]? FileIds { get; set; }

        /// <summary>Parent directory id when archiving a whole folder.</summary>
        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }

        /// <summary>File ids to exclude when parent_id is set.</summary>
        [JsonPropertyName("except_file_ids")]
        public long[]? ExceptFileIds { get; set; }

        /// <summary>Builds a request from optional archive selectors.</summary>
        public static KDriveArchiveRequest? From(IEnumerable<long>? fileIds, long? parentId, IEnumerable<long>? exceptFileIds)
        {
            if (fileIds is null && !parentId.HasValue && exceptFileIds is null)
                return null;
            return new KDriveArchiveRequest
            {
                FileIds = fileIds?.ToArray(),
                ParentId = parentId,
                ExceptFileIds = exceptFileIds?.ToArray(),
            };
        }
    }

    /// <summary>Body for creating/updating a dropbox (API v2 file dropbox, or v3 create).</summary>
    public class KDriveDropboxRequest
    {
        /// <summary>Dropbox display name.</summary>
        public string? Name { get; set; }

        /// <summary>Optional directory path (v2).</summary>
        public string? Directory { get; set; }

        /// <summary>Parent directory id when creating via POST /3/.../files/dropboxes.</summary>
        [JsonPropertyName("parent_directory_id")]
        public long? ParentDirectoryId { get; set; }

        /// <summary>Whether uploads are allowed.</summary>
        [JsonPropertyName("can_upload")]
        public bool? CanUpload { get; set; }
    }

    /// <summary>Body for canceling an upload by path (API v2).</summary>
    public class KDriveCancelUploadByPathRequest
    {
        /// <summary>Remote path of the upload to cancel.</summary>
        public string? Path { get; set; }

        /// <summary>Optional file id.</summary>
        [JsonPropertyName("file_id")]
        public long? FileId { get; set; }
    }
}
