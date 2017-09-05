namespace System.IO.Abstractions
{
    public interface IFileSystemEntry
    {
        FileAttributes Attributes { get; set; }
        DateTime CreationTime { get; set; }
        DateTime CreationTimeUtc { get; set; }
        bool Exists { get; }
        string Extension { get; }
        IFileSystem FileSystem { get; }
        string FullName { get; }
        DateTime LastAccessTime { get; set; }
        DateTime LastAccessTimeUtc { get; set; }
        DateTime LastWriteTime { get; set; }
        DateTime LastWriteTimeUtc { get; set; }
        string Name { get; }

        void Delete();
        void Refresh();
    }
}