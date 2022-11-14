namespace System.IO.Abstractions
{
    /// <summary>
    ///     Interface to support implementing extension methods on top of nested <see cref="IFileSystem" /> interfaces.
    /// </summary>
    public interface IFileSystemEntity
    {
        /// <summary>
        ///     Exposes the underlying file system implementation.
        ///     <para />
        ///     This is useful for implementing extension methods.
        /// </summary>
        IFileSystem FileSystem { get; }
    }
}