namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class MockDirectoryInfoFactory : IDirectoryInfoFactory
    {
        readonly IMockFileDataAccessor mockFileSystem;

        /// <inheritdoc />
        public MockDirectoryInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem;
        }

        /// <inheritdoc />
        public IFileSystem FileSystem
            => mockFileSystem;

        /// <inheritdoc />
        public IDirectoryInfo New(string path)
        {
            return new MockDirectoryInfo(mockFileSystem, path);
        }

        /// <inheritdoc />
        public IDirectoryInfo Wrap(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                return null;
            }

            return new MockDirectoryInfo(mockFileSystem, directoryInfo.FullName);
        }
    }
}