using System.Text;

namespace System.IO.Abstractions
{
    public interface IStreamWriterFactory
    {
#if !NETSTANDARD1_4
        IStreamWriter FromPath(string path);
        IStreamWriter FromPath(string path, bool append);
        IStreamWriter FromPath(string path, bool append, Encoding encoding);
        IStreamWriter FromPath(string path, bool append, Encoding encoding, int bufferSize);
#endif
        IStreamWriter FromStream(Stream stream);
        IStreamWriter FromStream(Stream stream, Encoding encoding);
        IStreamWriter FromStream(Stream stream, Encoding encoding, int bufferSize);
    }
}