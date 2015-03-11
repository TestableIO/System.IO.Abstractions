namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileStream : MemoryStream
    {
        readonly IMockFileDataAccessor mockFileDataAccessor;
        readonly string path;

        public MockFileStream(IMockFileDataAccessor mockFileDataAccessor, string path, bool forAppend = false)
        {
            this.mockFileDataAccessor = mockFileDataAccessor;
            this.path = path;

            if (mockFileDataAccessor.FileExists(path))
            {
                /* only way to make an expandable MemoryStream that starts with a particular content */
                var data = mockFileDataAccessor.GetFile(path).Contents;
                if (data != null && data.Length > 0)
                {
                    base.Write(data, 0, data.Length);
                    base.Seek(0, forAppend
                        ? SeekOrigin.End
                        : SeekOrigin.Begin);
                }
            }
            else
            {
                mockFileDataAccessor.AddFile(path, new MockFileData(""));
            }
        }

        public override void Close() 
        {
            InternalFlush();
        }

        public override void Flush()
        {
            InternalFlush();
        }

        private void InternalFlush()
        {
            if (mockFileDataAccessor.FileExists(path))
            {
                mockFileDataAccessor.RemoveFile(path);

                /* reset back to the beginning .. */
                base.Seek(0, SeekOrigin.Begin);
                /* .. read everything out */
                var data = new byte[base.Length];
                base.Read(data, 0, (int)base.Length);
                /* .. put it in the mock system */
                mockFileDataAccessor.AddFile(path, new MockFileData(data));
            }
        }
    }
}