namespace System.IO.Abstractions.TestingHelpers
{
#if NET40
    [Serializable]
#endif
    public class MockFileStream : MemoryStream
    {
        readonly IMockFileDataAccessor mockFileDataAccessor;
        readonly string path;

        public MockFileStream(IMockFileDataAccessor mockFileDataAccessor, string path, bool forAppend = false)
        {
            if (mockFileDataAccessor == null)
            {
                throw new ArgumentNullException("mockFileDataAccessor");
            }

            this.mockFileDataAccessor = mockFileDataAccessor;
            this.path = path;

            if (mockFileDataAccessor.FileExists(path))
            {
                /* only way to make an expandable MemoryStream that starts with a particular content */
                var data = mockFileDataAccessor.GetFile(path).Contents;
                if (data != null && data.Length > 0)
                {
                    Write(data, 0, data.Length);
                    Seek(0, forAppend
                        ? SeekOrigin.End
                        : SeekOrigin.Begin);
                }
            }
            else
            {
                mockFileDataAccessor.AddFile(path, new MockFileData(new byte[] { }));
            }
        }

#if NET40
        public override void Close()
        {
            InternalFlush();
        }
#endif

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
                Seek(0, SeekOrigin.Begin);
                /* .. read everything out */
                var data = new byte[Length];
                Read(data, 0, (int)Length);
                /* .. put it in the mock system */
                mockFileData.Contents = data;
            }
        }
    }
}