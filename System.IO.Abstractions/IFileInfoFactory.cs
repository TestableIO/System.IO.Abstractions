namespace System.IO.Abstractions
{
    public interface IFileInfoFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoBase"/> class, which acts as a wrapper for a file path.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the new file, or the relative file name.</param>
        FileInfoBase FromFileName(string fileName);
    }
}