namespace System.IO.Abstractions
{
    [Serializable]
    internal class DirectoryInfoFactory : IDirectoryInfoFactory
    {
        /// <inheritdoc />
        public IFileSystem FileSystem { get; }

        /// <inheritdoc />
        public DirectoryInfoFactory(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <inheritdoc />
        [Obsolete("Use `DirectoryInfoFactory.New(string)` instead")]
        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return New(directoryName);
        }

        /// <inheritdoc />
        public IDirectoryInfo New(string path)
        {
            var realDirectoryInfo = new DirectoryInfo(path);
            return new DirectoryInfoWrapper(FileSystem, realDirectoryInfo);
        }

        /// <inheritdoc />
        public IDirectoryInfo Wrap(DirectoryInfo directoryInfo)
        {
            return new DirectoryInfoWrapper(FileSystem, directoryInfo);
        }
    }
}
