namespace System.IO.Abstractions
{
    [Serializable]
    internal class DirectoryInfoFactory : IDirectoryInfoFactory
    {
        private readonly IFileSystem fileSystem;

        public DirectoryInfoFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            var realDirectoryInfo = new DirectoryInfo(directoryName);
            return new DirectoryInfoWrapper(fileSystem, realDirectoryInfo);
        }
    }
}
