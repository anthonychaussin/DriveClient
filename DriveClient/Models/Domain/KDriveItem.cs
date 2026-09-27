using kDriveClient.Models.Generated;

namespace kDriveClient.Models.Domain
{
    /// <summary>
    /// A remote file-system item on a kDrive (file or directory).
    /// </summary>
    /// <remarks>
    /// This is the consumer-facing domain model. Prefer it over the raw transport DTO
    /// <see cref="KDriveFileSystemItem"/>. Use pattern matching or
    /// <see cref="IsFile"/> / <see cref="IsDirectory"/> to distinguish files and folders.
    /// <para>
    /// Do not confuse with <see cref="KDriveFile"/>, which is the <b>local upload payload</b>
    /// (stream / path to upload), not a remote resource.
    /// </para>
    /// OpenAPI relation fields (<c>capabilities</c>, <c>sharelink</c>, <c>categories</c>, …)
    /// are mapped from generated <c>91ac10ff_*</c> DTOs when requested via <c>with=</c>.
    /// </remarks>
    /// <seealso cref="KDriveRemoteFile"/>
    /// <seealso cref="KDriveDirectory"/>
    public abstract class KDriveItem
    {
        /// <summary>Unique item identifier. Directories and files share the same id space.</summary>
        public long Id { get; init; }

        /// <summary>Display name.</summary>
        public string? Name { get; init; }

        /// <summary>Full path when requested via includes.</summary>
        public string? Path { get; init; }

        /// <summary>Typed item kind (file or directory).</summary>
        public KDriveItemType ItemType { get; init; }

        /// <summary>Raw API status string when present.</summary>
        public string? Status { get; init; }

        /// <summary>Typed visibility when the API value is recognized.</summary>
        public KDriveVisibility Visibility { get; init; }

        /// <summary>Owning drive id.</summary>
        public long? DriveId { get; init; }

        /// <summary>Parent directory id (root items may be null).</summary>
        public long? ParentId { get; init; }

        /// <summary>Depth in the tree when provided by the API.</summary>
        public int? Depth { get; init; }

        /// <summary>Size in bytes (files; directories may report aggregated size).</summary>
        public long? Size { get; init; }

        /// <summary>MIME type for files.</summary>
        public string? MimeType { get; init; }

        /// <summary>Extension classification from the API.</summary>
        public string? ExtensionType { get; init; }

        /// <summary>Content hash when provided.</summary>
        public string? Hash { get; init; }

        /// <summary>Entity tag for caching / concurrency.</summary>
        public string? Etag { get; init; }

        /// <summary>Directory color when set.</summary>
        public string? Color { get; init; }

        /// <summary>Unix timestamp (seconds) of creation.</summary>
        public long? CreatedAt { get; init; }

        /// <summary>Unix timestamp (seconds) of last update.</summary>
        public long? UpdatedAt { get; init; }

        /// <summary>Unix timestamp (seconds) of last content modification.</summary>
        public long? LastModifiedAt { get; init; }

        /// <summary>Unix timestamp (seconds) when moved to trash; null if not trashed.</summary>
        public long? DeletedAt { get; init; }

        /// <summary>Whether the item is marked as favorite.</summary>
        public bool? IsFavorite { get; init; }

        /// <summary>Creator user id (OpenAPI <c>created_by</c>).</summary>
        public long? CreatedBy { get; init; }

        /// <summary>Last modifier user id (OpenAPI <c>last_modified_by</c>).</summary>
        public long? LastModifiedBy { get; init; }

        /// <summary>Scan status when provided (files).</summary>
        public string? ScanStatus { get; init; }

        /// <summary>Typed capabilities when requested via <c>with=capabilities</c>.</summary>
        public KDriveApiFileSystemItemCapabilities? Capabilities { get; init; }

        /// <summary>Public share link when requested via <c>with=sharelink</c>.</summary>
        public KDriveApiShareLink? ShareLink { get; init; }

        /// <summary>Categories when requested via <c>with=categories</c>.</summary>
        public IReadOnlyList<KDriveApiFileCategory> Categories { get; init; } = [];

        /// <summary>Directory dropbox when present.</summary>
        public KDriveApiDropbox? Dropbox { get; init; }

        /// <summary>Underlying transport DTO when mapping from the API.</summary>
        public KDriveFileSystemItem? Source { get; init; }

        /// <summary>True when this item is a file.</summary>
        public bool IsFile => ItemType == KDriveItemType.File || this is KDriveRemoteFile;

        /// <summary>True when this item is a directory.</summary>
        public bool IsDirectory => ItemType == KDriveItemType.Directory || this is KDriveDirectory;

        /// <summary>True when the item is in the trash (<see cref="DeletedAt"/> set).</summary>
        public bool IsTrashed => DeletedAt is > 0;

