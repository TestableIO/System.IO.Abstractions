namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory for the creation of wrappers for <see cref="FileSystemWatcher" /> in a <see cref="IFileSystem" />.
    /// </summary>
    public interface IFileSystemWatcherFactory
    {
        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileSystemWatcher"/> which implements <see cref="IFileSystemWatcher"/>.
        /// </summary>
        /// <returns></returns>
        IFileSystemWatcher CreateNew();

        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileSystemWatcher"/> which implements <see cref="IFileSystemWatcher"/>.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <returns></returns>
        IFileSystemWatcher CreateNew(string path);

        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileSystemWatcher"/> which implements <see cref="IFileSystemWatcher"/>.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
        /// <returns></returns>
        IFileSystemWatcher CreateNew(string path, string filter);
    }
}
