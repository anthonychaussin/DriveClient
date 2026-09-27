namespace kDriveClient.Models
{
    public class KDriveUuidResource : ApiResultBase
    {
        public string? Uuid { get; set; }

        public override string ToString() => $"Uuid={Uuid}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveFeedbackResource : ApiResultBase
    {
        public long? Id { get; set; }

        public string? Result { get; set; }

        public string? Message { get; set; }

        public override string ToString() => $"Id={Id}, Result={Result}, Message={Message}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveFileHash : ApiResultBase
    {
        public string? Hash { get; set; }

        public override string ToString() => $"Hash={Hash}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveFileSizeInfo : ApiResultBase
    {
        public long? Size { get; set; }

        [JsonPropertyName("storage_size")]
        public long? StorageSize { get; set; }

        public override string ToString() => $"Size={Size}, StorageSize={StorageSize}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveTemporaryUrl : ApiResultBase
    {
        [JsonPropertyName("temporary_url")]
        public string? TemporaryUrl { get; set; }

        public override string ToString() => $"TemporaryUrl={TemporaryUrl}, ExtraDataCount={ExtraDataCount}";
    }

    /// <summary>
    /// Public share link on a file or directory (API v2).
    /// </summary>
    /// <remarks>
    /// A share link is a URL outsiders can open. It is different from collaborative ACL
    /// (<see cref="Domain.KDriveAccess"/>) and from a dropbox (upload inbox).
    /// </remarks>
    /// <seealso cref="Domain.KDriveAccess"/>
    /// <seealso cref="KDriveDropbox"/>
    public class KDriveShareLink : ApiResultBase
    {
        public string? Url { get; set; }

        [JsonPropertyName("file_id")]
        public long? FileId { get; set; }

        /// <summary>Raw API right string.</summary>
        public string? Right { get; set; }

        /// <summary>Typed access right when the API value is recognized.</summary>
        [JsonIgnore]
        public Domain.KDriveRight AccessRight
        {
            get => Domain.KDriveEnumFormatting.ParseRight(Right);
            set => Right = Domain.KDriveEnumFormatting.FormatRight(value);
        }

        [JsonPropertyName("valid_until")]
        public long? ValidUntil { get; set; }

        [JsonPropertyName("created_by")]
        public long? CreatedBy { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        [JsonPropertyName("access_blocked")]
        public bool? AccessBlocked { get; set; }

        public override string ToString() =>
            $"Url={Url}, FileId={FileId}, Right={Right}, ValidUntil={ValidUntil}, ExtraDataCount={ExtraDataCount}";
    }

    /// <summary>Request body for creating or updating a share link (API v2).</summary>
    public class KDriveShareLinkRequest
    {
        /// <summary>Raw API right string (e.g. <c>public</c>). Prefer <see cref="AccessRight"/>.</summary>
        [JsonPropertyName("right")]
        public string? Right { get; set; }

        /// <summary>
        /// Typed access right for the link (required on create for most drives).
        /// Typical value: <see cref="Domain.KDriveRight.Public"/>.
        /// </summary>
        [JsonIgnore]
        public Domain.KDriveRight? AccessRight
        {
            get => Right is null ? null : Domain.KDriveEnumFormatting.ParseRight(Right);
            set => Right = value is null ? null : Domain.KDriveEnumFormatting.FormatRight(value.Value);
        }

        [JsonPropertyName("can_comment")]
        public bool? CanComment { get; set; }

        [JsonPropertyName("can_download")]
        public bool? CanDownload { get; set; }

        [JsonPropertyName("can_edit")]
        public bool? CanEdit { get; set; }

        [JsonPropertyName("can_request_access")]
        public bool? CanRequestAccess { get; set; }

        [JsonPropertyName("can_see_info")]
        public bool? CanSeeInfo { get; set; }

        [JsonPropertyName("can_see_stats")]
        public bool? CanSeeStats { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("valid_until")]
        public long? ValidUntil { get; set; }
    }


    public class KDriveExternalImport : ApiResultBase
    {
        public long? Id { get; set; }

        public string? Application { get; set; }

        [JsonPropertyName("account_name")]
        public string? AccountName { get; set; }

        public string? Status { get; set; }

        [JsonPropertyName("error_code")]
        public string? ErrorCode { get; set; }

        public string? Path { get; set; }

        [JsonPropertyName("directory_id")]
        public long? DirectoryId { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        public override string ToString() => $"Id={Id}, Status={Status}, Path={Path}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveCategory : ApiResultBase
    {
        public long? Id { get; set; }

        public string? Name { get; set; }

        public string? Color { get; set; }

        [JsonPropertyName("is_predefined")]
        public bool? IsPredefined { get; set; }

        [JsonPropertyName("created_by")]
        public long? CreatedBy { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        public override string ToString() => $"Id={Id}, Name={Name}, Color={Color}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveCategoryPermission : ApiResultBase
    {
        [JsonPropertyName("can_create")]
        public bool? CanCreate { get; set; }

        [JsonPropertyName("can_edit")]
        public bool? CanEdit { get; set; }

        [JsonPropertyName("can_delete")]
        public bool? CanDelete { get; set; }

        [JsonPropertyName("can_read_on_file")]
        public bool? CanReadOnFile { get; set; }

        [JsonPropertyName("can_put_on_file")]
        public bool? CanPutOnFile { get; set; }

        public override string ToString() => $"CanCreate={CanCreate}, CanEdit={CanEdit}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveComment : ApiResultBase
    {
        public long? Id { get; set; }

        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }

        public string? Body { get; set; }

        [JsonPropertyName("is_resolved")]
        public bool? IsResolved { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        public bool? Liked { get; set; }

        [JsonPropertyName("likes_count")]
        public int? LikesCount { get; set; }

        [JsonPropertyName("responses_count")]
        public int? ResponsesCount { get; set; }

        public override string ToString() => $"Id={Id}, Body={Body}, IsResolved={IsResolved}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveDropbox : ApiResultBase
    {
        public long? Id { get; set; }

        public string? Uuid { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public string? Directory { get; set; }

        [JsonPropertyName("users_count")]
        public int? UsersCount { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        public override string ToString() => $"Id={Id}, Name={Name}, Url={Url}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveDriveDetail : ApiResultBase
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public long? Size { get; set; }

        [JsonPropertyName("used_size")]
        public long? UsedSize { get; set; }

        [JsonPropertyName("account_id")]
        public long? AccountId { get; set; }

        public string? Role { get; set; }

        public string? Status { get; set; }

        public override string ToString() => $"Id={Id}, Name={Name}, UsedSize={UsedSize}, Role={Role}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveDriveSettings : ApiResultBase
    {
        [JsonPropertyName("trash_delay")]
        public int? TrashDelay { get; set; }

        public override string ToString() => $"TrashDelay={TrashDelay}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveDriveInvitation : ApiResultBase
    {
        public long? Id { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }

        public string? Status { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        public override string ToString() => $"Id={Id}, Email={Email}, Role={Role}, Status={Status}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveUserPreference : ApiResultBase
    {
        public string? Density { get; set; }

        [JsonPropertyName("sort_recent_file")]
        public bool? SortRecentFile { get; set; }

        [JsonPropertyName("date_format")]
        public string? DateFormat { get; set; }

        [JsonPropertyName("use_shortcut")]
        public bool? UseShortcut { get; set; }

        [JsonPropertyName("default_drive")]
        public long? DefaultDrive { get; set; }

        public override string ToString() => $"DefaultDrive={DefaultDrive}, Density={Density}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveActivityReport : ApiResultBase
    {
        public long? Id { get; set; }

        public string? Status { get; set; }

        public string? Size { get; set; }

        [JsonPropertyName("download_url")]
        public string? DownloadUrl { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        public override string ToString() => $"Id={Id}, Status={Status}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveActivityV2 : ApiResultBase
    {
        public string? Action { get; set; }

        [JsonPropertyName("file_id")]
        public long? FileId { get; set; }

        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }

        [JsonPropertyName("file_type")]
        public string? FileType { get; set; }

        public long? Size { get; set; }

        public string? Path { get; set; }

        public long? Timestamp { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        public override string ToString() => $"Action={Action}, FileId={FileId}, Path={Path}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveFileAccessUser : ApiResultBase
    {
        public long? Id { get; set; }

        public string? Access { get; set; }

        public string? Name { get; set; }

        public string? Right { get; set; }

        public int? Color { get; set; }

        public string? Status { get; set; }

        public override string ToString() => $"Id={Id}, Name={Name}, Right={Right}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveFileAccessTeam : ApiResultBase
    {
        public long? Id { get; set; }

        public string? Access { get; set; }

        public string? Name { get; set; }

        public string? Right { get; set; }

        public int? Color { get; set; }

        public string? Status { get; set; }

        public override string ToString() => $"Id={Id}, Name={Name}, Right={Right}, ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveFileAccessRequest : ApiResultBase
    {
        public long? Id { get; set; }

        public string? Status { get; set; }

        public string? Message { get; set; }

        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }

        [JsonPropertyName("file_id")]
        public long? FileId { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        public override string ToString() => $"Id={Id}, Status={Status}, UserId={UserId}, ExtraDataCount={ExtraDataCount}";
    }
}
