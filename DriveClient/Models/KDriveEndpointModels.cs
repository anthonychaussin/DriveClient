using kDriveClient.Models.Generated;

namespace kDriveClient.Models
{
    /// <summary>
    /// Generic response wrapper with a single data payload.
    /// </summary>
    public class KDriveResourceResponse<TData> : ApiResultBase
    {
        public string Result { get; set; } = string.Empty;

        public TData? Data { get; set; }

        public override string ToString()
        {
            return $"Result={Result}, DataType={typeof(TData).Name}, HasData={Data is not null}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// Generic paginated response wrapper.
    /// </summary>
    public class KDrivePagedResponse<TItem> : ApiResultBase
    {
        public string Result { get; set; } = string.Empty;

        public List<TItem> Data { get; set; } = [];

        [JsonPropertyName("total")]
        public int? Total { get; set; }

        [JsonPropertyName("page")]
        public int? Page { get; set; }

        [JsonPropertyName("pages")]
        public int? Pages { get; set; }

        [JsonPropertyName("items_per_page")]
        public int? ItemsPerPage { get; set; }

        public override string ToString()
        {
            return $"Result={Result}, ItemType={typeof(TItem).Name}, DataCount={Data.Count}, Page={Page}, Pages={Pages}, Total={Total}, ItemsPerPage={ItemsPerPage}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// Generic cursor-based response wrapper.
    /// </summary>
    public class KDriveNavigatorResponse<TItem> : ApiResultBase
    {
        public string Result { get; set; } = string.Empty;

        public List<TItem> Data { get; set; } = [];

        [JsonPropertyName("cursor")]
        public string? Cursor { get; set; }

        [JsonPropertyName("has_more")]
        public bool? HasMore { get; set; }

        [JsonPropertyName("response_at")]
        public long? ResponseAt { get; set; }

        public override string ToString()
        {
            return $"Result={Result}, ItemType={typeof(TItem).Name}, DataCount={Data.Count}, Cursor={Cursor}, HasMore={HasMore}, ResponseAt={ResponseAt}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// Drive summary model.
    /// </summary>
    public class KDriveDriveSummary : ApiResultBase
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public long? Size { get; set; }

        [JsonPropertyName("used_size")]
        public long? UsedSize { get; set; }

        [JsonPropertyName("account_id")]
        public long? AccountId { get; set; }

        [JsonPropertyName("product_id")]
        public long? ProductId { get; set; }

        [JsonPropertyName("users_count")]
        public int? UsersCount { get; set; }

        [JsonPropertyName("users_quota")]
        public int? UsersQuota { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        [JsonPropertyName("expired_at")]
        public long? ExpiredAt { get; set; }

        [JsonPropertyName("is_locked")]
        public bool? IsLocked { get; set; }

        [JsonPropertyName("is_demo")]
        public bool? IsDemo { get; set; }

        [JsonPropertyName("in_maintenance")]
        public bool? InMaintenance { get; set; }

        public string? Role { get; set; }

        public string? Status { get; set; }

        public override string ToString()
        {
            return $"Id={Id}, Name={Name}, UsedSize={UsedSize}, Size={Size}, Users={UsersCount}/{UsersQuota}, Role={Role}, Status={Status}, IsLocked={IsLocked}, InMaintenance={InMaintenance}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// User summary model.
    /// </summary>
    public class KDriveUserSummary : ApiResultBase
    {
        public long Id { get; set; }

        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        public string? Email { get; set; }

        [JsonPropertyName("is_sso")]
        public bool? IsSso { get; set; }

        public string? Avatar { get; set; }

        public string? Type { get; set; }

        [JsonPropertyName("deleted_at")]
        public long? DeletedAt { get; set; }

        public override string ToString()
        {
            return $"Id={Id}, DisplayName={DisplayName}, Email={Email}, Type={Type}, IsSso={IsSso}, DeletedAt={DeletedAt}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// Drive user summary model.
    /// </summary>
    public class KDriveDriveUserSummary : KDriveUserSummary
    {
        [JsonPropertyName("drive_id")]
        public long? DriveId { get; set; }

        [JsonPropertyName("drive_name")]
        public string? DriveName { get; set; }

        [JsonPropertyName("account_id")]
        public long? AccountId { get; set; }

        [JsonPropertyName("product_id")]
        public long? ProductId { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        [JsonPropertyName("last_connection_at")]
        public long? LastConnectionAt { get; set; }

        public string? Status { get; set; }

        public string? Role { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, DriveId={DriveId}, DriveName={DriveName}, AccountId={AccountId}, ProductId={ProductId}, Status={Status}, Role={Role}, LastConnectionAt={LastConnectionAt}";
        }
    }

    /// <summary>
    /// Transport DTO for a unified file/directory payload from v3 endpoints.
    /// </summary>
    /// <remarks>
    /// Inherits OpenAPI <c>FileV3</c>/<c>DirectoryV3</c> union properties
    /// (<see cref="KDriveApiFileSystemItem"/>), including typed
    /// <see cref="KDriveApiFileSystemItem.Capabilities"/>,
    /// <see cref="KDriveApiFileSystemItem.ShareLink"/>, and
    /// <see cref="KDriveApiFileSystemItem.Categories"/>.
    /// Prefer mapping to <see cref="Domain.KDriveItem"/> in application code.
    /// </remarks>
    /// <seealso cref="Domain.KDriveItem"/>
    /// <seealso cref="KDriveApiFileV3"/>
    /// <seealso cref="KDriveApiDirectoryV3"/>
    public class KDriveFileSystemItem : KDriveApiFileSystemItem
    {
        /// <summary>Number of extension fields returned by the API.</summary>
        protected int ExtraDataCount => ExtraData?.Count ?? 0;

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Id={Id}, Name={Name}, Path={Path}, Type={Type}, Status={Status}, Visibility={Visibility}, ParentId={ParentId}, DriveId={DriveId}, Size={Size}, MimeType={MimeType}, Hash={Hash}, IsFavorite={IsFavorite}, LastModifiedAt={LastModifiedAt}, DeletedAt={DeletedAt}, Capabilities={Capabilities is not null}, ShareLink={ShareLink is not null}, Categories={Categories?.Count}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// Cancel resource model used by async-like operations.
    /// </summary>
    public class KDriveCancelResource : ApiResultBase
    {
        [JsonPropertyName("cancel_id")]
        public string? CancelId { get; set; }

        [JsonPropertyName("valid_until")]
        public long? ValidUntil { get; set; }

        public override string ToString()
        {
            return $"CancelId={CancelId}, ValidUntil={ValidUntil}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// Directory count summary model.
    /// </summary>
    public class KDriveDirectoryCount : ApiResultBase
    {
        [JsonPropertyName("files")]
        public long? Files { get; set; }

        [JsonPropertyName("directories")]
        public long? Directories { get; set; }

        [JsonPropertyName("total")]
        public long? Total { get; set; }

        public override string ToString()
        {
            return $"Files={Files}, Directories={Directories}, Total={Total}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// File version summary model.
    /// </summary>
    public class KDriveFileVersion : ApiResultBase
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("file_id")]
        public long? FileId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("size")]
        public long? Size { get; set; }

        [JsonPropertyName("hash")]
        public string? Hash { get; set; }

        [JsonPropertyName("mime_type")]
        public string? MimeType { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        [JsonPropertyName("last_modified_at")]
        public long? LastModifiedAt { get; set; }

        public override string ToString()
        {
            return $"Id={Id}, FileId={FileId}, Name={Name}, Size={Size}, Hash={Hash}, LastModifiedAt={LastModifiedAt}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// File activity summary model.
    /// </summary>
    public class KDriveFileActivity : ApiResultBase
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }

        public override string ToString()
        {
            return $"Id={Id}, Type={Type}, Action={Action}, UserId={UserId}, CreatedAt={CreatedAt}, ExtraDataCount={ExtraDataCount}";
        }
    }

    /// <summary>
    /// v3 drive user model (cursor-based users endpoint).
    /// </summary>
    public class KDriveUserV3 : ApiResultBase
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Id={Id}, DisplayName={DisplayName}, Email={Email}, Status={Status}, Type={Type}, ExtraDataCount={ExtraDataCount}";
        }
    }
}
