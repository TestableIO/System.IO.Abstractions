using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// The class represents the associated data of a file.
    /// </summary>
    [Serializable]
    public class MockFileData
    {
        /// <summary>
        /// The default encoding.
        /// </summary>
        public static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        /// <summary>
        /// The null object.
        /// </summary>
        public static readonly MockFileData NullObject = new MockFileData(string.Empty)
        {
            LastWriteTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
            LastAccessTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
            CreationTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
            Attributes = FileAttributes.Normal,
        };

        /// <summary>
        /// Gets the default date time offset.
        /// E.g. for not existing files.
        /// </summary>
        public static readonly DateTimeOffset DefaultDateTimeOffset = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc);

        /// <summary>
        /// The access control of the <see cref="MockFileData"/>.
        /// </summary>
        [NonSerialized]
        private FileSecurity accessControl;

        /// <summary>
        /// Gets a value indicating whether the <see cref="MockFileData"/> is a directory or not.
        /// </summary>
        public virtual bool IsDirectory { get { return false; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockFileData"/> class with an empty content.
        /// </summary>
        private MockFileData()
        {
            // empty
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockFileData"/> class with the content of <paramref name="textContents"/> using the encoding of <see cref="DefaultEncoding"/>.
        /// </summary>
        /// <param name="textContents">The textual content encoded into bytes with <see cref="DefaultEncoding"/>.</param>
        public MockFileData(string textContents)
            : this(DefaultEncoding.GetBytes(textContents))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockFileData"/> class with the content of <paramref name="textContents"/> using the encoding of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="textContents">The textual content.</param>
        /// <param name="encoding">The specific encoding used the encode the text.</param>
        /// <remarks>The constructor respect the BOM of <paramref name="encoding"/>.</remarks>
        public MockFileData(string textContents, Encoding encoding)
            : this()
        {
            Contents = encoding.GetPreamble().Concat(encoding.GetBytes(textContents)).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockFileData"/> class with the content of <paramref name="contents"/>.
        /// </summary>
        /// <param name="contents">The actual content.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contents"/> is <see langword="null" />.</exception>
        public MockFileData(byte[] contents)
        {
            Contents = contents ?? throw new ArgumentNullException(nameof(contents));
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MockFileData"/> class by copying the given <see cref="MockFileData"/>.
        /// </summary>
        /// <param name="template">The template instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="template"/> is <see langword="null" />.</exception>
        public MockFileData(MockFileData template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            accessControl = template.accessControl;
            Attributes = template.Attributes;
            Contents = template.Contents.ToArray();
            CreationTime = template.CreationTime;
            LastAccessTime = template.LastAccessTime;
            LastWriteTime = template.LastWriteTime;
        }

        /// <summary>
        /// Gets or sets the byte contents of the <see cref="MockFileData"/>.
        /// </summary>
        public byte[] Contents { get; set; }

        /// <summary>
        /// Gets or sets the string contents of the <see cref="MockFileData"/>.
        /// </summary>
        /// <remarks>
        /// The setter uses the <see cref="DefaultEncoding"/> using this can scramble the actual contents.
        /// </remarks>
        public string TextContents
        {
            get { return MockFile.ReadAllBytes(Contents, DefaultEncoding); }
            set { Contents = DefaultEncoding.GetBytes(value); }
        }

        /// <summary>
        /// Gets or sets the date and time the <see cref="MockFileData"/> was created.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; } = new DateTimeOffset(2010, 01, 02, 00, 00, 00, TimeSpan.FromHours(4));

        /// <summary>
        /// Gets or sets the date and time of the <see cref="MockFileData"/> was last accessed to.
        /// </summary>
        public DateTimeOffset LastAccessTime { get; set; } = new DateTimeOffset(2010, 02, 04, 00, 00, 00, TimeSpan.FromHours(4));

        /// <summary>
        /// Gets or sets the date and time of the <see cref="MockFileData"/> was last written to.
        /// </summary>
        public DateTimeOffset LastWriteTime { get; set; } = new DateTimeOffset(2010, 01, 04, 00, 00, 00, TimeSpan.FromHours(4));

        /// <summary>
        /// Casts a string into <see cref="MockFileData"/>.
        /// </summary>
        /// <param name="s">The path of the <see cref="MockFileData"/> to be created.</param>
        public static implicit operator MockFileData(string s)
        {
            return new MockFileData(s);
        }

        /// <summary>
        /// Gets or sets the specified <see cref="FileAttributes"/> of the <see cref="MockFileData"/>.
        /// </summary>
        public FileAttributes Attributes { get; set; } = FileAttributes.Normal;

        /// <summary>
        /// Gets or sets <see cref="FileSecurity"/> of the <see cref="MockFileData"/>. This is the object that is returned for this <see cref="MockFileData"/> when calling <see cref="FileBase.GetAccessControl(string)"/>.
        /// </summary>
        public FileSecurity AccessControl
        {
            get
            {
                // FileSecurity's constructor will throw PlatformNotSupportedException on non-Windows platform, so we initialize it in lazy way.
                // This let's us use this class as long as we don't use AccessControl property.
                return accessControl ?? (accessControl = new FileSecurity());
            }
            set { accessControl = value; }
        }

        /// <summary>
        /// Gets or sets the File sharing mode for this file, this allows you to lock a file for reading or writing.
        /// </summary>
        public FileShare AllowedFileShare { get; set; } = FileShare.ReadWrite | FileShare.Delete;
        /// <summary>
        /// Checks whether the file is accessible for this type of FileAccess. 
        /// MockfileData can be configured to have FileShare.None, which indicates it is locked by a 'different process'.
        /// 
        /// If the file is 'locked by a different process', an IOException will be thrown.
        /// </summary>
        /// <param name="path">The path is used in the IOException message to match the message in real life situations</param>
        /// <param name="access">The access type to check</param>
        internal void CheckFileAccess(string path, FileAccess access)
        {
            if (!AllowedFileShare.HasFlag((FileShare)access))
                throw CommonExceptions.ProcessCannotAccessFileInUse(path);
        }
    }
}
