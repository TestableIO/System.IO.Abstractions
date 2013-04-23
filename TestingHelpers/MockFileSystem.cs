using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileSystem : IFileSystem, IMockFileDataAccessor
    {
        readonly IDictionary<string, MockFileData> files;
        readonly FileBase file;
        readonly DirectoryBase directory;
        readonly IFileInfoFactory fileInfoFactory;
        readonly PathBase path;
        readonly IDirectoryInfoFactory directoryInfoFactory;

        public MockFileSystem(IDictionary<string, MockFileData> files) : this(files, @"C:\Foo\Bar") { }

        public MockFileSystem(IDictionary<string, MockFileData> files, string currentDirectory)
        {
            file = new MockFile(this);
            directory = new MockDirectory(this, file, currentDirectory);
            fileInfoFactory = new MockFileInfoFactory(this);
            path = new MockPath();
            directoryInfoFactory = new MockDirectoryInfoFactory(this);

            //For each mock file add a file to the files dictionary
            //Also add a file entry for all directories leading up to this file
            this.files = new Dictionary<string, MockFileData>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var entry in files)
            {
                var directoryPath = Path.GetDirectoryName(entry.Key);
                if (!directory.Exists(directoryPath))
                    directory.CreateDirectory(directoryPath);

                if (!file.Exists(entry.Key))
                    this.files.Add(entry.Key, entry.Value);
            }
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

        public PathBase Path
        {
            get { return path; }
        }

        public IDirectoryInfoFactory DirectoryInfo
        {
            get { return directoryInfoFactory; }
        }

        private string FixPath(string path)
        {
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        public MockFileData GetFile(string path, bool returnNullObject = false) {
            path = FixPath(path);
            return FileExists(path) ? files[path] : returnNullObject ? MockFileData.NullObject: null;
        }
  
        public void AddFile(string path, MockFileData mockFile)
        {
            path = FixPath(path);
            files.Add(path, mockFile);
        }

        public void RemoveFile(string path)
        {
            path = FixPath(path);
            files.Remove(path);
        }

        public bool FileExists(string path)
        {
            path = FixPath(path);
            return files.ContainsKey(path);
        }

        public IEnumerable<string> AllPaths
        {
            get { return files.Keys; }
        }

        public IEnumerable<string> AllFiles {
            get { return files.Where(f => !f.Value.IsDirectory).Select(f => f.Key); }
        }

        public IEnumerable<string> AllDirectories {
            get { return files.Where(f => f.Value.IsDirectory).Select(f => f.Key); }
        }
    }
}