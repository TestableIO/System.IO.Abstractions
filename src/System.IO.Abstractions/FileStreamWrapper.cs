using System.Diagnostics.CodeAnalysis;

namespace System.IO.Abstractions;

internal sealed class FileStreamWrapper : FileSystemStream
{
    public FileStreamWrapper(FileStream fileStream)
        : base(fileStream, fileStream.Name, fileStream.IsAsync)

    {
        ExtensionContainer = new FileSystemExtensionContainer(fileStream);
    }

    /// <inheritdoc cref="FileSystemStream.ExtensionContainer" />
    public override IFileSystemExtensionContainer ExtensionContainer
    {
        get;
    }
}