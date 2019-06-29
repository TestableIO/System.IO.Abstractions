using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    using XFS = MockUnixSupport;

    [Serializable]
    public class MockDirectoryInfo : DirectoryInfoBase
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;
        private readonly string directoryPath;
        private readonly string originalPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockDirectoryInfo"/> class.
        /// </summary>
        /// <param name="mockFileDataAccessor">The mock file data accessor.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mockFileDataAccessor"/> or <paramref name="directoryPath"/> is <see langref="null"/>.</exception>
        public MockDirectoryInfo(IMockFileDataAccessor mockFileDataAccessor, string directoryPath) : base(mockFileDataAccessor?.FileSystem)
        {
            this.mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));

            originalPath = directoryPath;
            directoryPath = mockFileDataAccessor.Path.GetFullPath(directoryPath);

            directoryPath = directoryPath.TrimSlashes();
            if (XFS.IsWindowsPlatform())
            {
                directoryPath = directoryPath.TrimEnd(' ');
            }
            this.directoryPath = directoryPath;
        }

        public override void Delete()
        {
            mockFileDataAccessor.Directory.Delete(directoryPath);
        }

        public override void Refresh()
        {
        }

        public override FileAttributes Attributes
        {
            get { return GetMockFileDataForRead().Attributes; }
            set { GetMockFileDataForWrite().Attributes = value; }
        }

        public override DateTime CreationTime
        {
            get { return GetMockFileDataForRead().CreationTime.DateTime; }
            set { GetMockFileDataForWrite().CreationTime = value; }
        }

        public override DateTime CreationTimeUtc
        {
            get { return GetMockFileDataForRead().CreationTime.UtcDateTime; }
            set { GetMockFileDataForWrite().CreationTime = value.ToLocalTime(); }
        }

        public override bool Exists
        {
            get { return mockFileDataAccessor.Directory.Exists(FullName); }
        }

        public override string Extension
        {
            get
            {
                // System.IO.Path.GetExtension does only string manipulation,
                // so it's safe to delegate.
                return Path.GetExtension(directoryPath);
            }
        }

        public override string FullName
        {
            get
            {
                var root = mockFileDataAccessor.Path.GetPathRoot(directoryPath);

                if (mockFileDataAccessor.StringOperations.Equals(directoryPath, root))
                {
                    // drives have the trailing slash
                    return directoryPath;
                }

                // directories do not have a trailing slash
                return directoryPath.TrimEnd('\\').TrimEnd('/');
            }
        }

        public override DateTime LastAccessTime
        {
            get { return GetMockFileDataForRead().LastAccessTime.DateTime; }
            set { GetMockFileDataForWrite().LastAccessTime = value; }
        }

        public override DateTime LastAccessTimeUtc
        {
            get { return GetMockFileDataForRead().LastAccessTime.UtcDateTime; }
            set { GetMockFileDataForWrite().LastAccessTime = value.ToLocalTime(); }
        }

        public override DateTime LastWriteTime
        {
            get { return GetMockFileDataForRead().LastWriteTime.DateTime; }
            set { GetMockFileDataForWrite().LastWriteTime = value; }
        }

        public override DateTime LastWriteTimeUtc
        {
            get { return GetMockFileDataForRead().LastWriteTime.UtcDateTime; }
            set { GetMockFileDataForWrite().LastWriteTime = value.ToLocalTime(); }
        }

        public override string Name
        {
            get { return new MockPath(mockFileDataAccessor).GetFileName(directoryPath.TrimEnd(mockFileDataAccessor.Path.DirectorySeparatorChar)); }
        }

        public override void Create()
        {
            mockFileDataAccessor.Directory.CreateDirectory(FullName);
        }

#if NET40
        public override void Create(DirectorySecurity directorySecurity)
        {
            mockFileDataAccessor.Directory.CreateDirectory(FullName, directorySecurity);
        }
