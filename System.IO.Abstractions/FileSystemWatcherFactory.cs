namespace System.IO.Abstractions
{
    [Serializable]
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        public IFileSystemWatcher CreateNew()
        {
            return new FileSystemWatcherWrapper();
        }

        public IFileSystemWatcher CreateNew(string path)
        {
            return new FileSystemWatcherWrapper(path);
        }

        public IFileSystemWatcher CreateNew(string path, string filter)
        {
            return new FileSystemWatcherWrapper(path, filter);
        }

        public IFileSystemWatcher FromPath(string path)
        {
            return new FileSystemWatcherWrapper(path);
        }
    }
}
