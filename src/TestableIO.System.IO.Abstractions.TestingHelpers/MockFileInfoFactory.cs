namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class MockFileInfoFactory : IFileInfoFactory
    {
        private readonly IMockFileDataAccessor mockFileSystem;

        /// <inheritdoc />
        public MockFileInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
        }

        /// <inheritdoc />
        public IFileSystem FileSystem
            => mockFileSystem;

        /// <inheritdoc />
        public IFileInfo New(string fileName)
        {
            return new MockFileInfo(mockFileSystem, fileName);
        }

        /// <inheritdoc />
        public IFileInfo Wrap(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return null;
            }

            return new MockFileInfo(mockFileSystem, fileInfo.FullName);
        }
    }
}