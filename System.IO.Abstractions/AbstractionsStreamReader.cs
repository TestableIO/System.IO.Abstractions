using System.Text;

namespace System.IO.Abstractions
{
    public class AbstractionsStreamReader : StreamReader, IStreamReader
    {
        public AbstractionsStreamReader(string path)
            : base(path)
        {
        }

        public AbstractionsStreamReader(string path, bool detectEncodingFromByteOrderMarks)
            : base(path, detectEncodingFromByteOrderMarks)
        {
        }

        public AbstractionsStreamReader(string path, Encoding encoding)
            : base(path, encoding)
        { }


        public AbstractionsStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
            : base(path, encoding, detectEncodingFromByteOrderMarks)
        { }

        public AbstractionsStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
            : base(path, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        { }

        public AbstractionsStreamReader(Stream stream)
            : base(stream)
        { }

        public AbstractionsStreamReader(Stream stream, bool detectEncodingFromByteOrderMarks)
            : base(stream, detectEncodingFromByteOrderMarks)
        { }

        public AbstractionsStreamReader(Stream stream, Encoding encoding)
            : base(stream, encoding)
        { }

        public AbstractionsStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
            : base(stream, encoding, detectEncodingFromByteOrderMarks)
        { }

        public AbstractionsStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
            : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        { }
    }
}