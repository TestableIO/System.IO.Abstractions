using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    using System.Linq;

    [Serializable]
    public class MockFileData
    {
        public static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        public static readonly MockFileData NullObject = new MockFileData(string.Empty) {
          LastWriteTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
          LastAccessTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
          CreationTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
        };

        byte[] contents;
        DateTimeOffset creationTime = new DateTimeOffset(2010, 01, 02, 00, 00, 00, TimeSpan.FromHours(4));
        DateTimeOffset lastAccessTime = new DateTimeOffset(2010, 02, 04, 00, 00, 00, TimeSpan.FromHours(4));
        DateTimeOffset lastWriteTime = new DateTimeOffset(2010, 01, 04, 00, 00, 00, TimeSpan.FromHours(4));

        private FileAttributes attributes = FileAttributes.Normal;

        public virtual bool IsDirectory { get { return false; } }

        private MockFileData()
        {
            // empty
        }

        public MockFileData(string textContents)
            : this(DefaultEncoding.GetBytes(textContents))
        {}

        public MockFileData(string textContents, Encoding encoding)
            : this()
        {
            contents = encoding.GetPreamble().Concat(encoding.GetBytes(textContents)).ToArray();
        }

        public MockFileData(byte[] contents)
        {
            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            this.contents = contents;
        }

        public byte[] Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        public string TextContents
        {
            get { return DefaultEncoding.GetString(contents); }
            set { contents = DefaultEncoding.GetBytes(value); }
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