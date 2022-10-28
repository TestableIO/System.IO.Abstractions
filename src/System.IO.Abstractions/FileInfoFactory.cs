namespace System.IO.Abstractions
{
    [Serializable]
    internal class FileInfoFactory : IFileInfoFactory
    {
        /// <inheritdoc />
        public IFileSystem FileSystem { get; }

        /// <inheritdoc />
        public FileInfoFactory(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <inheritdoc />
        [Obsolete("Use `FileInfoFactory.New(fileName)` instead")]
        public IFileInfo FromFileName(string fileName)
        {
            return New(fileName);
        }

        /// <inheritdoc />
        public IFileInfo New(string fileName)
        {
            var realFileInfo = new FileInfo(fileName);
            return new FileInfoWrapper(FileSystem, realFileInfo);
        }

        /// <inheritdoc />
        public IFileInfo Wrap(FileInfo fileInfo)
        {
            return new FileInfoWrapper(FileSystem, fileInfo);
        }
    }
}
