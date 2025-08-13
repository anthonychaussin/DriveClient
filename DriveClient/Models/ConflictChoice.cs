namespace kDriveClient.Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum ConflictChoice
    {
        /// <summary>
        /// Throw an error
        /// </summary>
        Error,
        /// <summary>
        /// Add a new version to file
        /// </summary>
        Version,
        /// <summary>
        /// Create a new file with an automated renaming
        /// </summary>
        Rename,
    }
}
