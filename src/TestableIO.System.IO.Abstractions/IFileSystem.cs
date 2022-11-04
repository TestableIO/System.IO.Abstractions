namespace System.IO.Abstractions
{
    /// <summary>
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Abstraction for static methods in <see cref="System.IO.File" />.
        /// </summary>
        IFile File { get; }

        /// <summary>
        /// Abstraction for static methods in <see cref="System.IO.Directory" />.
        /// </summary>
        IDirectory Directory { get; }

        /// <summary>
        /// A factory for the creation of wrappers for <see cref="System.IO.FileInfo" />.
        /// </summary>
        IFileInfoFactory FileInfo { get; }

        /// <summary>
        /// A factory for the creation of wrappers for <see cref="System.IO.FileStream" />.
        /// </summary>
        IFileStreamFactory FileStream { get; }

        /// <summary>
        /// Abstraction for static methods and properties in <see cref="System.IO.Path" />.
        /// </summary>
        IPath Path { get; }

        /// <summary>
        /// A factory for the creation of wrappers for <see cref="System.IO.DirectoryInfo" />.
        /// </summary>
        IDirectoryInfoFactory DirectoryInfo { get; }

        /// <summary>
        /// A factory for the creation of wrappers for <see cref="System.IO.DriveInfo" />.
        /// </summary>
        IDriveInfoFactory DriveInfo { get; }

        /// <summary>
        /// A factory for the creation of wrappers for <see cref="System.IO.FileSystemWatcher" />.
        /// </summary>
        IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}