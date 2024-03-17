namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
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
        {
            if (fileSystemWatcher == null)
            {
                return null;
            }

            throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));
        }
    }
}
