namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileInfoFactory : IFileInfoFactory
    {
        private readonly IMockFileDataAccessor mockFileSystem;

        public MockFileInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            if (mockFileSystem == null)
            {
                throw new ArgumentNullException("mockFileSystem");
            }

            this.mockFileSystem = mockFileSystem;
        }

        public FileInfoBase FromFileName(string fileName)
        {
            return new MockFileInfo(mockFileSystem, fileName);
        }
    }
}