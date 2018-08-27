namespace System.IO.Abstractions
{
    [Serializable]
    public class FileSystem : IFileSystem
    {
        public FileSystem()
        {
            driveInfoFactory = new Lazy<DriveInfoFactory>(() => new DriveInfoFactory(this));;
        }

        private DirectoryBase directory;

        public DirectoryBase Directory
        {
            get { return directory ?? (directory = new DirectoryWrapper(this)); }
        }

        FileBase file;
        public FileBase File
        {
            get { return file ?? (file = new FileWrapper(this)); }
        }

        FileInfoFactory fileInfoFactory;
        public IFileInfoFactory FileInfo
        {
            get { return fileInfoFactory ?? (fileInfoFactory = new FileInfoFactory(this)); }
        }

        FileStreamFactory fileStreamFactory;
        public IFileStreamFactory FileStream
        {
            get { return fileStreamFactory ?? (fileStreamFactory = new FileStreamFactory()); }
        }

        PathBase path;
        public PathBase Path
        {
            get { return path ?? (path = new PathWrapper(this)); }
        }

        DirectoryInfoFactory directoryInfoFactory;
        public IDirectoryInfoFactory DirectoryInfo
        {
            get { return directoryInfoFactory ?? (directoryInfoFactory = new DirectoryInfoFactory(this)); }
        }

        private readonly Lazy<DriveInfoFactory> driveInfoFactory;
        public IDriveInfoFactory DriveInfo
        {
            get { return driveInfoFactory.Value; }
        }

        private IFileSystemWatcherFactory fileSystemWatcherFactory;
        public IFileSystemWatcherFactory FileSystemWatcher
        {
            get { return fileSystemWatcherFactory ?? (fileSystemWatcherFactory = new FileSystemWatcherFactory()); }
        }
    }
}
