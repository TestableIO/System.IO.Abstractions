namespace System.IO.Abstractions
{
    [Serializable]
    internal class FileInfoFactory : IFileInfoFactory
    {
        private readonly IFileSystem fileSystem;

        /// <inheritdoc />
        public FileInfoFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <inheritdoc />
        [Obsolete("Use `IFileInfoFactory.New(string)` instead")]
        public IFileInfo FromFileName(string fileName)
        {
            return New(fileName);
        }

        /// <inheritdoc />
        public IFileInfo New(string fileName)
        {
            var realFileInfo = new FileInfo(fileName);
            return new FileInfoWrapper(fileSystem, realFileInfo);
        }

        /// <inheritdoc />
        public IFileInfo Wrap(FileInfo fileInfo)
        {
            return new FileInfoWrapper(fileSystem, fileInfo);
        }
    }
}
