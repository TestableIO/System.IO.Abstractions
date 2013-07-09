using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileData
    {
        static readonly Encoding defaultEncoding = Encoding.UTF8;
        public static readonly MockFileData NullObject = new MockFileData("") {
          LastWriteTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
          LastAccessTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
          CreationTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
        };

        byte[] contents;
        Encoding encoding;
        DateTimeOffset creationTime = new DateTimeOffset(2010, 01, 02, 00, 00, 00, TimeSpan.FromHours(4));
        DateTimeOffset lastAccessTime = new DateTimeOffset(2010, 02, 04, 00, 00, 00, TimeSpan.FromHours(4));
        DateTimeOffset lastWriteTime = new DateTimeOffset(2010, 01, 04, 00, 00, 00, TimeSpan.FromHours(4));

        private FileAttributes attributes = FileAttributes.Normal;

        public virtual bool IsDirectory { get { return false; } }
        
        public MockFileData(string textContents)
            : this(defaultEncoding.GetBytes(textContents))
        {}

        public MockFileData(string textContents, Encoding encoding)
            : this(encoding.GetBytes(textContents), encoding)
        { }

        public MockFileData(byte[] contents)
            : this(contents, defaultEncoding)
        { }

        public MockFileData(byte[] contents, Encoding encoding)
        {
            this.encoding = encoding;
            this.contents = contents;
        }

        public byte[] Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        public string TextContents
        {
            get { return encoding.GetString(contents); }
            set { contents = encoding.GetBytes(value); }
        }

        public DateTimeOffset CreationTime
        {
            get { return creationTime; }
            set { creationTime = value; }
        }

        public DateTimeOffset LastAccessTime
        {
            get { return lastAccessTime; }
            set { lastAccessTime = value; }
        }

        public DateTimeOffset LastWriteTime
        {
            get { return lastWriteTime; }
            set { lastWriteTime = value; }
        }

        public static implicit operator MockFileData(string s)
        {
            return new MockFileData(s);
        }

        public FileAttributes Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }
    }
}