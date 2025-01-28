namespace System.IO.Abstractions
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        /// <summary>
        /// Base factory class for creating a <see cref="IFileSystemWatcher"/>
        /// </summary>
        public FileSystemWatcherFactory(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <inheritdoc />
        public IFileSystem FileSystem { get; }

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
