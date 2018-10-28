using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockStreamWriterFactory : IStreamWriterFactory
    {
        private readonly IMockFileDataAccessor _mockfileSystem;

        public MockStreamWriterFactory(IMockFileDataAccessor mockFileSystem)
        {
            _mockfileSystem = mockFileSystem;
        }

        public IStreamWriter FromPath(string path)
        {
            return new MockStreamWriter(_mockfileSystem, path);
        }

        public IStreamWriter FromPath(string path, bool append)
        {
            return new MockStreamWriter(_mockfileSystem, path, append);
        }

        public IStreamWriter FromPath(string path, bool append, Encoding encoding)
        {
            return new MockStreamWriter(_mockfileSystem, path, append, encoding);
        }

        public IStreamWriter FromPath(string path, bool append, Encoding encoding, int bufferSize)
        {
            return new MockStreamWriter(_mockfileSystem, path, append, encoding, bufferSize);
        }

        public IStreamWriter FromStream(Stream stream)
        {
            return new MockStreamWriter(_mockfileSystem, stream);
        }

        public IStreamWriter FromStream(Stream stream, Encoding encoding)
        {
            return new MockStreamWriter(_mockfileSystem, stream, encoding);
        }

        public IStreamWriter FromStream(Stream stream, Encoding encoding, int bufferSize)
        {
            return new MockStreamWriter(_mockfileSystem, stream, encoding, bufferSize);
        }
    }
}
