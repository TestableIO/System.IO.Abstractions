namespace System.IO.Abstractions
{
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    internal class FileVersionInfoFactory : IFileVersionInfoFactory
    {
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Base factory class for creating a <see cref="IFileVersionInfo"/>
        /// </summary>
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
