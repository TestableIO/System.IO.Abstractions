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
        public IFileInfo FromFileName(string fileName)
        {
            var realFileInfo = new FileInfo(fileName);
            return new FileInfoWrapper(fileSystem, realFileInfo);
        }
    }
}
