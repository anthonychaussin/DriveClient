namespace kDriveClient.Helpers
{
    /// <summary>
    /// File stream backed by a temporary file that is deleted when disposed.
    /// </summary>
    internal sealed class TempFileStream : FileStream
    {
        private readonly string _path;
        private int _disposed;

        private TempFileStream(string path)
            : base(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 1024 * 256,
                options: FileOptions.Asynchronous | FileOptions.SequentialScan | FileOptions.DeleteOnClose)
        {
            _path = path;
        }

        public static async Task<TempFileStream> CreateFromContentAsync(HttpContent content, CancellationToken ct)
        {
            var path = Path.Combine(Path.GetTempPath(), $"kdrive-{Guid.NewGuid():N}.tmp");
            await using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None,
                             bufferSize: 1024 * 256, options: FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                await using var stream = await content.ReadAsStreamAsync(ct).ConfigureAwait(false);
                await stream.CopyToAsync(fs, ct).ConfigureAwait(false);
            }

            return new TempFileStream(path);
        }

        /// <summary>Opens an existing temp file that will be deleted on dispose.</summary>
        public static TempFileStream OpenExisting(string path) => new(path);

        protected override void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
            {
                base.Dispose(disposing);
                return;
            }

            try
            {
                base.Dispose(disposing);
            }
            finally
            {
                try
                {
                    if (File.Exists(_path))
                        File.Delete(_path);
                }
                catch
                {
                    // Best-effort cleanup if DeleteOnClose did not run.
                }
            }
        }
    }
}
