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
        [Obsolete("Use `IDirectoryInfoFactory.New(string)` instead")]
        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return New(directoryName);
        }

        /// <inheritdoc />
        public IDirectoryInfo New(string path)
        {
            var realDirectoryInfo = new DirectoryInfo(path);
            return new DirectoryInfoWrapper(fileSystem, realDirectoryInfo);
        }

        /// <inheritdoc />
        public IDirectoryInfo Wrap(DirectoryInfo directoryInfo)
        {
            return new DirectoryInfoWrapper(fileSystem, directoryInfo);
        }
    }
}
