using System.Collections.Generic;

namespace System.IO.Abstractions;

internal class FileSystemExtensionContainer : IFileSystemExtensionContainer
{
    private readonly object _wrappedInstance;
    private readonly Dictionary<string, object> _metadata = new();

    public FileSystemExtensionContainer(object wrappedInstance)
    {
        _wrappedInstance = wrappedInstance;
    }

    /// <inheritdoc cref="IFileSystemExtensionContainer.HasWrappedInstance{T}(out T)" />
    public bool HasWrappedInstance<T>(out T wrappedInstance)
    {
        wrappedInstance = _wrappedInstance is T
            ? (T)_wrappedInstance
            : default;
        return wrappedInstance != null;
    }

    /// <inheritdoc />
    public void StoreMetadata<T>(string key, T value)
    {
        _metadata[key] = value;
    }

    /// <inheritdoc />
    public T RetrieveMetadata<T>(string key)
    {
        if (_metadata.TryGetValue(key, out object value) &&
            value is T)
        {
            return (T)value;
        }

        return default;
    }
}