using kDriveClient.Helpers;
using System;
using System.Buffers;
using System.Net;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// KDriveClient provides methods to download files from the kDrive API.
    /// </summary>
    public partial class KDriveClient
    {
        /// <summary>
        /// Downloads a file from kDrive by its ID and writes it directly to a destination stream.
        /// </summary>
        /// <param name="fileId">File ID.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task<Stream> DownloadFileAsync(long fileId, CancellationToken ct = default)
        {
            this.Logger?.LogInformation("Downloading file with ID {FileId} from kDrive to destination stream.", fileId);
            var response = await SendAsync(KDriveRequestFactory.CreateDownloadRequest(this.DriveId, fileId), ct);

            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, "Failed to download file with ID {FileId} from kDrive to destination stream.", fileId);
                throw;
            }
            this.Logger?.LogInformation("Successfully downloaded file with ID {FileId} from kDrive to destination stream.", fileId);

            var tempFile = Path.GetTempFileName();
            await SaveStreamAsync(response.Content, tempFile, ct);
            return File.OpenRead(tempFile);
        }

        /// <summary>
        /// Downloads a file from kDrive by its ID and writes it directly to a destination stream.
        /// </summary>
        /// <param name="fileId">File ID.</param>
        /// <param name="filePath">File path to save the downloaded file.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DownloadFileAsync(long fileId, string filePath, CancellationToken ct = default)
        {
            this.Logger?.LogInformation("Downloading file with ID {FileId} from kDrive to destination stream.", fileId);
            var response = await SendAsync(KDriveRequestFactory.CreateDownloadRequest(this.DriveId, fileId), ct);

            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, "Failed to download file with ID {FileId} from kDrive to destination stream.", fileId);
                throw;
            }
            this.Logger?.LogInformation("Successfully downloaded file with ID {FileId} from kDrive to destination stream.", fileId);

            await SaveStreamAsync(response.Content, filePath, ct);
        }

        /// <summary>
        /// Saves the content of an HttpContent to a file asynchronously.
        /// </summary>
        /// <param name="content">Content to save.</param>
        /// <param name="path">Path to save the content.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        static async Task SaveStreamAsync(HttpContent content, string path, CancellationToken ct)
        {
            await using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 1024 * 256, options: FileOptions.Asynchronous | FileOptions.SequentialScan);

            using var stream = await content.ReadAsStreamAsync(ct).ConfigureAwait(false);

            var pool = ArrayPool<byte>.Shared;
            var buffer = pool.Rent(1024 * 256);
            try
            {
                int read;
                while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct).ConfigureAwait(false)) > 0)
                    await fs.WriteAsync(buffer.AsMemory(0, read), ct).ConfigureAwait(false);
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}