namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDirectoryInfoFactory : IDirectoryInfoFactory
    {
        readonly IMockFileDataAccessor mockFileSystem;

        public MockDirectoryInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem;
        }

        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return new MockDirectoryInfo(mockFileSystem, directoryName);
        }
    }
}