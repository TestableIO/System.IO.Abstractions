namespace System.IO.Abstractions
{
#if NET40
    [Serializable]
#endif
    public class FileSystem : IFileSystem
    {
        DirectoryBase directory;
        public DirectoryBase Directory
        {
            get { return directory ?? (directory = new DirectoryWrapper()); }
        }

        FileBase file;
        public FileBase File
        {
            get { return file ?? (file = new FileWrapper()); }
        }

        FileInfoFactory fileInfoFactory;
        public IFileInfoFactory FileInfo
        {
            get { return fileInfoFactory ?? (fileInfoFactory = new FileInfoFactory()); }
        }

        PathBase path;
        public PathBase Path
        {
            get { return path ?? (path = new PathWrapper()); }
        }

        DirectoryInfoFactory directoryInfoFactory;
        public IDirectoryInfoFactory DirectoryInfo
        {
            get { return directoryInfoFactory ?? (directoryInfoFactory = new DirectoryInfoFactory()); }
        }
    }
}