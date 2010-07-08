using System.Collections.Generic;
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFileSystem : IFileSystem, IMockFileDataAccessor
    {
        readonly IDictionary<string, MockFileData> files;

        public MockFileSystem(params MockFileData[] files)
        {
            this.files = new Dictionary<string, MockFileData>(
                files.ToDictionary(f => f.Path, f => f),
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