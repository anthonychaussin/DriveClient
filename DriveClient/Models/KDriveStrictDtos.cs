using System.Text.Json.Serialization;

namespace kDriveClient.Models
{
    // --- Response DTOs replacing KDriveStatisticsData ---

    public class KDriveFileAccess : ApiResultBase
    {
        public List<KDriveFileAccessUser> Users { get; set; } = [];
        public List<KDriveFileAccessTeam> Teams { get; set; } = [];
        public List<KDriveFileAccessInvitation> Invitations { get; set; } = [];

        [JsonPropertyName("parent_access")]
        public KDriveParentFileAccess? ParentAccess { get; set; }
    }

    public class KDriveFileAccessInvitation : ApiResultBase
    {
        public long? Id { get; set; }
        public string? Access { get; set; }
        public string? Name { get; set; }
        public string? Right { get; set; }
        public int? Color { get; set; }
        public string? Status { get; set; }
    }

    public class KDriveParentFileAccess : ApiResultBase
    {
        public List<KDriveFileAccessUser>? Users { get; set; }
        public List<KDriveFileAccessTeam>? Teams { get; set; }
    }

    public class KDriveStatisticShareLink : ApiResultBase
    {
        public string? Url { get; set; }

        [JsonPropertyName("file_id")]
        public long? FileId { get; set; }

        public string? Right { get; set; }

        [JsonPropertyName("valid_until")]
        public long? ValidUntil { get; set; }

        [JsonPropertyName("created_by")]
        public long? CreatedBy { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long? UpdatedAt { get; set; }

        public int? Views { get; set; }

        [JsonPropertyName("unique_views")]
        public int? UniqueViews { get; set; }

        [JsonPropertyName("access_blocked")]
        public bool? AccessBlocked { get; set; }
    }

    /// <summary>Open-ended statistics payload (known fields + extension data).</summary>
    public class KDriveStatisticsData : ApiResultBase
    {
        public override string ToString() => $"ExtraDataCount={ExtraDataCount}";
    }

    public class KDriveOAuthImportDrive : ApiResultBase
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
    }

    public class KDrivePendingAccessInvitation : ApiResultBase
    {
        [JsonPropertyName("invitation_id")]
        public long? InvitationId { get; set; }

        [JsonPropertyName("drive_invitation_id")]
        public long? DriveInvitationId { get; set; }

        [JsonPropertyName("has_invitation")]
        public string? HasInvitation { get; set; }

        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }

