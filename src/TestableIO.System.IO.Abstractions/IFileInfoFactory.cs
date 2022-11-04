namespace System.IO.Abstractions
{
    /// <summary>
    /// Provides factory methods for creating <see cref="IFileInfo"/> instances. 
    /// </summary>
    public interface IFileInfoFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IFileInfo"/> class, which acts as a wrapper for a file path.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the new file, or the relative file name.</param>
        IFileInfo FromFileName(string fileName);
    }
}