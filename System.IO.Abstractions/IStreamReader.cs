using System.Text;

namespace System.IO.Abstractions
{
    public interface IStreamReader
    {
        void Close();
        void DiscardBufferedData();
        int Peek();
        int Read();
        int Read(char[] buffer, int index, int count);
        string ReadToEnd();
        string ReadLine();
        Encoding CurrentEncoding { get; }
        Stream BaseStream { get; }
        bool EndOfStream { get; }
    }
}