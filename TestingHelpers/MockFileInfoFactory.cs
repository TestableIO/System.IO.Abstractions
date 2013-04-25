namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileInfoFactory : IFileInfoFactory
    {
        readonly IMockFileDataAccessor mockFileSystem;

        public MockFileInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem;
        }

        public FileInfoBase FromFileName(string fileName)
        {
            return new MockFileInfo(mockFileSystem, fileName);
        }
    }
}