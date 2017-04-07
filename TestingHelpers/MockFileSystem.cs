using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers
{
    using XFS = MockUnixSupport;

    [Serializable]
    public class MockFileSystem : IFileSystem, IMockFileDataAccessor
    {
        private readonly IDictionary<string, MockFileData> files;
        private readonly FileBase file;
        private readonly DirectoryBase directory;
        private readonly IFileInfoFactory fileInfoFactory;
        private readonly PathBase pathField;
        private readonly IDirectoryInfoFactory directoryInfoFactory;
        private readonly IDriveInfoFactory driveInfoFactory;

        [NonSerialized]
        private readonly PathVerifier pathVerifier;

        public MockFileSystem() : this(null) { }

        public MockFileSystem(IDictionary<string, MockFileData> files, string currentDirectory = "")
        {
            if (string.IsNullOrEmpty(currentDirectory))
                currentDirectory = IO.Path.GetTempPath();

            pathVerifier = new PathVerifier(this);

            this.files = new Dictionary<string, MockFileData>(StringComparer.OrdinalIgnoreCase);
            pathField = new MockPath(this);
            file = new MockFile(this);
            directory = new MockDirectory(this, file, currentDirectory);
            fileInfoFactory = new MockFileInfoFactory(this);
            directoryInfoFactory = new MockDirectoryInfoFactory(this);
            driveInfoFactory = new MockDriveInfoFactory(this);

            if (files != null)
            {
                foreach (var entry in files)
                {
                    AddFile(entry.Key, entry.Value);
                }
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
            get { return pathField; }
        }

        public IDirectoryInfoFactory DirectoryInfo
        {
            get { return directoryInfoFactory; }
        }

        public IDriveInfoFactory DriveInfo
        {
            get { return driveInfoFactory; }
        }

        public PathVerifier PathVerifier
        {
            get { return pathVerifier; }
        }

        private string FixPath(string path)
        {
            var pathSeparatorFixed = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            return pathField.GetFullPath(pathSeparatorFixed);
        }

        public MockFileData GetFile(string path)
        {
            path = FixPath(path);

            return GetFileWithoutFixingPath(path);
        }

        public void AddFile(string path, MockFileData mockFile)
        {
            var fixedPath = FixPath(path);
            lock (files)
            {
                if (FileExists(fixedPath))
                {
                    var isReadOnly = (files[fixedPath].Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                    var isHidden = (files[fixedPath].Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

                    if (isReadOnly || isHidden)
                    {
                        throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, path));
                    }
                }

                var directoryPath = Path.GetDirectoryName(fixedPath);

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
                    throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, path));

                var lastIndex = 0;

                bool isUnc =
                    path.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWith(@"//", StringComparison.OrdinalIgnoreCase);

                if (isUnc)
                {
                    //First, confirm they aren't trying to create '\\server\'
                    lastIndex = path.IndexOf(separator, 2, StringComparison.OrdinalIgnoreCase);
                    if (lastIndex < 0)
                        throw new ArgumentException(@"The UNC path should be of the form \\server\share.", "path");

                    /*
                     * Although CreateDirectory(@"\\server\share\") is not going to work in real code, we allow it here for the purposes of setting up test doubles.
                     * See PR https://github.com/tathamoddie/System.IO.Abstractions/pull/90 for conversation
                     */
                }

                while ((lastIndex = path.IndexOf(separator, lastIndex + 1, StringComparison.OrdinalIgnoreCase)) > -1)
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
            {
                if (FileExists(path) && (files[path].Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new UnauthorizedAccessException(string.Format("File {0} cannot be removed, it is read-only.", path));
                }
                files.Remove(path);
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

        private MockFileData GetFileWithoutFixingPath(string path)
        {
            lock (files)
            {
                MockFileData result;
                files.TryGetValue(path, out result);
                return result;
            }
        }
    }
}
