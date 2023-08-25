namespace System.IO.Abstractions
{
    /// <inheritdoc />
#if !NET8_0_OR_GREATER
    [Serializable]
#endif
    public abstract class FileSystemBase : IFileSystem
    {
        /// <inheritdoc />
        public abstract IDirectory Directory { get; }

        /// <inheritdoc />
        public abstract IFile File { get; }

        /// <inheritdoc />
        public abstract IFileInfoFactory FileInfo { get; }

        /// <inheritdoc />
        public abstract IFileStreamFactory FileStream { get; }

        /// <inheritdoc />
        public abstract IPath Path { get; }

        /// <inheritdoc />
        public abstract IDirectoryInfoFactory DirectoryInfo { get; }

        /// <inheritdoc />
        public abstract IDriveInfoFactory DriveInfo { get; }

        /// <inheritdoc />
        public abstract IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}
