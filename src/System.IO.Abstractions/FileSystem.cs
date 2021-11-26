namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class FileSystem : IFileSystem
    {
        /// <inheritdoc />
        public FileSystem()
        {
            DriveInfo = new DriveInfoFactory(this);
            DirectoryInfo = new DirectoryInfoFactory(this);
            FileInfo = new FileInfoFactory(this);
            Path = new PathWrapper(this);
            File = new FileWrapper(this);
            Directory = new DirectoryWrapper(this);
            FileStream = new FileStreamFactory();
            FileSystemWatcher = new FileSystemWatcherFactory();
        }

        /// <inheritdoc />
        public IDirectory Directory { get; }

        /// <inheritdoc />
        public IFile File { get; }

        /// <inheritdoc />
        public IFileInfoFactory FileInfo { get; }

        /// <inheritdoc />
        public IFileStreamFactory FileStream { get; }

        /// <inheritdoc />
        public IPath Path { get; }

        /// <inheritdoc />
        public IDirectoryInfoFactory DirectoryInfo { get; }

        /// <inheritdoc />
        public IDriveInfoFactory DriveInfo { get; }

        /// <inheritdoc />
        public IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}
