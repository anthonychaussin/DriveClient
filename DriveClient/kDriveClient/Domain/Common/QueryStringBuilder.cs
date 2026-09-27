using kDriveClient.Models;

namespace kDriveClient.kDriveClient.Domain.Common
{
    /// <summary>
    /// Builds API paths with optional query strings.
    /// </summary>
    public static class QueryStringBuilder
    {
        /// <summary>Appends a dictionary of query parameters to <paramref name="basePath"/>.</summary>
        public static string BuildPath(string basePath, IDictionary<string, string?>? query)
        {
            if (query is null || query.Count == 0)
                return basePath;

            var parts = query
                .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!)}")
                .ToArray();

            if (parts.Length == 0)
                return basePath;

            return $"{basePath}?{string.Join("&", parts)}";
        }

        /// <summary>Appends a typed <see cref="KDriveListQuery"/> to <paramref name="basePath"/>.</summary>
        public static string BuildPath(string basePath, KDriveListQuery? query)
            => BuildPath(basePath, query?.ToDictionary());
    }
}
