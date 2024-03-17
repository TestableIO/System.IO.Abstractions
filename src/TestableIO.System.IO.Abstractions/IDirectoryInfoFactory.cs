using System.Diagnostics.CodeAnalysis;

namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory for the creation of wrappers for <see cref="DirectoryInfo" /> in a <see cref="IFileSystem" />.
    /// </summary>
    public interface IDirectoryInfoFactory : IFileSystemEntity
    {
        /// <summary>
        /// Initializes a new instance of a wrapper for <see cref="DirectoryInfo"/> which implements <see cref="IDirectoryInfo"/>.
        /// </summary>
        /// <param name="path">A string specifying the path on which to create the <see cref="IDirectoryInfo" />.</param>
        IDirectoryInfo New(string path);

        /// <summary>
        /// Wraps the <paramref name="directoryInfo" /> in a wrapper for <see cref="DirectoryInfo"/> which implements <see cref="IDirectoryInfo" />.
        /// </summary>
        [return: NotNullIfNotNull("directoryInfo")]
        IDirectoryInfo? Wrap(DirectoryInfo? directoryInfo);
    }
}