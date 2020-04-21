namespace System.IO.Abstractions
{
    public interface IFileSystemWatcherFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemWatcherBase"/> class, which acts as a wrapper for a FileSystemWatcher
        /// </summary>
        /// <returns></returns>
        IFileSystemWatcher CreateNew();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemWatcherBase"/> class, given the specified directory to monitor, which acts as a wrapper for a FileSystemWatcher
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <returns></returns>
        IFileSystemWatcher CreateNew(string path);

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemWatcherBase"/> class, given the specified directory and type of files to monitor, which acts as a wrapper for a FileSystemWatcher
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
        /// <returns></returns>
        IFileSystemWatcher CreateNew(string path, string filter);

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemWatcherBase"/> class, which acts as a wrapper for a FileSystemWatcher
        /// </summary>
        /// <param name="path">Path to generate the FileSystemWatcherBase for</param>
        /// <returns></returns>
        IFileSystemWatcher FromPath(string path);
    }
}
