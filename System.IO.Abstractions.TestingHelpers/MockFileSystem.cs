using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileSystem : IFileSystem, IMockFileDataAccessor
    {
        private readonly IDictionary<string, MockFileData> files;
        [NonSerialized]
        private readonly PathVerifier pathVerifier;

        public MockFileSystem() : this(null) { }

        public MockFileSystem(IDictionary<string, MockFileData> files, string currentDirectory = "")
        {
            if (string.IsNullOrEmpty(currentDirectory))
            {
                currentDirectory = IO.Path.GetTempPath();
            }

            pathVerifier = new PathVerifier(this);

            this.files = new Dictionary<string, MockFileData>(StringComparer.OrdinalIgnoreCase);
            
            Path = new MockPath(this);
            File = new MockFile(this);
            Directory = new MockDirectory(this, File, currentDirectory);
            FileInfo = new MockFileInfoFactory(this);
            FileStream = new MockFileStreamFactory(this);
            DirectoryInfo = new MockDirectoryInfoFactory(this);
            DriveInfo = new MockDriveInfoFactory(this);
            FileSystemWatcher = new MockFileSystemWatcherFactory();

            if (files != null)
            {
                foreach (var entry in files)
                {
                    AddFile(entry.Key, entry.Value);
                }
            }
        }

        public FileBase File { get; }

        public DirectoryBase Directory { get; }

        public IFileInfoFactory FileInfo { get; }

        public IFileStreamFactory FileStream { get; }

        public PathBase Path { get; }

        public IDirectoryInfoFactory DirectoryInfo { get; }

        public IDriveInfoFactory DriveInfo { get; }

        public IFileSystemWatcherFactory FileSystemWatcher { get; }

        public IFileSystem FileSystem => this;

        public PathVerifier PathVerifier => pathVerifier;

        private string FixPath(string path, bool checkCaps = false)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path), StringResources.Manager.GetString("VALUE_CANNOT_BE_NULL"));
            }
            
            var pathSeparatorFixed = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            var fullPath = Path.GetFullPath(pathSeparatorFixed);

            return checkCaps ? GetPathWithCorrectDirectoryCapitalization(fullPath) : fullPath;
        }

        //If C:\foo exists, ensures that trying to save a file to "C:\FOO\file.txt" instead saves it to "C:\foo\file.txt".
        private string GetPathWithCorrectDirectoryCapitalization(string fullPath)
        {
            string[] splitPath = fullPath.Split(Path.DirectorySeparatorChar);
            string leftHalf = fullPath;
            string rightHalf = "";

            for (int i = splitPath.Length - 1; i > 1; i--)
            {
                rightHalf = i == splitPath.Length - 1 ? splitPath[i] : splitPath[i] + Path.DirectorySeparatorChar + rightHalf;
                int lastSeparator = leftHalf.LastIndexOf(Path.DirectorySeparatorChar);
                leftHalf = lastSeparator > 0 ? leftHalf.Substring(0, lastSeparator) : leftHalf;

                if (Directory.Exists(leftHalf))
                {
                    leftHalf = Path.GetFullPath(leftHalf).TrimSlashes();
                    string baseDirectory = AllDirectories.First(dir => dir.Equals(leftHalf, StringComparison.OrdinalIgnoreCase));
                    return baseDirectory + Path.DirectorySeparatorChar + rightHalf;
                }
            }

            return fullPath.TrimSlashes();
        }

        public MockFileData GetFile(string path)
        {
            path = FixPath(path).TrimSlashes();
            return GetFileWithoutFixingPath(path);
        }

        private void SetEntry(string path, MockFileData mockFile)
        {
            path = FixPath(path, true).TrimSlashes();
            files[path] = mockFile;
        }

        public void AddFile(string path, MockFileData mockFile)
        {
            var fixedPath = FixPath(path, true);
            lock (files)
            {
                var file = GetFile(fixedPath);

                if (file != null)
                {
                    var isReadOnly = (file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                    var isHidden = (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

                    if (isReadOnly || isHidden)
                    {
                        throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("ACCESS_TO_THE_PATH_IS_DENIED"), path));
                    }
                }

                var directoryPath = Path.GetDirectoryName(fixedPath);

                if (!Directory.Exists(directoryPath))
                {
                    AddDirectory(directoryPath);
                }

                SetEntry(fixedPath, mockFile ?? new MockFileData(string.Empty));
            }
        }

        public void AddDirectory(string path)
        {
            var fixedPath = FixPath(path, true);
            var separator = XFS.Separator();

            lock (files)
            {
                if (FileExists(fixedPath) &&
                    (GetFile(fixedPath).Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("ACCESS_TO_THE_PATH_IS_DENIED"), fixedPath));

                var lastIndex = 0;

                bool isUnc =
                    fixedPath.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase) ||
                    fixedPath.StartsWith(@"//", StringComparison.OrdinalIgnoreCase);

                if (isUnc)
                {
                    //First, confirm they aren't trying to create '\\server\'
                    lastIndex = fixedPath.IndexOf(separator, 2, StringComparison.OrdinalIgnoreCase);
                    if (lastIndex < 0)
                        throw new ArgumentException(@"The UNC path should be of the form \\server\share.", "path");

                    /*
                     * Although CreateDirectory(@"\\server\share\") is not going to work in real code, we allow it here for the purposes of setting up test doubles.
                     * See PR https://github.com/System-IO-Abstractions/System.IO.Abstractions/pull/90 for conversation
                     */
                }

                while ((lastIndex = fixedPath.IndexOf(separator, lastIndex + 1, StringComparison.OrdinalIgnoreCase)) > -1)
                {
                    var segment = fixedPath.Substring(0, lastIndex + 1);
                    if (!Directory.Exists(segment))
                    {
                        SetEntry(segment, new MockDirectoryData());
                    }
                }

                var s = fixedPath.EndsWith(separator, StringComparison.OrdinalIgnoreCase) ? fixedPath : fixedPath + separator;
                SetEntry(s, new MockDirectoryData());
            }
        }

        public void AddFileFromEmbeddedResource(string path, Assembly resourceAssembly, string embeddedResourcePath)
        {
            using (var embeddedResourceStream = resourceAssembly.GetManifestResourceStream(embeddedResourcePath))
            {
                if (embeddedResourceStream == null)
                {
                    throw new Exception("Resource not found in assembly");
                }

                using (var streamReader = new BinaryReader(embeddedResourceStream))
                {
                    var fileData = streamReader.ReadBytes((int)embeddedResourceStream.Length);
                    AddFile(path, new MockFileData(fileData));
                }
            }
        }

        public void AddFilesFromEmbeddedNamespace(string path, Assembly resourceAssembly, string embeddedRresourcePath)
        {
            var matchingResources = resourceAssembly.GetManifestResourceNames().Where(f => f.StartsWith(embeddedRresourcePath));
            foreach (var resource in matchingResources)
            {
                using (var embeddedResourceStream = resourceAssembly.GetManifestResourceStream(resource))
                using (var streamReader = new BinaryReader(embeddedResourceStream))
                {
                    var fileName = resource.Substring(embeddedRresourcePath.Length + 1);
                    var fileData = streamReader.ReadBytes((int)embeddedResourceStream.Length);
                    var filePath = Path.Combine(path, fileName);
                    AddFile(filePath, new MockFileData(fileData));
                }
            }
        }

        public void MoveDirectory(string sourcePath, string destPath)
        {
            sourcePath = FixPath(sourcePath);
            destPath = FixPath(destPath);

            lock (files)
            {
                var affectedPaths = files.Keys
                    .Where(p => p.StartsWith(sourcePath, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach(var path in affectedPaths)
                {
                    var newPath = path.Replace(sourcePath, destPath, StringComparison.OrdinalIgnoreCase);
                    files[newPath] = files[path];
                    files.Remove(path);
                }
            }
        }

        public void RemoveFile(string path)
        {
            path = FixPath(path);

            lock (files)
            {
                if (FileExists(path) && (GetFile(path).Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("ACCESS_TO_THE_PATH_IS_DENIED"), path));
                }

                files.Remove(path);
            }
        }

        public bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            path = FixPath(path).TrimSlashes();

            lock (files)
            {
                return files.ContainsKey(path);
            }
        }

        public IEnumerable<string> AllPaths
        {
            get
            {
                lock (files)
                {
                    return files.Keys.ToArray();
                }
            }
        }

        public IEnumerable<string> AllNodes
        {
            get
            {
                lock (files)
                {
                    return AllPaths.Where(path => !IsStartOfAnotherPath(path)).ToArray();
                }
            }
        }

        public IEnumerable<string> AllFiles
        {
            get
            {
                lock (files)
                {
                    return files.Where(f => !f.Value.IsDirectory).Select(f => f.Key).ToArray();
                }
            }
        }

        public IEnumerable<string> AllDirectories
        {
            get
            {
                lock (files)
                {
                    return files.Where(f => f.Value.IsDirectory).Select(f => f.Key).ToArray();
                }
            }
        }

        private bool IsStartOfAnotherPath(string path)
        {
            return AllPaths.Any(otherPath => otherPath.StartsWith(path) && otherPath != path);
        }

        private MockFileData GetFileWithoutFixingPath(string path)
        {
            lock (files)
            {
                files.TryGetValue(path, out var result);
                return result;
            }
        }
    }
}
