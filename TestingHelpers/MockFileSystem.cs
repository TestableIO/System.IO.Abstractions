using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFileSystem : IFileSystem, IMockFileDataAccessor
    {
        readonly IDictionary<string, MockFileData> files;

        public MockFileSystem(IDictionary<string, MockFileData> files)
        {
            this.files = new Dictionary<string, MockFileData>(
                files,
                StringComparer.InvariantCultureIgnoreCase);
        }

        public FileBase File
        {
            get { return new MockFile(this); }
        }

        public DirectoryBase Directory
        {
            get { throw new NotImplementedException(); }
        }

        public MockFileData GetFile(string path)
        {
            if (!FileExists(path))
                throw new FileNotFoundException("File not found in mock file system.", path);

            return files[path];
        }

        public bool FileExists(string path)
        {
            return files.ContainsKey(path);
        }
    }
}