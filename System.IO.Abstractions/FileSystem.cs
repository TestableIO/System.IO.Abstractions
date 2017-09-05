namespace System.IO.Abstractions
{
    [Serializable]
    public class FileSystem : IFileSystem
    {
        internal static readonly FileSystem Instance = new FileSystem();

        public IFileSystemInternals Internals { get; }

        public FileSystem()
        {
            Internals = new FileSystemInternals();
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