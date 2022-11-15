using System.Diagnostics.CodeAnalysis;

namespace System.IO.Abstractions
{
    /// <summary>
    ///     A container to give access to the wrapped instance and to store metadata on <see cref="IFileSystem" /> entities.
    ///     <br />
    ///     This is useful for writing extension methods.
    /// </summary>
    public interface IFileSystemExtensibility
    {
        /// <summary>
        ///     The wrapped instance on a real file system.
        /// </summary>
        /// <returns>
        ///     <see langword="null" /> when not on a real file system or if the requested type does not match,
        ///     otherwise the wrapped instance.
        /// </returns>
        bool TryGetWrappedInstance<T>([NotNullWhen(true)] out T? wrappedInstance);

        /// <summary>
        ///     Stores additional metadata to the <see cref="IFileSystemInfo" />.
        /// </summary>
        /// <typeparam name="T">The type of the value to store.</typeparam>
        /// <param name="key">The key under which to store the metadata.</param>
        /// <param name="value">The value to store under the given <paramref name="key" />.</param>
        void StoreMetadata<T>(string key, T? value);

        /// <summary>
        ///     Retrieves a previously stored metadata on the <see cref="IFileSystemInfo" />.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The key under which the metadata was stored.</param>
        /// <returns>The value of the previously stored metadata, or <see langword="default" />.</returns>
        T? RetrieveMetadata<T>(string key);
    }
}
