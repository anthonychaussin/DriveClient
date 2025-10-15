using kDriveClient.Helpers;
using kDriveClient.Models;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// KDriveClient class provides methods to upload files to the kDrive service.
    /// </summary>
    public partial class KDriveClient
    {
        /// <summary>
        /// Uploads a file directly to kDrive in one request (for small files).
        /// </summary>
        /// <param name="file"><see cref="KDriveFile"/></param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns><see cref="KDriveUploadResponse"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<KDriveUploadResponse> UploadFileDirectAsync(KDriveFile file, CancellationToken ct = default)
        {
            if (file.Content == null)
            {
                this.Logger?.LogError("File content is null or empty");
                throw new ArgumentException("File content is required", nameof(file));
            }
            else if (string.IsNullOrWhiteSpace(file.Name))
            {
                this.Logger?.LogError("File name is null or empty");
                throw new ArgumentException("File name is required", nameof(file));
            }
            else if (file.TotalSize <= 0)
            {
                this.Logger?.LogError("File size is less than or equal to 0");
                throw new ArgumentException("File size must be greater than 0", nameof(file));
            }

            this.Logger?.LogInformation("Starting direct upload for file '{FileName}' with size {FileSize} bytes...", file.Name, file.TotalSize);
            var response = await SendAsync(KDriveRequestFactory.CreateUploadDirectRequest(DriveId, file), ct);

            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
            }
            catch (HttpRequestException ex)
            {
                this.Logger?.LogError(ex, "Failed to upload file '{FileName}' directly", file.Name);
                throw;
            }

            this.Logger?.LogInformation("Direct upload for file '{FileName}' completed successfully.", file.Name);
            return KDriveJsonHelper.DeserializeUploadResponse(await response.Content.ReadAsStringAsync(ct));
        }

        /// <summary>
        /// Uploads a file to kDrive in chunks (for large files).
        /// </summary>
        /// <param name="file"><see cref="KDriveFile"/></param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns><see cref="KDriveUploadResponse"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<KDriveUploadResponse> UploadFileChunkedAsync(KDriveFile file, CancellationToken ct = default)
        {
            if (file.Content == null)
            {
                this.Logger?.LogError("File content is null or empty");
                throw new ArgumentException("File content is required", nameof(file));
            }

            this.Logger?.LogInformation("Starting chunked upload for file '{FileName}' with size {FileSize} bytes...", file.Name, file.TotalSize);
            var (sessionToken, uploadUrl) = await StartUploadSessionAsync(file, ct);
            this.Logger?.LogInformation("Upload session started with token '{SessionToken}' and URL '{UploadUrl}' for file '{FileName}'.", sessionToken, uploadUrl, file.Name);
            try
            {
                await Parallel.ForEachAsync(file.Chunks, new ParallelOptions
                {
                    MaxDegreeOfParallelism = Parallelism,
                    CancellationToken = ct
                }, async (chunk, token) =>
                {
                    this.Logger?.LogInformation("Uploading chunk {ChunkNumber}/{TotalChunks} for file '{FileName}'...", chunk.ChunkNumber + 1, file.Chunks.Count, file.Name);
                    await UploadChunkAsync(uploadUrl, sessionToken, chunk, file, token);
                    chunk.Clean();
                });

                this.Logger?.LogInformation("All chunks uploaded successfully for file '{FileName}'.", file.Name);
                return await FinishUploadSessionAsync(sessionToken, file.TotalChunkHash, ct);
            }
            catch (Exception ex)
            {
                this.Logger?.LogError("Error while uploading file: {error}.", ex.Message);
                await this.CancelUploadSessionRequest(sessionToken, ct);
                throw;
            }
        }

        /// <summary>
        /// Starts an upload session for a file.
        /// </summary>
        /// <param name="file"><see cref="KDriveFile"/></param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Session token and base url for upload</returns>
        private async Task<(string SessionToken, string UploadUrl)> StartUploadSessionAsync(KDriveFile file, CancellationToken ct)
        {
            this.Logger?.LogInformation("Starting upload session for file '{FileName}' with size {FileSize} bytes...", file.Name, file.TotalSize);
            var response = await SendAsync(KDriveRequestFactory.CreateStartSessionRequest(this.DriveId, file), ct);
            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
            }
            catch (HttpRequestException ex)
            {
                this.Logger?.LogError(ex, "Failed to start upload session for file '{FileName}'", file.Name);
                throw;
            }

            this.Logger?.LogInformation("Upload session for file '{FileName}' started successfully.", file.Name);
            return KDriveJsonHelper.ParseStartSessionResponse(await response.Content.ReadAsStringAsync(ct));
        }

        /// <summary>
        /// Uploads a single chunk to the kDrive service.
        /// </summary>
        /// <param name="uploadUrl">Base url to upload files</param>
        /// <param name="sessionToken">Session token</param>
        /// <param name="chunk"><see cref="KDriveChunk"/></param>
        /// <param name="file"><see cref="KDriveFile"/></param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task UploadChunkAsync(string uploadUrl, string sessionToken, KDriveChunk chunk, KDriveFile file, CancellationToken ct)
        {
            this.Logger?.LogInformation("Uploading chunk {ChunkNumber}/{TotalChunks} for file '{FileName}' with size {ChunkSize} bytes...",
                chunk.ChunkNumber + 1, file.Chunks.Count, file.Name, chunk.ChunkSize);

            var response = await SendAsync(KDriveRequestFactory.CreateChunkUploadRequest(uploadUrl, sessionToken, this.DriveId, chunk), ct);
            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct);

                chunk.Clean();
            }
            catch (HttpRequestException ex)
            {
                this.Logger?.LogError(ex, "Failed to upload chunk {ChunkNumber} for file '{FileName}'", chunk.ChunkNumber + 1, file.Name);
                throw;
            }

            var uploaded = (chunk.ChunkNumber + 1) * chunk.ChunkSize;
            var percent = Math.Min(100.0, uploaded * 100.0 / file.TotalSize);
            Progress?.Report(percent);
            this.Logger?.LogInformation("Chunk {ChunkNumber}/{TotalChunks} for file '{FileName}' uploaded successfully. Uploaded {UploadedSize} bytes ({Percent}%).",
                chunk.ChunkNumber + 1, file.Chunks.Count, file.Name, uploaded, percent);
        }

        /// <summary>
        /// Closes the upload session and finalizes the upload.
        /// </summary>
        /// <param name="sessionToken">Session token</param>
        /// <param name="totalChunkHash">Total chunk hash to check that all file was uploaded without issue</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns><see cref="KDriveUploadResponse"/></returns>
        private async Task<KDriveUploadResponse> FinishUploadSessionAsync(string sessionToken, string totalChunkHash, CancellationToken ct)
        {
            this.Logger?.LogInformation("Finishing upload session with token '{SessionToken}' and total chunk hash '{TotalChunkHash}'...", sessionToken, totalChunkHash);
            var response = await SendAsync(KDriveRequestFactory.CreateFinishSessionRequest(DriveId, sessionToken, totalChunkHash), ct);
            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
            }
            catch (HttpRequestException ex)
            {
                this.Logger?.LogError(ex, "Failed to finish upload session with token '{SessionToken}'", sessionToken);
                throw;
            }

            this.Logger?.LogInformation("Upload session with token '{SessionToken}' finished successfully.", sessionToken);
            return KDriveJsonHelper.DeserializeUploadResponse(await response.Content.ReadAsStringAsync(ct));
        }

        /// <summary>
        /// Cancels an upload session.
        /// </summary>
        /// <param name="sessionToken">Session token</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns></returns>
        private async Task<Object> CancelUploadSessionRequest(string sessionToken, CancellationToken ct)
        {
            this.Logger?.LogInformation("Cancelling upload session with token '{SessionToken}'...", sessionToken);
            var response = await SendAsync(KDriveRequestFactory.CreateCancelSessionRequest(DriveId, sessionToken), ct);
            try
            {
                response = await KDriveJsonHelper.DeserializeResponseAsync(response, ct);
            }
            catch (HttpRequestException ex)
            {
                this.Logger?.LogError(ex, "Failed to cancel upload session with token '{SessionToken}'", sessionToken);
                throw;
            }

            this.Logger?.LogInformation("Upload session with token '{SessionToken}' cancelled successfully.", sessionToken);
            return await response.Content.ReadAsStringAsync(ct);
        }
    }
}