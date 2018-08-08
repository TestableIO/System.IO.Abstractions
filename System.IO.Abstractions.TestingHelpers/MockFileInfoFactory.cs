namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileInfoFactory : IFileInfoFactory
    {
        private readonly IMockFileDataAccessor mockFileSystem;

        public MockFileInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
        }

        public FileInfoBase FromFileName(string fileName)
        {
            return new MockFileInfo(mockFileSystem, fileName);
        }
    }
}