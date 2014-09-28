using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers
{
    using XFS = MockUnixSupport;

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

        public MockFileSystem(IDictionary<string, MockFileData> files, string currentDirectory = "")
        {
            if (String.IsNullOrEmpty(currentDirectory))
                currentDirectory = System.IO.Path.GetTempPath();

            this.files = new Dictionary<string, MockFileData>(StringComparer.OrdinalIgnoreCase);
            pathField = new MockPath(this);
            file = new MockFile(this);
            directory = new MockDirectory(this, file, currentDirectory);
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
            if (FileExists(fixedPath) && (files[fixedPath].Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path));

            var directoryPath = Path.GetDirectoryName(fixedPath);

            lock (files)
            {
                if (!directory.Exists(directoryPath))
                {
                    AddDirectory(directoryPath);
                }

                files[fixedPath] = mockFile;
            }
        }

        public void AddDirectory(string path)
        {
            var fixedPath = FixPath(path);
            var separator = XFS.Separator();

            lock (files)
            {
                if (FileExists(path) &&
                    (files[fixedPath].Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path));

                var lastIndex = 0;

                bool isUnc =
                    path.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWith(@"//", StringComparison.OrdinalIgnoreCase);


                if (isUnc)
                {
                    if (path.Length <= 2)
                        throw new ArgumentException("'" + path + @"' is an invlid path.  The UNC path should be of the form \\server\share.", "path");

                    lastIndex = path.IndexOf(separator, 2);
                    if (lastIndex < 0)
                        throw new ArgumentException("'" + path + @"' is an invlid path.  The server portion of a UNC path cannot be created.", "path");

                    /* Note that we don't set 'lastIndex' here.  Even though in a real filesystem the share name must already
                     * exist (thus this potential error), in the Mock object, we do want to treat the share folder as a regular 
                     * folder that will need to exist if they're creating some sub-folder beneath it.
                     */
                    var shareEndSlashIndex = path.IndexOf(separator, lastIndex + 1);

                    if (shareEndSlashIndex + 1 == path.Length)
                        throw new ArgumentException("'" + path + @"' is an invlid path.  The share name of a UNC path cannot be created.", "path");

                }

                while ((lastIndex = path.IndexOf(separator, lastIndex + 1)) > -1)
                {
                    var segment = path.Substring(0, lastIndex + 1);
                    if (!directory.Exists(segment))
                    {
                        files[segment] = new MockDirectoryData();
                    }
                }

                var s = path.EndsWith(separator, StringComparison.OrdinalIgnoreCase) ? path : path + separator;
                files[s] = new MockDirectoryData();
            }
        }

        public void RemoveFile(string path)
        {
            path = FixPath(path);

            lock (files)
                files.Remove(path);
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