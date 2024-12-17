namespace System.IO.Abstractions
{
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    internal class FileVersionInfoFactory : IFileVersionInfoFactory
    {
        private readonly IFileSystem fileSystem;

        /// <inheritdoc />
        public FileVersionInfoFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <inheritdoc />
        public IFileSystem FileSystem => fileSystem;

        /// <inheritdoc />
        public IFileVersionInfo GetVersionInfo(string fileName)
        {
            Diagnostics.FileVersionInfo fileVersionInfo = Diagnostics.FileVersionInfo.GetVersionInfo(fileName);

            return new FileVersionInfoWrapper(fileVersionInfo);
        }
    }
}
