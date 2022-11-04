namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory for the creation of wrappers for <see cref="DriveInfo" /> in a <see cref="IFileSystem" />.
    /// </summary>
    public interface IDriveInfoFactory
    {
        /// <summary>
        /// Retrieves the drive names of all logical drives on a computer.
        /// </summary>
        /// <returns>An array of type <see cref="IDriveInfo"/> that represents the logical drives on a computer.</returns>
        IDriveInfo[] GetDrives();

        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="DriveInfo"/> which implements <see cref="IDriveInfo"/>.
        /// </summary>
        /// <param name="driveName">A valid drive path or drive letter.</param>
        IDriveInfo FromDriveName(string driveName);
    }
}
