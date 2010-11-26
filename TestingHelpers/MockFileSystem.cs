using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFileSystem : IFileSystem, IMockFileDataAccessor
    {
        readonly IDictionary<string, MockFileData> files;
        readonly FileBase file;
        readonly DirectoryBase directory;
        readonly IFileInfoFactory fileInfoFactory;

        public MockFileSystem(IDictionary<string, MockFileData> files)
        {
            this.files = new Dictionary<string, MockFileData>(
                files,
                StringComparer.InvariantCultureIgnoreCase);

            file = new MockFile(this);
            directory = new MockDirectory(this, file);
            fileInfoFactory = new MockFileInfoFactory(this);
        }

        public FileBase File
        {
            get { return file; }
        }

        public DirectoryBase Directory
        {
            get { return directory; }
        }

        public IFileInfoFactory FileInfo
        {
            get { return fileInfoFactory; }
        }

        public MockFileData GetFile(string path)
        {
            return FileExists(path) ? files[path] : null;
        }

        public bool FileExists(string path)
        {
            return files.ContainsKey(path);
        }

        public IEnumerable<string> AllPaths
        {
            get { return files.Keys; }
        }
    }
}