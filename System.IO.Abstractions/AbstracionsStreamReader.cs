using System.Text;

namespace System.IO.Abstractions
{
    public class AbstracionsStreamReader : StreamReader, IStreamReader
    {
        public AbstracionsStreamReader(string path)
            : base(path)
        {
        }

        public AbstracionsStreamReader(string path, bool detectEncodingFromByteOrderMarks)
            : base(path, detectEncodingFromByteOrderMarks)
        {
        }

        public AbstracionsStreamReader(string path, Encoding encoding)
            : base(path, encoding)
        { }


        public AbstracionsStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
            : base(path, encoding, detectEncodingFromByteOrderMarks)
        { }

        public AbstracionsStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
            : base(path, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        { }

        public AbstracionsStreamReader(Stream stream)
            : base(stream)
        { }

        public AbstracionsStreamReader(Stream stream, bool detectEncodingFromByteOrderMarks)
            : base(stream, detectEncodingFromByteOrderMarks)
        { }

        public AbstracionsStreamReader(Stream stream, Encoding encoding)
            : base(stream, encoding)
        { }

        public AbstracionsStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
            : base(stream, encoding, detectEncodingFromByteOrderMarks)
        { }

        public AbstracionsStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
            : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        { }
    }
}