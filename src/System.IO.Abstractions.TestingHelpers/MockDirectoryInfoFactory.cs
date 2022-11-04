namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
    [Serializable]
    public class MockDirectoryInfoFactory : IDirectoryInfoFactory
    {
        readonly IMockFileDataAccessor mockFileSystem;

        /// <inheritdoc />
        public MockDirectoryInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem;
        }

        /// <inheritdoc />
        [Obsolete("Use `IDirectoryInfoFactory.New(string)` instead")]
        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return New(directoryName);
        }

        /// <inheritdoc />
        public IDirectoryInfo New(string path)
        {
            return new MockDirectoryInfo(mockFileSystem, path);
        }

        /// <inheritdoc />
        public IDirectoryInfo Wrap(DirectoryInfo directoryInfo)
        {
            return new MockDirectoryInfo(mockFileSystem, directoryInfo.Name);
        }
    }
}