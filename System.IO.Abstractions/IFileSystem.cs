namespace System.IO.Abstractions
{
    public interface IFileSystem
    {
        FileBase File { get; }
        DirectoryBase Directory { get; }
        IFileInfoFactory FileInfo { get; }
        IFileStreamFactory FileStream { get; }
        PathBase Path { get; }
        IDirectoryInfoFactory DirectoryInfo { get; }
        IDriveInfoFactory DriveInfo { get; }
    }
}