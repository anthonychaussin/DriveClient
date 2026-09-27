using kDriveClient.Models.Domain;

namespace kDriveClient.Models
{
    /// <summary>
    /// Typed cursor/list query parameters for navigator and list endpoints.
    /// </summary>
    /// <remarks>
    /// Prefer this over a raw <c>IDictionary&lt;string, string?&gt;</c>. Properties map to
    /// well-known Infomaniak query parameters (<c>cursor</c>, <c>limit</c>, <c>with</c>,
    /// <c>order_by</c>, <c>order</c>).
    /// </remarks>
    public class KDriveListQuery
    {
        /// <summary>Pagination cursor from a previous navigator response.</summary>
        public string? Cursor { get; set; }

        /// <summary>Maximum number of items to return.</summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Typed <c>with</c> includes (path, users, capabilities, …).
        /// Combined with <see cref="With"/> when both are set.
        /// </summary>
        public KDriveItemIncludes Includes { get; set; } = KDriveItemIncludes.None;

        /// <summary>
        /// Optional raw <c>with</c> value for advanced / undocumented includes.
        /// Prefer <see cref="Includes"/> when possible.
        /// </summary>
        public string? With { get; set; }

        /// <summary>Sort field (API <c>order_by</c>), e.g. <c>name</c>, <c>last_modified_at</c>.</summary>
        public string? OrderBy { get; set; }

        /// <summary>Typed sort direction (API <c>order</c>).</summary>
        public KDriveSortOrder? SortOrder { get; set; }

        /// <summary>
        /// Optional raw <c>order</c> value. Used when <see cref="SortOrder"/> is null.
        /// </summary>
        public string? Order { get; set; }

        /// <summary>
        /// Extra raw query parameters for endpoint-specific keys not covered by typed properties.
        /// Prefer typed properties when available.
        /// </summary>
        public Dictionary<string, string?> Extra { get; set; } = new();

        /// <summary>Creates a shallow copy suitable for pagination loops.</summary>
        public KDriveListQuery Clone() => new()
        {
            Cursor = Cursor,
            Limit = Limit,
            Includes = Includes,
            With = With,
            OrderBy = OrderBy,
            SortOrder = SortOrder,
            Order = Order,
            Extra = new Dictionary<string, string?>(Extra)
        };

        /// <summary>Converts this query to a dictionary for the HTTP layer.</summary>
        public virtual IDictionary<string, string?> ToDictionary()
        {
            var query = new Dictionary<string, string?>();
            if (!string.IsNullOrWhiteSpace(Cursor))
                query["cursor"] = Cursor;
            if (Limit.HasValue)
                query["limit"] = Limit.Value.ToString();

            var includes = KDriveEnumFormatting.FormatIncludes(Includes);
            var withValue = MergeWith(includes, With);
            if (!string.IsNullOrWhiteSpace(withValue))
                query["with"] = withValue;

            if (!string.IsNullOrWhiteSpace(OrderBy))
                query["order_by"] = OrderBy;

            var order = SortOrder is { } sort
                ? KDriveEnumFormatting.FormatSortOrder(sort)
                : Order;
            if (!string.IsNullOrWhiteSpace(order))
                query["order"] = order;

            foreach (var kv in Extra)
            {
                if (!string.IsNullOrWhiteSpace(kv.Value))
                    query[kv.Key] = kv.Value;
            }

            return query;
        }

        /// <summary>Returns a dictionary, or null when <paramref name="query"/> is null.</summary>
        public static IDictionary<string, string?>? ToDictionaryOrNull(KDriveListQuery? query)
            => query?.ToDictionary();

        private static string? MergeWith(string? includes, string? raw)
        {
            if (string.IsNullOrWhiteSpace(includes))
                return string.IsNullOrWhiteSpace(raw) ? null : raw;
            if (string.IsNullOrWhiteSpace(raw))
                return includes;
            return $"{includes},{raw}";
        }
    }

    /// <summary>
    /// Typed search query parameters for file/item search endpoints.
    /// </summary>
    public class KDriveSearchQuery : KDriveListQuery
    {
        /// <summary>Free-text search string (API <c>query</c>).</summary>
        public string? Query { get; set; }

        /// <summary>Optional directory id that scopes the search.</summary>
        public long? DirectoryId { get; set; }

        /// <summary>Converts this search query to a dictionary for the HTTP layer.</summary>
        public override IDictionary<string, string?> ToDictionary()
        {
            var query = new Dictionary<string, string?>(base.ToDictionary());
            if (!string.IsNullOrWhiteSpace(Query))
                query["query"] = Query;
            if (DirectoryId.HasValue)
                query["directory_id"] = DirectoryId.Value.ToString();
            return query;
        }
    }

    /// <summary>
    /// Typed query for paged (page/pages) list endpoints that are not cursor-based.
    /// </summary>
    public class KDrivePagedQuery : KDriveListQuery
    {
        /// <summary>1-based page index (API <c>page</c>).</summary>
        public int? Page { get; set; }

        /// <summary>Items per page when the endpoint uses <c>items_per_page</c>.</summary>
        public int? ItemsPerPage { get; set; }

        /// <summary>Items per page when the endpoint uses <c>per_page</c> (versions, comments, …).</summary>
        public int? PerPage { get; set; }

        /// <summary>Converts this paged query to a dictionary for the HTTP layer.</summary>
        public override IDictionary<string, string?> ToDictionary()
        {
            var query = new Dictionary<string, string?>(base.ToDictionary());
            if (Page.HasValue)
                query["page"] = Page.Value.ToString();
            if (ItemsPerPage.HasValue)
                query["items_per_page"] = ItemsPerPage.Value.ToString();
            if (PerPage.HasValue)
                query["per_page"] = PerPage.Value.ToString();
            return query;
        }
    }
}
