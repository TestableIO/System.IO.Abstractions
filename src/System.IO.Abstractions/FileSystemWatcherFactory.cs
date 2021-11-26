namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        /// <inheritdoc />
        public IFileSystemWatcher CreateNew()
        {
            return new FileSystemWatcherWrapper();
        }

        /// <inheritdoc />
        public IFileSystemWatcher CreateNew(string path) =>
            new FileSystemWatcherWrapper(path);

        /// <inheritdoc />
        public IFileSystemWatcher CreateNew(string path, string filter)
            => new FileSystemWatcherWrapper(path, filter);
    }
}
