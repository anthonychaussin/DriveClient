namespace kDriveClient.Models.Domain
{
    /// <summary>
    /// A page of remote file-system items (cursor-based navigator response).
    /// </summary>
    public sealed class KDriveItemPage
    {
        /// <summary>Items on this page, as domain <see cref="KDriveItem"/> instances.</summary>
        public IReadOnlyList<KDriveItem> Items { get; init; } = [];

        /// <summary>Cursor for the next page, when <see cref="HasMore"/> is true.</summary>
        public string? Cursor { get; init; }

        /// <summary>Whether more pages are available.</summary>
        public bool? HasMore { get; init; }

        /// <summary>Server response timestamp when provided.</summary>
        public long? ResponseAt { get; init; }

        /// <summary>API envelope result string (typically <c>success</c>).</summary>
        public string? Result { get; init; }

        /// <summary>Maps a navigator transport response to a domain page.</summary>
        public static KDriveItemPage? From(KDriveNavigatorResponse<KDriveFileSystemItem>? response)
        {
            if (response is null)
                return null;

            return new KDriveItemPage
            {
                Items = KDriveItem.FromMany(response.Data),
                Cursor = response.Cursor,
                HasMore = response.HasMore,
                ResponseAt = response.ResponseAt,
                Result = response.Result
            };
        }
    }

    /// <summary>
    /// A single-item API response mapped to a domain <see cref="KDriveItem"/>.
    /// </summary>
    public sealed class KDriveItemResult
    {
        /// <summary>The remote item, or null when the payload was empty.</summary>
        public KDriveItem? Item { get; init; }

        /// <summary>API envelope result string (typically <c>success</c>).</summary>
        public string? Result { get; init; }

        /// <summary>Maps a resource transport response to a domain result.</summary>
        public static KDriveItemResult? From(KDriveResourceResponse<KDriveFileSystemItem>? response)
        {
            if (response is null)
                return null;

            return new KDriveItemResult
            {
                Item = KDriveItem.From(response.Data),
                Result = response.Result
            };
        }
    }

    /// <summary>
    /// Collaborative access (ACL) snapshot for a file or directory.
    /// </summary>
    /// <remarks>
    /// Distinct from a <see cref="Models.KDriveShareLink"/> (public URL) and from a dropbox
    /// (upload inbox). This models who can read/write/manage the item inside the drive.
    /// </remarks>
    public sealed class KDriveAccess
    {
        /// <summary>Users with explicit access.</summary>
        public IReadOnlyList<KDriveFileAccessUser> Users { get; init; } = [];

        /// <summary>Teams with explicit access.</summary>
        public IReadOnlyList<KDriveFileAccessTeam> Teams { get; init; } = [];

        /// <summary>Pending invitations.</summary>
        public IReadOnlyList<KDriveFileAccessInvitation> Invitations { get; init; } = [];

        /// <summary>Inherited parent access when provided.</summary>
        public KDriveParentFileAccess? ParentAccess { get; init; }

        /// <summary>Underlying transport DTO.</summary>
        public KDriveFileAccess? Source { get; init; }

        /// <summary>Maps the ACL transport DTO to a domain access object.</summary>
        public static KDriveAccess? From(KDriveFileAccess? dto)
        {
            if (dto is null)
                return null;

            return new KDriveAccess
            {
                Users = dto.Users ?? [],
                Teams = dto.Teams ?? [],
                Invitations = dto.Invitations ?? [],
                ParentAccess = dto.ParentAccess,
                Source = dto
            };
        }

        /// <summary>Maps a resource envelope to domain access.</summary>
        public static KDriveAccess? From(KDriveResourceResponse<KDriveFileAccess>? response)
            => From(response?.Data);
    }
}
