namespace System.IO.Abstractions.TestingHelpers
{
#if NET40
    [Serializable]
#endif
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