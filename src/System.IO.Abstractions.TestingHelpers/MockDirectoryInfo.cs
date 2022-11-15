using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    using XFS = MockUnixSupport;

    /// <inheritdoc />
    [Serializable]
    public class MockDirectoryInfo : DirectoryInfoBase
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;
        private readonly string directoryPath;
        private readonly string originalPath;
        private MockFileData cachedMockFileData;
        private bool refreshOnNextRead;

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

            if (directoryPath== null)
            {
                throw new ArgumentNullException("path", StringResources.Manager.GetString("VALUE_CANNOT_BE_NULL"));
            }
            if (directoryPath.Trim() == string.Empty)
            {
                throw CommonExceptions.PathIsNotOfALegalForm("path");
            }
            directoryPath = mockFileDataAccessor.Path.GetFullPath(directoryPath);

            directoryPath = directoryPath.TrimSlashes();
            if (XFS.IsWindowsPlatform())
            {
                directoryPath = directoryPath.TrimEnd(' ');
            }
            this.directoryPath = directoryPath;

            Refresh();
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override void CreateAsSymbolicLink(string pathToTarget)
        {
            throw new NotImplementedException();
        }
#endif

        /// <inheritdoc />
        public override void Delete()
        {
            mockFileDataAccessor.Directory.Delete(directoryPath);
            refreshOnNextRead = true;
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            var mockFileData = mockFileDataAccessor.GetFile(directoryPath) ?? MockFileData.NullObject;
            cachedMockFileData = mockFileData.Clone();
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override IFileSystemInfo ResolveLinkTarget(bool returnFinalTarget)
        {
            throw new NotImplementedException();
        }
#endif

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get { return GetMockFileDataForRead().Attributes; }
            set { GetMockFileDataForWrite().Attributes = value; }
        }

        /// <inheritdoc />
        public override DateTime CreationTime
        {
            get { return GetMockFileDataForRead().CreationTime.LocalDateTime; }
            set { GetMockFileDataForWrite().CreationTime = value; }
        }

        /// <inheritdoc />
        public override DateTime CreationTimeUtc
        {
            get { return GetMockFileDataForRead().CreationTime.UtcDateTime; }
            set { GetMockFileDataForWrite().CreationTime = value; }
        }

        /// <inheritdoc />
        public override bool Exists
        {
            get {
                var mockFileData = GetMockFileDataForRead();
                return (int)mockFileData.Attributes != -1 && mockFileData.IsDirectory;
            }
        }

        /// <inheritdoc />
        public override IFileSystemExtensibility Extensibility
        {
            get
            {
                var mockFileData = mockFileDataAccessor.GetFile(directoryPath);
                return mockFileData?.Extensibility ?? FileSystemExtensibility.GetNullObject(
                    () =>CommonExceptions.CouldNotFindPartOfPath(directoryPath));
            }
        }

        /// <inheritdoc />
        public override string Extension
        {
            get
            {
                // System.IO.Path.GetExtension does only string manipulation,
                // so it's safe to delegate.
                return Path.GetExtension(directoryPath);
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override DateTime LastAccessTime
        {
            get { return GetMockFileDataForRead().LastAccessTime.LocalDateTime; }
            set { GetMockFileDataForWrite().LastAccessTime = value; }
        }

        /// <inheritdoc />
        public override DateTime LastAccessTimeUtc
        {
            get { return GetMockFileDataForRead().LastAccessTime.UtcDateTime; }
            set { GetMockFileDataForWrite().LastAccessTime = value; }
        }

        /// <inheritdoc />
        public override DateTime LastWriteTime
        {
            get { return GetMockFileDataForRead().LastWriteTime.LocalDateTime; }
            set { GetMockFileDataForWrite().LastWriteTime = value; }
        }

        /// <inheritdoc />
        public override DateTime LastWriteTimeUtc
        {
            get { return GetMockFileDataForRead().LastWriteTime.UtcDateTime; }
            set { GetMockFileDataForWrite().LastWriteTime = value; }
        }

#if FEATURE_FILE_SYSTEM_INFO_LINK_TARGET
        /// <inheritdoc />
        public override string LinkTarget
        {
            get { return GetMockFileDataForRead().LinkTarget; }
        }
#endif

        /// <inheritdoc />
        public override string Name
        {
            get
            {
                var mockPath = new MockPath(mockFileDataAccessor);
                return string.Equals(mockPath.GetPathRoot(directoryPath), directoryPath) ? directoryPath : mockPath.GetFileName(directoryPath.TrimEnd(mockFileDataAccessor.Path.DirectorySeparatorChar));
            }
        }

        /// <inheritdoc />
        public override void Create()
        {
            mockFileDataAccessor.Directory.CreateDirectory(FullName);
            refreshOnNextRead = true;
        }
        
        /// <inheritdoc />
        public override IDirectoryInfo CreateSubdirectory(string path)
        {
            return mockFileDataAccessor.Directory.CreateDirectory(Path.Combine(FullName, path));
        }

        /// <inheritdoc />
        public override void Delete(bool recursive)
        {
            mockFileDataAccessor.Directory.Delete(directoryPath, recursive);
            refreshOnNextRead = true;
        }

        /// <inheritdoc />
        public override IEnumerable<IDirectoryInfo> EnumerateDirectories()
        {
            return GetDirectories();
        }

        /// <inheritdoc />
        public override IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern)
        {
            return GetDirectories(searchPattern);
        }

        /// <inheritdoc />
        public override IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return GetDirectories(searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return GetDirectories(searchPattern, enumerationOptions);
        }
#endif

        /// <inheritdoc />
        public override IEnumerable<IFileInfo> EnumerateFiles()
        {
            return GetFiles();
        }

        /// <inheritdoc />
        public override IEnumerable<IFileInfo> EnumerateFiles(string searchPattern)
        {
            return GetFiles(searchPattern);
        }

        /// <inheritdoc />
        public override IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return GetFiles(searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return GetFiles(searchPattern, enumerationOptions);
        }
#endif

        /// <inheritdoc />
        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
        {
            return GetFileSystemInfos();
        }

        /// <inheritdoc />
        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
        {
            return GetFileSystemInfos(searchPattern);
        }

        /// <inheritdoc />
        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return GetFileSystemInfos(searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return GetFileSystemInfos(searchPattern, enumerationOptions);
        }
#endif
        
        /// <inheritdoc />
        public override IDirectoryInfo[] GetDirectories()
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath));
        }

        /// <inheritdoc />
        public override IDirectoryInfo[] GetDirectories(string searchPattern)
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath, searchPattern));
        }

        /// <inheritdoc />
        public override IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath, searchPattern, searchOption));
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath, searchPattern, enumerationOptions));
        }
