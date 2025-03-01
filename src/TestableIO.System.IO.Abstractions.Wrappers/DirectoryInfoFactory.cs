namespace System.IO.Abstractions;
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
internal class DirectoryInfoFactory : IDirectoryInfoFactory
{
    private readonly IFileSystem fileSystem;

    /// <summary>
    /// Base factory class for creating a <see cref="IDirectoryInfo"/>
    /// </summary>
    public DirectoryInfoFactory(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }

    /// <inheritdoc />
    public IFileSystem FileSystem
        => fileSystem;

    /// <inheritdoc />
    public IDirectoryInfo New(string path)
    {
        var realDirectoryInfo = new DirectoryInfo(path);
        return new DirectoryInfoWrapper(fileSystem, realDirectoryInfo);
    }
        
    /// <inheritdoc />
    public IDirectoryInfo Wrap(DirectoryInfo directoryInfo)
    {
        if (directoryInfo == null)
        {
            return null;
        }

        return new DirectoryInfoWrapper(fileSystem, directoryInfo);
    }
}