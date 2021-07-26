namespace System.IO.Abstractions
{
    [Serializable]
    internal class DirectoryInfoFactory : IDirectoryInfoFactory
    {
        private readonly IFileSystem fileSystem;

        /// <inheritdoc />
        public DirectoryInfoFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <inheritdoc />
        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            var realDirectoryInfo = new DirectoryInfo(directoryName);
            return new DirectoryInfoWrapper(fileSystem, realDirectoryInfo);
        }
    }
}
