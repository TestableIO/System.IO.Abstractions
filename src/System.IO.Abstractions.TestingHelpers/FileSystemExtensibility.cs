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
            if (_metadata.ContainsKey(key))
            {
                var value = _metadata[key];
                if (value is T)
                {
                    return (T) value;
                }
            }

            return default;
        }

        public static IFileSystemExtensibility GetNullObject(Func<Exception> factory)
            => new NullFileSystemExtensibility(factory);

        private class NullFileSystemExtensibility : IFileSystemExtensibility
        {
            private readonly Func<Exception> _factory;

            public NullFileSystemExtensibility(Func<Exception> factory)
            {
                _factory = factory;
            }
            public bool TryGetWrappedInstance<T>(out T wrappedInstance)
            {
                wrappedInstance = default;
                return false;
            }

            public void StoreMetadata<T>(string key, T value)
            {
                throw _factory.Invoke();
            }

            public T RetrieveMetadata<T>(string key)
            {
                throw _factory.Invoke();
            }
        }
    }
}