using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    internal class FileSystemExtensibility
    {
        private readonly Dictionary<string, object> _metadata = new();
        
        public virtual void StoreMetadata(string key, object value)
        {
            _metadata[key] = value;
        }
        
        public virtual object RetrieveMetadata(string key)
        {
            if (_metadata.TryGetValue(key, out object value))
            {
                return value;
            }

            return default;
        }

        internal void CloneFrom(FileSystemExtensibility template)
        {
            foreach (var item in template._metadata)
            {
                _metadata[item.Key] = item.Value;
            }
        }
    }
}