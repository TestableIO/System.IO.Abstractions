namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// Base class for mock file system events.
    /// </summary>
    public abstract class MockFileSystemEvent
    {
        /// <summary>
        /// By setting the <see cref="ExceptionToThrow"/> the corresponding exception will be thrown instead of changing the mock file system.
        /// </summary>
        public Exception ExceptionToThrow { get; set; }
    }

    /// <summary>
    /// Notifies about a pending change of a directory.
    /// </summary>
    public class MockDirectoryChanging : MockFileSystemEvent
    {
        /// <summary>
        /// The path of the directory that is changed.
        /// </summary>
        public string Path { get; }

        internal MockDirectoryChanging(string path)
        {
            Path = path;
        }
    }

    /// <summary>
    /// Notifies about a pending change of a file.
    /// </summary>
    public class MockFileChanging : MockFileSystemEvent
    {
        /// <summary>
        /// The path of the file that is changed.
        /// </summary>
        public string Path { get; }

        internal MockFileChanging(string path)
        {
            Path = path;
        }
    }
}
