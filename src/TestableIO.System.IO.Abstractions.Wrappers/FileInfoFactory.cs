namespace System.IO.Abstractions;
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
internal class FileInfoFactory : IFileInfoFactory
{
    private readonly IFileSystem fileSystem;

    /// <summary>
    /// Base factory class for creating a <see cref="IFileInfo"/>
    /// </summary>
    public FileInfoFactory(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }

    /// <inheritdoc />
    public IFileSystem FileSystem
        => fileSystem;


    /// <inheritdoc />
    public IFileInfo New(string fileName)
    {
        var realFileInfo = new FileInfo(fileName);
        return new FileInfoWrapper(fileSystem, realFileInfo);
    }
        
    /// <inheritdoc />
    public IFileInfo Wrap(FileInfo fileInfo)
    {
        if (fileInfo == null)
        {
            return null;
        }

        return new FileInfoWrapper(fileSystem, fileInfo);
    }
}