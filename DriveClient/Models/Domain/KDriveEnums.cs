using System.Text.Json.Serialization;

namespace kDriveClient.Models.Domain
{
    /// <summary>
    /// Discriminator for a remote file-system item in kDrive (file vs directory).
    /// </summary>
    /// <remarks>
    /// The Infomaniak API exposes this as the JSON string property <c>type</c>
    /// (<c>file</c> or <c>dir</c>). Prefer this enum over comparing raw strings.
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum KDriveItemType
    {
        /// <summary>Unknown or unrecognized API value.</summary>
        Unknown = 0,

        /// <summary>A regular file.</summary>
        [JsonStringEnumMemberName("file")]
        File = 1,

        /// <summary>A directory (folder). API value is <c>dir</c>.</summary>
        [JsonStringEnumMemberName("dir")]
        Directory = 2
    }

    /// <summary>
    /// Access right granted on a file, directory, share link, or invitation.
    /// </summary>
    /// <remarks>
    /// Common Infomaniak values: <c>none</c>, <c>read</c>, <c>write</c>, <c>manage</c>,
    /// plus share-link style values such as <c>public</c> / <c>password</c> depending on the endpoint.
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum KDriveRight
    {
        /// <summary>Unknown or unrecognized API value.</summary>
        Unknown = 0,

        /// <summary>No access.</summary>
        [JsonStringEnumMemberName("none")]
        None = 1,

        /// <summary>Read-only access.</summary>
        [JsonStringEnumMemberName("read")]
        Read = 2,

        /// <summary>Read and write access.</summary>
        [JsonStringEnumMemberName("write")]
        Write = 3,

        /// <summary>Full management access (ACL, share, delete, etc.).</summary>
        [JsonStringEnumMemberName("manage")]
        Manage = 4,

        /// <summary>Public share-link style access (share links).</summary>
        [JsonStringEnumMemberName("public")]
        Public = 5,

        /// <summary>Password-protected share-link style access.</summary>
        [JsonStringEnumMemberName("password")]
        Password = 6
    }

    /// <summary>
    /// Visibility of a file-system item as returned by the API.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum KDriveVisibility
    {
        /// <summary>Unknown or unrecognized API value.</summary>
        Unknown = 0,

        /// <summary>Inherited from parent.</summary>
        [JsonStringEnumMemberName("is_inherited")]
        Inherited = 1,

        /// <summary>Private to the owner / limited ACL.</summary>
        [JsonStringEnumMemberName("is_private")]
        Private = 2,

        /// <summary>Shared collaboratively.</summary>
        [JsonStringEnumMemberName("is_shared")]
        Shared = 3,

        /// <summary>Team / collaborative directory.</summary>
        [JsonStringEnumMemberName("is_team_space")]
        TeamSpace = 4,

        /// <summary>Root of a team space.</summary>
        [JsonStringEnumMemberName("is_team_space_folder")]
        TeamSpaceFolder = 5,

        /// <summary>Inside a team space folder.</summary>
        [JsonStringEnumMemberName("is_in_team_space_folder")]
        InTeamSpaceFolder = 6
    }

    /// <summary>
    /// Role of a user on a drive.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum KDriveUserRole
    {
        /// <summary>Unknown or unrecognized API value.</summary>
        Unknown = 0,

        /// <summary>External / limited user.</summary>
        [JsonStringEnumMemberName("external")]
        External = 1,

        /// <summary>Standard user.</summary>
        [JsonStringEnumMemberName("user")]
        User = 2,

        /// <summary>Drive administrator.</summary>
        [JsonStringEnumMemberName("admin")]
        Admin = 3,

        /// <summary>Drive owner.</summary>
        [JsonStringEnumMemberName("owner")]
        Owner = 4
    }

    /// <summary>
    /// Typed <c>with</c> includes for list/detail endpoints (comma-separated in the query string).
    /// </summary>
    /// <remarks>
    /// Combine flags with <c>|</c>. They are serialized to the API <c>with</c> parameter
    /// (for example <c>path,users</c>).
    /// </remarks>
    [Flags]
    public enum KDriveItemIncludes
    {
        /// <summary>No extra includes.</summary>
        None = 0,

        /// <summary>Include resolved path information.</summary>
        Path = 1 << 0,

        /// <summary>Include users related to the item.</summary>
        Users = 1 << 1,

        /// <summary>Include permission / capabilities payload.</summary>
        Capabilities = 1 << 2,

        /// <summary>Include categories attached to the item.</summary>
        Categories = 1 << 3,

        /// <summary>Include share-link information when present.</summary>
        ShareLink = 1 << 4,

        /// <summary>Include dropbox information when present.</summary>
        Dropbox = 1 << 5,

        /// <summary>Include version information.</summary>
        Version = 1 << 6,

