using kDriveClient.Helpers;
using System.Net.Http.Headers;

namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// KDriveClient extension methods for speed testing and upload strategy initialization.
    /// </summary>
    public partial class KDriveClient
    {
        /// <summary>
        /// Initializes the upload strategy by performing a speed test.
        /// </summary>
        /// <param name="customChunkSize">Optional custom chunk size in bytes. If specified, only DirectUploadThresholdBytes will be calculated from speed test.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task InitializeUploadStrategyAsync(int? customChunkSize = null, CancellationToken ct = default)
        {
            if (customChunkSize != null)
            {
                DynamicChunkSizeBytes = (int)customChunkSize;
                DirectUploadThresholdBytes = (int)customChunkSize;
                this.Logger?.LogInformation("Custom chunk size is provided. Speed test is no longer needed");
                this.Logger?.LogInformation("Upload strategy initialized: DirectUploadThresholdBytes = {DirectUploadThresholdBytes}, DynamicChunkSizeBytes = {DynamicChunkSizeBytes} (custom)",
                    DirectUploadThresholdBytes, DynamicChunkSizeBytes);
                return;
            }

            this.Logger?.LogInformation("Starting upload strategy initialization...");
            var buffer = new byte[1024 * 1024];
            RandomNumberGenerator.Fill(buffer);
            this.Logger?.LogInformation("Generated test Data of size {Size} bytes.", buffer.Length);
            var testFile = new Models.KDriveFile
            {
                Name = "speedtest.dat",
                DirectoryPath = "/Private",
                Content = new ByteArrayContent(buffer)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/octet-stream"),
                        ContentLength = buffer.Length
                    }
                }.ReadAsStream(ct)
            };
            testFile.SplitIntoChunks(buffer.Length);
            this.Logger?.LogInformation("Test file created with {ChunkCount} chunks of size {ChunkSize} bytes.", testFile.Chunks.Count, buffer.Length);

            this.Logger?.LogInformation("Starting upload session for speed test...");
            var (SessionToken, UploadUrl) = await StartUploadSessionAsync(testFile, ct);
            this.Logger?.LogInformation("Upload session started with token: {SessionToken} and URL: {UploadUrl}", SessionToken, UploadUrl);

            this.Logger?.LogInformation("Uploading first chunk of size {ChunkSize} bytes...", testFile.Chunks.First().ChunkSize);
            var chunkRequest = KDriveRequestFactory.CreateChunkUploadRequest(UploadUrl, SessionToken, this.DriveId, testFile.Chunks.First());

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var response = await SendAsync(chunkRequest, ct);
            sw.Stop();
            this.Logger?.LogInformation("Chunk upload completed in {ElapsedMilliseconds} ms.", sw.ElapsedMilliseconds);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                this.Logger?.LogError(ex, "Failed to upload chunk: {Message}", ex.Message);
                throw;
            }

            this.Logger?.LogInformation("Chunk uploaded successfully. Response: {Response}", await response.Content.ReadAsStringAsync(ct));
            this.Logger?.LogInformation("Finalizing upload session...");
            await CancelUploadSessionRequest(SessionToken, ct);
            this.Logger?.LogInformation("Upload session finalized successfully.");

            var speedBytesPerSec = buffer.Length / (sw.ElapsedMilliseconds / 1000.0);

            DirectUploadThresholdBytes = (long)speedBytesPerSec;

            DynamicChunkSizeBytes = (int)(speedBytesPerSec * 0.9);
            this.Logger?.LogInformation("Upload strategy initialized: DirectUploadThresholdBytes = {DirectUploadThresholdBytes}, DynamicChunkSizeBytes = {DynamicChunkSizeBytes} (calculated from speed test)",
                DirectUploadThresholdBytes, DynamicChunkSizeBytes);
        }
    }
}