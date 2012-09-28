using System.Text;

namespace System.IO.Abstractions
{
    public class AbstractionsStreamWriter : StreamWriter, IStreamWriter
    {
        public AbstractionsStreamWriter(Stream stream)
            : base(stream)
        { }

        public AbstractionsStreamWriter(Stream stream, Encoding encoding)
            : base(stream, encoding)
        { }

        public AbstractionsStreamWriter(Stream stream, Encoding encoding, int bufferSize)
            : base(stream, encoding, bufferSize)
        { }

        public AbstractionsStreamWriter(string path)
            : base(path)
        { }

        public AbstractionsStreamWriter(string path, bool append)
            : base(path, append)
        { }

        public AbstractionsStreamWriter(string path, bool append, Encoding encoding)
            : base(path, append, encoding)
        { }

        public AbstractionsStreamWriter(string path, bool append, Encoding encoding, int bufferSize)
            : base(path, append, encoding, bufferSize)
        { }

       
    }
}