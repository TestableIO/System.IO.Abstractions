using System.Collections.Generic;
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
        readonly PathBase pathField;
        readonly IDirectoryInfoFactory directoryInfoFactory;

        public MockFileSystem() : this(null) { }

        public MockFileSystem(IDictionary<string, MockFileData> files, string currentDirectory = @"C:\Foo\Bar")
        {
            this.files = new Dictionary<string, MockFileData>(StringComparer.InvariantCultureIgnoreCase);
            pathField = new MockPath(this);
            file = new MockFile(this);
            directory = new MockDirectory(this, file, FixPath(currentDirectory));
            fileInfoFactory = new MockFileInfoFactory(this);
            directoryInfoFactory = new MockDirectoryInfoFactory(this);

            if (files == null) return;
            foreach (var entry in files)
                AddFile(entry.Key, entry.Value);
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
            get { return pathField; }
        }

        public IDirectoryInfoFactory DirectoryInfo
        {
            get { return directoryInfoFactory; }
        }

        private string FixPath(string path)
        {
            var pathSeparatorFixed = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            return pathField.GetFullPath(pathSeparatorFixed);
        }

        public MockFileData GetFile(string path, bool returnNullObject = false) 
        {
            path = FixPath(path);

            lock (files)
                return FileExists(path) ? files[path] : returnNullObject ? MockFileData.NullObject : null;
        }
  
        public void AddFile(string path, MockFileData mockFile)
        {
            var fixedPath = FixPath(path);

            throwExceptionWhenReadonly(path, fixedPath);

            var directoryPath = Path.GetDirectoryName(fixedPath);

            lock (files)
            {
                if (!directory.Exists(directoryPath))
                    directory.CreateDirectory(directoryPath);

                files[fixedPath] = mockFile;
            }
        }

        public void AddDirectory(string path)
        {
            var fixedPath = FixPath(path);

            lock (files)
            {
                throwExceptionWhenReadonly(path, fixedPath);
                files[fixedPath] = new MockDirectoryData();
            }
        }

        private void throwExceptionWhenReadonly(string path, string fixedPath)
        {
            if (FileExists(path) &&
                (files[fixedPath].Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                throw new UnauthorizedAccessException(string.Format("Access to the path '{0}' is denied.", path));
        }

        public void RemoveFile(string path)
        {
            var fixedPath = FixPath(path);

            lock (files)
            {
                throwExceptionWhenReadonly(path, fixedPath);
                if (!AllDirectories.Contains(Path.GetDirectoryName(path)))
                    throw new DirectoryNotFoundException();

                files.Remove(fixedPath);
            }
        }

        public bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            path = FixPath(path);

            lock (files)
                return files.ContainsKey(path);
        }

        public IEnumerable<string> AllPaths
        {
            get
            {
                lock (files)
                    return files.Keys.ToArray();
            }
        }

        public IEnumerable<string> AllFiles
        {
            get
            {
                lock (file)
                    return files.Where(f => !f.Value.IsDirectory).Select(f => f.Key).ToArray();
            }
        }

        public IEnumerable<string> AllDirectories
        {
            get
            {
                lock (files)
                    return files.Where(f => f.Value.IsDirectory).Select(f => f.Key).ToArray();
            }
        }
    }
}