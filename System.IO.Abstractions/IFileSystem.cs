namespace System.IO.Abstractions
{
    public interface IFileSystem
    {
        IFileSystemInternals Internals { get; }
        IFile ParseFile(string fullName);
        IDirectory ParseDirectory(string fullName);
    }
}