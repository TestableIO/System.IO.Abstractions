using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.IO.Abstractions.TestingHelpers
{
    using XFS = MockUnixSupport;

    /// <inheritdoc />
    [Serializable]
    public class MockFileSystem : FileSystemBase, IMockFileDataAccessor
    {
        private const string DEFAULT_CURRENT_DIRECTORY = @"C:\";
        private const string TEMP_DIRECTORY = @"C:\temp";

        private readonly IDictionary<string, FileSystemEntry> files;
        private readonly PathVerifier pathVerifier;
        [NonSerialized]
        private Func<DateTime> dateTimeProvider = defaultDateTimeProvider;
        private static Func<DateTime> defaultDateTimeProvider = () => DateTime.UtcNow;

        /// <inheritdoc />
        public MockFileSystem() : this(null) { }

        /// <inheritdoc />
        public MockFileSystem(IDictionary<string, MockFileData> files, string currentDirectory = "")
        {
            if (string.IsNullOrEmpty(currentDirectory))
            {
                currentDirectory = XFS.Path(DEFAULT_CURRENT_DIRECTORY);
            }
            else if (!System.IO.Path.IsPathRooted(currentDirectory))
            {
                throw new ArgumentException("Current directory needs to be rooted.", nameof(currentDirectory));
            }

            var defaultTempDirectory = XFS.Path(TEMP_DIRECTORY);

            StringOperations = new StringOperations(XFS.IsUnixPlatform());
            pathVerifier = new PathVerifier(this);
            this.files = new Dictionary<string, FileSystemEntry>(StringOperations.Comparer);

            Path = new MockPath(this, defaultTempDirectory);
            File = new MockFile(this);
            Directory = new MockDirectory(this, currentDirectory);
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

            if (!FileExists(currentDirectory))
            {
                AddDirectory(currentDirectory);
            }

            if (!FileExists(defaultTempDirectory))
            {
                AddDirectory(defaultTempDirectory);
            }
        }

        /// <inheritdoc />
        public StringOperations StringOperations { get; }
        /// <inheritdoc />
        public override IFile File { get; }
        /// <inheritdoc />
        public override IDirectory Directory { get; }
        /// <inheritdoc />
        public override IFileInfoFactory FileInfo { get; }
        /// <inheritdoc />
        public override IFileStreamFactory FileStream { get; }
        /// <inheritdoc />
        public override IPath Path { get; }
        /// <inheritdoc />
        public override IDirectoryInfoFactory DirectoryInfo { get; }
        /// <inheritdoc />
        public override IDriveInfoFactory DriveInfo { get; }
        /// <inheritdoc />
        public override IFileSystemWatcherFactory FileSystemWatcher { get; }
        /// <inheritdoc />
        public IFileSystem FileSystem => this;
        /// <inheritdoc />
        public PathVerifier PathVerifier => pathVerifier;

        /// <summary>
        /// Replaces the time provider with a mocked instance. This allows to influence the used time in tests.
        /// <para />
        /// If not set, the default implementation returns <see cref="DateTime.Now"/>.
        /// </summary>
        /// <param name="dateTimeProvider">The function that returns the current <see cref="DateTime"/>.</param>
        /// <returns></returns>
        public MockFileSystem MockTime(Func<DateTime> dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider ?? defaultDateTimeProvider;
            return this;
        }

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

                if (DirectoryExistsWithoutFixingPath(leftHalf))
                {
                    string baseDirectory = files[leftHalf].Path;
                    return baseDirectory + Path.DirectorySeparatorChar + rightHalf;
                }
            }

            return fullPath.TrimSlashes();
        }

        /// <inheritdoc />
        public MockFileData AdjustTimes(MockFileData fileData, TimeAdjustments timeAdjustments)
        {
            var now = dateTimeProvider();
            if (timeAdjustments.HasFlag(TimeAdjustments.CreationTime))
            {
                fileData.CreationTime = now;
            }

            if (timeAdjustments.HasFlag(TimeAdjustments.LastAccessTime))
            {
                fileData.LastAccessTime = now;
            }

            if (timeAdjustments.HasFlag(TimeAdjustments.LastWriteTime))
            {
                fileData.LastWriteTime = now;
            }

            return fileData;
        }

        /// <inheritdoc />
        public MockFileData GetFile(string path)
        {
            path = FixPath(path).TrimSlashes();
            return GetFileWithoutFixingPath(path);
        }

        private void SetEntry(string path, MockFileData mockFile)
        {
            path = FixPath(path, true).TrimSlashes();
            files[path] = new FileSystemEntry { Path = path, Data = mockFile };
        }

        /// <inheritdoc />
        public void AddFile(string path, MockFileData mockFile)
        {
            var fixedPath = FixPath(path, true);
            lock (files)
            {
                mockFile ??= new MockFileData(string.Empty);
                var file = GetFile(fixedPath);

                if (file != null)
                {
                    var isReadOnly = (file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                    var isHidden = (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

                    if (isReadOnly || isHidden)
                    {
                        throw CommonExceptions.AccessDenied(path);
                    }
                    file.CheckFileAccess(fixedPath, FileAccess.Write);
                    mockFile.CreationTime = file.CreationTime;
                }

                var directoryPath = Path.GetDirectoryName(fixedPath);

                if (!DirectoryExistsWithoutFixingPath(directoryPath))
                {
                    AddDirectory(directoryPath);
                }

                SetEntry(fixedPath, mockFile);
            }
        }

        /// <inheritdoc />
        public void AddDirectory(string path)
        {
            var fixedPath = FixPath(path, true);
            var separator = Path.DirectorySeparatorChar.ToString();

            lock (files)
            {
                if (FileExists(fixedPath) && FileIsReadOnly(fixedPath))
                {
                    throw CommonExceptions.AccessDenied(fixedPath);
                }
                var lastIndex = 0;
                var isUnc =
                    StringOperations.StartsWith(fixedPath, @"\\") ||
                    StringOperations.StartsWith(fixedPath, @"//");

                if (isUnc)
                {
                    //First, confirm they aren't trying to create '\\server\'
                    lastIndex = StringOperations.IndexOf(fixedPath, separator, 2);

                    if (lastIndex < 0)
                    {
                        throw CommonExceptions.InvalidUncPath(nameof(path));
                    }

                    /*
                     * Although CreateDirectory(@"\\server\share\") is not going to work in real code, we allow it here for the purposes of setting up test doubles.
                     * See PR https://github.com/TestableIO/System.IO.Abstractions/pull/90 for conversation
                     */
                }

                while ((lastIndex = StringOperations.IndexOf(fixedPath, separator, lastIndex + 1)) > -1)
                {
                    var segment = fixedPath.Substring(0, lastIndex + 1);
                    if (!DirectoryExistsWithoutFixingPath(segment))
                    {
                        SetEntry(segment, new MockDirectoryData());
                    }
                }

                var s = StringOperations.EndsWith(fixedPath, separator) ? fixedPath : fixedPath + separator;
                SetEntry(s, new MockDirectoryData());
            }
        }

        /// <inheritdoc />
        public void AddFileFromEmbeddedResource(string path, Assembly resourceAssembly, string embeddedResourcePath)
        {
            using (var embeddedResourceStream = resourceAssembly.GetManifestResourceStream(embeddedResourcePath))
            {
                if (embeddedResourceStream == null)
                {
                    throw new ArgumentException("Resource not found in assembly", nameof(embeddedResourcePath));
                }

                using (var streamReader = new BinaryReader(embeddedResourceStream))
                {
                    var fileData = streamReader.ReadBytes((int)embeddedResourceStream.Length);
                    AddFile(path, new MockFileData(fileData));
                }
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void MoveDirectory(string sourcePath, string destPath)
        {
            sourcePath = FixPath(sourcePath);
            destPath = FixPath(destPath);

            var sourcePathSequence = sourcePath.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

            lock (files)
            {
                var affectedPaths = files.Keys
                    .Where(p => PathStartsWith(p, sourcePathSequence))
                    .ToList();

                foreach (var path in affectedPaths)
                {
                    var newPath = Path.Combine(destPath, path.Substring(sourcePath.Length).TrimStart(Path.DirectorySeparatorChar));
                    var entry = files[path];
                    entry.Path = newPath;
                    files[newPath] = entry;
                    files.Remove(path);
                }
            }

            bool PathStartsWith(string path, string[] minMatch)
            {
                var pathSequence = path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                if (pathSequence.Length < minMatch.Length)
                {
                    return false;
                }

                for (var i = 0; i < minMatch.Length; i++)
                {
                    if (!StringOperations.Equals(minMatch[i], pathSequence[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <inheritdoc />
        public void RemoveFile(string path)
        {
            path = FixPath(path);

            lock (files)
            {
                if (FileExists(path) && (FileIsReadOnly(path) || Directory.Exists(path) && AnyFileIsReadOnly(path)))
                {
                    throw CommonExceptions.AccessDenied(path);
                }

                files.Remove(path);
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IEnumerable<string> AllFiles
        {
            get
            {
                lock (files)
                {
                    return files.Where(f => !f.Value.Data.IsDirectory).Select(f => f.Key).ToArray();
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<string> AllDirectories
        {
            get
            {
                lock (files)
                {
                    return files.Where(f => f.Value.Data.IsDirectory).Select(f => f.Key).ToArray();
                }
            }
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext c)
        {
            dateTimeProvider = defaultDateTimeProvider;
        }

        private bool AnyFileIsReadOnly(string path)
        {
            return Directory.GetFiles(path).Any(file => FileIsReadOnly(file));
        }

        private bool IsStartOfAnotherPath(string path)
        {
            return AllPaths.Any(otherPath => otherPath.StartsWith(path) && otherPath != path);
        }

        private MockFileData GetFileWithoutFixingPath(string path)
        {
            lock (files)
            {
                return files.TryGetValue(path, out var result) ? result.Data : null;
            }
        }

        private bool DirectoryExistsWithoutFixingPath(string path)
        {
            lock (files)
            {
                return files.TryGetValue(path, out var result) && result.Data.IsDirectory;
            }
        }

        private bool FileIsReadOnly(string path)
        {
            return (GetFile(path).Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
        }

        [Serializable]
        private class FileSystemEntry
        {
            public string Path { get; set; }
            public MockFileData Data { get; set; }
        }
    }
}
