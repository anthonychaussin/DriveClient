namespace kDriveClient.Models.Domain
{
    /// <summary>
    /// Result of discovering account drives and selecting a working drive id.
    /// </summary>
    public sealed class KDriveBootstrapContext
    {
        /// <summary>Account id used for discovery.</summary>
        public long AccountId { get; init; }

        /// <summary>Selected drive id (preferred or first available).</summary>
        public long DriveId { get; init; }

        /// <summary>Selected drive summary when available.</summary>
        public Models.KDriveDriveSummary? Drive { get; init; }

        /// <summary>All drives returned for the account.</summary>
        public IReadOnlyList<Models.KDriveDriveSummary> Drives { get; init; } = [];
    }
}


