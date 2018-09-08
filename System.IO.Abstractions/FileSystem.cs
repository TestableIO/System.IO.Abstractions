namespace System.IO.Abstractions
{
    [Serializable]
    public class FileSystem : IFileSystem
    {
        public FileSystem()
        {
            DriveInfo = new DriveInfoFactory(this);
            DirectoryInfo = new DirectoryInfoFactory(this);
            FileInfo = new FileInfoFactory(this);
            Path = new PathWrapper(this);
            File = new FileWrapper(this);
            Directory = new DirectoryWrapper(this);
            FileStream = new FileStreamFactory();
            FileSystemWatcher = new FileSystemWatcherFactory();
        }

        public DirectoryBase Directory { get; }

        public FileBase File { get; }

        public IFileInfoFactory FileInfo { get; }

        public IFileStreamFactory FileStream { get; }

        public PathBase Path { get; }

        public IDirectoryInfoFactory DirectoryInfo { get; }

        public IDriveInfoFactory DriveInfo { get; }

        public IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}
