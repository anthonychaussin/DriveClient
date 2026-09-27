namespace kDriveClient.kDriveClient
{
    /// <summary>
    /// Aggregate kDrive client surface (upload, download, endpoints, smart helpers).
    /// </summary>
    public partial interface IKDriveClient
        : IKDriveUpload,
          IKDriveDownload,
          IKDriveFiles,
          IKDriveShares,
          IKDriveComments,
          IKDriveCategories,
          IKDriveDrive,
          IKDriveSmart
    {
    }
}
