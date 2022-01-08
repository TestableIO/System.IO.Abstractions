namespace System.IO.Abstractions
{
    /// <summary>
    /// A <see cref="Stream"/> that has properties similar to <see cref="System.IO.FileStream"/>
    /// </summary>
    public class FileStream : Stream
    {
        private readonly Stream _wrappedStream;

        /// <summary>
        /// Constructor for <see cref="FileStream"/>
        /// </summary>
        /// <param name="wrappedStream">The underlying wrapped <see cref="Stream"/></param>
        /// <param name="name">The name of the underlying stream object</param>
        public FileStream(Stream wrappedStream, string name)
        {
            _wrappedStream = wrappedStream;

            Name = name;
        }

        /// <summary>
        /// Name of the underlying stream object.
        /// See <see cref="System.IO.FileStream.Name"/>
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
        public override bool CanRead => _wrappedStream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => _wrappedStream.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => _wrappedStream.CanWrite;

        /// <inheritdoc />
        public override long Length => _wrappedStream.Length;

        /// <inheritdoc />
        public override long Position { get => _wrappedStream.Position; set => _wrappedStream.Position = value; }

        /// <inheritdoc />
        public override void Flush() => _wrappedStream.Flush();

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count) => _wrappedStream.Read(buffer, offset, count);

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin) => _wrappedStream.Seek(offset, origin);

        /// <inheritdoc />
        public override void SetLength(long value) => _wrappedStream.SetLength(value);

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count) => _wrappedStream.Write(buffer, offset, count);
    }
}
