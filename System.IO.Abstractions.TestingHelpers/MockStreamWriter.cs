using System.Text;
using System.Threading.Tasks;
using static System.IO.Abstractions.TestingHelpers.MockFileStream;

namespace System.IO.Abstractions.TestingHelpers
{
    internal class MockStreamWriter : IStreamWriter
    {
        internal const int DefaultBufferSize = 1024;

        private StreamWriter _streamWriter;

        private static Encoding _UTF8NoBOM = new UTF8Encoding(false, true);

        public MockStreamWriter(IMockFileDataAccessor mockfileSystem, string path)
            : this(mockfileSystem, path, false, _UTF8NoBOM, DefaultBufferSize)
        {
        }

        public MockStreamWriter(IMockFileDataAccessor mockfileSystem, string path, bool append)
             : this(mockfileSystem, path, append, _UTF8NoBOM, DefaultBufferSize)
        {
        }

        public MockStreamWriter(IMockFileDataAccessor mockfileSystem, string path, bool append, Encoding encoding)
             : this(mockfileSystem, path, append, encoding, DefaultBufferSize)
        {
        }

        public MockStreamWriter(IMockFileDataAccessor mockfileSystem, string path, bool append, Encoding encoding, int bufferSize)
            : this(mockfileSystem, CreateStream(mockfileSystem, path, append), encoding, bufferSize)
        {
        }

        public MockStreamWriter(IMockFileDataAccessor mockfileSystem, Stream stream)
            : this(mockfileSystem, stream, _UTF8NoBOM, DefaultBufferSize)
        {
        }

        public MockStreamWriter(IMockFileDataAccessor mockfileSystem, Stream stream, Encoding encoding)
            : this(mockfileSystem, stream, encoding, DefaultBufferSize)
        {
        }

        public MockStreamWriter(IMockFileDataAccessor mockfileSystem, Stream stream, Encoding encoding, int bufferSize)
        {
            if (stream == null || encoding == null)
            {
                throw new ArgumentNullException((stream == null ? "stream" : "encoding"));
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException("Argument_StreamNotWritable");
            }

            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            if (!(stream is MemoryStream))
            {
                throw new InvalidOperationException("Only memory streams are supported!");
            }

            Init(stream, encoding, bufferSize);
        }

        public bool AutoFlush
        {
            get
            {
                return _streamWriter.AutoFlush;
            }

            set
            {
                _streamWriter.AutoFlush = value;
            }
        }

        public Stream BaseStream
        {
            get
            {
                return _streamWriter.BaseStream;
            }
        }

        public Encoding Encoding
        {
            get
            {
                return _streamWriter.Encoding;
            }
        }

#if !NETSTANDARD1_4
        public void Close()
        {
            _streamWriter.Close();
        }
#endif
        public void Flush()
        {
            _streamWriter.Flush();
        }

        public void Write(bool value)
        {
            _streamWriter.Write(value);
        }

        public void Write(char value)
        {
            _streamWriter.Write(value);
        }

        public void Write(char[] buffer)
        {
            _streamWriter.Write(buffer);
        }

        public void Write(char[] buffer, int index, int count)
        {
            _streamWriter.Write(buffer, index, count);
        }

        public void Write(decimal value)
        {
            _streamWriter.Write(value);
        }

        public void Write(double value)
        {
            _streamWriter.Write(value);
        }

        public void Write(float value)
        {
            _streamWriter.Write(value);
        }

        public void Write(int value)
        {
            _streamWriter.Write(value);
        }

        public void Write(long value)
        {
            _streamWriter.Write(value);
        }

        public void Write(object value)
        {
            _streamWriter.Write(value);
        }

        public void Write(string value)
        {
            _streamWriter.Write(value);
        }

        public void Write(string format, object arg0)
        {
            _streamWriter.Write(format, arg0);
        }

        public void Write(string format, object arg0, object arg1)
        {
            _streamWriter.Write(format, arg0, arg1);
        }

        public void Write(string format, object arg0, object arg1, object arg2)
        {
            _streamWriter.Write(format, arg0, arg1, arg2);
        }

        public void Write(string format, params object[] arg)
        {
            _streamWriter.Write(format, arg);
        }

        public void Write(uint value)
        {
            _streamWriter.Write(value);
        }

        public void Write(ulong value)
        {
            _streamWriter.Write(value);
        }

        public void WriteLine()
        {
            _streamWriter.WriteLine();
        }

        public void WriteLine(bool value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(char value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(char[] buffer)
        {
            _streamWriter.WriteLine(buffer);
        }

        public void WriteLine(char[] buffer, int index, int count)
        {
            _streamWriter.WriteLine(buffer, index, count);
        }

        public void WriteLine(decimal value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(double value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(float value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(int value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(long value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(object value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(string value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(string format, object arg0)
        {
            _streamWriter.WriteLine(format, arg0);
        }

        public void WriteLine(string format, object arg0, object arg1)
        {
            _streamWriter.WriteLine(format, arg0, arg1);
        }

        public void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            _streamWriter.WriteLine(format, arg0, arg1, arg2);
        }

        public void WriteLine(string format, params object[] arg)
        {
            _streamWriter.WriteLine(format, arg);
        }

        public void WriteLine(uint value)
        {
            _streamWriter.WriteLine(value);
        }

        public void WriteLine(ulong value)
        {
            _streamWriter.WriteLine(value);
        }

        public void Dispose()
        {
            _streamWriter.Dispose();
        }

#if !NET40
        public Task FlushAsync()
        {
            return _streamWriter.FlushAsync();
        }

        public Task WriteAsync(char value)
        {
            return _streamWriter.WriteAsync(value);
        }

        public Task WriteAsync(char[] buffer, int index, int count)
        {
            return _streamWriter.WriteAsync(buffer, index, count);
        }

        public Task WriteAsync(string value)
        {
            return _streamWriter.WriteAsync(value);
        }

        public Task WriteLineAsync()
        {
            return _streamWriter.WriteLineAsync();
        }

        public Task WriteLineAsync(char value)
        {
            return _streamWriter.WriteLineAsync(value);
        }

        public Task WriteLineAsync(char[] buffer, int index, int count)
        {
            return _streamWriter.WriteLineAsync(buffer, index, count);
        }

        public Task WriteLineAsync(string value)
        {
            return _streamWriter.WriteLineAsync(value);
        }
#endif

        private void Init(Stream stream, Encoding encoding, int bufferSize)
        {
            _streamWriter = new StreamWriter(stream, encoding, bufferSize);
        }

        private static Stream CreateStream(IMockFileDataAccessor filesystem, string path, bool append)
        {
            StreamType mode = append ? StreamType.APPEND : StreamType.CREATE;
            MockFileStream stream = new MockFileStream(filesystem, path, mode, FileOptions.SequentialScan);
            return stream;
        }
    }
}