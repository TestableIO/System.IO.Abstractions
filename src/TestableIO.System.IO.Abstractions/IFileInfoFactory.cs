using System.Diagnostics.CodeAnalysis;

namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory for the creation of wrappers for <see cref="FileInfo" /> in a <see cref="IFileSystem" />.
    /// </summary>
    public interface IFileInfoFactory : IFileSystemEntity
    {
        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="FileInfo"/> which implements <see cref="IFileInfo"/>.
        /// </summary>
        /// <param name="fileName">
        /// The fully qualified name of the new file, or the relative file name.
        /// Do not end the path with the directory separator character.
        /// </param>
        IFileInfo New(string fileName);

        /// <summary>
        /// Wraps the <paramref name="fileInfo" /> in a wrapper for <see cref="FileInfo"/> which implements <see cref="IFileInfo" />.
        /// </summary>
        [return: NotNullIfNotNull("fileInfo")]
        IFileInfo? Wrap(FileInfo? fileInfo);
    }
}