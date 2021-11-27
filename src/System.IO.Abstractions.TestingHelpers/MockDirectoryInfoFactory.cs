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
        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return new MockDirectoryInfo(mockFileSystem, directoryName);
        }
    }
}