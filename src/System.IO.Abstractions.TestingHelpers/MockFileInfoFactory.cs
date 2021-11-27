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
        public IFileInfo FromFileName(string fileName)
        {
            return new MockFileInfo(mockFileSystem, fileName);
        }
    }
}