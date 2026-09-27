namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// File navigation, CRUD, trash, versions, and related item operations.
    /// </summary>
    /// <remarks>
    /// Prefer business aliases such as <c>GetItemAsync</c>, <c>GetItemsAsync</c>, and
    /// <c>CreateFolderAsync</c>, which return domain <see cref="Models.Domain.KDriveItem"/>
    /// types. Low-level OpenAPI-named methods return transport DTOs when you need the raw JSON shape.
    /// <para>
    /// Parameters named <c>fileId</c> often accept a directory id as well — Infomaniak
    /// uses a shared id space and the path segment name <c>file_id</c> for both.
    /// </para>
    /// </remarks>
    public partial interface IKDriveFiles
    {
    }
}
