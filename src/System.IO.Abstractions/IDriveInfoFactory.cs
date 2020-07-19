namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory to create all <see cref="DriveInfoBase"/> for a <see cref="IFileSystem"/>.
    /// </summary>
    public interface IDriveInfoFactory
    {
        /// <summary>
        /// Retrieves the drive names of all logical drives on a computer.
        /// </summary>
        /// <returns>An array of type <see cref="DriveInfoBase"/> that represents the logical drives on a computer.</returns>
        IDriveInfo[] GetDrives();

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveInfoBase"/> class, which acts as a wrapper for a logical drive.
        /// </summary>
        /// <param name="driveName">A valid drive path or drive letter.</param>
        IDriveInfo FromDriveName(string driveName);
    }
}
