using System.Text;

namespace System.IO.Abstractions
{
    public class AbstracionsStreamWriter : StreamWriter, IStreamWriter
    {
        public AbstracionsStreamWriter(Stream stream)
            : base(stream)
        { }

        public AbstracionsStreamWriter(Stream stream, Encoding encoding)
            : base(stream, encoding)
        { }

        public AbstracionsStreamWriter(Stream stream, Encoding encoding, int bufferSize)
            : base(stream, encoding, bufferSize)
        { }

        public AbstracionsStreamWriter(string path)
            : base(path)
        { }

        public AbstracionsStreamWriter(string path, bool append)
            : base(path, append)
        { }

        public AbstracionsStreamWriter(string path, bool append, Encoding encoding)
            : base(path, append, encoding)
        { }

        public AbstracionsStreamWriter(string path, bool append, Encoding encoding, int bufferSize)
            : base(path, append, encoding, bufferSize)
        { }

       
    }
}