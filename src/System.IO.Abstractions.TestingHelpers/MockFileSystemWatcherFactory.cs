namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
    [Serializable]
    public class MockFileSystemWatcherFactory : IFileSystemWatcherFactory
    {

        ///
        public MockFileSystemWatcherFactory(MockFileSystem mockFileSystem)
        {
            FileSystem = mockFileSystem;
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
            => throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));

        /// <inheritdoc />
        public IFileSystemWatcher New(string path)
            => throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));

        /// <inheritdoc />
        public IFileSystemWatcher New(string path, string filter)
            => throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));

        /// <inheritdoc />
        public IFileSystemWatcher Wrap(FileSystemWatcher fileSystemWatcher)
            => throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));
    }
}