        public string? Email { get; set; }
    }

    public class KDriveExportData : ApiResultBase
    {
        public string? Url { get; set; }
        public string? Status { get; set; }
        public override string ToString() => $"Url={Url}, Status={Status}, ExtraDataCount={ExtraDataCount}";
    }

    // --- Request DTOs ---

    public class KDriveSetFileAccessRequest
    {
        public bool? Inherit { get; set; }
        public string? Right { get; set; }
        [JsonPropertyName("user_ids")]
        public long[]? UserIds { get; set; }
        [JsonPropertyName("team_ids")]
        public long[]? TeamIds { get; set; }
        [JsonPropertyName("emails")]
        public string[]? Emails { get; set; }
        public string? Message { get; set; }
    }

    /// <summary>Request body to grant users access to a file or directory.</summary>
    public class KDriveFileAccessUsersRequest
    {
        [JsonPropertyName("user_ids")]
        public long[]? UserIds { get; set; }

        /// <summary>Raw API right string. Prefer <see cref="AccessRight"/>.</summary>
        public string? Right { get; set; }

        /// <summary>Typed access right (e.g. <see cref="Domain.KDriveRight.Write"/>).</summary>
        [JsonIgnore]
        public Domain.KDriveRight? AccessRight
        {
            get => Right is null ? null : Domain.KDriveEnumFormatting.ParseRight(Right);
            set => Right = value is null ? null : Domain.KDriveEnumFormatting.FormatRight(value.Value);
        }

        public string? Message { get; set; }
    }

    /// <summary>Request body to update a user's access right on an item.</summary>
    public class KDriveFileAccessUserUpdateRequest
    {
        /// <summary>Raw API right string. Prefer <see cref="AccessRight"/>.</summary>
        public string? Right { get; set; }

        /// <summary>Typed access right.</summary>
        [JsonIgnore]
        public Domain.KDriveRight? AccessRight
        {
            get => Right is null ? null : Domain.KDriveEnumFormatting.ParseRight(Right);
            set => Right = value is null ? null : Domain.KDriveEnumFormatting.FormatRight(value.Value);
        }
    }

    /// <summary>Request body to grant teams access to a file or directory.</summary>
    public class KDriveFileAccessTeamsRequest
    {
        [JsonPropertyName("team_ids")]
        public long[]? TeamIds { get; set; }

        /// <summary>Raw API right string. Prefer <see cref="AccessRight"/>.</summary>
        public string? Right { get; set; }

        /// <summary>Typed access right.</summary>
        [JsonIgnore]
        public Domain.KDriveRight? AccessRight
        {
            get => Right is null ? null : Domain.KDriveEnumFormatting.ParseRight(Right);
            set => Right = value is null ? null : Domain.KDriveEnumFormatting.FormatRight(value.Value);
        }

        public string? Message { get; set; }
    }

    /// <summary>Request body to update a team's access right on an item.</summary>
    public class KDriveFileAccessTeamUpdateRequest
    {
        /// <summary>Raw API right string. Prefer <see cref="AccessRight"/>.</summary>
        public string? Right { get; set; }

        /// <summary>Typed access right.</summary>
        [JsonIgnore]
        public Domain.KDriveRight? AccessRight
        {
            get => Right is null ? null : Domain.KDriveEnumFormatting.ParseRight(Right);
            set => Right = value is null ? null : Domain.KDriveEnumFormatting.FormatRight(value.Value);
        }
    }

    public class KDriveFileAccessInvitationsRequest
    {
        public string[]? Emails { get; set; }
        public string? Right { get; set; }
        public string? Message { get; set; }
        public string? Lang { get; set; }
    }

    public class KDriveFileAccessInvitationsCheckRequest
    {
        public string[]? Emails { get; set; }
        [JsonPropertyName("user_ids")]
        public long[]? UserIds { get; set; }
    }

    public class KDriveCreateFileAccessRequestBody
    {
        public string? Message { get; set; }
        public string? Right { get; set; }
    }

    public class KDriveFileAccessApplicationsRequest
    {
        [JsonPropertyName("application_ids")]
        public long[]? ApplicationIds { get; set; }
        public string? Right { get; set; }
    }

    public class KDriveFileAccessCheckRequest
    {
        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }
        public string? Email { get; set; }
    }

    public class KDriveFileAccessForceRequest
    {
        [JsonPropertyName("user_ids")]
        public long[]? UserIds { get; set; }
        [JsonPropertyName("team_ids")]
        public long[]? TeamIds { get; set; }
        public string[]? Emails { get; set; }
        public string? Right { get; set; }
    }

    public class KDriveCreateUserRequest
    {
        public string? Role { get; set; }
        public string[]? Emails { get; set; }
        [JsonPropertyName("file_id")]
        public long? FileId { get; set; }
        public string? Lang { get; set; }
        public string? Message { get; set; }
    }

    public class KDriveUpdateUserRequest
    {
        public string? Role { get; set; }
        public string? Status { get; set; }
    }

    public class KDrivePatchUserManagerRequest
    {
        [JsonPropertyName("manager_id")]
        public long? ManagerId { get; set; }
    }

    public class KDriveUpdateDriveRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class KDriveDriveSettingsAiRequest
    {
        [JsonPropertyName("ai_enabled")]
        public bool? AiEnabled { get; set; }
    }

    public class KDriveDriveSettingsLinkRequest
    {
        [JsonPropertyName("default_right")]
        public string? DefaultRight { get; set; }
        [JsonPropertyName("can_edit")]
        public bool? CanEdit { get; set; }
        [JsonPropertyName("can_download")]
        public bool? CanDownload { get; set; }
    }

    public class KDriveDriveSettingsOfficeRequest
    {
        [JsonPropertyName("onlyoffice_enabled")]
        public bool? OnlyOfficeEnabled { get; set; }
    }

    public class KDriveDriveSettingsTrashRequest
    {
        [JsonPropertyName("purge_after_days")]
        public int? PurgeAfterDays { get; set; }
    }

    public class KDriveUserPreferencesRequest
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
    }

    public class KDriveUpdateInvitationRequest
    {
        public string? Role { get; set; }
        public string? Status { get; set; }
    }

    public class KDriveStartOAuthImportRequest
    {
        public string? Provider { get; set; }
        [JsonPropertyName("remote_drive_id")]
        public string? RemoteDriveId { get; set; }
        [JsonPropertyName("destination_directory_id")]
        public long? DestinationDirectoryId { get; set; }
        public string? Code { get; set; }
    }

    public class KDriveStartKdriveImportRequest
    {
        [JsonPropertyName("source_drive_id")]
        public long? SourceDriveId { get; set; }
        [JsonPropertyName("source_file_id")]
        public long? SourceFileId { get; set; }
        [JsonPropertyName("destination_directory_id")]
        public long? DestinationDirectoryId { get; set; }
    }

    public class KDriveStartWebdavImportRequest
    {
        public string? Url { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        [JsonPropertyName("destination_directory_id")]
        public long? DestinationDirectoryId { get; set; }
    }

    public class KDriveStartSharelinkImportRequest
    {
        public string? Url { get; set; }
        public string? Password { get; set; }
        [JsonPropertyName("destination_directory_id")]
        public long? DestinationDirectoryId { get; set; }
    }

    public class KDriveCategoryFilesRequest
    {
        [JsonPropertyName("file_ids")]
        public long[]? FileIds { get; set; }
    }

    public class KDriveCategoryRightsRequest
    {
        [JsonPropertyName("can_create")]
        public bool? CanCreate { get; set; }
        [JsonPropertyName("can_edit")]
        public bool? CanEdit { get; set; }
    }

    public class KDriveCreateActivityReportRequest
    {
        public string? Name { get; set; }
        [JsonPropertyName("from_date")]
        public long? FromDate { get; set; }
        [JsonPropertyName("to_date")]
        public long? ToDate { get; set; }
        public string[]? Actions { get; set; }
    }

    public class KDriveDropboxInviteRequest
    {
        public string[]? Emails { get; set; }
        public string? Message { get; set; }
        public string? Lang { get; set; }
    }

    public class KDriveCategoryAiFeedbackRequest
    {
        public string? Feedback { get; set; }
        public bool? Useful { get; set; }
    }

    public class KDriveSetCurrentVersionRequest
    {
        [JsonPropertyName("version_id")]
        public long? VersionId { get; set; }
    }

    public class KDriveUpdateFileVersionRequest
    {
        public string? Name { get; set; }
        [JsonPropertyName("keep_forever")]
        public bool? KeepForever { get; set; }
    }

    public class KDriveUploadSessionBatchFile
    {
        public string? Name { get; set; }
        public long? Size { get; set; }
        [JsonPropertyName("total_size")]
        public long? TotalSize { get; set; }
        [JsonPropertyName("directory_id")]
        public long? DirectoryId { get; set; }
        [JsonPropertyName("directory_path")]
        public string? DirectoryPath { get; set; }
        public string? Hash { get; set; }
    }

    public class KDriveUploadSessionBatchRequest
    {
        public KDriveUploadSessionBatchFile[]? Files { get; set; }
        public string? Conflict { get; set; }
        [JsonPropertyName("directory_id")]
        public long? DirectoryId { get; set; }
    }

    public class KDriveFinishUploadSessionBatchRequest
    {
        public string[]? Tokens { get; set; }
        [JsonPropertyName("total_chunk_hash")]
        public string? TotalChunkHash { get; set; }
    }
}
