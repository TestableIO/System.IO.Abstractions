namespace System.IO.Abstractions
{
    public interface IDriveInfo
    {
        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        IFileSystem FileSystem { get; }
        /// <inheritdoc cref="DriveInfo.AvailableFreeSpace"/>
        long AvailableFreeSpace { get; }
        /// <inheritdoc cref="DriveInfo.DriveFormat"/>
        string DriveFormat { get; }
        /// <inheritdoc cref="DriveInfo.DriveType"/>
        DriveType DriveType { get; }
        /// <inheritdoc cref="DriveInfo.IsReady"/>
        bool IsReady { get; }
        /// <inheritdoc cref="DriveInfo.Name"/>
        string Name { get; }
        /// <inheritdoc cref="DriveInfo.RootDirectory"/>
        IDirectoryInfo RootDirectory { get; }
        /// <inheritdoc cref="DriveInfo.TotalFreeSpace"/>
        long TotalFreeSpace { get; }
        /// <inheritdoc cref="DriveInfo.TotalSize"/>
        long TotalSize { get; }
        /// <inheritdoc cref="DriveInfo.VolumeLabel"/>
        string VolumeLabel { get; set; }
    }
}