using kDriveClient.Helpers;
using Microsoft.Extensions.Logging;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// KDriveClient provides methods to download files from the kDrive API.
    /// </summary>
    public partial class KDriveClient
    {
        /// <summary>
        /// Downloads a file from kDrive by its ID.
        /// </summary>
        /// <param name="fileId">File ID.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Stream containing the file data.</returns>
        public async Task<Stream> DownloadFileAsync(long fileId, CancellationToken ct = default)
        {
            this.Logger?.LogInformation("Downloading file with ID {FileId} from kDrive.", fileId);
            var response = await SendAsync(KDriveRequestFactory.CreateDownloadRequest(this.DriveId, fileId), ct);
            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
            } catch (Exception ex)
            {
                this.Logger?.LogError(ex, "Failed to download file with ID {FileId} from kDrive.", fileId);
                throw;
            }

            this.Logger?.LogInformation("Successfully downloaded file with ID {FileId} from kDrive.", fileId);

            var ms = new MemoryStream();
            await response.Content.CopyToAsync(ms, ct);
            this.Logger?.LogInformation("File with ID {FileId} downloaded successfully and copied to memory stream.", fileId);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Downloads a file from kDrive by its ID and writes it directly to a destination stream.
        /// </summary>
        /// <param name="fileId">File ID.</param>
        /// <param name="destination">Destination stream to write the file data.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DownloadFileAsync(long fileId, Stream destination, CancellationToken ct = default)
        {
            this.Logger?.LogInformation("Downloading file with ID {FileId} from kDrive to destination stream.", fileId);
            var response = await SendAsync(KDriveRequestFactory.CreateDownloadRequest(this.DriveId, fileId), ct);

            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
            } catch (Exception ex)
            {
                this.Logger?.LogError(ex, "Failed to download file with ID {FileId} from kDrive to destination stream.", fileId);
                throw;
            }
            this.Logger?.LogInformation("Successfully downloaded file with ID {FileId} from kDrive to destination stream.", fileId);

            await response.Content.CopyToAsync(destination, ct);
        }
    }
}