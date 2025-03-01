using System.Runtime.Versioning;

namespace System.IO.Abstractions;

/// <inheritdoc cref="FileSystemInfo"/>
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
public abstract class FileSystemInfoBase : IFileSystemInfo
{
    /// <summary>
    /// Base class for calling methods of <see cref="FileSystemInfo"/>
    /// </summary>
    protected FileSystemInfoBase(IFileSystem fileSystem)
    {
        FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    [Obsolete("This constructor only exists to support mocking libraries.", error: true)]
    internal FileSystemInfoBase() { }

    /// <summary>
    /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
    /// </summary>
    public IFileSystem FileSystem { get; }

#if FEATURE_CREATE_SYMBOLIC_LINK
    /// <inheritdoc cref="FileSystemInfo.CreateAsSymbolicLink(string)"/>
    public abstract void CreateAsSymbolicLink(string pathToTarget);
#endif

    /// <inheritdoc cref="FileSystemInfo.Delete"/>
    public abstract void Delete();

    /// <inheritdoc cref="FileSystemInfo.Refresh"/>
    public abstract void Refresh();

#if FEATURE_CREATE_SYMBOLIC_LINK
    /// <inheritdoc cref="FileSystemInfo.ResolveLinkTarget(bool)"/>
    public abstract IFileSystemInfo ResolveLinkTarget(bool returnFinalTarget);
#endif

    /// <inheritdoc cref="FileSystemInfo.Attributes"/>
    public abstract FileAttributes Attributes { get; set; }

    /// <inheritdoc cref="FileSystemInfo.CreationTime"/>
    public abstract DateTime CreationTime { get; set; }

    /// <inheritdoc cref="FileSystemInfo.CreationTimeUtc"/>
    public abstract DateTime CreationTimeUtc { get; set; }

    /// <inheritdoc cref="FileSystemInfo.Exists"/>
    public abstract bool Exists { get; }

    /// <inheritdoc cref="FileSystemInfo.Extension"/>
    public abstract string Extension { get; }

    /// <inheritdoc cref="FileSystemInfo.FullName"/>
    public abstract string FullName { get; }

    /// <inheritdoc cref="FileSystemInfo.LastAccessTime"/>
    public abstract DateTime LastAccessTime { get; set; }

    /// <inheritdoc cref="FileSystemInfo.LastAccessTimeUtc"/>
    public abstract DateTime LastAccessTimeUtc { get; set; }

    /// <inheritdoc cref="FileSystemInfo.LastWriteTime"/>
    public abstract DateTime LastWriteTime { get; set; }

    /// <inheritdoc cref="FileSystemInfo.LastWriteTimeUtc"/>
    public abstract DateTime LastWriteTimeUtc { get; set; }

#if FEATURE_FILE_SYSTEM_INFO_LINK_TARGET
    /// <inheritdoc cref="FileSystemInfo.LinkTarget"/>
    public abstract string LinkTarget { get; }
#endif

    /// <inheritdoc cref="FileSystemInfo.Name"/>
    public abstract string Name { get; }

#if FEATURE_UNIX_FILE_MODE
        /// <inheritdoc cref="IFileSystemInfo.UnixFileMode"/>
        public UnixFileMode UnixFileMode
        {
            get;
            [UnsupportedOSPlatform("windows")] set;
        }
#endif
}