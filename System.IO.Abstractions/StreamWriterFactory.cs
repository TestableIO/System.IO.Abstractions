using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions
{
    [Serializable]
    internal class StreamWriterFactory : IStreamWriterFactory
    {
        private readonly IFileSystem _fileSystem;

        public StreamWriterFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IStreamWriter FromStream(Stream stream)
        {
            var realStreamWriter = new StreamWriter(stream);
            return new StreamWriterWrapper(_fileSystem, realStreamWriter);
        }

        public IStreamWriter FromStream(Stream stream, Encoding encoding)
        {
            var realStreamWriter = new StreamWriter(stream, encoding);
            return new StreamWriterWrapper(_fileSystem, realStreamWriter);
        }

        public IStreamWriter FromStream(Stream stream, Encoding encoding, int bufferSize)
        {
            var realStreamWriter = new StreamWriter(stream, encoding, bufferSize);
            return new StreamWriterWrapper(_fileSystem, realStreamWriter);
        }

#if !NETSTANDARD1_4
        public IStreamWriter FromPath(string path)
        {
            var realStreamWriter = new StreamWriter(path);
            return new StreamWriterWrapper(_fileSystem, realStreamWriter);
        }

        public IStreamWriter FromPath(string path, bool append)
        {
            var realStreamWriter = new StreamWriter(path, append);
            return new StreamWriterWrapper(_fileSystem, realStreamWriter);
        }

        public IStreamWriter FromPath(string path, bool append, Encoding encoding)
        {
            var realStreamWriter = new StreamWriter(path, append, encoding);
            return new StreamWriterWrapper(_fileSystem, realStreamWriter);
        }

        public IStreamWriter FromPath(string path, bool append, Encoding encoding, int bufferSize)
        {
            var realStreamWriter = new StreamWriter(path, append, encoding, bufferSize);
            return new StreamWriterWrapper(_fileSystem, realStreamWriter);
        }
#endif
    }
}
