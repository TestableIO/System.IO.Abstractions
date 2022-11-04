namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory for the creation of wrappers for <see cref="FileInfo" /> in a <see cref="IFileSystem" />.
    /// </summary>
    public interface IFileInfoFactory
    {
        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileInfo"/> which implements <see cref="IFileInfo"/>.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the new file, or the relative file name.</param>
        IFileInfo FromFileName(string fileName);
    }
}