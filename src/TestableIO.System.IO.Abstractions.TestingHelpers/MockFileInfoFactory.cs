namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
    [Serializable]
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
        [Obsolete("Use `IFileInfoFactory.New(string)` instead")]
        public IFileInfo FromFileName(string fileName)
        {
            return New(fileName);
        }

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

            return new MockFileInfo(mockFileSystem, fileInfo.Name);
        }
    }
}