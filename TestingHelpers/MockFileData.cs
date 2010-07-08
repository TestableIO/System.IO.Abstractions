using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFileData
    {
        static readonly Encoding encoding = Encoding.UTF8;

        readonly string path;
        readonly byte[] contents;

        public MockFileData(string path, string textContents)
            : this(path, encoding.GetBytes(textContents))
        {}

        public MockFileData(string path, byte[] contents)
        {
            this.path = path;
            this.contents = contents;
        }

        public string Path
        {
            get { return path; }
        }

        public byte[] Contents
        {
            get { return contents; }
        }

        public string TextContents
        {
            get { return encoding.GetString(contents); }
        }
    }
}