using System.Text;
using System.Threading.Tasks;

namespace System.IO.Abstractions
{
    [Serializable]
    [CLSCompliant(false)]
    public class StreamWriterWrapper : StreamWriterBase
    {
        private readonly StreamWriter _writer;

        public StreamWriterWrapper(IFileSystem fileSystem, StreamWriter instance) : base(fileSystem)
        {
            _writer = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public override bool AutoFlush
        {
            get
            {
                return _writer.AutoFlush;
            }

            set
            {
                _writer.AutoFlush = value;
            }
        }

        public override Stream BaseStream
        {
            get
            {
                return _writer.BaseStream;
            }
        }

        public override Encoding Encoding
        {
            get
            {
                return _writer.Encoding;
            }
        }

#if NET40
        public override void Close()
        {
            _writer.Close();
        }
#endif

        public override void Flush()
        {
            _writer.Flush();
        }

#if !NET40
        public override Task FlushAsync()
        {
            return _writer.FlushAsync();
        }
#endif
        public override void Write(bool value)
        {
            _writer.Write(value);
        }

        public override void Write(string format, params object[] arg)
        {
            _writer.Write(format, arg);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            _writer.Write(format, arg0, arg1, arg2);
        }

        public override void Write(string format, object arg0)
        {
            _writer.Write(format, arg0);
        }

        public override void Write(object value)
        {
            _writer.Write(value);
        }

        public override void Write(string value)
        {
            _writer.Write(value);
        }

        public override void Write(decimal value)
        {
            _writer.Write(value);
        }

        public override void Write(double value)
        {
            _writer.Write(value);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            _writer.Write(format, arg0, arg1);
        }

        public override void Write(ulong value)
        {
            _writer.Write(value);
        }

        public override void Write(long value)
        {
            _writer.Write(value);
        }

        public override void Write(uint value)
        {
            _writer.Write(value);
        }

        public override void Write(int value)
        {
            _writer.Write(value);
        }

        public override void Write(char value)
        {
            _writer.Write(value);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            _writer.Write(buffer, index, count);
        }

        public override void Write(char[] buffer)
        {
            _writer.Write(buffer);
        }

        public override void Write(float value)
        {
            _writer.Write(value);
        }

#if !NET40
        public override Task WriteAsync(char value)
        {
            return _writer.WriteAsync(value);
        }

        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            return _writer.WriteAsync(buffer, index, count);
        }

        public override Task WriteAsync(string value)
        {
            return _writer.WriteAsync(value);
        }
#endif

        public override void WriteLine(string value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            _writer.Write(format, arg);
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            _writer.Write(format, arg0, arg1);
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            _writer.Write(format, arg0, arg1, arg2);
        }

        public override void WriteLine(decimal value)
        {
            _writer.Write(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            _writer.WriteLine(format, arg0);
        }

        public override void WriteLine(double value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(uint value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(int value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(ulong value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(bool value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            _writer.WriteLine(buffer, index, count);
        }

        public override void WriteLine(char[] buffer)
        {
            _writer.WriteLine(buffer);
        }

        public override void WriteLine(char value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine()
        {
            _writer.WriteLine();
        }

#if !NET40
        public override Task WriteLineAsync()
        {
            return _writer.WriteLineAsync();
        }

        public override Task WriteLineAsync(char value)
        {
            return _writer.WriteLineAsync(value);
        }

        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            return _writer.WriteLineAsync(buffer, index, count);
        }

        public override Task WriteLineAsync(string value)
        {
            return _writer.WriteLineAsync(value);
        }
#endif
    }
}
