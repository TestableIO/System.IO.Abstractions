namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileStream : MemoryStream
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;
        private readonly string path;
        private readonly bool canWrite = true;
        private readonly FileOptions options;

        private bool disposed;

        public enum StreamType
        {
            READ,
            WRITE,
            APPEND,
            TRUNCATE
        }

        public MockFileStream(
            IMockFileDataAccessor mockFileDataAccessor,
            string path,
            StreamType streamType,
            FileMode fileMode)
            : this(mockFileDataAccessor, path, streamType, FileOptions.None, fileMode)
        {
        }

        public MockFileStream(
            IMockFileDataAccessor mockFileDataAccessor,
            string path,
            StreamType streamType)
            : this(mockFileDataAccessor, path, streamType, FileOptions.None, FileMode.Append)
        {
        }

        public MockFileStream(
            IMockFileDataAccessor mockFileDataAccessor,
            string path,
            StreamType streamType,
            FileOptions options,
            FileMode fileMode = FileMode.Append)
        {
            this.mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));
            this.path = path;
            this.options = options;

            if (mockFileDataAccessor.FileExists(path))
            {
                if (fileMode.Equals(FileMode.CreateNew))
                {
                    throw CommonExceptions.FileAlreadyExists(path);
                }

                var fileData = mockFileDataAccessor.GetFile(path);
                fileData.CheckFileAccess(path, streamType != StreamType.READ ? FileAccess.Write : FileAccess.Read);

                /* only way to make an expandable MemoryStream that starts with a particular content */
                var data = fileData.Contents;
                if (data != null && data.Length > 0 && streamType != StreamType.TRUNCATE)
                {
                    Write(data, 0, data.Length);
                    Seek(0, StreamType.APPEND.Equals(streamType)
                        ? SeekOrigin.End
                        : SeekOrigin.Begin);
                }
            }
            else
            {
                if (!mockFileDataAccessor.Directory.Exists(mockFileDataAccessor.Path.GetDirectoryName(path)))
                {
                    throw CommonExceptions.CouldNotFindPartOfPath(path);
                }

                if (StreamType.READ.Equals(streamType))
                {
                    throw CommonExceptions.FileNotFound(path);
                }

                mockFileDataAccessor.AddFile(path, new MockFileData(new byte[] { }));
            }

            canWrite = streamType != StreamType.READ;
        }

        public override bool CanWrite => canWrite;

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
                mockFileDataAccessor.FileInfo.FromFileName(path).Encrypt();
            }
        }
    }
}