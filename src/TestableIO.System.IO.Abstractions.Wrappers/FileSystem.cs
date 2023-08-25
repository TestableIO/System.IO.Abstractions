namespace System.IO.Abstractions
{
    /// <inheritdoc />
#if !NET8_0_OR_GREATER
    [Serializable]
#endif
    public class FileSystem : FileSystemBase
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
            FileStream = new FileStreamFactory(this);
            FileSystemWatcher = new FileSystemWatcherFactory(this);
        }

        /// <inheritdoc />
        public override IDirectory Directory { get; }

        /// <inheritdoc />
        public override IFile File { get; }

        /// <inheritdoc />
        public override IFileInfoFactory FileInfo { get; }

        /// <inheritdoc />
        public override IFileStreamFactory FileStream { get; }

        /// <inheritdoc />
        public override IPath Path { get; }

        /// <inheritdoc />
        public override IDirectoryInfoFactory DirectoryInfo { get; }

        /// <inheritdoc />
        public override IDriveInfoFactory DriveInfo { get; }

        /// <inheritdoc />
        public override IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}