#endif

        private DirectoryInfoBase[] ConvertStringsToDirectories(IEnumerable<string> paths)
        {
            return paths
                .Select(path => new MockDirectoryInfo(mockFileDataAccessor, path))
                .Cast<DirectoryInfoBase>()
                .ToArray();
        }

        /// <inheritdoc />
        public override IFileInfo[] GetFiles()
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName));
        }

        /// <inheritdoc />
        public override IFileInfo[] GetFiles(string searchPattern)
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName, searchPattern));
        }

        /// <inheritdoc />
        public override IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName, searchPattern, searchOption));
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName, searchPattern, enumerationOptions));
        }
#endif

        IFileInfo[] ConvertStringsToFiles(IEnumerable<string> paths)
        {
            return paths
                  .Select(mockFileDataAccessor.FileInfo.New)
                  .ToArray();
        }

        /// <inheritdoc />
        public override IFileSystemInfo[] GetFileSystemInfos()
        {
            return GetFileSystemInfos("*");
        }

        /// <inheritdoc />
        public override IFileSystemInfo[] GetFileSystemInfos(string searchPattern)
        {
            return GetFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <inheritdoc />
        public override IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return GetDirectories(searchPattern, searchOption).OfType<IFileSystemInfo>().Concat(GetFiles(searchPattern, searchOption)).ToArray();
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return GetDirectories(searchPattern, enumerationOptions).OfType<IFileSystemInfo>().Concat(GetFiles(searchPattern, enumerationOptions)).ToArray();
        }
#endif

        /// <inheritdoc />
        public override void MoveTo(string destDirName)
        {
            mockFileDataAccessor.Directory.Move(directoryPath, destDirName);
        }
        
        /// <inheritdoc />
        public override IDirectoryInfo Parent
        {
            get
            {
                return mockFileDataAccessor.Directory.GetParent(directoryPath);
            }
        }

        /// <inheritdoc />
        public override IDirectoryInfo Root
        {
            get
            {
                return new MockDirectoryInfo(mockFileDataAccessor, mockFileDataAccessor.Directory.GetDirectoryRoot(FullName));
            }
        }

        private MockFileData GetMockFileDataForRead()
        {
            if (refreshOnNextRead)
            {
                Refresh();
                refreshOnNextRead = false;
            }
            return cachedMockFileData;
        }

        private MockFileData GetMockFileDataForWrite()
        {
            refreshOnNextRead = true;
            return mockFileDataAccessor.GetFile(directoryPath)
                ?? throw CommonExceptions.CouldNotFindPartOfPath(directoryPath);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return originalPath;
        }
    }
}
