namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFileInfoFactory : IFileInfoFactory
    {
        readonly IMockFileDataAccessor mockFileSystem;
        public FileInfoBase FileInfo { get; set; }
        public MockFileInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem;
        }

        public FileInfoBase FromFileName(string fileName)
        {
            FileInfo = new MockFileInfo(mockFileSystem, fileName);
            return FileInfo;
        }
    }
}