        /// <summary>
        /// Creates a typed domain item from the API transport DTO.
        /// </summary>
        /// <param name="dto">Raw file-system item from the API. May be null.</param>
        /// <returns>
        /// A <see cref="KDriveRemoteFile"/>, <see cref="KDriveDirectory"/>, or null when
        /// <paramref name="dto"/> is null. Unknown types default to <see cref="KDriveRemoteFile"/>.
        /// </returns>
        public static KDriveItem? From(KDriveFileSystemItem? dto)
        {
            if (dto is null)
                return null;

            var type = KDriveEnumFormatting.ParseItemType(dto.Type);
            return type switch
            {
                KDriveItemType.Directory => KDriveDirectory.FromDto(dto),
                _ => KDriveRemoteFile.FromDto(dto, type)
            };
        }

        /// <summary>Maps a sequence of transport DTOs to domain items (skips nulls).</summary>
        public static IReadOnlyList<KDriveItem> FromMany(IEnumerable<KDriveFileSystemItem>? items)
        {
            if (items is null)
                return [];

            return items.Select(From).Where(i => i is not null).Cast<KDriveItem>().ToList();
        }

        /// <inheritdoc />
        public override string ToString() =>
            $"{GetType().Name}(Id={Id}, Name={Name}, ItemType={ItemType}, ParentId={ParentId}, Path={Path})";
    }

    /// <summary>
    /// A remote file stored on kDrive (not a local upload payload).
    /// </summary>
    /// <seealso cref="KDriveFile"/>
    /// <seealso cref="KDriveDirectory"/>
    public sealed class KDriveRemoteFile : KDriveItem
    {
        /// <summary>Builds a remote file domain object from the transport DTO.</summary>
        public static KDriveRemoteFile FromDto(KDriveFileSystemItem dto, KDriveItemType? type = null) => new()
        {
            Id = dto.Id ?? 0,
            Name = dto.Name,
            Path = dto.Path,
            ItemType = type ?? KDriveEnumFormatting.ParseItemType(dto.Type),
            Status = dto.Status,
            Visibility = KDriveEnumFormatting.ParseVisibility(dto.Visibility),
            DriveId = dto.DriveId,
            ParentId = dto.ParentId,
            Depth = dto.Depth is null ? null : checked((int)dto.Depth.Value),
            Size = dto.Size,
            MimeType = dto.MimeType,
            ExtensionType = dto.ExtensionType,
            Hash = dto.Hash,
            Etag = dto.Etag,
            Color = dto.Color,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            LastModifiedAt = dto.LastModifiedAt,
            DeletedAt = dto.DeletedAt,
            IsFavorite = dto.IsFavorite,
            CreatedBy = dto.CreatedBy,
            LastModifiedBy = dto.LastModifiedBy,
            ScanStatus = dto.ScanStatus,
            Capabilities = dto.Capabilities,
            ShareLink = dto.ShareLink,
            Categories = dto.Categories ?? [],
            Dropbox = dto.Dropbox,
            Source = dto
        };
    }

    /// <summary>
    /// A remote directory (folder) on kDrive.
    /// </summary>
    /// <remarks>
    /// API paths often still use the parameter name <c>file_id</c> for directories;
    /// pass <see cref="KDriveItem.Id"/> the same way you would for a file.
    /// </remarks>
    public sealed class KDriveDirectory : KDriveItem
    {
        /// <summary>Builds a directory domain object from the transport DTO.</summary>
        public static KDriveDirectory FromDto(KDriveFileSystemItem dto) => new()
        {
            Id = dto.Id ?? 0,
            Name = dto.Name,
            Path = dto.Path,
            ItemType = KDriveItemType.Directory,
            Status = dto.Status,
            Visibility = KDriveEnumFormatting.ParseVisibility(dto.Visibility),
            DriveId = dto.DriveId,
            ParentId = dto.ParentId,
            Depth = dto.Depth is null ? null : checked((int)dto.Depth.Value),
            Size = dto.Size,
            MimeType = dto.MimeType,
            ExtensionType = dto.ExtensionType,
            Hash = dto.Hash,
            Etag = dto.Etag,
            Color = dto.Color,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            LastModifiedAt = dto.LastModifiedAt,
            DeletedAt = dto.DeletedAt,
            IsFavorite = dto.IsFavorite,
            CreatedBy = dto.CreatedBy,
            LastModifiedBy = dto.LastModifiedBy,
            ScanStatus = dto.ScanStatus,
            Capabilities = dto.Capabilities,
            ShareLink = dto.ShareLink,
            Categories = dto.Categories ?? [],
            Dropbox = dto.Dropbox,
            Source = dto
        };
    }
}
