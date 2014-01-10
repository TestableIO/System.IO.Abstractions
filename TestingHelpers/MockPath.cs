namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    ///     PathWrapper calls direct to Path but all this does is string manipulation so we can inherit directly from PathWrapper as no IO is done
    /// </summary>
    [Serializable]
    public class MockPath : PathWrapper
    {
        readonly IMockFileDataAccessor mockFileDataAccessor;

        public MockPath(IMockFileDataAccessor mockFileDataAccessor)
        {
            this.mockFileDataAccessor = mockFileDataAccessor;
        }

        public override string GetFullPath(string path)
        {
            path = path.Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);

            string root = GetPathRoot(path);

            if (root == "")
                path = Combine(mockFileDataAccessor.Directory.GetCurrentDirectory(), path);

            if (root == "/")
                path = Combine(GetPathRoot(mockFileDataAccessor.Directory.GetCurrentDirectory()), path);

            return path;
        }

        public override string GetTempFileName()
        {
            string fileName = mockFileDataAccessor.Path.GetRandomFileName();
            string tempDir = mockFileDataAccessor.Path.GetTempPath();

            string fullPath = mockFileDataAccessor.Path.Combine(tempDir, fileName);

            mockFileDataAccessor.AddFile(fullPath, new MockFileData(String.Empty));

            return fullPath;
        }
    }
}
