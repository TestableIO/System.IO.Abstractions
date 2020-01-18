namespace System.IO.Abstractions
{
    public interface IFileSystemInfo
    {
        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        IFileSystem FileSystem { get; }
        /// <inheritdoc cref="FileSystemInfo.Delete"/>
        void Delete();
        /// <inheritdoc cref="FileSystemInfo.Refresh"/>
        void Refresh();
        /// <inheritdoc cref="FileSystemInfo.Attributes"/>
        FileAttributes Attributes { get; set; }
        /// <inheritdoc cref="FileSystemInfo.CreationTime"/>
        DateTime CreationTime { get; set; }
        /// <inheritdoc cref="FileSystemInfo.CreationTimeUtc"/>
        DateTime CreationTimeUtc { get; set; }
        /// <inheritdoc cref="FileSystemInfo.Exists"/>
        bool Exists { get; }
        /// <inheritdoc cref="FileSystemInfo.Extension"/>
        string Extension { get; }
        /// <inheritdoc cref="FileSystemInfo.FullName"/>
        string FullName { get; }
        /// <inheritdoc cref="FileSystemInfo.LastAccessTime"/>
        DateTime LastAccessTime { get; set; }
        /// <inheritdoc cref="FileSystemInfo.LastAccessTimeUtc"/>
        DateTime LastAccessTimeUtc { get; set; }
        /// <inheritdoc cref="FileSystemInfo.LastWriteTime"/>
        DateTime LastWriteTime { get; set; }
        /// <inheritdoc cref="FileSystemInfo.LastWriteTimeUtc"/>
        DateTime LastWriteTimeUtc { get; set; }
        /// <inheritdoc cref="FileSystemInfo.Name"/>
        string Name { get; }
    }
}