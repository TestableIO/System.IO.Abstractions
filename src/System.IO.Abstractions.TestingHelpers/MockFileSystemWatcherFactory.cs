namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
    [Serializable]
    public class MockFileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        /// <inheritdoc />
        public IFileSystemWatcher CreateNew() =>
                  throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));

        /// <inheritdoc />
        public IFileSystemWatcher CreateNew(string path) =>
                throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));

        /// <inheritdoc />
        public IFileSystemWatcher CreateNew(string path, string filter) =>
                  throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));

        /// <inheritdoc />
        public IFileSystemWatcher FromPath(string path) =>
                  throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));
    }
}
