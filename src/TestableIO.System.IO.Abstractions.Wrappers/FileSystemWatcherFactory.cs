namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        ///
        public FileSystemWatcherFactory(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <inheritdoc />
        public IFileSystem FileSystem { get; }

        /// <inheritdoc />
        [Obsolete("Use `IFileSystemWatcherFactory.New()` instead")]
        public IFileSystemWatcher CreateNew()
            => New();

        /// <inheritdoc />
        [Obsolete("Use `IFileSystemWatcherFactory.New(string)` instead")]
        public IFileSystemWatcher CreateNew(string path)
            => New(path);

        /// <inheritdoc />
        [Obsolete("Use `IFileSystemWatcherFactory.New(string, string)` instead")]
        public IFileSystemWatcher CreateNew(string path, string filter)
            => New(path, filter);

        /// <inheritdoc />
        public IFileSystemWatcher New()
            => new FileSystemWatcherWrapper(FileSystem);

        /// <inheritdoc />
        public IFileSystemWatcher New(string path)
            => new FileSystemWatcherWrapper(FileSystem, path);

        /// <inheritdoc />
        public IFileSystemWatcher New(string path, string filter)
            => new FileSystemWatcherWrapper(FileSystem, path, filter);

        /// <inheritdoc />
        public IFileSystemWatcher Wrap(FileSystemWatcher fileSystemWatcher)
        {
            if (fileSystemWatcher == null)
            {
                return null;
            }

            return new FileSystemWatcherWrapper(FileSystem, fileSystemWatcher);
        }
    }
}
