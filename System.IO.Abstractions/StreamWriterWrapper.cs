using System.Text;

namespace System.IO.Abstractions
{
    [Serializable]
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

        public override void Close()
        {
            _writer.Close();
        }

        public override void Flush()
        {
            _writer.Flush();
        }

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(decimal value)
        {
            _writer.WriteLine(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(double value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(uint value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(long value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(int value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(ulong value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(bool value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(char[] buffer)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(char value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(float value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine()
        {
            throw new NotImplementedException();
        }
    }
}