        /// <summary>Include conversion capabilities.</summary>
        Conversion = 1 << 7
    }

    /// <summary>
    /// Sort direction for list endpoints (<c>order</c> query parameter).
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum KDriveSortOrder
    {
        /// <summary>Ascending.</summary>
        [JsonStringEnumMemberName("asc")]
        Asc = 0,

        /// <summary>Descending.</summary>
        [JsonStringEnumMemberName("desc")]
        Desc = 1
    }

    /// <summary>
    /// Helpers to parse and format API string enums / include flags.
    /// </summary>
    public static class KDriveEnumFormatting
    {
        /// <summary>Parses an API <c>type</c> value into <see cref="KDriveItemType"/>.</summary>
        public static KDriveItemType ParseItemType(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return KDriveItemType.Unknown;

            return value.Trim().ToLowerInvariant() switch
            {
                "file" => KDriveItemType.File,
                "dir" or "directory" or "folder" => KDriveItemType.Directory,
                _ => KDriveItemType.Unknown
            };
        }

        /// <summary>Formats <see cref="KDriveItemType"/> to the API <c>type</c> string.</summary>
        public static string? FormatItemType(KDriveItemType type) => type switch
        {
            KDriveItemType.File => "file",
            KDriveItemType.Directory => "dir",
            _ => null
        };

        /// <summary>Parses an API right string into <see cref="KDriveRight"/>.</summary>
        public static KDriveRight ParseRight(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return KDriveRight.Unknown;

            return value.Trim().ToLowerInvariant() switch
            {
                "none" => KDriveRight.None,
                "read" => KDriveRight.Read,
                "write" => KDriveRight.Write,
                "manage" => KDriveRight.Manage,
                "public" => KDriveRight.Public,
                "password" => KDriveRight.Password,
                _ => KDriveRight.Unknown
            };
        }

        /// <summary>Formats <see cref="KDriveRight"/> to the API string.</summary>
        public static string? FormatRight(KDriveRight right) => right switch
        {
            KDriveRight.None => "none",
            KDriveRight.Read => "read",
            KDriveRight.Write => "write",
            KDriveRight.Manage => "manage",
            KDriveRight.Public => "public",
            KDriveRight.Password => "password",
            _ => null
        };

        /// <summary>Parses an API visibility string into <see cref="KDriveVisibility"/>.</summary>
        public static KDriveVisibility ParseVisibility(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return KDriveVisibility.Unknown;

            return value.Trim().ToLowerInvariant() switch
            {
                "is_inherited" => KDriveVisibility.Inherited,
                "is_private" => KDriveVisibility.Private,
                "is_shared" => KDriveVisibility.Shared,
                "is_team_space" => KDriveVisibility.TeamSpace,
                "is_team_space_folder" => KDriveVisibility.TeamSpaceFolder,
                "is_in_team_space_folder" => KDriveVisibility.InTeamSpaceFolder,
                _ => KDriveVisibility.Unknown
            };
        }

        /// <summary>Parses an API role string into <see cref="KDriveUserRole"/>.</summary>
        public static KDriveUserRole ParseUserRole(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return KDriveUserRole.Unknown;

            return value.Trim().ToLowerInvariant() switch
            {
                "external" => KDriveUserRole.External,
                "user" => KDriveUserRole.User,
                "admin" => KDriveUserRole.Admin,
                "owner" => KDriveUserRole.Owner,
                _ => KDriveUserRole.Unknown
            };
        }

        /// <summary>Formats include flags to a comma-separated <c>with</c> query value.</summary>
        public static string? FormatIncludes(KDriveItemIncludes includes)
        {
            if (includes == KDriveItemIncludes.None)
                return null;

            var parts = new List<string>();
            if (includes.HasFlag(KDriveItemIncludes.Path)) parts.Add("path");
            if (includes.HasFlag(KDriveItemIncludes.Users)) parts.Add("users");
            if (includes.HasFlag(KDriveItemIncludes.Capabilities)) parts.Add("capabilities");
            if (includes.HasFlag(KDriveItemIncludes.Categories)) parts.Add("categories");
            if (includes.HasFlag(KDriveItemIncludes.ShareLink)) parts.Add("sharelink");
            if (includes.HasFlag(KDriveItemIncludes.Dropbox)) parts.Add("dropbox");
            if (includes.HasFlag(KDriveItemIncludes.Version)) parts.Add("version");
            if (includes.HasFlag(KDriveItemIncludes.Conversion)) parts.Add("conversion");
            return parts.Count == 0 ? null : string.Join(",", parts);
        }

        /// <summary>Formats <see cref="KDriveSortOrder"/> to the API <c>order</c> value.</summary>
        public static string FormatSortOrder(KDriveSortOrder order) => order switch
        {
            KDriveSortOrder.Desc => "desc",
            _ => "asc"
        };
    }
}
