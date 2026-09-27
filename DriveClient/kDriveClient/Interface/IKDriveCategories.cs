using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// Drive category (tag) management and applying categories on items.
    /// </summary>
    public partial interface IKDriveCategories
    {
        /// <summary>
        /// Gets categories.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveCategory>?> GetCategoriesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Creates category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCategory>?> CreateCategoryAsync(string name, string? color = null, CancellationToken ct = default);

        /// <summary>
        /// Lists categories.
        /// </summary>
        /// <param name="query">Optional typed query filters (limit, cursor, includes, …).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDrivePagedResponse<KDriveCategory>?> ListCategoriesAsync(KDriveListQuery? query = null, CancellationToken ct = default);

        /// <summary>
        /// Creates drive Category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCategory>?> CreateDriveCategoryAsync(string name, string? color = null, CancellationToken ct = default);

        /// <summary>
        /// Tags item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> TagItemAsync(long fileId, long categoryId, CancellationToken ct = default);

        /// <summary>
        /// Performs untag Item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UntagItemAsync(long fileId, long categoryId, CancellationToken ct = default);

        /// <summary>
        /// Adds category On Items.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> AddCategoryOnItemsAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Removes drive Category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveDriveCategoryAsync(long categoryId, CancellationToken ct = default);

        /// <summary>
        /// Gets drive Category Rights.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCategoryPermission>?> GetDriveCategoryRightsAsync(CancellationToken ct = default);

        /// <summary>
        /// Removes all Categories From Item.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveAllCategoriesFromItemAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Removes category From Items.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveCategoryFromItemsAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Sends item Category Ai Feedback.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SendItemCategoryAiFeedbackAsync(long fileId, long categoryId, KDriveCategoryAiFeedbackRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Performs set Drive Category Rights.
        /// </summary>
        /// <param name="rightsPayload">The rights payload.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SetDriveCategoryRightsAsync(KDriveCategoryRightsRequest rightsPayload, CancellationToken ct = default);

        /// <summary>
        /// Updates drive Category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UpdateDriveCategoryAsync(long categoryId, string? name = null, string? color = null, CancellationToken ct = default);

        /// <summary>
        /// Updates category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> UpdateCategoryAsync(long categoryId, string? name = null, string? color = null, CancellationToken ct = default);

        /// <summary>
        /// Deletes category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> DeleteCategoryAsync(long categoryId, CancellationToken ct = default);

        /// <summary>
        /// Gets category Rights.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<KDriveCategoryPermission>?> GetCategoryRightsAsync(CancellationToken ct = default);

        /// <summary>
        /// Performs set Category Rights.
        /// </summary>
        /// <param name="rightsPayload">The rights payload.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SetCategoryRightsAsync(KDriveCategoryRightsRequest rightsPayload, CancellationToken ct = default);

        /// <summary>
        /// Adds category To File.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> AddCategoryToFileAsync(long fileId, long categoryId, CancellationToken ct = default);

        /// <summary>
        /// Removes category From File.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveCategoryFromFileAsync(long fileId, long categoryId, CancellationToken ct = default);

        /// <summary>
        /// Removes all Categories From File.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveAllCategoriesFromFileAsync(long fileId, CancellationToken ct = default);

        /// <summary>
        /// Sends category Ai Feedback.
        /// </summary>
        /// <param name="fileId">Remote file or directory id (API uses file_id for both).</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> SendCategoryAiFeedbackAsync(long fileId, long categoryId, KDriveCategoryAiFeedbackRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Adds category On Files.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> AddCategoryOnFilesAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default);

        /// <summary>
        /// Removes category From Files.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="payload">Typed request body.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>
        Task<KDriveResourceResponse<bool>?> RemoveCategoryFromFilesAsync(long categoryId, KDriveCategoryFilesRequest payload, CancellationToken ct = default);

    }
}
