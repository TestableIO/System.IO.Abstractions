namespace System.IO.Abstractions
{
    public interface IFileSystem
    {
        IFile File { get; }
        IDirectory Directory { get; }
        IFileInfoFactory FileInfo { get; }
        IFileStreamFactory FileStream { get; }
        IPath Path { get; }
        IDirectoryInfoFactory DirectoryInfo { get; }
        IDriveInfoFactory DriveInfo { get; }
        IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}