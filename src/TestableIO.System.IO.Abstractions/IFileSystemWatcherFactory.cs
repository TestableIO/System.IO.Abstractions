using System.Diagnostics.CodeAnalysis;

namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory for the creation of wrappers for <see cref="FileSystemWatcher" /> in a <see cref="IFileSystem" />.
    /// </summary>
    public interface IFileSystemWatcherFactory : IFileSystemExtensionPoint
    {
        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileSystemWatcher"/> which implements <see cref="IFileSystemWatcher"/>.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use `IFileSystemWatcherFactory.New()` instead")]
        IFileSystemWatcher CreateNew();

        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileSystemWatcher"/> which implements <see cref="IFileSystemWatcher"/>.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <returns></returns>
        [Obsolete("Use `IFileSystemWatcherFactory.New(string)` instead")]
        IFileSystemWatcher CreateNew(string path);

        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileSystemWatcher"/> which implements <see cref="IFileSystemWatcher"/>.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
        /// <returns></returns>
        [Obsolete("Use `IFileSystemWatcherFactory.New(string, string)` instead")]
        IFileSystemWatcher CreateNew(string path, string filter);

        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileSystemWatcher"/> which implements <see cref="IFileSystemWatcher"/>.
        /// </summary>
        IFileSystemWatcher New();

        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileSystemWatcher"/> which implements <see cref="IFileSystemWatcher"/>.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        IFileSystemWatcher New(string path);

        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileSystemWatcher"/> which implements <see cref="IFileSystemWatcher"/>.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="filter">
        /// The type of files to watch.
        /// For example, <c>"*.txt"</c> watches for changes to all text files.
        /// </param>
        IFileSystemWatcher New(string path, string filter);

        /// <summary>
        /// Wraps the <paramref name="fileSystemWatcher" /> to the testable interface <see cref="IFileSystemWatcher" />.
        /// </summary>
        [return: NotNullIfNotNull("fileSystemWatcher")]
        IFileSystemWatcher? Wrap(FileSystemWatcher? fileSystemWatcher);
    }
}
