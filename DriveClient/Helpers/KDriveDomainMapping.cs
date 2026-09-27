using kDriveClient.Models;
using kDriveClient.Models.Domain;
using kDriveClient.Models.Generated;

namespace kDriveClient.Helpers
{
    /// <summary>
    /// Maps transport DTOs and queries to domain / HTTP dictionary forms.
    /// Generated OpenAPI types (<see cref="KDriveApiFileV3"/>, …) are the transport source of truth;
    /// domain types (<see cref="KDriveItem"/>) are the stable public surface.
    /// </summary>
    public static class KDriveDomainMapping
    {
        /// <summary>Converts a typed list query to an HTTP query dictionary (null-safe).</summary>
        public static IDictionary<string, string?>? ToQuery(KDriveListQuery? query)
            => KDriveListQuery.ToDictionaryOrNull(query);

        /// <summary>Converts a typed search query to an HTTP query dictionary (null-safe).</summary>
        public static IDictionary<string, string?>? ToQuery(KDriveSearchQuery? query)
            => query?.ToDictionary();

        /// <summary>Converts a typed paged query to an HTTP query dictionary (null-safe).</summary>
        public static IDictionary<string, string?>? ToQuery(KDrivePagedQuery? query)
            => query?.ToDictionary();

        /// <summary>Maps a navigator response to a domain item page.</summary>
        public static KDriveItemPage? ToItemPage(KDriveNavigatorResponse<KDriveFileSystemItem>? response)
            => KDriveItemPage.From(response);

        /// <summary>Maps a resource response to a domain item result.</summary>
        public static KDriveItemResult? ToItemResult(KDriveResourceResponse<KDriveFileSystemItem>? response)
            => KDriveItemResult.From(response);

        /// <summary>Maps a resource response to domain access.</summary>
        public static KDriveAccess? ToAccess(KDriveResourceResponse<KDriveFileAccess>? response)
            => KDriveAccess.From(response);

        /// <summary>Maps many file-system DTOs to domain items.</summary>
        public static IReadOnlyList<KDriveItem> ToItems(IEnumerable<KDriveFileSystemItem>? items)
            => KDriveItem.FromMany(items);

        /// <summary>Maps a generated OpenAPI FileV3 DTO to a domain item.</summary>
        public static KDriveItem? ToItem(KDriveApiFileV3? dto)
            => dto is null ? null : KDriveItem.From(ToFileSystemItem(dto));

        /// <summary>Maps a generated OpenAPI DirectoryV3 DTO to a domain item.</summary>
        public static KDriveItem? ToItem(KDriveApiDirectoryV3? dto)
            => dto is null ? null : KDriveItem.From(ToFileSystemItem(dto));

        /// <summary>
        /// Projects OpenAPI FileV3 onto <see cref="KDriveFileSystemItem"/>.
        /// Scalars are copied field-to-field; nested relation shapes that diverge across
        /// generated schemas are aligned via a targeted JSON projection.
        /// </summary>
        public static KDriveFileSystemItem ToFileSystemItem(KDriveApiFileV3 dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var item = new KDriveFileSystemItem
            {
                Id = dto.Id,
                Name = dto.Name,
                SortedName = dto.SortedName,
                Path = dto.Path,
                Type = dto.Type ?? "file",
                Status = dto.Status,
                Visibility = dto.Visibility,
                DriveId = dto.DriveId,
                Depth = dto.Depth,
                CreatedBy = dto.CreatedBy,
                CreatedAt = dto.CreatedAt,
                AddedAt = dto.AddedAt,
                LastModifiedAt = dto.LastModifiedAt,
                LastModifiedBy = dto.LastModifiedBy,
                RevisedAt = dto.RevisedAt,
                UpdatedAt = dto.UpdatedAt,
                ParentId = dto.ParentId,
                DeletedAt = dto.DeletedAt,
                Size = dto.Size,
                MimeType = dto.MimeType,
                Hash = dto.Hash,
                Etag = dto.Etag,
                IsFavorite = dto.IsFavorite,
                ExtensionType = dto.ExtensionType,
                ScanStatus = dto.ScanStatus,
                ShareLink = dto.ShareLink,
                Categories = dto.Categories,
            };
            AlignNestedViaJson(dto, item);
            return item;
        }

        /// <summary>
        /// Projects OpenAPI DirectoryV3 onto <see cref="KDriveFileSystemItem"/>.
        /// </summary>
        public static KDriveFileSystemItem ToFileSystemItem(KDriveApiDirectoryV3 dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var item = new KDriveFileSystemItem
            {
                Id = dto.Id,
                Name = dto.Name,
                SortedName = dto.SortedName,
                Path = dto.Path,
                Type = dto.Type ?? "dir",
                Status = dto.Status,
                Visibility = dto.Visibility,
                DriveId = dto.DriveId,
                Depth = dto.Depth,
                CreatedBy = dto.CreatedBy,
                CreatedAt = dto.CreatedAt,
                AddedAt = dto.AddedAt,
                LastModifiedAt = dto.LastModifiedAt,
                LastModifiedBy = dto.LastModifiedBy,
                RevisedAt = dto.RevisedAt,
                UpdatedAt = dto.UpdatedAt,
                ParentId = dto.ParentId,
                DeletedAt = dto.DeletedAt,
                Etag = dto.Etag,
                Color = dto.Color,
                IsFavorite = dto.IsFavorite,
                ShareLink = dto.ShareLink,
                Categories = dto.Categories,
                Dropbox = dto.Dropbox,
            };
            AlignNestedViaJson(dto, item);
            return item;
        }

        static void AlignNestedViaJson<T>(T source, KDriveFileSystemItem target)
        {
            // Nested capability / conversion shapes differ between FileV3, DirectoryV3, and FileSystemItem.
            // A single targeted serialize/deserialize fills those without re-mapping every scalar.
            var json = source switch
            {
                KDriveApiFileV3 file => JsonSerializer.Serialize(file, KDriveOpenApiJsonContext.Default.KDriveApiFileV3),
                KDriveApiDirectoryV3 dir => JsonSerializer.Serialize(dir, KDriveOpenApiJsonContext.Default.KDriveApiDirectoryV3),
                _ => null
            };
            if (json is null)
                return;

            var bridge = JsonSerializer.Deserialize(json, KDriveJsonContext.Default.KDriveFileSystemItem);
            if (bridge is null)
                return;

            target.Capabilities ??= bridge.Capabilities;
            target.ConversionCapabilities ??= bridge.ConversionCapabilities;
            target.Dropbox ??= bridge.Dropbox;
            target.Color ??= bridge.Color;
            target.Size ??= bridge.Size;
            target.MimeType ??= bridge.MimeType;
            target.Hash ??= bridge.Hash;
            target.ExtensionType ??= bridge.ExtensionType;
            target.ScanStatus ??= bridge.ScanStatus;
        }
    }
}
