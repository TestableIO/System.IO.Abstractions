namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
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
            => new FileSystemWatcherWrapper();

        /// <inheritdoc />
        public IFileSystemWatcher New(string path)
            => new FileSystemWatcherWrapper(path);

        /// <inheritdoc />
        public IFileSystemWatcher New(string path, string filter)
            => new FileSystemWatcherWrapper(path, filter);

        /// <inheritdoc />
        public IFileSystemWatcher Wrap(FileSystemWatcher fileSystemWatcher)
            => new FileSystemWatcherWrapper(fileSystemWatcher);
    }
}
