namespace System.IO.Abstractions
{
    /// <summary>
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// </summary>
        IFile File { get; }

        /// <summary>
        /// </summary>
        IDirectory Directory { get; }

        /// <summary>
        /// </summary>
        IFileInfoFactory FileInfo { get; }

        /// <summary>
        /// </summary>
        IFileStreamFactory FileStream { get; }

        /// <summary>
        /// </summary>
        IPath Path { get; }

        /// <summary>
        /// </summary>
        IDirectoryInfoFactory DirectoryInfo { get; }

        /// <summary>
        /// </summary>
        IDriveInfoFactory DriveInfo { get; }

        /// <summary>
        /// </summary>
        IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}