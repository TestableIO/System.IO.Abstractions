using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockStreamWriter : StreamWriter
    {
        readonly IMockFileDataAccessor _mockFileDataAccessor;
        readonly string _path;
        public MockStreamWriter(Stream stream): base(stream)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public MockStreamWriter(Stream stream, Encoding encoding): base(stream, encoding)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public MockStreamWriter(string path, bool overwritten): base(path, overwritten)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public MockStreamWriter(Stream stream, Encoding encoding, Int32 bufferSize): base(stream, encoding, bufferSize)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public MockStreamWriter(string path, bool overwritten, Encoding encoding): base(path, overwritten, encoding)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public MockStreamWriter(string path, bool overwritten, Encoding encoding, Int32 bufferSize): base(path, overwritten, encoding, bufferSize)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public MockStreamWriter(string path, IMockFileDataAccessor mockFileDataAccessor): base(mockFileDataAccessor.FileInfo.FromFileName(path).OpenWrite())
        {
            this._mockFileDataAccessor = mockFileDataAccessor;
            this._path = path;

            if (_mockFileDataAccessor.FileExists(_path))
            {
                /* only way to make an expandable MemoryStream that starts with a particular content */
                var data = _mockFileDataAccessor.GetFile(_path).TextContents;
                base.Write(data);
            }
        }

        public override void Write(string value)
        {
            base.Write(value);
        }

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
        }

        public override void Close()
        {
            base.Close();
        }
    }


}