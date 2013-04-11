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
                path = Combine(mockFileDataAccessor.WorkingDirectory, path);

            if (root == "/")
                path = Combine(GetPathRoot(mockFileDataAccessor.WorkingDirectory), path);

            return path;
        }
    }
}