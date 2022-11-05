using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
    [Serializable]
    public class MockFileStream : FileSystemStream
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;
        private readonly string path;
        private readonly FileAccess access = FileAccess.ReadWrite;
        private readonly FileOptions options;
        private readonly MockFileData fileData;
        private bool disposed;

        /// <inheritdoc />
        public MockFileStream(
                  IMockFileDataAccessor mockFileDataAccessor,
                  string path,
                  FileMode mode,
                  FileAccess access = FileAccess.ReadWrite,
                  FileOptions options = FileOptions.None)
        : base(new MemoryStream(),
               path == null ? null : Path.GetFullPath(path),
               (options & FileOptions.Asynchronous) != 0)

        {
            this.Extensibility = new FileSystemExtensibility();
            this.mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));
            this.path = path;
            this.options = options;

            if (mockFileDataAccessor.FileExists(path))
            {
                if (mode.Equals(FileMode.CreateNew))
                {
                    throw CommonExceptions.FileAlreadyExists(path);
                }

                fileData = mockFileDataAccessor.GetFile(path);
                fileData.CheckFileAccess(path, access);

                var timeAdjustments = GetTimeAdjustmentsForFileStreamWhenFileExists(mode, access);
                mockFileDataAccessor.AdjustTimes(fileData, timeAdjustments);
                var existingContents = fileData.Contents;
                var keepExistingContents =
                    existingContents?.Length > 0 &&
                    mode != FileMode.Truncate && mode != FileMode.Create;
                if (keepExistingContents)
                {
                    base.Write(existingContents, 0, existingContents.Length);
                    base.Seek(0, mode == FileMode.Append
                        ? SeekOrigin.End
                        : SeekOrigin.Begin);
                }
            }
            else
            {
                var directoryPath = mockFileDataAccessor.Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directoryPath) && !mockFileDataAccessor.Directory.Exists(directoryPath))
                {
                    throw CommonExceptions.CouldNotFindPartOfPath(path);
                }

                if (mode.Equals(FileMode.Open) || mode.Equals(FileMode.Truncate))
                {
                    throw CommonExceptions.FileNotFound(path);
                }

                fileData = new MockFileData(new byte[] { });
                mockFileDataAccessor.AdjustTimes(fileData,
                    TimeAdjustments.CreationTime | TimeAdjustments.LastAccessTime);
                mockFileDataAccessor.AddFile(path, fileData);
            }

            this.access = access;
        }

        /// <inheritdoc />
        public override bool CanRead => access.HasFlag(FileAccess.Read);

        /// <inheritdoc />
        public override bool CanWrite => access.HasFlag(FileAccess.Write);

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.LastAccessTime);
            return base.Read(buffer, offset, count);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            InternalFlush();
            base.Dispose(disposing);
            OnClose();
            disposed = true;
        }

        /// <inheritdoc cref="FileSystemStream.Extensibility" />
        public override IFileSystemExtensibility Extensibility { get; }

        /// <inheritdoc cref="FileSystemStream.EndWrite(IAsyncResult)" />
        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (!CanWrite)
            {
                throw new NotSupportedException("Stream does not support writing.");
            }
            base.EndWrite(asyncResult);
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            if (!CanWrite)
            {
                throw new NotSupportedException("Stream does not support writing.");
            }

            base.SetLength(value);
        }

        /// <inheritdoc cref="FileSystemStream.Write(byte[], int, int)" />
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite)
            {
                throw new NotSupportedException("Stream does not support writing.");
            }
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
            base.Write(buffer, offset, count);
        }

#if FEATURE_SPAN
        /// <inheritdoc />
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            if (!CanWrite)
            {
                throw new NotSupportedException("Stream does not support writing.");
            }
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
            base.Write(buffer);
        }
#endif

        /// <inheritdoc cref="FileSystemStream.WriteAsync(byte[], int, int, CancellationToken)" />
        public override Task WriteAsync(byte[] buffer, int offset, int count,
            CancellationToken cancellationToken)
        {
            if (!CanWrite)
            {
                throw new NotSupportedException("Stream does not support writing.");
            }
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
            return base.WriteAsync(buffer, offset, count, cancellationToken);
        }

#if FEATURE_SPAN
        /// <inheritdoc />
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer,
                                             CancellationToken cancellationToken = new())
        {
            if (!CanWrite)
            {
                throw new NotSupportedException("Stream does not support writing.");
            }
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
            return base.WriteAsync(buffer, cancellationToken);
        }
#endif

        /// <inheritdoc cref="FileSystemStream.WriteByte(byte)" />
        public override void WriteByte(byte value)
        {
            if (!CanWrite)
            {
                throw new NotSupportedException("Stream does not support writing.");
            }
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
            base.WriteByte(value);
        }

        /// <inheritdoc />
        public override void Flush()
        {
            InternalFlush();
        }

        private void InternalFlush()
        {
            if (mockFileDataAccessor.FileExists(path))
            {
                var mockFileData = mockFileDataAccessor.GetFile(path);
                /* reset back to the beginning .. */
                var position = Position;
                Seek(0, SeekOrigin.Begin);
                /* .. read everything out */
                var data = new byte[Length];
                Read(data, 0, (int)Length);
                /* restore to original position */
                Seek(position, SeekOrigin.Begin);
                /* .. put it in the mock system */
                mockFileData.Contents = data;
            }
        }

        private void OnClose()
        {
            if (options.HasFlag(FileOptions.DeleteOnClose) && mockFileDataAccessor.FileExists(path))
            {
                mockFileDataAccessor.RemoveFile(path);
            }

            if (options.HasFlag(FileOptions.Encrypted) && mockFileDataAccessor.FileExists(path))
            {
#pragma warning disable CA1416 // Ignore SupportedOSPlatform for testing helper encryption
                mockFileDataAccessor.FileInfo.New(path).Encrypt();
#pragma warning restore CA1416
            }
        }

        private TimeAdjustments GetTimeAdjustmentsForFileStreamWhenFileExists(FileMode mode, FileAccess access)
        {
            switch (mode)
            {
                case FileMode.Append:
                case FileMode.CreateNew:
                    if (access.HasFlag(FileAccess.Read))
                    {
                        return TimeAdjustments.LastAccessTime;
                    }
                    return TimeAdjustments.None;
                case FileMode.Create:
                case FileMode.Truncate:
                    if (access.HasFlag(FileAccess.Write))
                    {
                        return TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime;
                    }
                    return TimeAdjustments.LastAccessTime;
                case FileMode.Open:
                case FileMode.OpenOrCreate:
                default:
                    return TimeAdjustments.None;
            }
        }
    }
}