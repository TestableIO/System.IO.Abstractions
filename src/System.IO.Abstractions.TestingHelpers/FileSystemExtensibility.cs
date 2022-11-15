using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    internal class FileSystemExtensibility : IFileSystemExtensibility
    {
        private readonly Dictionary<string, object> _metadata = new();

        /// <inheritdoc cref="IFileSystemExtensibility.TryGetWrappedInstance{T}(out T)" />
        public bool TryGetWrappedInstance<T>(out T wrappedInstance)
        {
            wrappedInstance = default;
            return false;
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
                value is T castedValue)
            {
                return castedValue;
            }

            return default;
        }
    }
}