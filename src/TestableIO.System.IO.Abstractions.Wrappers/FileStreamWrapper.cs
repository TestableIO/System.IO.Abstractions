namespace System.IO.Abstractions
{
    internal sealed class FileStreamWrapper : FileSystemStream
    {
        public FileStreamWrapper(FileStream fileStream)
            : base(fileStream, fileStream.Name, fileStream.IsAsync)

        {
            Extensibility = new FileSystemExtensibility(fileStream);
        }

        /// <inheritdoc />
        public override IFileSystemExtensibility Extensibility { get; }
    }
}