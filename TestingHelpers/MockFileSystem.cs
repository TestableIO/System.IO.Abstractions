using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFileSystem : IFileSystem, IMockFileDataAccessor
    {
        readonly IDictionary<string, MockFileData> files;
        readonly FileBase file;
        readonly DirectoryBase directory;

        public MockFileSystem(IDictionary<string, MockFileData> files)
        {
            this.files = new Dictionary<string, MockFileData>(
                files,
                StringComparer.InvariantCultureIgnoreCase);

            file = new MockFile(this);
            directory = new MockDirectory(file);
        }

        public FileBase File
        {
            get { return file; }
        }

        public DirectoryBase Directory
        {
            get { return directory; }
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