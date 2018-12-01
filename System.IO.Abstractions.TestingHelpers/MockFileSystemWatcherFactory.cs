namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        public FileSystemWatcherBase CreateNew() =>
            throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));

        public FileSystemWatcherBase FromPath(string path) =>
            throw new NotImplementedException(StringResources.Manager.GetString("FILE_SYSTEM_WATCHER_NOT_IMPLEMENTED_EXCEPTION"));
    }
}
