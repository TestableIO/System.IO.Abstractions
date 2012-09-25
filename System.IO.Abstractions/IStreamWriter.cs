using System.Text;

namespace System.IO.Abstractions
{
    public interface IStreamWriter
    {
        void Close();
        void Flush();
        void Write(char value);
        void Write(char[] buffer);
        void Write(char[] buffer, int index, int count);
        void Write(string value);
        void WriteLine(string value);
        bool AutoFlush { get; set; }
        Stream BaseStream { get; }
        Encoding Encoding { get; }
    }
}