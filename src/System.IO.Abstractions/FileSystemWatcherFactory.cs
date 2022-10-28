using Testably.Abstractions.FileSystem;
using static System.Net.WebRequestMethods;

namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        /// <inheritdoc />
        public IFileSystem FileSystem { get; }

        internal FileSystemWatcherFactory(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <inheritdoc />
        [Obsolete("Use `FileSystemWatcherFactory.New` instead")]
        public IFileSystemWatcher CreateNew()
            => New();

        /// <inheritdoc />
        [Obsolete("Use `FileSystemWatcherFactory.New` instead")]
        public IFileSystemWatcher CreateNew(string path) =>
            New(path);

        /// <inheritdoc />
        [Obsolete("Use `FileSystemWatcherFactory.New` instead")]
        public IFileSystemWatcher CreateNew(string path, string filter)
            => New(path, filter);

        /// <inheritdoc />
        public IFileSystemWatcher New()
        {
            return new FileSystemWatcherWrapper(FileSystem, new FileSystemWatcher());
        }

        /// <inheritdoc />
        public IFileSystemWatcher New(string path)
        {
            return new FileSystemWatcherWrapper(FileSystem, new FileSystemWatcher(path));
        }

        /// <inheritdoc />
        public IFileSystemWatcher New(string path, string filter)
        {
            return new FileSystemWatcherWrapper(FileSystem, new FileSystemWatcher(path, filter));
        }

        /// <inheritdoc />
        public IFileSystemWatcher Wrap(FileSystemWatcher fileSystemWatcher)
        {
            return new FileSystemWatcherWrapper(FileSystem, fileSystemWatcher);
        }
    }
}