#endif

        public override IDirectoryInfo CreateSubdirectory(string path)
        {
            return mockFileDataAccessor.Directory.CreateDirectory(Path.Combine(FullName, path));
        }

#if NET40
        public override IDirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
        {
            return mockFileDataAccessor.Directory.CreateDirectory(Path.Combine(FullName, path), directorySecurity);
        }
#endif

        public override void Delete(bool recursive)
        {
            mockFileDataAccessor.Directory.Delete(directoryPath, recursive);
        }

        public override IEnumerable<IDirectoryInfo> EnumerateDirectories()
        {
            return GetDirectories();
        }

        public override IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern)
        {
            return GetDirectories(searchPattern);
        }

        public override IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return GetDirectories(searchPattern, searchOption);
        }

        public override IEnumerable<IFileInfo> EnumerateFiles()
        {
            return GetFiles();
        }

        public override IEnumerable<IFileInfo> EnumerateFiles(string searchPattern)
        {
            return GetFiles(searchPattern);
        }

        public override IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return GetFiles(searchPattern, searchOption);
        }

        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
        {
            return GetFileSystemInfos();
        }

        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
        {
            return GetFileSystemInfos(searchPattern);
        }

        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return GetFileSystemInfos(searchPattern, searchOption);
        }

        public override DirectorySecurity GetAccessControl()
        {
            return mockFileDataAccessor.Directory.GetAccessControl(directoryPath);
        }

        public override DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return mockFileDataAccessor.Directory.GetAccessControl(directoryPath, includeSections);
        }

        public override IDirectoryInfo[] GetDirectories()
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath));
        }

        public override IDirectoryInfo[] GetDirectories(string searchPattern)
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath, searchPattern));
        }

        public override IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath, searchPattern, searchOption));
        }

        private DirectoryInfoBase[] ConvertStringsToDirectories(IEnumerable<string> paths)
        {
            return paths
                .Select(path => new MockDirectoryInfo(mockFileDataAccessor, path))
                .Cast<DirectoryInfoBase>()
                .ToArray();
        }

        public override IFileInfo[] GetFiles()
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName));
        }

        public override IFileInfo[] GetFiles(string searchPattern)
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName, searchPattern));
        }

        public override IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName, searchPattern, searchOption));
        }

        IFileInfo[] ConvertStringsToFiles(IEnumerable<string> paths)
        {
            return paths
                  .Select(mockFileDataAccessor.FileInfo.FromFileName)
                  .ToArray();
        }

        public override IFileSystemInfo[] GetFileSystemInfos()
        {
            return GetFileSystemInfos("*");
        }

        public override IFileSystemInfo[] GetFileSystemInfos(string searchPattern)
        {
            return GetFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
        }

        public override IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return GetDirectories(searchPattern, searchOption).OfType<IFileSystemInfo>().Concat(GetFiles(searchPattern, searchOption)).ToArray();
        }

        public override void MoveTo(string destDirName)
        {
            mockFileDataAccessor.Directory.Move(directoryPath, destDirName);
        }

        public override void SetAccessControl(DirectorySecurity directorySecurity)
        {
            mockFileDataAccessor.Directory.SetAccessControl(directoryPath, directorySecurity);
        }

        public override IDirectoryInfo Parent
        {
            get
            {
                return mockFileDataAccessor.Directory.GetParent(directoryPath);
            }
        }

        public override IDirectoryInfo Root
        {
            get
            {
                return new MockDirectoryInfo(mockFileDataAccessor, mockFileDataAccessor.Directory.GetDirectoryRoot(FullName));
            }
        }

        private MockFileData GetMockFileDataForRead()
        {
            return mockFileDataAccessor.GetFile(directoryPath) ?? MockFileData.NullObject;
        }

        private MockFileData GetMockFileDataForWrite()
        {
            return mockFileDataAccessor.GetFile(directoryPath)
                ?? throw CommonExceptions.FileNotFound(directoryPath);
        }

        public override string ToString()
        {
            return originalPath;
        }
    }
}
