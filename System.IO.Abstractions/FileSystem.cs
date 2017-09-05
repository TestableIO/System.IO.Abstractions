namespace System.IO.Abstractions
{
    [Serializable]
    public class FileSystem : IFileSystem
    {
        internal static readonly FileSystem Instance = new FileSystem();
        
        private IFileSystemInternals internals;
        public IFileSystemInternals Internals
        {
            get { return internals; }
        }

        public IFile ParseFile(string fullName)
        {
            return Internals.FileInfo.FromFileName(fullName);
        }

        public IDirectory ParseDirectory(string fullName)
        {
            return Internals.DirectoryInfo.FromDirectoryName(fullName);
        }
    }
}