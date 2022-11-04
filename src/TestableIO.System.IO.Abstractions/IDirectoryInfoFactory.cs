namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory for the creation of wrappers for <see cref="DirectoryInfo" /> in a <see cref="IFileSystem" />.
    /// </summary>
    public interface IDirectoryInfoFactory
    {
        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="DirectoryInfo"/> which implements <see cref="IDirectoryInfo"/>.
        /// </summary>
        /// <param name="directoryName">The fully qualified name of the new directory, or the relative directory name.</param>
        IDirectoryInfo FromDirectoryName(string directoryName);
    }
}