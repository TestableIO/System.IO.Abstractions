using System.Runtime.Versioning;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="FileSystemInfo" />
    public interface IFileSystemInfo
    {
        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        /// <remarks>
        /// The property is always a global object related to the global current directory.
        /// </remarks>        
        IFileSystem FileSystem { get; }

        /// <inheritdoc cref="FileSystemInfo.Attributes" />
        FileAttributes Attributes { get; set; }

        /// <inheritdoc cref="FileSystemInfo.CreationTime" />
        DateTime CreationTime { get; set; }

        /// <inheritdoc cref="FileSystemInfo.CreationTimeUtc" />
        DateTime CreationTimeUtc { get; set; }

        /// <inheritdoc cref="FileSystemInfo.Exists" />
        bool Exists { get; }

        /// <inheritdoc cref="FileSystemInfo.Extension" />
        string Extension { get; }

        /// <inheritdoc cref="FileSystemInfo.FullName" />
        string FullName { get; }

        /// <inheritdoc cref="FileSystemInfo.LastAccessTime" />
        DateTime LastAccessTime { get; set; }

        /// <inheritdoc cref="FileSystemInfo.LastAccessTimeUtc" />
        DateTime LastAccessTimeUtc { get; set; }

        /// <inheritdoc cref="FileSystemInfo.LastWriteTime" />
        DateTime LastWriteTime { get; set; }

        /// <inheritdoc cref="FileSystemInfo.LastWriteTimeUtc" />
        DateTime LastWriteTimeUtc { get; set; }

#if FEATURE_FILE_SYSTEM_INFO_LINK_TARGET
        /// <inheritdoc cref="FileSystemInfo.LinkTarget" />
        string? LinkTarget { get; }
#endif

        /// <inheritdoc cref="FileSystemInfo.Name" />
        string Name { get; }

#if FEATURE_UNIX_FILE_MODE
        /// <inheritdoc cref="FileSystemInfo.UnixFileMode" />
        UnixFileMode UnixFileMode
        {
            get;
            [UnsupportedOSPlatform("windows")]
            set;
        }
#endif

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc cref="FileSystemInfo.CreateAsSymbolicLink(string)" />
        void CreateAsSymbolicLink(string pathToTarget);
#endif

        /// <inheritdoc cref="FileSystemInfo.Delete()" />
        void Delete();

        /// <inheritdoc cref="FileSystemInfo.Refresh()" />
        void Refresh();

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc cref="FileSystemInfo.ResolveLinkTarget(bool)" />
        IFileSystemInfo? ResolveLinkTarget(bool returnFinalTarget);
#endif
    }
}
