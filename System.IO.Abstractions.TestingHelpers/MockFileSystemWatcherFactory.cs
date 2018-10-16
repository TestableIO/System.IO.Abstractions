namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;

        public MockFileSystemWatcherFactory(IMockFileDataAccessor mockFileDataAccessor) =>
            this.mockFileDataAccessor = mockFileDataAccessor;

        public FileSystemWatcherBase CreateNew() =>
            new MockFileSystemWatcher(mockFileDataAccessor);

        public FileSystemWatcherBase FromPath(string path) =>
            new MockFileSystemWatcher(mockFileDataAccessor, path);
    }
}
