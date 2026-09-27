using kDriveClient.Helpers;
using kDriveClient.Models;
using System.Buffers;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// KDriveClient provides methods to download files from the kDrive API.
    /// </summary>
    public partial class KDriveClient
    {
        /// <summary>
        /// Downloads a file from kDrive by its ID into a temporary stream (deleted on dispose).
        /// </summary>
        public Task<Stream> DownloadFileAsync(long fileId, CancellationToken ct = default)
            => DownloadFileAsync(fileId, options: null, ct);

        /// <summary>
        /// Downloads a file from kDrive by its ID into a temporary stream (deleted on dispose).
        /// </summary>
        public async Task<Stream> DownloadFileAsync(long fileId, KDriveDownloadOptions? options, CancellationToken ct = default)
        {
            this.Logger?.LogInformation("Downloading file with ID {FileId} from kDrive to destination stream.", fileId);
            var response = await SendDownloadAsync(fileId, options, ct).ConfigureAwait(false);

            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, "Failed to download file with ID {FileId} from kDrive to destination stream.", fileId);
                throw;
            }

            this.Logger?.LogInformation("Successfully downloaded file with ID {FileId} from kDrive to destination stream.", fileId);
            return await TempFileStream.CreateFromContentAsync(response.Content, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads a file from kDrive by its ID and writes it to a path.
        /// </summary>
        public Task DownloadFileAsync(long fileId, string filePath, CancellationToken ct = default)
            => DownloadFileAsync(fileId, filePath, options: null, ct);

        /// <summary>
        /// Downloads a file from kDrive by its ID and writes it to a path.
        /// </summary>
        public async Task DownloadFileAsync(long fileId, string filePath, KDriveDownloadOptions? options, CancellationToken ct = default)
        {
            this.Logger?.LogInformation("Downloading file with ID {FileId} from kDrive to {FilePath}.", fileId, filePath);
            var response = await SendDownloadAsync(fileId, options, ct).ConfigureAwait(false);

            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, "Failed to download file with ID {FileId} from kDrive to {FilePath}.", fileId, filePath);
                throw;
            }

            this.Logger?.LogInformation("Successfully downloaded file with ID {FileId} from kDrive to {FilePath}.", fileId, filePath);
            await SaveStreamAsync(response.Content, filePath, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads a built archive ZIP by its UUID.
        /// </summary>
        public async Task<Stream> DownloadArchiveAsync(string archiveUuid, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(archiveUuid);
            var response = await SendAsync(KDriveRequestFactory.CreateArchiveDownloadRequest(this.DriveId, archiveUuid), ct).ConfigureAwait(false);
            return await TempFileStream.CreateFromContentAsync(response.Content, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads a share-link archive ZIP by share UUID and archive UUID.
        /// </summary>
        public async Task<Stream> DownloadShareLinkArchiveAsync(string sharelinkUuid, string archiveUuid, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sharelinkUuid);
            ArgumentException.ThrowIfNullOrWhiteSpace(archiveUuid);
            var response = await SendAsync(KDriveRequestFactory.CreateShareLinkArchiveDownloadRequest(this.DriveId, sharelinkUuid, archiveUuid), ct).ConfigureAwait(false);
            return await TempFileStream.CreateFromContentAsync(response.Content, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads a file preview stream.
        /// </summary>
        public async Task<Stream> DownloadFilePreviewAsync(long fileId, CancellationToken ct = default)
        {
            var response = await SendAsync(KDriveRequestFactory.CreateFilePreviewRequest(this.DriveId, fileId), ct).ConfigureAwait(false);
            return await TempFileStream.CreateFromContentAsync(response.Content, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads a file thumbnail stream.
        /// </summary>
        public async Task<Stream> DownloadFileThumbnailAsync(long fileId, CancellationToken ct = default)
        {
            var response = await SendAsync(KDriveRequestFactory.CreateFileThumbnailRequest(this.DriveId, fileId), ct).ConfigureAwait(false);
            return await TempFileStream.CreateFromContentAsync(response.Content, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads a trashed item thumbnail stream.
        /// </summary>
        public async Task<Stream> DownloadTrashThumbnailAsync(long fileId, CancellationToken ct = default)
        {
            var response = await SendAsync(KDriveRequestFactory.CreateTrashThumbnailRequest(this.DriveId, fileId), ct).ConfigureAwait(false);
            return await TempFileStream.CreateFromContentAsync(response.Content, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads a specific file version stream (v2).
        /// </summary>
        public async Task<Stream> DownloadFileVersionAsync(long fileId, long versionId, CancellationToken ct = default)
        {
            var response = await SendAsync(KDriveRequestFactory.CreateFileVersionDownloadRequest(this.DriveId, fileId, versionId), ct).ConfigureAwait(false);
            return await TempFileStream.CreateFromContentAsync(response.Content, ct).ConfigureAwait(false);
        }

        async Task<HttpResponseMessage> SendDownloadAsync(long fileId, KDriveDownloadOptions? options, CancellationToken ct)
        {
            if (options?.UseTemporaryUrl == true)
            {
                var duration = options.TemporaryUrlDurationSeconds;
                var temp = await EndpointsService.GetFileTemporaryUrlAsync(fileId, duration, ct).ConfigureAwait(false);
                var url = temp?.Data?.TemporaryUrl;
                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException($"Temporary URL was empty for file {fileId}.");

                using var req = new HttpRequestMessage(HttpMethod.Get, url);
                if (!string.IsNullOrEmpty(options.Password))
                    req.Headers.TryAddWithoutValidation("x-kdrive-file-password", options.Password);

                return await SendAsync(req, ct).ConfigureAwait(false);
            }

            using var downloadReq = KDriveRequestFactory.CreateDownloadRequest(
                this.DriveId,
                fileId,
                convertAs: options?.ConvertAs,
                password: options?.Password);
            return await SendAsync(downloadReq, ct).ConfigureAwait(false);
        }

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
