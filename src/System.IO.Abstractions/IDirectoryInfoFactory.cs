namespace System.IO.Abstractions
{
    public interface IDirectoryInfoFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryInfoBase"/> class, which acts as a wrapper for a directory path.
        /// </summary>
        /// <param name="directoryName">The fully qualified name of the new directory, or the relative directory name.</param>
        DirectoryInfoBase FromDirectoryName(string directoryName);
    }
}