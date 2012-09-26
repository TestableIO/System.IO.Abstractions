using System.Collections.Generic;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockStreamReader : IStreamReader
    {
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void DiscardBufferedData()
        {
            throw new NotImplementedException();
        }

        public int Peek()
        {
            throw new NotImplementedException();
        }

        public int Read()
        {
            throw new NotImplementedException();
        }

        public int Read(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public string ReadToEnd()
        {
            if (PreviouslyWrittenData != null)
            {
                return Encoding.Default.GetString(PreviouslyWrittenData.ToArray());

            }
            else
            {
                return string.Empty;
            }
        }

        public string ReadLine()
        {
            throw new NotImplementedException();
        }

        public Encoding CurrentEncoding { get; private set; }
        public Stream BaseStream { get; private set; }
        public bool EndOfStream { get; private set; }

        public MemoryStream PreviouslyWrittenData { get; set; }
    }
}