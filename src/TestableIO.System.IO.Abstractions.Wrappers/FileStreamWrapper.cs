namespace System.IO.Abstractions
{
    internal sealed class FileStreamWrapper : FileSystemStream
    {
        public FileStreamWrapper(FileStream fileStream)
            : base(fileStream, fileStream.Name, fileStream.IsAsync)

        {
        }
    }
}